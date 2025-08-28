using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel.Bet
{
    public class PlayerBet
    {
        public int Id { get; set; }
        public Player? Player { get; set; }
        public Tournament? Tournament { get; set; }
        public Match? Match{ get; set; }

        public Team? Winner { get; set; }

        
        public string GetTeam1Color()
        {
            if (Winner == null)
                return "";
            else if (Winner == Match.Team1)
                return "background-color:#8df87aff";
            else if (Winner == Match.Team2)
                return "background-color:#f87ca3ff";
            return "";
        }
        public string GetTeam2Color()
        {
            if (Winner == null)
                return "";
            else if (Winner == Match.Team2)
                return "background-color:#8df87aff";
            else if (Winner == Match.Team1)
                return "background-color:#f87ca3ff";
            return "";
        }
    }
}
