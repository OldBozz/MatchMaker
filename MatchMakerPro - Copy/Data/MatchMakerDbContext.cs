using MatchMakerLib.MatchMakerModel;
using MatchMakerLib.MatchMakerModel.Bet;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MatchMakerPro.Pages;
using MudBlazor.Services;

namespace MatchMakerPro.Data
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
            //modelBuilder.Entity<Tournament>().Navigation(e => e.Matches).Navigation(e => e.Teams).AutoInclude();
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
        public async void ImportTournament(Tournament? importournament)
        {
            if(importournament == null)
                return;
            Tournament? newtournament = null;
            await Task.Run(() => newtournament = ImportTournamentData(importournament));
            //Add(newtournament);
            //await SaveChangesAsync();

        }
        public Tournament ImportTournamentData(Tournament importournament)
        {
            Tournament newtournament = new Tournament();
            newtournament.Name = importournament.Name;
            newtournament.PlayDate = importournament.PlayDate;
            newtournament.Startdate = importournament.Startdate;
            newtournament.Finishdate = importournament.Finishdate;
            newtournament.PointsToWin = importournament.PointsToWin;
            newtournament.State = importournament.State;
            newtournament.TotalRounds = importournament.TotalRounds;
            newtournament.Mainevent = ImportMainevent(importournament.Mainevent);
            newtournament.Club = ImportClub(importournament.Club);
            foreach (Match importmatch in importournament.Matches)
            {
                Match? dbmatch = ImportMatch(importmatch, newtournament);
                if (dbmatch != null)
                {
                    dbmatch.Tournament = newtournament;
                    if (dbmatch.Court != null && !newtournament.Courts.Contains(dbmatch.Court))
                        newtournament.Courts.Add(dbmatch.Court);
                    if (dbmatch.Team1 != null && !newtournament.Teams.Contains(dbmatch.Team1))
                        newtournament.Teams.Add(dbmatch.Team1);
                    if (dbmatch.Team2 != null && !newtournament.Teams.Contains(dbmatch.Team2))
                        newtournament.Teams.Add(dbmatch.Team2);
                    dbmatch.Tournament = newtournament;
                    newtournament.Players.AddRange(dbmatch.Players.Except(newtournament.Players));
                    newtournament.Matches.Add(dbmatch);
                }
            }
            return newtournament;
        }
        public Match? ImportMatch(Match? importmatch, Tournament tournament)
        {
            Match? dbmatch = null;
            if (importmatch != null)
            {
                dbmatch = new Match(); ;
                dbmatch.Court = ImportCourt(importmatch.Court);
                dbmatch.Team1 = ImportTeam(importmatch.Team1,tournament,dbmatch);
                dbmatch.Team2 = ImportTeam(importmatch.Team2, tournament, dbmatch);
                dbmatch.Team1Points = importmatch.Team1Points;
                dbmatch.Team2Points = importmatch.Team2Points;
                dbmatch.Team1Status= importmatch.Team1Status;
                dbmatch.Team2Status = importmatch.Team2Status;

            }
            return dbmatch;
        }
        public Court? ImportCourt(Court? importcourt)
        {
            Court? dbcourt = null;
            if (importcourt != null)
            {
                foreach (Court c in Courts.ToList())
                {
                    if (c.Name.Equals(importcourt.Name))
                    {
                        dbcourt = c;
                        break;
                    }

                }
                if (dbcourt == null)
                {
                    dbcourt = new Court();
                    dbcourt.Name = importcourt.Name;
                    Courts.Add(dbcourt);
                }
            }
            return dbcourt;
        }
        public Team? ImportTeam(Team? importteam, Tournament tournament, Match match)
        {
            Team? dbteam = null;
            if (importteam != null)
            {
                foreach (Team t in Teams.ToList())
                {
                    if (t.Name.Equals(importteam.Name))
                    {
                        dbteam = t;
                        break;
                    }

                }
                if (dbteam == null)
                {
                    dbteam = new Team();
                    dbteam.Name = importteam.Name;
                    Teams.Add(dbteam);
                }
                
                dbteam.Players.AddRange(ImportPlayers(importteam.Players,tournament,dbteam,match).Except(dbteam.Players));
            }
            return dbteam;
        }
        public List<Player> ImportPlayers(List<Player>? importplayers,Tournament tournament,Team team,Match match)
        {
            List<Player> dbplayers = new();
            Player? dbplayer = null;
            if (importplayers != null)
            {
                foreach(Player importplayer in importplayers)
                {
                    foreach (Player dbp in Players.ToList())
                    {

                        if (dbp.Displayname.Equals(importplayer.Displayname))
                        {
                            dbplayer = dbp; 
                            break;
                        }
                    }
                    if (dbplayer == null)
                    {
                        dbplayer = new Player();
                        dbplayer.Copy(importplayer);
                        if(!dbplayer.Tournaments.Contains(tournament))
                            dbplayer.Tournaments.Add(tournament);
                        if (!dbplayer.Teams.Contains(team))
                            dbplayer.Teams.Add(team);
                        if (!dbplayer.Matches.Contains(match))
                            dbplayer.Matches.Add(match);
                        if (tournament.Club != null && !dbplayer.Clubs.Contains(tournament.Club))
                            dbplayer.Clubs.Add(tournament.Club);
                    }
                    dbplayers.Add(dbplayer);
                }
            }
            return dbplayers;
        }
        public Mainevent? ImportMainevent(Mainevent? importevent)
        {
            Mainevent? dbevent = null;
            if (importevent != null)
            {
                foreach (Mainevent me in Mainevents.ToList())
                {
                    if (me.Name.Equals(importevent.Name))
                    {
                        dbevent = me;
                        break;
                    }

                }
                if (dbevent == null)
                {
                    dbevent = new Mainevent(importevent.Name);
                    Mainevents.Add(dbevent);
                }
            }
            return dbevent;
        }
        public Club? ImportClub(Club? importclub)
        {
            Club? dbclub = null;
            if (importclub != null)
            {
                foreach (Club c in Clubs.ToList())
                {
                    if (c.Name.Equals(importclub.Name))
                    {
                        dbclub = c;
                        break;
                    }

                }
                if (dbclub == null)
                {
                    dbclub = new Club();
                    dbclub.Name = importclub.Name;
                    Clubs.Add(dbclub);
                }
            }
            return dbclub;
        }

    }

}
