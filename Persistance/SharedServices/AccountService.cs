using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using Microsoft.AspNetCore.Identity;
using Persistance.IdentityModels;


namespace Persistance.SharedServices
{
	public class AccountService : IAccountService
	{
        private readonly UserManager<ApplicationUser> _userManager;
		public AccountService(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
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
	}
}
