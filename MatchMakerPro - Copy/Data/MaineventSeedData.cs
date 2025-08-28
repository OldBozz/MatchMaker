using MatchMakerLib.MatchMakerModel;
using MatchMakerPro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerPro.Data
{
    public static class MaineventSeedData
    {
        public static void Initialize(MatchMakerDbContext db)
        {
            var mainevents = new Mainevent[]
            {
                new Mainevent()
                {
                    Name = "Sommer Beach",
                },
                new Mainevent()
                {
                    Name = "Vinter Beach",
                },
                new Mainevent()
                {
                    Name = "Melby Beach Cup",
                },
            };
            db.Mainevents.AddRange(mainevents);
            db.SaveChanges();

        }
    }
}
