using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureForm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsPopularFromFoodItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FoodItems_IsPopular",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "FoodItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "FoodItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_IsPopular",
                table: "FoodItems",
                column: "IsPopular");
        }
    }
}
