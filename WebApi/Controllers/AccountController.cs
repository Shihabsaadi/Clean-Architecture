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
	}
}
