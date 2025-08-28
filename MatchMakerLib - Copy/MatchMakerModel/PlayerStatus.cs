using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
    public class PlayerStatus
    {
        public Player Player = Player.NOPLAYER;
        public int MatchesTotal { get; set; } = 0;
        public int MatchesWon { get; set; } = 0;
        public int MatchesLost { get; set; } = 0;
        public int MatchesLeft { get; set; } = 0;
        public PlayerStatus() { }

    }
}
