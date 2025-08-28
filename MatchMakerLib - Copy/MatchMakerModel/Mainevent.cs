using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
 
    public class Mainevent : MatchMakerElement
    {
        [NotMapped]
        public static readonly Mainevent NOEVENT = new();
        public Club Club { get; set; } = Club.NOCLUB;

    }
}
