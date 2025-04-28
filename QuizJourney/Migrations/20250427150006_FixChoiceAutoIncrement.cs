using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizJourney.Migrations
{
    /// <inheritdoc />
    public partial class FixChoiceAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Choices",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Choices");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e");
        }
    }
}
