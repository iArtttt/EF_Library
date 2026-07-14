using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovingFromBorrowedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.DropIndex(
                name: "IX_BorrowedBooks_LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "BorrowedBooks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibrarianId",
                table: "BorrowedBooks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedBooks_LibrarianId",
                table: "BorrowedBooks",
                column: "LibrarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id");
        }
    }
}
