using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBorrowedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.AlterColumn<int>(
                name: "LibrarianId",
                table: "BorrowedBooks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Readers",
                keyColumn: "Id",
                keyValue: -3,
                column: "Birthday",
                value: new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.AlterColumn<int>(
                name: "LibrarianId",
                table: "BorrowedBooks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Readers",
                keyColumn: "Id",
                keyValue: -3,
                column: "Birthday",
                value: new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
