using Application.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistance.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Seeds
{
	public static class DefaultUsers
	{
		public static async Task SeedUserssAsync(IServiceProvider serviceProvider)
		{
			var userManger = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var user = new ApplicationUser();
			user.FirstName = "Shihab";
			user.LastName = "Saadi";
			user.Gender = (int)Gender.Male;
			user.UserName = "shihabsaadi";
			user.Email = "s.saadi2047@gmail.com";
			user.NormalizedUserName = user.UserName.ToUpper();
			user.NormalizedEmail = user.Email.ToUpper();
			user.EmailConfirmed = true;
			user.PhoneNumberConfirmed = true;

			if (userManger.Users.All(x => x.Id != user.Id))
			{
				var result = await userManger.FindByEmailAsync(user.Email);
				if (result == null)
				{
					var roles=new List<string>
					{
						Roles.SuperAdmin.ToString(),
						Roles.Admin.ToString(),
						Roles.Basic.ToString()
					};
					await userManger.CreateAsync(user,"Sadd@@123");
					await userManger.AddToRolesAsync(user, roles);
				}

			}
		}
	}
}
