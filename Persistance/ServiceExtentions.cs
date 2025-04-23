using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
namespace Persistance
{
	public static class ServiceExtentions
	{
		public static void AddPersistanceServiceExtentions(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(
				configuration.GetConnectionString("DefaultConnection")
				));
		}
	}
}
