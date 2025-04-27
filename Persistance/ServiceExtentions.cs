using Application.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Persistance.IdentityModels;
using Persistance.Seeds;
using Persistance.SharedServices;
namespace Persistance
{
	public static class ServiceExtentions
	{
		public static void AddPersistanceServiceExtentions(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(
				configuration.GetConnectionString("DefaultConnection")
				));
			services.AddDataProtection();
			services.AddIdentityCore<ApplicationUser>()
				.AddRoles<ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			services.Configure<DataProtectionTokenProviderOptions>(options =>
			{
				options.TokenLifespan = TimeSpan.FromMinutes(30);
			});
			services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
			services.AddTransient<IAccountService, AccountService>();
			DefaultRoles.SeedRolesAsync(services.BuildServiceProvider()).Wait();//seed roles
			DefaultUsers.SeedUserssAsync(services.BuildServiceProvider()).Wait();//seed users

		}
	}
}
