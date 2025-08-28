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
        public int PointsWon { get; set; } = 0;
        public int PointsLost { get; set; } = 0;
        public int PointsResult { get; set; } = 0;
        public int MatchesLeft { get; set; } = 0;
        public PlayerStatus() { }

        public string GetPointsReultAsString()
        {
            //return string.Format("{0}({1}-{2})",PointsResult,PointsWon,PointsLost);
            return string.Format("{0}", PointsResult);
        }

    }
}
