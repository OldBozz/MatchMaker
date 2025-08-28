using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MatchMakerLib.MatchMakerModel
{
    public class Tournament : MatchMakerElement
    {

        public DateTime PlayDate { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Finishdate { get; set; }
        public Club? Club { get; set; }
        public Mainevent? Mainevent { get; set; }
        [JsonIgnore] 
        public List<Player> Players { get; set; } = new();
        [JsonIgnore] 
        public List<Court> Courts { get; set; } = new();
        public List<Match> Matches { get; set; } = new();
        [JsonIgnore] 
        public List<Team> Teams { get; set; } = new();
        public int CurrentRound { get; set; } = 1;
        public int TotalRounds { get; set; } = 1;
        public int PointsToWin { get; set; } = 15;

        [NotMapped]
        [JsonIgnore]
        public Match FinalMatch { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string FinalMatchStr { get; set; }
        [NotMapped]
        [JsonIgnore]
        public List<PlayerStatus> PlayersStatus { get; set; } = new List<PlayerStatus>();
        [NotMapped]
        [JsonIgnore]
        public List<Team> ExistingTeams { get; set; } = new();
        [NotMapped]
        [JsonIgnore]
        public TournamentGenerator.ALGORITHM algorithm { get; set; } = TournamentGenerator.ALGORITHM.PLAYERSPLAYSWITHALLOTHERPLAYERS;
        [NotMapped]
        [JsonIgnore]
        public TournamentGenerator.RANKINGUSE rankinguse { get; set; } = TournamentGenerator.RANKINGUSE.BALANCED;
        [NotMapped]
        [JsonIgnore]
        public bool shuffle { get; set; } = true;
        [NotMapped]
        [JsonIgnore]
        public List<Match> unusedmatches { get; set; } = new();
        //private LinkedList<Team> TG { get; set; } = new();
        //private int NextCourtIdx = 1;

        public Tournament()
        {
        }

        public Tournament(string name) : base(name)
        {
        }
        public override string ToJson()
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return JsonSerializer.Serialize<Tournament>(this, options);

        }
        public static new Tournament? FromJson(string jsonstring)
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return JsonSerializer.Deserialize<Tournament>(jsonstring, options);
        }
        public int GetMaxRound()
        {
            int result = 0;
            foreach (Match match in Matches)
            {
                if (match.Round > result)
                    result = match.Round;
            }
            return result;
        }
        public List<Match> GetMatchesForCurrentRound()
        {
            return GetMatchesForRound(CurrentRound);
        }


        public List<Match> GetMatchesForRound(int round)
        {
            List<Match> roundmatches = new();
            foreach (Match match in Matches)
            {
                if (match.Round == round)
                {
                    roundmatches.Add(match);
                }
            }
            return roundmatches;
        }
        public List<Match> GetMatchesForRoundAndCourt(int round, Court court)
        {
            List<Match> roundmatches = new();
            foreach (Match match in Matches)
            {
                if (match.Round == round && match.Court == court)
                {
                    roundmatches.Add(match);
                }
            }
            return roundmatches;
        }
        public List<Court> GetCourtsForRound(int round)
        {
            List<Court> usedcourts = new();
            List<Match> roundmatches = GetMatchesForRound(round);
            foreach (Match match in roundmatches)
            {
                usedcourts.Add(match.Court);
            }
            return usedcourts;
        }
        public int GetNextFreeRound()
        {
            int result = 1;
            for (int round = 1; round <= (Matches.Count / Courts.Count); round++)
            {
                result = round+1;
                if (GetMatchesForRound(round).Count() < Courts.Count())
                {
                    result = round;
                    break;
                }
            }
            return result;
        }
        public Court GetFreeCourtInRound(int round)
        {
            List<Court> usedcourts = GetCourtsForRound(round);
            foreach (Court court in Courts)
            {
                if (!usedcourts.Contains(court))
                    return court;
            }
            return Courts.LastOrDefault();
        }
        public int GetFinishedMatchesCount()
        {
            int result = 0;
            foreach (Match match in Matches)
            {
                if (match.State == ElementStatus.FINISHED)
                    result++;

            }
            return result;
        }
        public void UpdateScore(List<Team> uniqueteams)
        {
            //List<Match> removelist = new List<Match>();
            foreach (Match match in Matches)
            {
                if(match != null)
                  match.UpdateScore();
                //else removelist.Add(match);

            }
            //Matches.RemoveAt
            UpdatePlayerStatus(uniqueteams);
        }
        public List<Player> GetPlayersSittingOutInCurrentRound()
        {
            List<Match> matchlist = GetMatchesForCurrentRound();
            List<Player> sitoutlist = new();
            //string matchstr = string.Join(",", GetMatchesForCurrentRound());
            foreach (Player player in Players)
            {
                bool playing = false;
                foreach (Match match in matchlist)
                {
                    playing = match.HasPlayer(player);
                    if (playing)
                    {
                        break;
                    }
                }
                if (!playing)
                    sitoutlist.Add(player);
            }
            return sitoutlist;
        }
        public List<Player> GetPlayersSittingOutInRound(int round)
        { 
            List<Match> matchlist = GetMatchesForRound(round);
            List<Player> sitoutlist = new();
            //string matchstr = string.Join(",", GetMatchesForCurrentRound());
            foreach (Player player in Players)
            {
                bool playing = false;
                foreach (Match match in matchlist)
                {
                    playing = match.HasPlayer(player);
                    if (playing)
                    {
                        break;
                    }
                }
                if (!playing)
                    sitoutlist.Add(player);
            }
            return sitoutlist;
        }

        private void UpdatePlayerStatus(List<Team> uniqueteams,bool sort = true)
        {
            int matchesfinished = 0;
            PlayersStatus.Clear();
            foreach (Player player in Players)
            {
                PlayerStatus status = new();
                status.Player = player;
                foreach (Match match in Matches)
                {
                    if (match != null && match.HasPlayer(player))
                    {
                        status.MatchesTotal++;
                        if (match.State == ElementStatus.FINISHED)
                        {
                            if (match.Team1.Players.Contains(player))
                            {
                                if (match.Team1Status == Team.TeamStatus.WINNER)
                                    status.MatchesWon++;
                                if (match.Team1Status == Team.TeamStatus.LOSER)
                                    status.MatchesLost++;
                                status.PointsWon += match.Team1Points;
                                status.PointsLost += match.Team2Points;
                                matchesfinished++;
                            }
                            if (match.Team2.Players.Contains(player))
                            {
                                if (match.Team2Status == Team.TeamStatus.WINNER)
                                    status.MatchesWon++;
                                if (match.Team2Status == Team.TeamStatus.LOSER)
                                    status.MatchesLost++;
                                status.PointsWon += match.Team2Points;
                                status.PointsLost += match.Team1Points;
                                matchesfinished++;

                            }
                        }
                    }
                }
                status.MatchesLeft = status.MatchesTotal - (status.MatchesWon + status.MatchesLost);
                status.PointsResult = status.PointsWon - status.PointsLost;
                PlayersStatus.Add(status);
            }
            if (sort)
                //PlayersStatus.Sort((x, y) => y.MatchesWon.CompareTo(x.MatchesWon));
                PlayersStatus = PlayersStatus.OrderByDescending(x => x.MatchesWon).ThenByDescending(x => x.PointsResult).ToList();
            FinalMatch = new Match();
            if(State == ElementStatus.READY)
            {
                FinalMatchStr = string.Format("Tournament ready");
            }
            if (State == ElementStatus.FINISHED)
            {
                FinalMatchStr = string.Format("Tournament finished. Winner: {0}", PlayersStatus.ElementAt(0).Player.Displayname);
            }
            if (State == ElementStatus.RUNNING)
            {
                if (matchesfinished >= Players.Count && PlayersStatus.Count > 0)
                {
                    List<Player> team1players = new List<Player>();
                    team1players.Add(PlayersStatus.ElementAt(0).Player);
                    team1players.Add(PlayersStatus.ElementAt(3).Player);
                    List<Player> team2players = new List<Player>();
                    team2players.Add(PlayersStatus.ElementAt(1).Player);
                    team2players.Add(PlayersStatus.ElementAt(2).Player);

                    FinalMatch.Team1 = TournamentGenerator.FindTeamWithPlayersFromTeamlist(uniqueteams, team1players);
                    FinalMatch.Team2 = TournamentGenerator.FindTeamWithPlayersFromTeamlist(uniqueteams, team2players);
                    FinalMatchStr = string.Format("{0}/{1} vs {2}/{3}", PlayersStatus.ElementAt(0).Player.Displayname, PlayersStatus.ElementAt(3).Player.Displayname, PlayersStatus.ElementAt(1).Player.Displayname, PlayersStatus.ElementAt(2).Player.Displayname);
                }
                else
                {
                    FinalMatchStr = string.Format("Insufficient data");

                }
            }


        }
        //public List<PlayerStatus> GetPlayersStatus(bool sort = true)
        //{
        //    List<PlayerStatus> result = new();
        //    foreach (Player player in Players)
        //    {
        //        PlayerStatus status = new();
        //        status.Player = player;
        //        foreach (Match match in Matches)
        //        {
        //            if (match.HasPlayer(player))
        //            {
        //                status.MatchesTotal++;
        //                if (match.Team1.Players.Contains(player))
        //                {
        //                    if (match.Team1Status == Team.TeamStatus.WINNER)
        //                        status.MatchesWon++;
        //                    if (match.Team1Status == Team.TeamStatus.LOSER)
        //                        status.MatchesLost++;
        //                    status.PointsWon+=match.Team1Points;
        //                    status.PointsLost += match.Team2Points;

        //                }
        //                if (match.Team2.Players.Contains(player))
        //                {
        //                    if (match.Team2Status == Team.TeamStatus.WINNER)
        //                        status.MatchesWon++;
        //                    if (match.Team2Status == Team.TeamStatus.LOSER)
        //                        status.MatchesLost++;
        //                    status.PointsWon += match.Team2Points;
        //                    status.PointsLost += match.Team1Points;
        //                }
        //            }
        //        }
        //        status.MatchesLeft = status.MatchesTotal - (status.MatchesWon + status.MatchesLost);
        //        status.PointsResult = status.PointsWon - status.PointsLost;
        //        result.Add(status);
        //    }
        //    if (sort)
        //        //PlayersStatus.Sort((x, y) => y.MatchesWon.CompareTo(x.MatchesWon));
        //        result = result.OrderByDescending(x => x.MatchesWon).ThenByDescending(x => x.PointsResult).ToList();
        //    FinalMatch = string.Format("{0}/{1} vs {2}/{3}",result.ElementAt(0).Player.Displayname, result.ElementAt(4).Player.Displayname, result.ElementAt(2).Player.Displayname, result.ElementAt(3).Player.Displayname);
        //    return result;

        //}

        public void Copy(Tournament intour)
        {
            Name = intour.Name;
            //TotalRounds = intour.TotalRounds;
            foreach (Player player in intour.Players)
            {
                Players.Add(player);
            }
            foreach (Court court in intour.Courts)
            {
                Courts.Add(court);
            }
            foreach (Team team in intour.Teams)
            {
                Teams.Add(team);
            }
            foreach (Match match in intour.Matches)
            {
                Matches.Add(match);
            }

        }

        public void ParsePlayers(string playerslist, bool clear = true)
        {
            if (clear)
            {
                Players.Clear();
            }
            if (playerslist != null)
            {
                foreach (var player in playerslist.Split(',').ToList())
                {
                    if (player.Length > 2)
                        Players.Add(new Player(player.TrimStart()));

                }
            }
        }
        public void ParseCourts(string courtslist, bool clear = true)
        {
            if (clear)
            {
                Courts.Clear();
            }
            if (courtslist != null)
            {
                foreach (var court in courtslist.Split(',').ToList())
                {
                    Courts.Add(new Court(court));

                }
            }
        }


        public int GetMatchCountForPlayer(Player p)
        {
            int count = 0;
            foreach (Match m in Matches)
            {
                if (m.HasPlayer(p))
                    count++;
            }
            return count;
        }

        private string GetMatchInfoForPlayer(Player p)
        {
            string tmp = "";
            int matches = 0;
            foreach (Match m in Matches)
            {
                if (m.HasPlayer(p))
                {
                    matches++;
                    tmp += string.Format("\n\t Round {0}: {1}", m.Round, m);
                }
            }
            return string.Format("{0} matches: {1}", matches, tmp);
        }
        private string GetOpponents(Player p)
        {
            List<Player> oppónents = new List<Player>();
            foreach (Match m in Matches)
            {
                List<Player> list = new List<Player>();
                if (m.Team1.Member(p))
                {
                    foreach (Player p2 in m.Team2.Players)
                    {
                        if (p2 != p && !oppónents.Contains(p2))
                        {
                            oppónents.Add(p2);
                        }
                    }
                }
                if (m.Team2.Member(p))
                {
                    foreach (Player p2 in m.Team1.Players)
                    {
                        if (p2 != p && !oppónents.Contains(p2))
                        {
                            oppónents.Add(p2);
                        }
                    }
                }
            }
            oppónents.Sort((x, y) => x.Name.CompareTo(y.Name));
            return string.Format("{0} others: {1}", oppónents.Count, string.Join(",", oppónents));
        }
        private string GetTeammates(Player p)
        {
            List<Player> mates = new List<Player>();
            foreach (Match m in Matches)
            {
                List<Player> list = new List<Player>();
                if (m.Team1.Member(p))
                {
                    foreach (Player p2 in m.Team1.Players)
                    {
                        if (p2 != p && !mates.Contains(p2))
                        {
                            mates.Add(p2);
                        }
                    }
                }
                if (m.Team2.Member(p))
                {
                    foreach (Player p2 in m.Team2.Players)
                    {
                        if (p2 != p && !mates.Contains(p2))
                        {
                            mates.Add(p2);
                        }
                    }
                }
            }
            mates.Sort((x, y) => x.Name.CompareTo(y.Name));
            return string.Format("{0} others: {1}", mates.Count, string.Join(",", mates));
        }

        private string GetMatchInfoForTeam(Team t)
        {
            string result = "";
            foreach (Match m in Matches)
            {
                if (m.HasTeam(t))
                    result += string.Format(" plays round {0}: {1}", m.Round, m);
            }
            return result;
        }
        public override string ToString()
        {
            return Name;
            //return string.Format("{0} {1} courts,{2} players", Name, Courts.Count, Players.Count);

        }

        public string StateInfo()
        {
            string result = "";
            result += string.Format("State {0}{1}{2}", State, Startdate != null ? ", Started " + Startdate : "", Finishdate != null ? ", Finished " + Finishdate : "");
            //result += string.Format("State {0}", State);
            return result;
        }
        public string DetailInfo()
        {
            string result = "";
            result += string.Format("{0} Players, {2} Matches, {1} Courts,  {3} Rounds, {4} points to win", Players.Count, Courts.Count, Matches.Count, GetMaxRound(), PointsToWin);
            return result;
        }



        public string ToInfoHTML()
        {
            string result = "";
            string head = "<head>\r\n<meta name = \"viewport\" content=\"width=device-width6, initial-scale=1\">\r\n<style>\r\nul, #myUL {\r\n  list-style-type: none;\r\n}\r\n\r\n# myUL {\r\n    margin: 0;\r\n  padding: 0;\r\n}\r\n\r\n.caret {\r\n  cursor: pointer;\r\n-webkit - user - select: none; /* Safari 3.1+ */\r\n-moz - user - select: none; /* Firefox 2+ */\r\n-ms - user - select: none; /* IE 10+ */\r\nuser - select: none;\r\n}\r\n\r\n.caret::before {\r\ncontent: \"\\25B6\";\r\ncolor: black;\r\ndisplay: inline - block;\r\n    margin - right: 6px;\r\n}\r\n\r\n.caret - down::before {\r\n    -ms - transform: rotate(90deg); /* IE 9 */\r\n    -webkit - transform: rotate(90deg); /* Safari */'\r\n  transform: rotate(90deg);\r\n}\r\n\r\n.nested {\r\n  display: none;\r\n}\r\n\r\n.active {\r\n  display: block;\r\n}\r\n</ style ></ head >";
            string body = "< body >\r\n\r\n< h2 > Tree View </ h2 >\r\n< p > A tree view represents a hierarchical view of information, where each item can have a number of subitems.</p>\r\n<p>Click on the arrow(s) to open or close the tree branches.</p>\r\n\r\n<ul id=\"myUL\">\r\n  <li><span class= \"caret\" > Beverages </ span >\r\n    < ul class= \"nested\" >\r\n      < li > Water </ li >\r\n      < li > Coffee </ li >\r\n      < li >< span class= \"caret\" > Tea </ span >\r\n        < ul class= \"nested\" >\r\n          < li > Black Tea </ li >\r\n          < li > White Tea </ li >\r\n          < li >< span class= \"caret\" > Green Tea </ span >\r\n            < ul class= \"nested\" >\r\n              < li > Sencha </ li >\r\n              < li > Gyokuro </ li >\r\n              < li > Matcha </ li >\r\n              < li > Pi Lo Chun</li>\r\n            </ul>\r\n          </li>\r\n        </ul>\r\n      </li>  \r\n    </ul>\r\n  </li>\r\n</ul>\r\n\r\n<script>\r\nvar toggler = document.getElementsByClassName(\"caret\");\r\nvar i;\r\n\r\nfor (i = 0; i < toggler.length; i++)\r\n{\r\n    toggler[i].addEventListener(\"click\", function() {\r\n        this.parentElement.querySelector(\".nested\").classList.toggle(\"active\");\r\n        this.classList.toggle(\"caret-down\");\r\n    });\r\n}\r\n</ script >\r\n\r\n</ body >\r\n";
            result += string.Format("<html>\r\n{0}{1}</ html >", head, body);
            return result.ReplaceLineEndings();

        }

        public string ToInfoString()
        {
            string result = "";
            result += string.Format("{0}\n{1} courts,{2} players\n", Name, Courts.Count, Players.Count);
            result += "\nSitting out:\n";
            for (int r = 1; r < GetMaxRound(); r++)
            { 
                result += string.Format("\tRound {0}: {1}\n",r, string.Join(",", GetPlayersSittingOutInRound(r)));
            }
            result += "\n"+Matches.Count + " Matches:\n";
            foreach (Match m in Matches)
            {
                result += string.Format("\tRound {0} Court {1}: {2}\n", m.Round, m.Court, m.ToString());
            }
            result += "\n Team mates:\n";
            foreach (Player p in Players)
            {
                result += "\t" + p.ToString() + " plays with " + GetTeammates(p) + "\n\n";
            }
            result += "\n\n Opponents:\n";
            foreach (Player p in Players)
            {
                result += "\t" + p.ToString() + " plays against " + GetOpponents(p) + "\n\n";
            }
            result += "\n\n Unused matches:\n";
            foreach (Match m in unusedmatches)
            {
                result += string.Format("\t{0}\n", m.ToString());
            }
            return result.ReplaceLineEndings();
        }

        public string ToLongString()
        {
            string result = "";
            result += string.Format("Tournament planner - {0}\n{1} courts,{2} players\n", Name, Courts.Count, Players.Count);
            //result += String.format("Expected %d teams\n",binomcoef(_participants.size(),2));

            result += Matches.Count + " Matches:\n";
            foreach (Match m in Matches)
            {
                result += string.Format("\tCourt {0}: {1}\n", m.Court, m.ToString());
            }
            /*result += _courts.size() +" Courts:\n";
			for(Court c : _courts){
				result += "\t"+c.toString()+"\n";
			}*/
            result += Teams.Count + " Teams\n";
            //foreach (Team t in Teams)
            //{
            //    if(t != null)
            //        result += "\t" + t.ToString() + GetMatchInfoForTeam(t) + "\n"; ;
            //}
            //result += Players.Count + " Players:\n";
            //foreach (Player p in Players)
            //{
            //    result += "\t" + p.ToString() + " " + GetMatchInfoForPlayer(p) + "\n\n";
            //}
            result += "Players teammates\n";
            foreach (Player p in Players)
            {
                result += "\t" + p.ToString() + " plays with " + GetTeammates(p) + "\n";
            }
            result += "Players opponents\n";
            foreach (Player p in Players)
            {
                result += "\t" + p.ToString() + " plays against " + GetOpponents(p) + "\n";
            }
            //result += string.Format("Tip en {0}'er:\n", Matches.Count);
            //foreach (Match m in Matches)
            //{
            //	result += string.Format("\t{0}\t1\tX\t2\n", m.ToString());
            //}
            return result.ReplaceLineEndings();
        }

    }
}
