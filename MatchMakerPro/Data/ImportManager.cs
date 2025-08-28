using MatchMakerLib.MatchMakerModel;

namespace MatchMakerPro.Data
{
    public static class ImportManager
    {
        public static void ImportPlayers(MatchMakerDbContext context, List<Player> importplayers)
        {
            List<Player> existingplayers = context.Players.ToList();
            foreach (Player importplayer in importplayers)
            {
                bool idmatch = existingplayers.Find(p => p.Id == importplayer.Id) != null;
                bool displaymatch = existingplayers.Find(p => p.Displayname.Equals(importplayer.Displayname)) != null;
                if (idmatch && displaymatch)
                {
                    continue;
                }
                else if (idmatch)
                {
                    importplayer.Id = 0;
                    context.Players.Add(importplayer);
                }
                else
                {
                    context.Players.Add(importplayer);
                }
            }
            context.SaveChanges();
        }
        public static void ImportTeams(MatchMakerDbContext context, List<Team> importteams)
        {
            List<Team> existingteams = context.Teams.ToList();
            foreach (Team importteam in importteams)
            {
                bool idmatch = existingteams.Find(p => p.Id == importteam.Id) != null;
                bool namematch = existingteams.Find(p => p.Name.Equals(importteam.Name)) != null;
                if (idmatch && namematch)
                {
                    continue;
                }
                //else if (idmatch)
                //{
                //    importplayer.Id = 0;
                //    context.Players.Add(importplayer);
                //}
                //else
                //{
                //    context.Players.Add(importplayer);
                //}
            }
            context.SaveChanges();
        }

    }
}
