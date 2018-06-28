using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoApi.Migrations
{
    public partial class Updateditemfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_ToDoLists_TDListID",
                table: "ToDoItems");

            migrationBuilder.RenameColumn(
                name: "TDListID",
                table: "ToDoItems",
                newName: "ToDoListID");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoItems_TDListID",
                table: "ToDoItems",
                newName: "IX_ToDoItems_ToDoListID");

            migrationBuilder.AddColumn<string>(
                name: "ToDoList",
                table: "ToDoItems",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_ToDoLists_ToDoListID",
                table: "ToDoItems",
                column: "ToDoListID",
                principalTable: "ToDoLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_ToDoLists_ToDoListID",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "ToDoList",
                table: "ToDoItems");

            migrationBuilder.RenameColumn(
                name: "ToDoListID",
                table: "ToDoItems",
                newName: "TDListID");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoItems_ToDoListID",
                table: "ToDoItems",
                newName: "IX_ToDoItems_TDListID");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_ToDoLists_TDListID",
                table: "ToDoItems",
                column: "TDListID",
                principalTable: "ToDoLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
