using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistance.IdentityModels;

namespace Persistance.Context
{
	public class ApplicationDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,Guid>, IApplicationDbContext
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
