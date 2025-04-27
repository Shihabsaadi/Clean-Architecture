using Application.DTOs;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;
		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpPost("RegisterUser")]
		public async Task<IActionResult> RegisterUser(RegisterRequest registerRequestModel,CancellationToken cancellationToken)
		{
			var result = await _accountService.RegisterUser(registerRequestModel);
			return Ok(result);
		}
		[HttpPost("Authentication")]
		public async Task<IActionResult> AuthenticationUser(AuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
		{
			var result =await _accountService.AuthenticationUser(authenticationRequest);
			return Ok(result);
		}
		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token, CancellationToken cancellationToken)
		{
			var result = await _accountService.ConfirmEmail(userId, token);
			return Ok(result);
		}

		[HttpGet("resend-confirm-email")]
		public async Task<IActionResult> ResendConfirmEmail([FromQuery] string email, CancellationToken cancellationToken)
		{
			var result = await _accountService.ResendConfirmationEmailAsync(email);
			return Ok(result);
		}

		[HttpGet("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromQuery] string email, CancellationToken cancellationToken)
		{
			var result = await _accountService.ForgotPasswordAsync(email);
			return Ok(result);
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPassword, CancellationToken cancellationToken)
		{
			var result = await _accountService.ResetPasswordAsync(resetPassword);
			return Ok(result);
		}
	}
}
