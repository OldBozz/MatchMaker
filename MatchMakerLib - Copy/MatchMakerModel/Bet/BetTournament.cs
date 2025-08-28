using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel.Bet
{
    public class BetTournament : Bet
    {
        public Tournament? Tournament { get; set; }
        public List<BetMatch> BetMatches { get; set; } = new();
    }
}
