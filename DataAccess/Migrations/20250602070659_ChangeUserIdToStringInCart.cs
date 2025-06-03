using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToStringInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Meal_mealId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "mealId",
                table: "Carts",
                newName: "MealId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_mealId",
                table: "Carts",
                newName: "IX_Carts_MealId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Meal_MealId",
                table: "Carts",
                column: "MealId",
                principalTable: "Meal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Meal_MealId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "MealId",
                table: "Carts",
                newName: "mealId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_MealId",
                table: "Carts",
                newName: "IX_Carts_mealId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Carts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Meal_mealId",
                table: "Carts",
                column: "mealId",
                principalTable: "Meal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
