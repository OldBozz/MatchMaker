using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel.Bet
{
    public class BetPlayer :Bet
    {
        public Tournament? Tournament { get; set; }
        public Player? Player { get; set; }  
    }
}
