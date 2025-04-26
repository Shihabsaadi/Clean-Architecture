using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using Microsoft.AspNetCore.Identity;
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

		public AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		public async Task<ApiResponse<Guid>> RegisterUser(RegisterRequest registerRequest)
		{
			var user = await _userManager.FindByEmailAsync(registerRequest.Email);
			if (user != null) 
			{
				throw new ApiException($"User already taken {registerRequest.Email}");
			}
			var userModel = new ApplicationUser();
			userModel.FirstName = registerRequest.FirstName;
			userModel.LastName = registerRequest.LastName;
			userModel.Gender = registerRequest.Gender;
			userModel.UserName = registerRequest.UserName;
			userModel.Email = registerRequest.Email;
			userModel.NormalizedUserName = registerRequest.UserName.ToUpper();
			userModel.NormalizedEmail = registerRequest.Email.ToUpper();
			userModel.EmailConfirmed = true;
			userModel.PhoneNumberConfirmed = true;

			var result= await _userManager.CreateAsync(userModel, registerRequest.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(userModel, Roles.Basic.ToString());
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
	}
}
