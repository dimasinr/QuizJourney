using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizJourney.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToStudentAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentAnswers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$C2r07CsTQHFZmYnlGjy0beQnFZXG2IGRXe1Npg52sQEdiI3OStr.O");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$C2r07CsTQHFZmYnlGjy0beQnFZXG2IGRXe1Npg52sQEdiI3OStr.O");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudentAnswers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$F3uYmeJaOoJ7Geut7hlnW.cMJuEK3MX1Luz/bCqSiSszc04qjLxJm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$F3uYmeJaOoJ7Geut7hlnW.cMJuEK3MX1Luz/bCqSiSszc04qjLxJm");
        }
    }
}
