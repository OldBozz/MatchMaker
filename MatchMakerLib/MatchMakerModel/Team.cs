using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MatchMakerLib.MatchMakerModel
{
    public class Team : MatchMakerElement
    {
        public enum TeamStatus { WINNER, LOSER, LEADING, BEHIND, EVEN };

        public List<Player> Players { get; set; } = new();
        [JsonIgnore]
        public List<Tournament> Tournaments { get; set; } = new();
        [NotMapped]
        public bool HasPlayed { get; set; } = false;
        [NotMapped]
        public int Rank
        {
            get
            {
                int _rank = 0;
                int sum = 0;
                foreach (Player player in Players)
                {
                    sum += player.Rank;
                }
                if (Players.Count > 0)
                    _rank = sum / Players.Count;
                return _rank;
            }
        }

        public Team() { }
        public Team(string name) : base(name)
        {
        }
        public bool Member(Player player)
        {
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
            //return Name;
            string result = "";
            foreach (Player p in Players)
            {
                if (result.Length > 0)
                    result += "/" + p.Displayname;
                else result += p.Displayname;
            }
            result += string.Format(" ({0})", Rank);
            return result;
        }

    }
}
