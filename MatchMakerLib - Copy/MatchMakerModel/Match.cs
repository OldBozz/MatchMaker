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
        [NotMapped]
        public static readonly Match NOMATCH = new();
        public DateTime? Finishdate { get; set; }
        public int Round { get; set; }
        public Court Court { get; set; } = Court.NOCOURT;
        public Team Team1 { get; set; } = Team.NOTEAM;
        public Team Team2 { get; set; } = Team.NOTEAM;
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public TeamStatus Team1Status { get; set; } = TeamStatus.EVEN;
        public TeamStatus Team2Status { get; set; } = TeamStatus.EVEN;
        public Tournament Tournament { get; set; } = new();
        public List<Player> Players { get; set; } = new();

        public Match()
        {

        }
        public Match(int round, Court c, Team t1, Team t2) : base("")
        {
            Round = round;
            Court = c;
            Team1 = t1;
            Team2 = t2;
            Name += string.Format("{0,2}.{1} {2} vs. {3}", round, Court, Team1.Name, Team2.Name);
        }

        public bool HasPlayer(Player p)
        {
            return Team1.Member(p) || (Team2.Member(p));
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
        }
        public Team WinningTeam()
        {
            if (Team1Status == TeamStatus.WINNER)
            {
                return Team1;
            }
            else if (Team2Status == TeamStatus.WINNER)
                return Team2;
            else return Team.NOTEAM;
        }
        private bool Winner(int teampoints, int otherteampoints)
        {
            return teampoints >= /*Tournament.PointsToWin*/15 && teampoints > otherteampoints && teampoints - otherteampoints > 1;


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

    }


}
