using MatchMakerLib.MatchMakerModel;
using System.Diagnostics;

namespace MatchMakerUnitTest
{
    public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			List<string> players = new List<string> { "A", "B", "C" };
			List<string> courts = new List<string> { "1", "2"};
			Tournament tournament = TournamentGenerator.Generate("Test",players,courts);
			Console.WriteLine(tournament.ToString());
			Debug.WriteLine(tournament.ToString());



		}
	}
}