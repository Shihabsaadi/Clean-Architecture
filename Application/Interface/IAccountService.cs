using Application.DTOs;
using Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
	public interface IAccountService
	{
		Task<ApiResponse<Guid>> RegisterUser(RegisterRequest registerRequest);
		Task<ApiResponse<AuthenticationResponse>> AuthenticationUser(AuthenticationRequest authenticationRequest);
		Task<ApiResponse<bool>> ConfirmEmail(string userId, string token);
		Task<ApiResponse<bool>> ResendConfirmationEmailAsync(string email);
		Task<ApiResponse<bool>> ForgotPasswordAsync(string userEmail);
		Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest resetPassword);
	}
}
