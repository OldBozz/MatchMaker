using MatchMakerAPI.Models.club;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MatchMakerAPI.Models
{

    public class MatchMakerBaseContext : DbContext
    {
    
        public MatchMakerBaseContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();
        //    connbuilder["Data Source"] = builder.Configuration["ConnectionParams:AzureMySql:Server"];
        //    connbuilder.UserID = builder.Configuration["ConnectionParams:AzureMySql:UserId"];
        //    connbuilder.Password = builder.Configuration["ConnectionParams:AzureMySql:Password"];
        //    connbuilder["Database"] = builder.Configuration["ConnectionParams:AzureMySql:Database"];
        //    Debug.WriteLine(connbuilder.ConnectionString);
        //    optionsBuilder.UseMySQL(connbuilder.ConnectionString);
        //}
        public DbSet<Club> club { get; set; } = null!;
        public DbSet<Player> player { get; set; } = null!;

    }
}
