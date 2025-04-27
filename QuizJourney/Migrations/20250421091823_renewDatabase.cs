using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizJourney.Migrations
{
    /// <inheritdoc />
    public partial class renewDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Rooms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Rooms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$wFc55uqOrzJwR95jpXIH9eIzuuTF3JQuPISNoBfdaKdDqtWQGsmwm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$wFc55uqOrzJwR95jpXIH9eIzuuTF3JQuPISNoBfdaKdDqtWQGsmwm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Rooms");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$3Z8z0kFtAf7VPiKFusf2YO38jHB6sDgDYVxxdCnBy618ZhZmkPfjW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$3Z8z0kFtAf7VPiKFusf2YO38jHB6sDgDYVxxdCnBy618ZhZmkPfjW");
        }
    }
}
