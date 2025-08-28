using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
    public class Player : MatchMakerElement
    {
        [NotMapped]
        public static readonly Player NOPLAYER = new();
        public string Displayname { get; set; } = "";
        public DateTime? Dob { get; set; }
        public int Rank { get; set; }
        public string? Identity { get; set; }
        [JsonIgnore]
        public List<Club> Clubs { get; set; } = new();
        [JsonIgnore]
        public List<Team> Teams{ get; set; } = new();
        [JsonIgnore]
        public List<Tournament> Tournaments { get; set; } = new();
        [JsonIgnore]
        public List<Match> Matches { get; set; } = new();
        [NotMapped]
        [JsonIgnore]
        public bool PlaysThisRound { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public string ClubsAsCSV { get { return string.Join(",", Clubs.ToList()); } }
        [NotMapped]
        [JsonIgnore]
        public int RankPosition { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int RankMatchCount { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int RankWins { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int PreviousRankPosition { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int PreviousRank { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string RankInfo { get; set; }


        public Player()
        {
        }
        public Player(string name) : base(name)
        {
        }
        public void Copy(Player p)
        {
            Name = p.Name;
            Displayname = p.Displayname;
            Dob = p.Dob;
            Rank = p.Rank;
            Identity = p.Identity;
        }
        public override string ToString()
        {
            return string.Format("{0} ({1})",Displayname,Rank);
        }
        public string RankString()
        {
            return string.Format("{0} ({1})", Rank, Rank-PreviousRank);

        }
        public string RankPositionString()
        {
            string result = Displayname;
            int rankchange = RankPosition - PreviousRankPosition;
            if(rankchange != 0)
                result += string.Format(" {0}",rankchange.ToString(" +0;-0;+00"));
            return result;

        }
        public string RankMatchString()
        {
            return string.Format("{0} ({1})", RankMatchCount, RankWins);

        }

        public override string ToJson()
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return JsonSerializer.Serialize<Player>(this, options);

        }
        public static new Player? FromJson(string jsonstring)
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<Player>(jsonstring,options);

        }


    }
}
