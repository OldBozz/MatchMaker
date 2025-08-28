using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
    public class Court : MatchMakerElement
    {
        [NotMapped]
        public static readonly Court NOCOURT = new();

        public List<Match> Matches { get; set; } = new();
        public List<Tournament> Tournaments { get; set; } = new();
        public Court()
        {

        }
        public Court(string name) : base(name)
        {
        }

     
    }
}
