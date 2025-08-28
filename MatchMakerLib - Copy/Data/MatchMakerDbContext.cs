using MatchMakerLib.MatchMakerModel;
using MatchMakerLib.MatchMakerModel.Bet;
using MatchMakerLib.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MatchMakerLib.Data
{
	public class MatchMakerDbContext : DbContext
	{

		public MatchMakerDbContext(DbContextOptions<MatchMakerDbContext> options)
			: base(options)
		{
		}
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Mainevent> Mainevents { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Court> Courts{ get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<BetPlayer> PlayerBets { get; set; }
        public DbSet<BetTournament> TournamentBets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var env = this.GetService<IHostingEnvironment>();
            //DEV
            modelBuilder.HasDefaultSchema("matchmaker");
            //PROD
            //modelBuilder.HasDefaultSchema("matchmaker");
            //if (env.IsDevelopment())
            //{
            //    modelBuilder.HasDefaultSchema("matchmakertest");
            //}
            //if (env.IsProduction())
            //{
            //    modelBuilder.HasDefaultSchema("matchmaker");
            //}
            //modelBuilder.Entity<Tournament>().AutoInclude();
            //modelBuilder.Entity<Player>().HasMany(e => Teams).WithMany(e => Players);
            //       .ToTable("player") ;

            // Configuring a many-to-many special -> topping relationship that is friendly for serialization
            //modelBuilder.Entity<PizzaTopping>().HasKey(pst => new { pst.PizzaId, pst.ToppingId });
            //modelBuilder.Entity<PizzaTopping>().HasOne<Pizza>().WithMany(ps => ps.Toppings);
            //modelBuilder.Entity<PizzaTopping>().HasOne(pst => pst.Topping).WithMany();
        }
    }

}
