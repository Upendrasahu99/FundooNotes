using Microsoft.EntityFrameworkCore.Migrations;

namespace RepoLayer.Migrations
{
    public partial class CollabMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collabs",
                columns: table => new
                {
                    CollabId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    NoteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collabs", x => x.CollabId);
                    table.ForeignKey(
                        name: "FK_Collabs_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction); // In place of Cascade we use NoAction
                    // Cascade will remove data if Parent table data will deleted
                    table.ForeignKey(
                        name: "FK_Collabs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collabs_NoteId",
                table: "Collabs",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Collabs_UserId",
                table: "Collabs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collabs");
        }
    }
}
