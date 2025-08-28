using MatchMakerLib.MatchMakerModel;
using MatchMakerPro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerPro.Data
{
    public class CourtSeedData
    {
         
        public static void Initialize(MatchMakerDbContext db)
        {
            var courts = new Court[]
            {
                new Court()
                {
                    Name = "1",
                },
                new Court()
                {
                    Name = "2",
                },
            };
            db.Courts.AddRange(courts);
            db.SaveChanges();

        }

    }
}
