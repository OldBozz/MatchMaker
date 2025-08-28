// See https://aka.ms/new-console-template for more information
using MatchMakerLib.MatchMakerModel;
using Microsoft.Extensions.Options;
using System.Xml.Linq;


List<string> players = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K","L" };
List<string> courts = new() { "1", "2"};
Tournament tournament = new();
int id = 0;
tournament.Name = "Test";
tournament.Club = new Club();
tournament.Club.Name = "EVK";
tournament.Mainevent = new Mainevent("Beach");
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
tournament.shuffle = true;
tournament.algorithm = TournamentGenerator.ALGORITHM.PLAYERSPLAYSWITHALLOTHERPLAYERS;
tournament.rankinguse = TournamentGenerator.RANKINGUSE.NONE;
//TournamentGenerator.Generate("Sommerbeach 17/5 2023", players, courts);
TournamentGenerator.CombineAllPossiblePlayerTeamsAgainstRandomTeams(tournament);
//Console.WriteLine(tournament.ToLongString());
string filename =Path.Combine("C:\\tmp\\",string.Format("tournaments_{0}.json", DateTime.Now.ToShortDateString()));
File.WriteAllText(filename, tournament.ToJson());
string jsonstring = File.ReadAllText(filename);
Tournament? importtour = Tournament.FromJson(jsonstring);
if(importtour != null)  
    Console.WriteLine(importtour.ToLongString());

