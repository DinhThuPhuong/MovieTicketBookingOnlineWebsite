using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSoTL.Migrations
{
    /// <inheritdoc />
    public partial class AddSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
     name: "Seats",
     columns: table => new
     {
         Id = table.Column<int>(type: "int", nullable: false)
             .Annotation("SqlServer:Identity", "1, 1"),
         Row = table.Column<string>(type: "nvarchar(max)", nullable: false),
         Number = table.Column<int>(type: "int", nullable: false),
         IsAvailable = table.Column<bool>(type: "bit", nullable: false),
         CinemaId = table.Column<int>(type: "int", nullable: false),
         ScreeningId = table.Column<int>(type: "int", nullable: false)
     },
     constraints: table =>
     {
         table.PrimaryKey("PK_Seats", x => x.Id);
         table.ForeignKey(
             name: "FK_Seats_Cinemas_CinemaId",
             column: x => x.CinemaId,
             principalTable: "Cinemas",
             principalColumn: "Id",
             onDelete: ReferentialAction.Restrict); // Thay đổi từ ReferentialAction.Cascade thành ReferentialAction.Restrict
         table.ForeignKey(
             name: "FK_Seats_Screenings_ScreeningId",
             column: x => x.ScreeningId,
             principalTable: "Screenings",
             principalColumn: "Id",
             onDelete: ReferentialAction.Restrict); // Thay đổi từ ReferentialAction.Cascade thành ReferentialAction.Restrict
     });


            migrationBuilder.CreateIndex(
                name: "IX_Seats_CinemaId",
                table: "Seats",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ScreeningId",
                table: "Seats",
                column: "ScreeningId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats");
        }
    }
}
