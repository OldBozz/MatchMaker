using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MatchMakerLib.MatchMakerModel
{

    public class Mainevent : MatchMakerElement
    {
        public Club? Club { get; set; }

        public Mainevent() { }
        public Mainevent(string name) : base(name) { }

    }
}
