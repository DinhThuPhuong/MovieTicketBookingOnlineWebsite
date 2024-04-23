using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSoTL.Migrations
{
    /// <inheritdoc />
    public partial class Them : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesInCinema_Cinemas_CinemaId",
                table: "MoviesInCinema");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesInCinema_Movies_MovieId",
                table: "MoviesInCinema");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesInCinema",
                table: "MoviesInCinema");

            migrationBuilder.RenameTable(
                name: "MoviesInCinema",
                newName: "MovieInCinemas");

            migrationBuilder.RenameIndex(
                name: "IX_MoviesInCinema_MovieId",
                table: "MovieInCinemas",
                newName: "IX_MovieInCinemas_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviesInCinema_CinemaId",
                table: "MovieInCinemas",
                newName: "IX_MovieInCinemas_CinemaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieInCinemas",
                table: "MovieInCinemas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieInCinemas_Cinemas_CinemaId",
                table: "MovieInCinemas",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieInCinemas_Movies_MovieId",
                table: "MovieInCinemas",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieInCinemas_Cinemas_CinemaId",
                table: "MovieInCinemas");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieInCinemas_Movies_MovieId",
                table: "MovieInCinemas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieInCinemas",
                table: "MovieInCinemas");

            migrationBuilder.RenameTable(
                name: "MovieInCinemas",
                newName: "MoviesInCinema");

            migrationBuilder.RenameIndex(
                name: "IX_MovieInCinemas_MovieId",
                table: "MoviesInCinema",
                newName: "IX_MoviesInCinema_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieInCinemas_CinemaId",
                table: "MoviesInCinema",
                newName: "IX_MoviesInCinema_CinemaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesInCinema",
                table: "MoviesInCinema",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesInCinema_Cinemas_CinemaId",
                table: "MoviesInCinema",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesInCinema_Movies_MovieId",
                table: "MoviesInCinema",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
