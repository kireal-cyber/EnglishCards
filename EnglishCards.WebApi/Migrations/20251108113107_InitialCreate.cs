using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCards.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnglishWord = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Transcription = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Translation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ExampleSentence = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    PronunciationUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordCards_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordCards_DeckId",
                table: "WordCards",
                column: "DeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordCards");

            migrationBuilder.DropTable(
                name: "Decks");
        }
    }
}
