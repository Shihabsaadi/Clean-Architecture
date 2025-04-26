﻿using Application.Interface;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class ServiceExtensions
	{
		public static void AddInfrastructureServiceExtentions(this IServiceCollection services)
		{
			services.AddTransient<IEmailService,EmailService>();
		}
	}
}
