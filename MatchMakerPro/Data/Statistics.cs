using MatchMakerLib.MatchMakerModel;
using MatchMakerPro.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.Defaults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatchMakerPro
{
    public class Filter
    {
        public Club? Club;
        public List<Mainevent> Events = new();
        public DateTime Startdate = new(2020, 1, 1);
        public DateTime Enddate = DateTime.Now;
        public int MaxMatches = 20;
    }
    public static class Statistics
    {
        public static List<DateTime> GetDatesPlayed(MatchMakerDbContext db, Filter filter)
        {
            List<DateTime> result = new();
            var query = db.Tournaments.AsQueryable();
            query = db.Tournaments.Where(t => t.State == MatchMakerElement.ElementStatus.FINISHED).Where(t => t.Finishdate >= filter.Startdate).Where(t => t.Finishdate <= filter.Enddate);
            //Doesn't work
            //query = dbcontext.Matches.Where(m => m.Finishdate >= startdate);
            //query = dbcontext.Matches.Where(m => m.Finishdate <= enddate);
            //if (selectedmainevent != Mainevent.NOEVENT)
            //    query =  dbcontext.Matches.Where(m => m.Tournament.Mainevent == selectedmainevent);
            List<Tournament> tmp = query.ToList();
            foreach (Tournament t in tmp)
            {
                if (filter.Events.Count == 0 || filter.Events.Contains(t.Mainevent))
                {
                    if (t.Finishdate != null)
                        result.Add(t.Finishdate ?? DateTime.Now);
                }

            }
            result = result.OrderBy(d => d).ToList();


            return result;
        }

        public static int GetRankForPlayerOnDate(MatchMakerDbContext db, Filter filter, Player player, DateTime date)
        {
            int result = 0;
            var query = db.Matches.AsQueryable();
            query = db.Matches.Where(m => m.State == MatchMakerElement.ElementStatus.FINISHED).Where(m => m.Finishdate >= filter.Startdate).Where(m => m.Finishdate <= date);
            List<Match> tmp = query.Include(t => t.Team1).ThenInclude(p => p.Players).Include(t => t.Team2).ThenInclude(p => p.Players).ToList();
            List<Match> matchlist = new();
            foreach (Match m in tmp)
            {
                if (filter.Events.Count == 0 || filter.Events.Contains(m.Tournament.Mainevent))
                    matchlist.Add(m);

            }
            //matchlist.Sort((m,m2) => m.Finishdate.CompareTo(m2.Finishdate));
            matchlist = matchlist.OrderByDescending(m => m.Finishdate).ToList();
            result = GetPlayerRankForMatches(player, matchlist, filter.MaxMatches);
            return result;
        }

        public static int GetPlayerRankForMatches(Player player, List<Match> matchlist, int maxmatches)
        {
            int result = 0;
            double won = 0;
            double played = 0;
            foreach (Match match in matchlist)
            {
                if (match.HasPlayer(player))
                {
                    played++;
                    if (match.PlayerWon(player))
                        won++;
                }
                if (played >= maxmatches)
                    break;

            }
            if (played > 0)
                result = (int)((won / played) * 100);
            else
                result = 0;

            return result;
        }
        public static void GetResulsForPlayerAgainstAndWithPlayer(MatchMakerDbContext db, Filter filter, Player player, Player opponent, ref int winsagainst, ref int lossesagainst, ref int winswith, ref int losseswith)
        {
            var query = db.Matches.AsQueryable();
            query = db.Matches.Where(m => m.State == MatchMakerElement.ElementStatus.FINISHED).Where(m => m.Finishdate >= filter.Startdate).Where(m => m.Finishdate <= filter.Enddate);
            List<Match> tmp = query.Include(t => t.Team1).ThenInclude(p => p.Players).Include(t => t.Team2).ThenInclude(p => p.Players).ToList();
            List<Match> matchlist = new();
            foreach (Match m in tmp)
            {
                if (filter.Events.Count == 0 || filter.Events.Contains(m.Tournament.Mainevent))
                {
                    if (m.HasPlayer(player) && m.PlayerWon(player) && m.HasPlayer(opponent) && !m.PlayerWon(opponent))
                        winsagainst++;
                    if (m.HasPlayer(player) && !m.PlayerWon(player) && m.HasPlayer(opponent) && m.PlayerWon(opponent))
                        lossesagainst++;
                    if (m.HasPlayer(player) && m.PlayerWon(player) && m.HasPlayer(opponent) && m.PlayerWon(opponent))
                        winswith++;
                    if (m.HasPlayer(player) && !m.PlayerWon(player) && m.HasPlayer(opponent) && !m.PlayerWon(opponent))
                        losseswith++;
                }

            }

        }
    }
}
