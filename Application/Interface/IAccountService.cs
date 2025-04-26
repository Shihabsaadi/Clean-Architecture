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
	}
}
