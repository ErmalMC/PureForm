using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureForm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DailyCalorieGoal",
                table: "Users",
                type: "decimal(7,2)",
                precision: 7,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyCarbsGoal",
                table: "Users",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyFatsGoal",
                table: "Users",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyProteinGoal",
                table: "Users",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CaloriesPer100g = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ProteinPer100g = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CarbsPer100g = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FatsPer100g = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DefaultUnit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPopular = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_Category",
                table: "FoodItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_IsPopular",
                table: "FoodItems",
                column: "IsPopular");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_Name",
                table: "FoodItems",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropColumn(
                name: "DailyCalorieGoal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyCarbsGoal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyFatsGoal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyProteinGoal",
                table: "Users");
        }
    }
}
