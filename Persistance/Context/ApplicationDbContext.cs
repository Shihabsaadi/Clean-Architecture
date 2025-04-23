using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
	public class ApplicationDbContext:DbContext, IApplicationDbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public async Task<int> SaveChangesAsysnc()
        {
            return await base.SaveChangesAsync();
        }
    }
}
