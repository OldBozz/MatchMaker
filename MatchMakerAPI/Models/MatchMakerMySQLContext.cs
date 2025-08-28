using Microsoft.EntityFrameworkCore;

namespace MatchMakerAPI.Models
{
    public class MatchMakerMySQLContext : MatchMakerBaseContext
    {
        public MatchMakerMySQLContext(DbContextOptions<MatchMakerMySQLContext> options)
            : base(options)
        {
        }
  
    }
}
