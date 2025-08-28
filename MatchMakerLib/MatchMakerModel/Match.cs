using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MatchMakerLib.MatchMakerModel.Team;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatchMakerLib.MatchMakerModel
{


    public class Match : MatchMakerElement
    {
        public DateTime? Finishdate { get; set; }
        public int Round { get; set; }
        public Court? Court { get; set; } 
        public Team? Team1 { get; set; } 
        public Team? Team2 { get; set; } 
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public TeamStatus Team1Status { get; set; } = TeamStatus.EVEN;
        public TeamStatus Team2Status { get; set; } = TeamStatus.EVEN;
        public Tournament Tournament { get; set; }
        public List<Player> Players { get; set; } = new();

        public Match()
        {

        }
        public Match(Team t1, Team t2)
        {
            Team1 = t1;
            Team2 = t2;
            foreach (Player p in Team1.Players)
            {
                Players.Add(p);
            }
            foreach (Player p in Team2.Players)
            {
                Players.Add(p);
            }

            Name += string.Format("{0} vs. {1}", Team1.Name, Team2.Name);
        }
        //public List<Player> GetPlayers()
        //{
        //    List<Player> result = new();
        //    Players.Clear();
        //    foreach (Player p in Team1.Players)
        //    {
        //        result.Add(p);
        //        //Players.Add(p);
        //    }
        //    foreach (Player p in Team2.Players)
        //    {
        //        result.Add(p);
        //        //Players.Add(p);
        //    }
        //    return result;
        //}
        public bool HasPlayer(Player p)
        {
            return (Team1 != null && Team1.Member(p)) || (Team2 != null && Team2.Member(p));
        }
        public bool PlayerWon(Player p)
        {
            return WinningTeam().Member(p);
        }
        public void UpdateScore()
        {
            if (Winner(Team1Points, Team2Points))
            {
                Team1Status = TeamStatus.WINNER;
                Team2Status = TeamStatus.LOSER;
            }
            else if (Winner(Team2Points, Team1Points))
            {
                Team2Status = TeamStatus.WINNER;
                Team1Status = TeamStatus.LOSER;
            }
            else if (Leader(Team1Points, Team2Points))
            {
                Team1Status = TeamStatus.LEADING;
                Team2Status = TeamStatus.BEHIND;
            }
            else if (Leader(Team2Points, Team1Points))
            {
                Team2Status = TeamStatus.LEADING;
                Team1Status = TeamStatus.BEHIND;
            }
            else
            {
                Team1Status = TeamStatus.EVEN;
                Team2Status = TeamStatus.EVEN;
            }
            switch (Team1Status)
            {
                case TeamStatus.BEHIND:
                case TeamStatus.LEADING:
                    this.State = ElementStatus.RUNNING; 
                    break;
                case TeamStatus.WINNER:
                case TeamStatus.LOSER:
                    this.State = ElementStatus.FINISHED; 
                    Finishdate = DateTime.Now;  
                    break;
                default:
                    this.State = ElementStatus.READY; break;
            }
        }
        public Team? WinningTeam()
        {
            if (Team1Status == TeamStatus.WINNER)
            {
                return Team1;
            }
            else if (Team2Status == TeamStatus.WINNER)
            {
                return Team2;
            }
            return null;
        }
        private bool Winner(int teampoints, int otherteampoints)
        {
            return teampoints >= Tournament.PointsToWin && teampoints > otherteampoints && teampoints - otherteampoints > 1;


        }
        private bool Leader(int teampoints, int otherteampoints)
        {
            return (teampoints > otherteampoints);


        }

        public bool HasTeam(Team t)
        {
            if (Team1 == t)
                return true;
            else if (Team2 == t)
                return true;
            else return false;
        }
        public override string ToString()
        {
            return string.Format("{0,2}.{1} {2} vs. {3}", Round, Court, Team1, Team2);

        }

    }


}
