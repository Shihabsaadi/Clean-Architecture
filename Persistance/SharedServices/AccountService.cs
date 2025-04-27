using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistance.IdentityModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Persistance.SharedServices
{
	public class AccountService : IAccountService
	{
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly IEmailService _emailService;
		public AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService)
		{
			_userManager = userManager;
			_configuration = configuration;
			_emailService = emailService;
		}

		public async Task<ApiResponse<Guid>> RegisterUser(RegisterRequest registerRequest)
		{
			var user = await _userManager.FindByEmailAsync(registerRequest.Email);
			if (user != null) 
			{
				throw new ApiException($"User already taken {registerRequest.Email}");
			}
			var userModel = new ApplicationUser
			{
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
				Gender = registerRequest.Gender,
				UserName = registerRequest.UserName,
				Email = registerRequest.Email,
				NormalizedUserName = registerRequest.UserName.ToUpper(),
				NormalizedEmail = registerRequest.Email.ToUpper(),
				PhoneNumberConfirmed = true
			};
			
			var result= await _userManager.CreateAsync(userModel, registerRequest.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(userModel, Roles.Basic.ToString());
				//var emailRequest = new EmailRequest
				//{
				//	To = userModel.Email,
				//	Subject = $"Welcome {userModel.Email} to the clean architecture!",
				//	Body = "User Registration Successful!"
				//};
				//await _emailService.SendAsync(emailRequest);
				await SendConfirmationEmailAsync(userModel);
				return new ApiResponse<Guid>(userModel.Id, "User Registered Successfully!");
			}
			else { throw new ApiException(result.Errors.ToString()); }
		}

		public async Task<ApiResponse<AuthenticationResponse>> AuthenticationUser(AuthenticationRequest authenticationRequest)
		{
			var user = await _userManager.FindByEmailAsync(authenticationRequest.Email);
			if (user == null)
			{
				throw new ApiException($"User not found with this {authenticationRequest.Email}");
			}
		 	var succedded = await _userManager.CheckPasswordAsync(user,authenticationRequest.Password);
			if(!succedded)
			{
				throw new ApiException($"Email or Password incorrect!");
			}
			var jwtSecurity = await GenerateTokenAsync(user);
			var authecticationReponse = new AuthenticationResponse
			{
				Id = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				IsVerified = user.EmailConfirmed,
				Roles = (await _userManager.GetRolesAsync(user)).ToList(),
				JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurity)
			};
			return new ApiResponse<AuthenticationResponse>(authecticationReponse,"Authenticaed User");
		}

		private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
		{
			var dbClaims = await _userManager.GetClaimsAsync(user);
			var roles = (await _userManager.GetRolesAsync(user));
			var roleClaims = new List<Claim>();
			foreach (var role in roles) 
			{
				roleClaims.Add(new Claim("roles", role));
			}
			string ipAddress = "192.168.10.90";
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim("uid",user.Id.ToString()),
				new Claim("ip",ipAddress),
			}
			.Union(dbClaims)
			.Union(roleClaims);

			var symmetrucSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
			var signingCredentials = new SigningCredentials(symmetrucSecurityKey,SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires:DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
				signingCredentials:signingCredentials);
			return jwtSecurityToken;
		}

		private async Task SendConfirmationEmailAsync(ApplicationUser userModel)
		{
		  string token = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
		  token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
		  string verificationUrl = $"{_configuration["ClientUrl"]}/api/account/confirm-email?userId={userModel.Id}&token={token}";

			var emailRequest = new EmailRequest
			{
				To = userModel.Email,
				Subject = $"Confirm your mail {userModel.Email} to the clean architecture!",
				Body = $"<p>User Registration Successful! Please verify your account by clicking on this link: {verificationUrl} </p> <br>",
				IsHtmlBody = true 
			};
			await _emailService.SendAsync(emailRequest);
		}
		public async Task<ApiResponse<bool>> ConfirmEmail(string userId, string token)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ApiException($"User not found with this {userId}");
			}

			token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
			var result = await _userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
				return new ApiResponse<bool>(true, "Email confirmed successfully");
			}
			else
			{
				throw new ApiException(result.Errors.ToString());
			}
		}

		public async Task<ApiResponse<bool>> ResendConfirmationEmailAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new ApiException($"User not found with this {email}");
			}

			if (user.EmailConfirmed)
			{
				throw new ApiException($"Email already confirmed");
			}

			await SendConfirmationEmailAsync(user);
			return new ApiResponse<bool>(true, "Verification email has been sent to your account, pls verify your account.");
		}

		public async Task<ApiResponse<bool>> ForgotPasswordAsync(string userEmail)
		{
			var user = await _userManager.FindByEmailAsync(userEmail);
			if (user == null)
			{
				throw new ApiException($"User not found with this {userEmail}");
			}

			string token = await _userManager.GeneratePasswordResetTokenAsync(user);

			token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

			string resetPasswordLink = $"{_configuration["ClientUrl"]}/api/account/reset-password?email={userEmail}&token={token}";

			var emailRequest = new EmailRequest()
			{
				To = userEmail,
				Body = $"<p>To reset your password click on this link: <a href='{resetPasswordLink}'>Click here to reset password</a> </p>",
				Subject = $"Reset password",
				IsHtmlBody = true,
			};

			await _emailService.SendAsync(emailRequest);
			return new ApiResponse<bool>(true, "Reset password link has been sent to your account, pls check your email.");
		}

		public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest resetPassword)
		{
			var user = await _userManager.FindByEmailAsync(resetPassword.Email);
			if (user == null)
			{
				throw new ApiException($"User not found with this {resetPassword.Email}");
			}

			var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Token));

			var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);

			if (result.Succeeded)
			{
				return new ApiResponse<bool>(true, "Password reset successfully");
			}
			else
			{
				throw new ApiException(result.Errors.ToString());
			}
		}
	}
}
