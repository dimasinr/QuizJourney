using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizJourney.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash" },
                values: new object[] { "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash" },
                values: new object[] { "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash" },
                values: new object[] { "$2a$11$wFc55uqOrzJwR95jpXIH9eIzuuTF3JQuPISNoBfdaKdDqtWQGsmwm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash" },
                values: new object[] { "$2a$11$wFc55uqOrzJwR95jpXIH9eIzuuTF3JQuPISNoBfdaKdDqtWQGsmwm" });
        }
    }
} 