using Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace Persistance.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>
	{
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public Gender Gender { get; set; }
	}
}
