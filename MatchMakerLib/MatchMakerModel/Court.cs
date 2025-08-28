using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MatchMakerLib.MatchMakerModel
{
    public class Court : MatchMakerElement
    {
        [JsonIgnore]
        public List<Tournament> Tournaments { get; set; } = new();
        public Court()
        {

        }
        public Court(string name) : base(name)
        {
        }
    }
}
