using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interface
{
	public interface IApplicationDbContext
	{
		DbSet<Product> Products { get; set; }
		Task<int> SaveChangesAsysnc();
	}
}
