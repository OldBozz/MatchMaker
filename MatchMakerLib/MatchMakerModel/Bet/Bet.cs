using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel.Bet
{
    public abstract class Bet : MatchMakerElement
    {
        public enum BetStatus { WON, LOST, PENDING };

        public BetStatus Status { get; set; } = BetStatus.PENDING;   

    }
}
