using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatchMakerLib.MatchMakerModel
{
    public class Team : MatchMakerElement
    {
        public enum TeamStatus { WINNER, LOSER, LEADING, BEHIND, EVEN };

        [NotMapped]
        public static readonly Team NOTEAM = new();
        [NotMapped]
        public bool HasPlayed { get; set; } = false;
        [NotMapped]
        public int Matchingattemps { get; set; } = 0;
        public List<Player> Players { get; set; } = new ();
        public List<Tournament> Tournaments { get; set; } = new();

        public Team(){ }
        public Team(string name) : base(name)
        {
        }
        public bool Member(Player player) { 
            return Players.Contains(player); 
        }

        public bool ShareMembers(Team OtherTeam)
        {
            foreach (Player p in Players)
            {
                if (OtherTeam.Member(p))
                    return true;
            }
            return false;
        }
		public int GetMatchCountForPlayers(Tournament tournament)
		{
			int count = 0;
			foreach (Player p in Players)
				count += tournament.GetMatchCountForPlayer(p);
			return count;
		}

		public override string ToString() 
        {
            if( Name.Length < 1 && Players.Count > 1)
            {
                return string.Format("{0}/{1}", Players[0], Players[1]);
            }else
                return Name;
        }
    }
}
