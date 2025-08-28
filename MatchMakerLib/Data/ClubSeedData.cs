using MatchMakerLib.MatchMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.Data
{
    public class ClubSeedData
    {
        public static void Initialize(MatchMakerDbContext db)
        {
            var clubs = new Club[]
            {
                new Club()
                {
                    Name = "EVK",
                },
                new Club()
                {
                    Name = "FBVK",
                },
            };
            db.Clubs.AddRange(clubs);
            db.SaveChanges();

        }
    }
}
