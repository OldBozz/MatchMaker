using MatchMakerLib.MatchMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MatchMakerLib.MatchMakerModel.Team;

namespace MatchMakerLib
{
    public class MudMatch : MatchMakerElement
    {
        public int Round { get; set; }
        public Court? Court { get; set; }
        public string Team1Name { get; set; } = "No team";
        public string Team2Name { get; set; } = "No team";
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public int Team1Points { get; set; } = 0;
        public int Team2Points { get; set; } = 0;
        public TeamStatus Team1Status { get; set; } = TeamStatus.EVEN;
        public TeamStatus Team2Status { get; set; } = TeamStatus.EVEN;
        public bool Virtual { get; set; } = false;

        public Match? matchparent;

        public MudMatch() { }

        public MudMatch(Match match)
        {
            Copy(match);
        }
        public void Copy(Match match)
        {
            Id = match.Id;
            Name = match.Name;
            Round = match.Round;
            Court = match.Court;
            Team1Name = match.Team1.ToString();
            Team2Name = match.Team2.ToString();
            Team1 = match.Team1;
            Team2 = match.Team2;
            Team1Points = match.Team1Points;
            Team2Points = match.Team2Points;
            Team1Status = match.Team1Status;
            Team2Status = match.Team2Status;
            matchparent = match;
        }

        public void TransferToParent()
        {
            if (matchparent != null)
            {
                matchparent.Round = Round;
                matchparent.Court = Court;
                matchparent.Team1 = Team1;
                matchparent.Team2 = Team2;
                matchparent.Team1Points = Team1Points;
                matchparent.Team2Points = Team2Points;
                matchparent.Team1Status = Team1Status;
                matchparent.Team2Status = Team2Status;
            }
        }
        public void TransferFromParent()
        {
            if (matchparent != null)
            {
                Copy(matchparent);
            }
        }
        public static void TransferMudData(List<MudMatch> mudmatches)
        {
            foreach (MudMatch m in mudmatches)
            {
                m.TransferToParent();
            }

        }
        public static void UpdateMudData(Tournament Tournament, ref List<MudMatch> tournamentmudmatches)
        {
            tournamentmudmatches.Clear();
            List<Court> courts = new();
            courts.AddRange(Tournament.Courts.OrderBy(x => x.Name).ToList());

            for (int round = 1; round <= Tournament.GetMaxRound(); round++)
            {
                int courtidx = 0;
                while (courtidx < courts.Count)
                {
                    Court mudcourt = courts.ElementAt(courtidx++);

                    MudMatch? mudmatch;
                    List<Match> roundcourtmatches = Tournament.GetMatchesForRoundAndCourt(round, mudcourt);
                    if (roundcourtmatches.Count > 0)
                    {
                        foreach (Match m in roundcourtmatches)
                        {
                            mudmatch = new MudMatch(m);
                            tournamentmudmatches.Add(mudmatch);
                        }
                    }
                    else
                    {
                        mudmatch = new MudMatch();
                        mudmatch.Virtual = true;
                        mudmatch.Round = round;
                        mudmatch.Court = mudcourt;
                        tournamentmudmatches.Add(mudmatch);
                    }
                }
            }
        }


        public static void UpdateMudData(Tournament Tournament, ref List<MudMatch> tournamentmudmatches, ref List<MudMatch> currentroundmudmatches)
        {
            currentroundmudmatches.Clear();
            tournamentmudmatches.Clear();
            List<Court> courts = new();
            courts.AddRange(Tournament.Courts.OrderBy(x => x.Name).ToList());

            for (int round = 1; round <= Tournament.GetMaxRound(); round++)
            {
                int courtidx = 0;
                while (courtidx < courts.Count)
                {
                    Court mudcourt = courts.ElementAt(courtidx++);

                    MudMatch? mudmatch;
                    List<Match> roundcourtmatches = Tournament.GetMatchesForRoundAndCourt(round, mudcourt);
                    if (roundcourtmatches.Count > 0)
                    {
                        foreach (Match m in roundcourtmatches)
                        {
                            mudmatch = new MudMatch(m);
                            tournamentmudmatches.Add(mudmatch);
                            if (mudmatch.Round == Tournament.CurrentRound)
                                currentroundmudmatches.Add(mudmatch);
                        }
                    }
                    else
                    {
                        mudmatch = new MudMatch();
                        mudmatch.Virtual = true;
                        mudmatch.Round = round;
                        mudmatch.Court = mudcourt;
                        tournamentmudmatches.Add(mudmatch);
                        if (mudmatch.Round == Tournament.CurrentRound)
                            currentroundmudmatches.Add(mudmatch);

                    }
                }
            }
        }

    }
}

