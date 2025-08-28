using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MatchMakerAPI.Models
{
     public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<MatchMakerBaseContext>
    {
        public MatchMakerBaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MatchMakerBaseContext>();
            optionsBuilder.UseInMemoryDatabase("MatchMaker");


            return new MatchMakerBaseContext(optionsBuilder.Options);
        }
    }
}
