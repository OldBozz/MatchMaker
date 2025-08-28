using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Match = MatchMakerLib.MatchMakerModel.Match;

namespace MatchMakerLib.MatchMakerModel
{
    public static class TournamentGenerator
    {
        public enum ALGORITHM { NONE, PLAYERSPLAYSWITHALLOTHERPLAYERS, PLAYERSPLAYSAGAINSTALLOTHERPLAYERS };
        public enum RANKINGUSE { NONE, BALANCED, UNBALANCED };
        public static async Task GenerateMatchesAsync(Tournament tournament)
        {
            switch (tournament.algorithm)
            {
                case ALGORITHM.PLAYERSPLAYSWITHALLOTHERPLAYERS:
                    await Task.Run(() => GenrerateAllPlayersWithAllOtherPLayersMatches(tournament));
                    break;
                case ALGORITHM.PLAYERSPLAYSAGAINSTALLOTHERPLAYERS:
                    await Task.Run(() => GenrerateAllPlayersAgainstAllOtherPLayersMatches(tournament)); ;
                    break;

                default: break;
            }

        }

        private static void GenrerateAllPlayersWithAllOtherPLayersMatches(Tournament tournament)
        {
            CombineAllPossiblePlayerTeamsAgainstRandomTeams(tournament);
        }
        private static void GenrerateAllPlayersAgainstAllOtherPLayersMatches(Tournament tournament)
        {
            CombineAllPlayersAgaintAllPlayersRandomTeams(tournament);

        }

        public static void CombineAllPlayersAgaintAllPlayersRandomTeams(Tournament tournament, bool shuffle = false)
        {
            int CurrentRound = 1;
            //List<Team> globalteams = tournament.ExistingTeams;
            List<Team> allteams = new();
            List<Team> usedteams = new();
            List<Team> existingteams = new();
            List<Player> players = tournament.Players;
            List<Match> existingmatches = tournament.Matches;
            List<Match> matches = new();
            List<Match> matchpool = new();

            if (shuffle)
                players.Shuffle();
            existingteams.AddRange(tournament.ExistingTeams);
            //existingteams.AddRange(tournament.Teams);
            allteams = GenerateTeams(existingteams, players);

            int existingroundidx = 1;
            List<Match> matchesforround = GetMatchesForRoundInList(existingmatches, existingroundidx++);
            while (matchesforround.Count > 0)
            {
                bool roundused = false;
                foreach (Match roundmatch in matchesforround)
                {
                    if (roundmatch.State == MatchMakerElement.ElementStatus.FINISHED)
                    {
                        roundmatch.Round = CurrentRound;
                        matches.Add(roundmatch);
                        roundused = true;
                        break;

                    }
                }
                if (roundused)
                    CurrentRound++;
                matchesforround = GetMatchesForRoundInList(existingmatches, existingroundidx++);
            }
            matchpool = GenerateMatchPool(existingmatches, allteams);
            //matchpool.Shuffle();
            Match? match = GetNextMatch(matchpool, matches, CurrentRound, tournament.rankinguse);
            bool toofewplayersforround = tournament.Courts.Count * 4 > players.Count;
            //int maxrounds = (tournament.Courts.Count * 4) / players.Count;
            List<Match> roundmatches = new List<Match>();
            int attempts = 0;
            while (GenerateRound(matchpool, tournament, CurrentRound, tournament.Courts.Count, ref attempts))
            {
                CurrentRound++;
            }
            //while (match != null && !toofewplayersforround)
            //{
            //    match.Round = CurrentRound;
            //    match.Court = getNextCourt(ref CourtIdx, ref CurrentRound, tournament.Courts);
            //    matches.Add(match);
            //    match.Team1.HasPlayed = true;
            //    match.Team2.HasPlayed = true;
            //    usedteams.Add(match.Team1);
            //    usedteams.Add(match.Team2);
            //    match = GetNextMatch(matchpool, matches, CurrentRound);
            //}
            //tournamentTotalRounds = CurrentRound;
            //tournament.Teams = usedteams;
            //tournament.Matches.AddRange(matches);

        }
        public static void CombineAllPossiblePlayerTeamsAgainstRandomTeams(Tournament tournament)
        {
            int currentround = 1;
            List<Team> allteams = new();
            List<Player> players = tournament.Players;
            List<Match> matches = new();
            List<Match> matchpool = new();
            List<Team> existingteams = new();

            if (tournament.shuffle)
                players.Shuffle();
            existingteams.AddRange(tournament.ExistingTeams);
            //existingteams.AddRange(tournament.Teams);
            allteams = GenerateTeams(existingteams, players);
            //tournament.ExistingTeams.Clear();
            //tournament.ExistingTeams.AddRange(existingteams);
            //int existingroundidx = 1;
            matchpool = GenerateMatchPool(allteams);
            int attempts = 0;
            tournament.Matches.Clear();
            while (GenerateRoundAllPlayerTeams(matchpool, tournament, currentround, tournament.Courts.Count, ref attempts))
            {
                attempts = 0;
                currentround++;
            }
            tournament.unusedmatches = matchpool;

        }
        private static bool GenerateRound(List<Match> matchpool, Tournament tournament, int round, int roundsize, ref int attempts)
        {
            //Match firstinpool = matchpool.First();
            if (attempts++ > 200) return false;
            List<Match> possibleroundmatches = new List<Match>();
            matchpool.Shuffle();
            for (int r = 0; r < roundsize; r++)
            {
                Match? match = GetNextMatch(matchpool, tournament.Matches, round, tournament.rankinguse);
                if (match != null && match.Team1 != null && match.Team2 != null)
                {
                    match.Team1.HasPlayed = true;
                    match.Team2.HasPlayed = true;
                    foreach (Player p in match.Players)
                    {
                        p.PlaysThisRound = true;
                    }
                    possibleroundmatches.Add(match);
                }
            }
            if (possibleroundmatches.Count == roundsize)
            {
                int courtidx = 0;
                foreach (Match match in possibleroundmatches)
                {
                    match.Court = tournament.Courts[courtidx++];
                    match.Round = round;

                    tournament.Teams.Add(match.Team1);
                    tournament.Teams.Add(match.Team2);
                    foreach (Player p in match.Players)
                    {
                        p.PlaysThisRound = false;
                    }
                    tournament.Matches.Add(match);
                    matchpool.Remove(match);
                }
                return true;
            }
            else
            {
                //Match fm = matchpool.Last();
                //matchpool.MoveToFirst(fm);

                foreach (Match match in possibleroundmatches)
                {
                    if (match != null && match.Team1 != null && match.Team2 != null)
                    {
                        match.Team1.HasPlayed = false;
                        foreach (Player p in match.Players)
                        {
                            p.PlaysThisRound = false;
                        }
                    }
                }
                return GenerateRound(matchpool, tournament, round, roundsize, ref attempts);
            }
        }
        private static bool GenerateRoundAllPlayerTeams(List<Match> matchpool, Tournament tournament, int round, int roundsize, ref int attempts)
        {
            //Match firstinpool = matchpool.First();
            if (attempts++ > 200)
                return false;
            List<Match> possibleroundmatches = new List<Match>();
            if ( attempts > 0)
                matchpool.Shuffle();
            for (int r = 0; r < roundsize; r++)
            {
                Match? match = GetNextMatch(matchpool, tournament.Matches, round, tournament.rankinguse);
                if (match != null && match.Team1 != null && match.Team2 != null)
                {
                    match.Team1.HasPlayed = true;
                    match.Team2.HasPlayed = true;
                    foreach (Player p in match.Players)
                    {
                        p.PlaysThisRound = true;
                    }
                    possibleroundmatches.Add(match);
                }
            }
            if (attempts > 199 && possibleroundmatches.Count < roundsize)
            {
                List<Player> players = GetPlayersWithFewerMatchesThanNeeded(tournament);
                List<Player> removelist = new();
                foreach (Player p in players){
                    if(p.PlaysThisRound)
                        removelist.Add(p);  
                }
                foreach (Player p in removelist)
                {
                    players.Remove(p);
                }
                if (players.Count > 1)
                {
                    Match emptymatch = new Match();
                    List<Player> teamplayers = new();
                    teamplayers.Add(players[0]);
                    teamplayers.Add(players[1]);
                    Team? team = FindTeamWithPlayersFromTeamlist(tournament.ExistingTeams, teamplayers);
                    if (team != null)
                    {
                        emptymatch.Team1 = team;
                    }
                    else
                    {
                        emptymatch.Team1 = new Team();
                        emptymatch.Team1.Players.Add(players[0]);
                        emptymatch.Team1.Players.Add(players[1]);
                    }
                    emptymatch.Team2 = new Team();
                    if (players.Count > 2)
                        emptymatch.Team2.Players.Add(players[2]);
                    if (players.Count > 3)
                        emptymatch.Team2.Players.Add(players[3]);

                    possibleroundmatches.Add(emptymatch);
                    emptymatch.Team1.HasPlayed = true;
                    emptymatch.Team2.HasPlayed = true;
                    foreach (Player p in emptymatch.Players)
                    {
                        p.PlaysThisRound = true;
                    }
                    //Console.WriteLine("Added "+emptymatch);

                }

            }
            if (possibleroundmatches.Count == roundsize || (attempts > 199 && possibleroundmatches.Count > 0))
            {
                int courtidx = 0;
                foreach (Match match in possibleroundmatches)
                {
                    match.Court = tournament.Courts[courtidx++];
                    match.Round = round;

                    tournament.Teams.Add(match.Team1);
                    tournament.Teams.Add(match.Team2);
                    foreach (Player p in match.Players)
                    {
                        p.PlaysThisRound = false;
                    }
                    tournament.Matches.Add(match);
                    //matchpool.Remove(match);
                    //List<Match> removelist = new List<Match>();
                    //foreach (Match m in matchpool)
                    //{
                    //    if (m.HasTeam(match.Team1) || m.HasTeam(match.Team2))
                    //    {
                    //        removelist.Add(m);
                    //    }
                    //}
                    //foreach (Match m in removelist)
                    //{
                    //    matchpool.Remove(m);
                    //}
                }
                return true;
            }
            else
            {
                //Match fm = matchpool.Last();
                //matchpool.MoveToFirst(fm);

                foreach (Match match in possibleroundmatches)
                {
                    if (match != null && match.Team1 != null && match.Team2 != null)
                    {
                        match.Team1.HasPlayed = false;
                        match.Team2.HasPlayed = false;
                        foreach (Player p in match.Players)
                        {
                            p.PlaysThisRound = false;
                        }
                    }
                }
                return GenerateRoundAllPlayerTeams(matchpool, tournament, round, roundsize, ref attempts);
            }
        }

        private static List<Player> GetPlayersWithFewerMatchesThanNeeded(Tournament tournament)
        {
            List<Player> result = new List<Player>();
            foreach (Player p in tournament.Players)
            {
                int mcount = GetMatchCountForPlayer(tournament.Matches, p);
                //if(mcount >= tournament.Players.Count)
                //    Console.WriteLine("Player " + p + " matchcount=" + mcount);

                if (mcount < tournament.Players.Count - 1)
                {
                    //Console.WriteLine("Player " + p + " matchcount=" + mcount);
                    result.Add(p);
                }

            }
            return result;
        }

        private static Match? GetNextMatch(List<Match> pool, List<Match> plannedmatches, int round, RANKINGUSE rankuse)
        {
            Match? result = null;
            int bestcount = int.MaxValue;
            int bestminrankdif = int.MaxValue;
            int bestmaxrankdif = int.MinValue;
            List<Match> roundmatches = GetMatchesForRoundInList(plannedmatches, round);
            foreach (Match match in pool)
            {
                if (match.State == MatchMakerElement.ElementStatus.FINISHED)
                    continue;
                if (match.Team1 == null || match.Team2 == null)
                    continue;
                if (match.Team1.HasPlayed || match.Team2.HasPlayed)
                    continue;
                //if (IsMatchTeamsInList(roundmatches, match))
                //    continue;
                bool playershaveplayed = false;
                foreach (Player p in match.Players)
                {
                    if (p.PlaysThisRound)
                    {
                        playershaveplayed = true;
                        break;

                    }
                }
                if (playershaveplayed)
                    continue;

                if (IsMatchPlayersInList(roundmatches, match))
                    continue;
                int matchcount = GetMatchCountForMatch(plannedmatches, match);
                int rankdif = GetRankDifForMatch(match);
                if (matchcount < bestcount)
                {
                    result = match;
                    bestcount = matchcount;
                }else if (matchcount == bestcount)
                {
                    if (rankuse == RANKINGUSE.BALANCED && rankdif < bestminrankdif)
                    {
                        bestminrankdif = rankdif;
                        result = match;
                    }
                    if (rankuse == RANKINGUSE.UNBALANCED && rankdif > bestmaxrankdif)
                    {
                        bestmaxrankdif = rankdif;
                        result = match;
                    }
                }
            }
            return result;
        }
        //static public Tournament Generate(string tournamentname, List<string> players, List<string> courts)
        //{
        //    Tournament tournament = new Tournament(tournamentname);
        //    int id = 0;
        //    foreach (var player in players)
        //    {
        //        Player p = new(player);
        //        p.Id = id++;
        //        p.Displayname = player;

        //        tournament.Players.Add(p);

        //    }
        //    foreach (var court in courts)
        //    {
        //        tournament.Courts.Add(new Court(court));

        //    }
        //    tournament.GenerateNew(false);
        //    //Match match = tournament.GenerateMatch();
        //    //while (match != Match.NOMATCH)
        //    //{
        //    //	tournament.Matches.Add(match);
        //    //	match = tournament.GenerateMatch();
        //    //}
        //    return tournament;
        //}

        private static List<Match> GenerateMatchPool(List<Match> existingmathces, List<Team> teams)
        {
            List<Match> result = new();
            foreach (Team team1 in teams)
            {
                foreach (Team team2 in teams)
                {
                    bool ellegible = true;
                    if (team1 == team2)
                    {
                        continue;
                    }
                    if (DoTeamsSharePlayers(team1, team2))
                        continue;
                    foreach (Match match in result)
                    {
                        if (IsTeamInMatch(match, team1) && IsTeamInMatch(match, team2))
                        {
                            ellegible = false;
                            break;

                        }
                    }
                    if (ellegible)
                    {
                        foreach (Match match in existingmathces)
                        {
                            if (IsTeamInMatch(match, team1) && IsTeamInMatch(match, team2))
                            {
                                ellegible = false;
                                break;

                            }
                            if (match.State == MatchMakerElement.ElementStatus.FINISHED)
                            {
                                ellegible = false;
                                break;

                            }
                        }
                    }
                    if (ellegible)
                    {
                        Match match = new(team1, team2);
                        result.Add(match);
                    }
                }
            }
            return result;
        }
        private static List<Match> GenerateMatchPool(List<Team> teams)
        {
            List<Match> result = new();
            foreach (Team team1 in teams)
            {
                foreach (Team team2 in teams)
                {
                    bool ellegible = true;
                    if (team1 == team2)
                    {
                        continue;
                    }
                    if (DoTeamsSharePlayers(team1, team2))
                        continue;
                    foreach (Match match in result)
                    {
                        if (IsTeamInMatch(match, team1) && IsTeamInMatch(match, team2))
                        {
                            ellegible = false;
                            break;

                        }
                    }
                    if (ellegible)
                    {
                        Match match = new(team1, team2);
                        result.Add(match);
                    }
                }
            }
            return result;
        }
        public static List<Team> GenerateTeams(List<Team> existingteams, List<Player> players)
        {
            List<Team> result = new();

            for (int i = 0; i < players.Count; i++)
            {
                for (int j = i + 1; j < players.Count; j++)
                {
                    List<Player> teamplayers = new();
                    teamplayers.Add(players[i]);
                    teamplayers.Add(players[j]);
                    Team? Team = FindTeamWithPlayersFromTeamlist(existingteams, teamplayers);
                    if (Team == null)
                    {
                        Team = new();
                        Team.Players.AddRange(teamplayers);
                    }
                    Team.HasPlayed = false;
                    result.Add(Team);
                }
            }
            return result;
        }
        private static Court getNextCourt(ref int CourtIdx, ref int Round, List<Court> courts)
        {
            Court result = courts[CourtIdx++];
            if (CourtIdx == courts.Count)
            {
                CourtIdx = 0;
                Round++;
            }
            return result;
        }
        private static Court getCurrentCourt(int CourtIdx, List<Court> courts)
        {
            return courts[CourtIdx];
        }
        public static void SwitchMatchTeamsToExistingTeams(List<Team> existingteams, MudMatch match)
        {
            if (match.Team1 != null)
            {
                Team? existingteam1 = FindTeamWithPlayersFromTeamlist(existingteams, match.Team1.Players);
                if (existingteam1 != null)
                {
                    match.Team1 = existingteam1;
                }
            }
            if (match.Team2 != null)
            {
                Team? existingteam2 = FindTeamWithPlayersFromTeamlist(existingteams, match.Team2.Players);
                if (existingteam2 != null)
                {
                    match.Team2 = existingteam2;
                }
            }
        }
        public static List<Team> GetUniqueTeams(List<Team> existingteams)
        {
            List<Team> result = new();
            foreach (Team t in existingteams)
            {
                Team? existingteam = FindTeamWithPlayersFromTeamlist(result, t.Players);
                if (existingteam == null)
                    result.Add(t);

            }
            return result;
        }
        //public static void ConsolidateMatchTeams(List<Team> existingteams,List<Match> matches)
        //{
        //    foreach(Match match in matches)
        //    {
        //        SwitchMatchTeamsToExistingTeams(GetUniqueTeams(existingteams),  match);
        //    }

        //}

        //private static Team? FindTeamWithPlayersFromTeamlist(List<Team> teamlist, Player player1, Player player2)
        //{
        //    Team? result = null;
        //    foreach (Team t in teamlist)
        //    {
        //        if (IsPlayerInTeam(t, player1) && IsPlayerInTeam(t, player2))
        //        {
        //            result = t;
        //            break;
        //        }
        //    }
        //    return result;
        //}
        public static Team? FindTeamWithPlayersFromTeamlist(List<Team> teamlist, List<Player> players)
        {
            Team? result = null;
            foreach (Team t in teamlist)
            {
                bool teammatch = false;
                foreach (Player p in players)
                {
                    if (IsPlayerInTeam(t, p))
                    {
                        teammatch = true;

                    }
                    else
                    {
                        teammatch = false;
                        break;
                    }

                }
                if (teammatch)
                {
                    result = t;
                    break;
                }
            }
            return result;
        }

        private static List<Match> GetMatchesForRoundInList(List<Match> matches, int round)
        {
            List<Match> result = new();
            foreach (Match match in matches)
            {
                if (match.Round == round)
                    result.Add(match);
            }
            return result;
        }
        private static bool IsPlayerInTeam(Team team, Player player)
        {
            return team.Players.Contains(player);
        }

        private static bool IsTeamInMatch(Match match, Team team)
        {
            return match.Team1 == team || match.Team2 == team;
        }
        private static bool DoTeamsSharePlayers(Team team1, Team team2)
        {
            bool result = false;
            foreach (Player player in team1.Players)
            {
                if (IsPlayerInTeam(team2, player))
                {
                    result = true;
                    break;
                }
            }
            if (!result)
            {
                foreach (Player player in team2.Players)
                {
                    if (IsPlayerInTeam(team1, player))
                    {
                        result = true;
                        break;
                    }
                }

            }
            return result;
        }
        public static int GetMatchCountForPlayer(List<Match> matches, Player p)
        {
            int count = 0;
            foreach (Match m in matches)
            {
                if (m.HasPlayer(p))
                    count++;
            }
            return count;
        }

        private static int GetMatchCountForMatch(List<Match> matches, Match match)
        {
            int result = 0;
            foreach (Player p in match.Players)
                result += GetMatchCountForPlayer(matches, p);
            return result;

        }
        private static int GetRankDifForMatch(Match match)
        {
            return Math.Abs(match.Team1.Rank - match.Team2.Rank);
        }
        private static bool IsMatchPlayersInList(List<Match> matchlist, Match match)
        {
            bool result = false;
            foreach (Match m in matchlist)
            {
                foreach (Player p in match.Players)
                {
                    if (IsPlayerInTeam(m.Team1, p) || IsPlayerInTeam(m.Team2, p))
                    {
                        return true;

                    }

                }
            }
            return result;
        }

        private static bool IsMatchTeamsInList(List<Match> matchlist, Match match)
        {
            foreach (Match match2 in matchlist)
            {
                if (IsTeamInMatch(match2, match.Team1) || IsTeamInMatch(match2, match.Team2))
                    return true;
            }

            return false;
        }


    }
}