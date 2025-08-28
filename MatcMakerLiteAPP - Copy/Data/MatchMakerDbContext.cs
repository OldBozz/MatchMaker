using MatchMakerLib.MatchMakerModel;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System.Reflection.Metadata;

namespace MatcMakerLiteAPP.Data
{
	public class MatchMakerDbContext : DbContext
	{

		public MatchMakerDbContext(DbContextOptions<MatchMakerDbContext> options)
			: base(options)
		{
		}
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("matchmaker");
            //modelBuilder.Entity<Player>()
            //       .ToTable("player") ;

            // Configuring a many-to-many special -> topping relationship that is friendly for serialization
            //modelBuilder.Entity<PizzaTopping>().HasKey(pst => new { pst.PizzaId, pst.ToppingId });
            //modelBuilder.Entity<PizzaTopping>().HasOne<Pizza>().WithMany(ps => ps.Toppings);
            //modelBuilder.Entity<PizzaTopping>().HasOne(pst => pst.Topping).WithMany();
        }
    }

}
