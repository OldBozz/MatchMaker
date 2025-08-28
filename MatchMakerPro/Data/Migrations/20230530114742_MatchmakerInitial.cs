using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MatchMakerPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class MatchmakerInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "matchmaker");

            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clubs",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courts",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Displayname = table.Column<string>(type: "longtext", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    Identity = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mainevents",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mainevents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mainevents_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalSchema: "matchmaker",
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClubPlayer",
                schema: "matchmaker",
                columns: table => new
                {
                    ClubsId = table.Column<int>(type: "int", nullable: false),
                    playersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubPlayer", x => new { x.ClubsId, x.playersId });
                    table.ForeignKey(
                        name: "FK_ClubPlayer_Clubs_ClubsId",
                        column: x => x.ClubsId,
                        principalSchema: "matchmaker",
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubPlayer_Players_playersId",
                        column: x => x.playersId,
                        principalSchema: "matchmaker",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerTeam",
                schema: "matchmaker",
                columns: table => new
                {
                    PlayersId = table.Column<int>(type: "int", nullable: false),
                    TeamsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTeam", x => new { x.PlayersId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_PlayerTeam_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalSchema: "matchmaker",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalSchema: "matchmaker",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tournaments",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Startdate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Finishdate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MaineventId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    PlayDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CurrentRound = table.Column<int>(type: "int", nullable: false),
                    TotalRounds = table.Column<int>(type: "int", nullable: false),
                    PointsToWin = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalSchema: "matchmaker",
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournaments_Mainevents_MaineventId",
                        column: x => x.MaineventId,
                        principalSchema: "matchmaker",
                        principalTable: "Mainevents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourtTournament",
                schema: "matchmaker",
                columns: table => new
                {
                    CourtsId = table.Column<int>(type: "int", nullable: false),
                    TournamentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtTournament", x => new { x.CourtsId, x.TournamentsId });
                    table.ForeignKey(
                        name: "FK_CourtTournament_Courts_CourtsId",
                        column: x => x.CourtsId,
                        principalSchema: "matchmaker",
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourtTournament_Tournaments_TournamentsId",
                        column: x => x.TournamentsId,
                        principalSchema: "matchmaker",
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Matches",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Finishdate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: false),
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    Team1Id = table.Column<int>(type: "int", nullable: false),
                    Team2Id = table.Column<int>(type: "int", nullable: false),
                    Team1Points = table.Column<int>(type: "int", nullable: false),
                    Team2Points = table.Column<int>(type: "int", nullable: false),
                    Team1Status = table.Column<int>(type: "int", nullable: false),
                    Team2Status = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Courts_CourtId",
                        column: x => x.CourtId,
                        principalSchema: "matchmaker",
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "matchmaker",
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team1Id",
                        column: x => x.Team1Id,
                        principalSchema: "matchmaker",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team2Id",
                        column: x => x.Team2Id,
                        principalSchema: "matchmaker",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalSchema: "matchmaker",
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerBets",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TournamentId = table.Column<int>(type: "int", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBets_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "matchmaker",
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerBets_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalSchema: "matchmaker",
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerTournament",
                schema: "matchmaker",
                columns: table => new
                {
                    PlayersId = table.Column<int>(type: "int", nullable: false),
                    TournamentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTournament", x => new { x.PlayersId, x.TournamentsId });
                    table.ForeignKey(
                        name: "FK_PlayerTournament_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalSchema: "matchmaker",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTournament_Tournaments_TournamentsId",
                        column: x => x.TournamentsId,
                        principalSchema: "matchmaker",
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeamTournament",
                schema: "matchmaker",
                columns: table => new
                {
                    TeamsId = table.Column<int>(type: "int", nullable: false),
                    TournamentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTournament", x => new { x.TeamsId, x.TournamentsId });
                    table.ForeignKey(
                        name: "FK_TeamTournament_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalSchema: "matchmaker",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTournament_Tournaments_TournamentsId",
                        column: x => x.TournamentsId,
                        principalSchema: "matchmaker",
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TournamentBets",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TournamentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentBets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentBets_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalSchema: "matchmaker",
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BetMatch",
                schema: "matchmaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MatchId = table.Column<int>(type: "int", nullable: true),
                    BetTournamentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetMatch_Matches_MatchId",
                        column: x => x.MatchId,
                        principalSchema: "matchmaker",
                        principalTable: "Matches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BetMatch_TournamentBets_BetTournamentId",
                        column: x => x.BetTournamentId,
                        principalSchema: "matchmaker",
                        principalTable: "TournamentBets",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BetMatch_BetTournamentId",
                schema: "matchmaker",
                table: "BetMatch",
                column: "BetTournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_BetMatch_MatchId",
                schema: "matchmaker",
                table: "BetMatch",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubPlayer_playersId",
                schema: "matchmaker",
                table: "ClubPlayer",
                column: "playersId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtTournament_TournamentsId",
                schema: "matchmaker",
                table: "CourtTournament",
                column: "TournamentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Mainevents_ClubId",
                schema: "matchmaker",
                table: "Mainevents",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CourtId",
                schema: "matchmaker",
                table: "Matches",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerId",
                schema: "matchmaker",
                table: "Matches",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team1Id",
                schema: "matchmaker",
                table: "Matches",
                column: "Team1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team2Id",
                schema: "matchmaker",
                table: "Matches",
                column: "Team2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                schema: "matchmaker",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBets_PlayerId",
                schema: "matchmaker",
                table: "PlayerBets",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBets_TournamentId",
                schema: "matchmaker",
                table: "PlayerBets",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeam_TeamsId",
                schema: "matchmaker",
                table: "PlayerTeam",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTournament_TournamentsId",
                schema: "matchmaker",
                table: "PlayerTournament",
                column: "TournamentsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournament_TournamentsId",
                schema: "matchmaker",
                table: "TeamTournament",
                column: "TournamentsId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentBets_TournamentId",
                schema: "matchmaker",
                table: "TournamentBets",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_ClubId",
                schema: "matchmaker",
                table: "Tournaments",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_MaineventId",
                schema: "matchmaker",
                table: "Tournaments",
                column: "MaineventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BetMatch",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "ClubPlayer",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "CourtTournament",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "PlayerBets",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "PlayerTeam",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "PlayerTournament",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "TeamTournament",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Matches",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "TournamentBets",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Courts",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Players",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Tournaments",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Mainevents",
                schema: "matchmaker");

            migrationBuilder.DropTable(
                name: "Clubs",
                schema: "matchmaker");
        }
    }
}
