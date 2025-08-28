using MatchMakerLib.MatchMakerModel;

namespace MatchMakerLib.Data
{
    public static class PlayerSeedData
    {
        public static void Initialize(MatchMakerDbContext db)
        {
            var players = new Player[]
            {
                new Player()
                {
                    Displayname = "Bosse",
                },
                new Player()
                {
                    Displayname = "Flemming",
                },
                new Player()
                {
                    Displayname = "Nana",
                },
                new Player()
                {
                    Displayname = "Mads",
                },
                new Player()
                {
                    Displayname = "Kyller",
                },
                new Player()
                {
                    Displayname = "Peter",
                },
                new Player()
                {
                    Displayname = "Henrik",
                },
                new Player()
                {
                    Displayname = "Tim",
                },
                   new Player()
                {
                    Displayname = "Kim",
                },
                  new Player()
                {
                    Displayname = "John",
                },
                  new Player()
                {
                    Displayname = "Curtis",
                },
                  new Player()
                {
                    Displayname = "Stefano",
                },
                  new Player()
                {
                    Displayname = "Bruno",
                },
                 new Player()
                {
                    Displayname = "Niels",
                },
                 new Player()
                {
                    Displayname = "Caroline",
                },
                         new Player()
                {
                    Displayname = "Antonio",
                },
             };
            db.Players.AddRange(players);
            db.SaveChanges();
        }
    }
}
