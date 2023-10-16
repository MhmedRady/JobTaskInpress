using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzTask.Migrations.DailyTask
{
    public partial class dailyTask_db_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DaySummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSummary = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRounds = table.Column<int>(type: "int", nullable: false),
                    TotalTasks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaySummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoundId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaySummaryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayDetails_DaySummary_DaySummaryId",
                        column: x => x.DaySummaryId,
                        principalTable: "DaySummary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayDetails_DaySummaryId",
                table: "DayDetails",
                column: "DaySummaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayDetails");

            migrationBuilder.DropTable(
                name: "DaySummary");
        }
    }
}
