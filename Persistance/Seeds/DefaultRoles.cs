using Application.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistance.IdentityModels;

namespace Persistance.Seeds
{
	public static class DefaultRoles
	{
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
           var roleManger =  serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var superAdmin = new ApplicationRole();
			superAdmin.Name = Roles.SuperAdmin.ToString();
			superAdmin.NormalizedName = Roles.SuperAdmin.ToString().ToUpper();
			await roleManger.CreateAsync(superAdmin);

			var admin = new ApplicationRole();
			admin.Name = Roles.Admin.ToString();
			admin.NormalizedName = Roles.Admin.ToString().ToUpper();
			await roleManger.CreateAsync(admin);

			var basic = new ApplicationRole();
			basic.Name = Roles.Basic.ToString();
			basic.NormalizedName = Roles.Basic.ToString().ToUpper();
			await roleManger.CreateAsync(basic);
		}
    }
}
