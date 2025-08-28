using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatchMakerLib.MatchMakerModel
{
    public class Tournament : MatchMakerElement
    {
        public DateTime? Startdate { get; set; }
        public DateTime? Finishdate { get; set; }

        public Mainevent Mainevent { get; set; } = Mainevent.NOEVENT;
        public Club Club { get; set; } = Club.NOCLUB;
        public DateTime PlayDate { get; set; }
        public List<Player> Players { get; set; } = new();
        public List<Court> Courts { get; set; } = new();
        public List<Match> Matches { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
        public int CurrentRound { get; set; } = 1;
        public int TotalRounds { get; set; } = 1;

        public int PointsToWin { get; set; } = 15;

        [NotMapped]
        protected List<PlayerStatus> PlayersStatus { get; set; } = new();
        private LinkedList<Team> TG { get; set; } = new();

        private int NextCourtIdx { get; set; } = 0;
        private bool morecombinationspossible = true;
        //static private readonly Team NOTEAM = new ("");
        //static public readonly Match NOMATCH = new();
        public Tournament()
        {
        }

        public Tournament(string name) : base(name)
        {
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
        public void UpdateScore()
        {
            foreach (Match match in Matches)
            {
                match.UpdateScore();

            }
        }
        public List<Player> GetPlayersSittingOutInCurrentRound()
        {
            //List<Match> matchlist = GetMatchesForCurrentRound();
            List<Player> sitoutlist = new();
            string matchstr = string.Join(",", GetMatchesForCurrentRound());
            foreach (Player player in Players)
            {
                if (!matchstr.Contains(player.Displayname))
                    sitoutlist.Add(player);
                //foreach (Match match in matchlist)
                //            {
                //                if (!(match.Team1.Name.Contains(player.Displayname) && match.Team2.Name.Contains(player.Displayname)))
                //	{
                //		sitoutlist.Add(player);
                //		break;
                //	}
                //} 
            }
            return sitoutlist;
        }

        public List<PlayerStatus> GetPlayersStatus(bool sort = true)
        {
            PlayersStatus.Clear();
            foreach (Player player in Players)
            {
                PlayerStatus status = new();
                status.Player = player;
                foreach (Match match in Matches)
                {
                    if (match.Name.Contains(player.Displayname))
                    {
                        status.MatchesTotal++;
                        if (match.Team1.Name.Contains(player.Displayname))
                        {
                            if (match.Team1Status == Team.TeamStatus.WINNER)
                                status.MatchesWon++;
                            if (match.Team1Status == Team.TeamStatus.LOSER)
                                status.MatchesLost++;

                        }
                        if (match.Team2.Name.Contains(player.Displayname))
                        {
                            if (match.Team2Status == Team.TeamStatus.WINNER)
                                status.MatchesWon++;
                            if (match.Team2Status == Team.TeamStatus.LOSER)
                                status.MatchesLost++;

                        }
                        //if (match.WinningTeam() != Team.NOTEAM)
                        //{
                        //    _ = match.PlayerWon(player) ? status.MatchesWon++ : status.MatchesLost++;

                        //}

                    }
                }
                status.MatchesLeft = status.MatchesTotal - (status.MatchesWon + status.MatchesLost);
                PlayersStatus.Add(status);
            }
            if (sort)
                PlayersStatus.Sort((x, y) => y.MatchesWon.CompareTo(x.MatchesWon));

            return PlayersStatus;

        }
        public string ToJson()
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                },
            };
            return JsonConvert.SerializeObject(this, settings);

        }
        //     public List<PlayerStatus> GetPlayerStatusOrderedByWins()
        //     {
        //         List<Player> orderedplayers = new();
        //foreach (Player player in Players)
        //{
        //	player.MatchesWon = 0;
        //             player.MatchesLost = 0;
        //             orderedplayers.Add(player);	

        //             foreach (Match match in Matches)
        //	{
        //		//match.UpdatePlayerStatus(player);
        //	}
        //         }
        //         orderedplayers.Sort((x, y) => x.MatchesLost.CompareTo(y.MatchesLost));

        //         //return orderedplayers.OrderBy(w => w.MatchesWon).ToList(); ;
        //         return orderedplayers;
        //     }

        public void Copy(Tournament intour)
        {
            Name = intour.Name;
            TotalRounds = intour.TotalRounds;
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

        public void GenerateNew(bool shuffle = true)
        {
            NextCourtIdx = 0;
            TotalRounds = 1;
            TG.Clear();
            Matches.Clear();

            //         foreach (var player in players)
            //{
            //	Players.Add(new Player(player));

            //}
            //foreach (var court in courts)
            //{
            //	Courts.Add(new Court(court));

            //}
            GenerateTeams(shuffle);
            int attempts = 0;
            while (attempts < 100)
            {
                List<Match> round = GenerateRound();
                foreach (Match m in round)
                {
                    Matches.Add(m);
                }
                attempts++;
            }
            //Match match = GenerateMatch();
            //while (match != Match.NOMATCH)
            //{
            //	Matches.Add(match);
            //	match = GenerateMatch();
            //}

        }
        public void ClearAll()
        {
            Players.Clear();
            Courts.Clear();
            Teams.Clear();
            Matches.Clear();
        }

        protected void GenerateTeams(bool shuffle)
        {
            List<Player> shuffledplayers = Players;
            if (shuffle)
                Utilities.Shuffle(shuffledplayers);

            for (int i = 0; i < shuffledplayers.Count; i++)
            {
                for (int j = i + 1; j < Players.Count; j++)
                {
                    Player player1 = shuffledplayers[i];
                    Player player2 = shuffledplayers[j];
                    Team t = new(player1.Displayname + "/" + player2.Displayname);
                    t.Players.Add(player1);
                    t.Players.Add(player2);
                    Teams.Add(t);
                    TG.AddLast(t);

                }
            }

        }
        public List<Match> GenerateRound()
        {
            List<Match> result = new();
            int currentround = TotalRounds;
            List<Team> roundteams = new();
            while (currentround == TotalRounds )
            {
                Match m = GenerateMatch(roundteams);
                if (m != Match.NOMATCH)
                {
                    result.Add(m);
                    if (result.Count == Courts.Count)
                        TotalRounds++;
                }
                else
                {
                    foreach (Match m2 in result)
                    {
                        m2.Team1.HasPlayed = false;
                        m2.Team2.HasPlayed = false;
                    }
                    result.Clear();
                    break;
                }
            }

            //if (result.Count == 0)
            //    TotalRounds--;
            return result;


        }
        public Match GenerateMatch(List<Team> roundteams)
        {
            Match m = Match.NOMATCH;
            Court c = getNextCourt();
            Team t1 = GetNextTeamWithoutMembersFromRoundForTeam(Team.NOTEAM,roundteams); 
            if (t1 != Team.NOTEAM)
            {
                t1.HasPlayed = true;
                roundteams.Add(t1);
                Team t2 = GetNextTeamWithoutMembersFromRoundForTeam(t1,roundteams);
                if (t2 != Team.NOTEAM)
                {
                    t2.HasPlayed= true;
                    roundteams.Add(t2);
                    m = new Match(TotalRounds, c, t1, t2);
                    m.Players.AddRange(t1.Players);
                    m.Players.AddRange(t2.Players);
                }
                else
                {

                    //t2 = new Team("X1/X2");
                    //t2.Players.Add(new Player("X1"));
                    //t2.Players.Add(new Player("X2"));
                    //Teams.Add(t2);
                    //t1.HasPlayed = false;
                    t1.Matchingattemps++;
                    t2 = Team.NOTEAM;
                    TG.Remove(t1);
                    TG.AddLast(t1);
                }
                //if (c != null)

            }
            else
            {
                morecombinationspossible = false;
            }
            return m;
        }

        private Team GetNextTeamWithoutMembersFromRoundForTeam(Team forteam,List<Team> roundteams)
        {
            int minmatchno = int.MaxValue;
            Team bestTeam = Team.NOTEAM;
            string info = "";
            foreach (Team t in TG)
            {
                if (t.Matchingattemps > 10)
                    continue;

                if (t.HasPlayed)
                {
                    //info += string.Format("{0} not used to match because {0} has playedNL", t.Name);
                    continue;
                }
                bool candidate = true;
                if (t.ShareMembers(forteam))
                    continue;
                foreach (Team other in roundteams)
                {
                    if (t.ShareMembers(other))
                    {
                        candidate = false;
                        //info += string.Format("{0} not used to match {1} because they share members with team {2} in roundteams {3}\n", t.Name,forteam, other.Name,string.Join(",",roundteams));
                        break;
                    }
                    //if (playsThisRound(t))
                    //{
                    //    info += string.Format("{0} not used to match {1} because {0} plays this round\n", t.Name, other.Name);
                    //    playsThisRound(t);
                    //    break;
                    //}
                }
                if (!candidate)
                    continue;
                int pmcount = t.GetMatchCountForPlayers(this);
                if (pmcount < minmatchno)
                {
                    bestTeam = t;
                    minmatchno = pmcount;
                }
            }
            //if (bestTeam == Team.NOTEAM)
            //    Console.WriteLine(info);

            return bestTeam;
        }
        private Team getNextTeam()
        {
            int minmatchno = int.MaxValue;
            Team bestTeam = Team.NOTEAM;
            foreach (Team t in TG)
            {
                if (t.Matchingattemps > 10)
                    continue;
                if (!t.HasPlayed && !playsThisRound(t))
                {
                    int pmcount = t.GetMatchCountForPlayers(this);
                    if (pmcount < minmatchno)
                    {
                        bestTeam = t;
                        minmatchno = pmcount;
                    }
                }
            }
            return bestTeam;
        }
        private Court getNextCourt()
        {
            if (NextCourtIdx == Courts.Count)
            {
                NextCourtIdx = 0;
            }
            return Courts[NextCourtIdx++];
        }

        private bool playsThisRound(Team t)
        {
            foreach (Match m in Matches)
            {
                foreach (Player p in t.Players)
                {
                    if (m.HasPlayer(p) && m.Round == TotalRounds)
                        return true;
                }
            }
            return false;
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
                    tmp += string.Format("\n\t Round {0}: {1}", m.Round, m.Name);
                }
            }
            return string.Format("{0} matches: {1}", matches, tmp);
        }

        private string GetMatchInfoForTeam(Team t)
        {
            string result = "";
            foreach (Match m in Matches)
            {
                if (m.HasTeam(t))
                    result += string.Format(" plays round {0}: {1}", m.Round, m.Name);
            }
            return result;
        }
        public override string ToString()
        {
            return string.Format("{0} {1} courts,{2} players", Name, Courts.Count, Players.Count);
        }


        public string ToLongString()
        {
            string result = "";
            result += string.Format("Tournament planner - {0}\n{1} courts,{2} players\n", Name, Courts.Count, Players.Count);
            //result += String.format("Expected %d teams\n",binomcoef(_participants.size(),2));

            result += Matches.Count + " Matches:\n";
            foreach (Match m in Matches)
            {
                result += string.Format("\t{0}: {1}\n", m.Court, m.ToString());
            }
            /*result += _courts.size() +" Courts:\n";
			for(Court c : _courts){
				result += "\t"+c.toString()+"\n";
			}*/
            result += Teams.Count + " Teams:\n";
            foreach (Team t in Teams)
            {
                result += "\t" + t.ToString() + GetMatchInfoForTeam(t) + "\n"; ;
            }
            result += Players.Count + " Players:\n";
            foreach (Player p in Players)
            {
                result += "\t" + p.ToString() + " " + GetMatchInfoForPlayer(p) + ":\n"; ;
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
