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
        public List<Player>? Players { get; set; } = new();

        public Club()
        {

        }
    }
}
