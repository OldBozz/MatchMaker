using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
    public class Player : MatchMakerElement
    {
        [NotMapped]
        public static readonly Player NOPLAYER = new();
        public string Displayname { get; set; } = "";
        public DateTime? Dob { get; set; }
        public string? Identity { get; set; }
        //public List<Club>? Clubs { get; set; }
        public List<Team> Teams{ get; set; } = new();
        public List<Tournament> Tournaments { get; set; } = new();
        public List<Match> Matches { get; set; } = new();

        public Player()
        {
        }
        public Player(string name) : base(name)
        {
        }
        public override string ToString()
        {
            return Displayname;
        }

    }
}
