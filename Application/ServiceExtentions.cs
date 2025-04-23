using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
	public static class ServiceExtentions
	{
		public static void AddApplicationServiceExtentions(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddMediatR(conf=>conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		}
	}
}
