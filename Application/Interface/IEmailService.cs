﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
	public interface IEmailService
	{
		Task SendAsync(EmailRequest request);
	}
}
