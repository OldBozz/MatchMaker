using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
    public class Club : MatchMakerElement
    {
        [NotMapped]
        public static readonly Club NOCLUB = new();
        public List<Player>? players { get; set; }

        public Club()
        {

        }

    }
}
