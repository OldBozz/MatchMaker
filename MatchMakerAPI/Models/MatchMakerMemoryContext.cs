using Microsoft.EntityFrameworkCore;

namespace MatchMakerAPI.Models
{
    public class MatchMakerMemoryContext : MatchMakerBaseContext
    {
        public MatchMakerMemoryContext(DbContextOptions<MatchMakerMemoryContext> options)
            : base(options)
        {
        }

    }
}
