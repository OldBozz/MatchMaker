using MatchMakerLib.MatchMakerModel;

namespace MatchMakerLib
{
	public class TournamentGenerator
	{
		static public Tournament Generate(string tournamentname,List<string> players,List<string> courts)
		{
			Tournament tournament = new Tournament(tournamentname);
			int id = 0;
			foreach (var player in players)
			{
				Player p = new Player(player);
				p.Id = id++; 
				p.Displayname = player; 

                tournament.Players.Add(p);

			}
			foreach (var court in courts)
			{
				tournament.Courts.Add(new Court(court));

			}
			tournament.GenerateNew(false);
			//Match match = tournament.GenerateMatch();
			//while (match != Match.NOMATCH)
			//{
			//	tournament.Matches.Add(match);
			//	match = tournament.GenerateMatch();
			//}
			return tournament;
		}



	}

}