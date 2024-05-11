﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSoTL.Migrations
{
    /// <inheritdoc />
    public partial class AddRowForSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Row",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "Seats");
        }
    }
}
