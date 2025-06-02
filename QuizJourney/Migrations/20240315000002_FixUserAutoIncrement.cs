using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace QuizJourney.Migrations
{
    /// <inheritdoc />
    public partial class FixUserAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing table
            migrationBuilder.DropTable(name: "Users");

            // Recreate the table with proper auto-increment
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            // Reinsert the seed data
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { "dimas", "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e", "Teacher" },
                    { "ricky", "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e", "Student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This is a destructive migration, so we'll just recreate the table as it was
            migrationBuilder.DropTable(name: "Users");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { "dimas", "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e", "Teacher" },
                    { "ricky", "$2a$11$jRORk/3SRvfpwoErLCWBC.bnXqHhnLlwBGYLwMaaNiQCJANYuNs1e", "Student" }
                });
        }
    }
} 