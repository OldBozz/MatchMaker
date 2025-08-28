using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatcMakerLiteAPP.Data
{
	public class MyIdentityDbContext : IdentityDbContext
	{
		public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
			: base(options)
		{
		}
	}
}