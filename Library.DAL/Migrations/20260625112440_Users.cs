using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Librarians_LibrarianId",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_LibrarianId",
                table: "Readers");

            migrationBuilder.DeleteData(
                table: "Librarians",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Librarians",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Readers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Readers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Librarians");
            /*
                        migrationBuilder.AlterColumn<int>(
                            name: "Id",
                            table: "Readers",
                            type: "int",
                            nullable: false,
                            oldClrType: typeof(int),
                            oldType: "int")
                            .OldAnnotation("SqlServer:Identity", "1, 1");

                        migrationBuilder.AlterColumn<int>(
                            name: "Id",
                            table: "Librarians",
                            type: "int",
                            nullable: false,
                            oldClrType: typeof(int),
                            oldType: "int")
                            .OldAnnotation("SqlServer:Identity", "1, 1");
            */

            // ================================================================
            // ВМЕСТО СТАРОГО ALTER_COLUMN ДЛЯ READERS (ВСТАВИТЬ СЮДА)
            // ================================================================

            // 1. Временно отключаем ключ, который держит таблицу BorrowedBooks
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBooks_Readers_ReaderId",
                table: "BorrowedBooks");

            // 2. Снимаем старый первичный ключ с таблицы Readers
            migrationBuilder.DropPrimaryKey(
                name: "PK_Readers",
                table: "Readers");

            // 3. Удаляем старую колонку Id со сломанным автоинкрементом
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Readers");

            // 4. Добавляем чистую колонку Id без автоинкремента (для связи TPT)
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Readers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 5. Возвращаем первичный ключ для таблицы Readers
            migrationBuilder.AddPrimaryKey(
                name: "PK_Readers",
                table: "Readers",
                column: "Id");

            // 6. Возвращаем на место связь с таблицей BorrowedBooks
            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBooks_Readers_ReaderId",
                table: "BorrowedBooks",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);


            // ================================================================
            // ВМЕСТО СТАРОГО ALTER_COLUMN ДЛЯ LIBRARIANS
            // ================================================================

            // 1. Снимаем старый первичный ключ с таблицы Librarians
            migrationBuilder.DropPrimaryKey(
                name: "PK_Librarians",
                table: "Librarians");

            // 2. ДОБАВЛЯЕМ ВРЕМЕННУЮ КОЛОНКУ, чтобы таблица не осталась пустой при удалении Id
            migrationBuilder.AddColumn<int>(
                name: "TempColumn",
                table: "Librarians",
                type: "int",
                nullable: true);

            // 3. Теперь спокойно удаляем старую колонку Id со сломанным автоинкрементом
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Librarians");

            // 4. Добавляем чистую колонку Id без автоинкремента (для связи TPT)
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Librarians",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 5. Возвращаем первичный ключ для таблицы Librarians
            migrationBuilder.AddPrimaryKey(
                name: "PK_Librarians",
                table: "Librarians",
                column: "Id");

            // 6. УДАЛЯЕМ ВРЕМЕННУЮ КОЛОНКУ, так как Id вернулся и таблица больше не опустеет
            migrationBuilder.DropColumn(
                name: "TempColumn",
                table: "Librarians");

            // ================================================================

            migrationBuilder.AddColumn<int>(
                name: "LibrarianId",
                table: "BorrowedBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Login", "Password" },
                values: new object[,]
                {
                    { -4, "rEAr@gmail.com", "Reader1", "1423" },
                    { -3, "reader@gmail.com", "Reader", "1234" },
                    { -2, "admin1@gmail.com", "Admin1", "4567" },
                    { -1, "admin@gmail.com", "Admin", "1234" }
                });

            migrationBuilder.InsertData(
                table: "Librarians",
                column: "Id",
                values: new object[]
                {
                    -2,
                    -1
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "Id", "Birthday", "DocumentNumber", "DocumentType", "LastName", "Name" },
                values: new object[,]
                {
                    { -4, new DateTime(1993, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "777789", 0, "Zeroph", "Alex" },
                    { -3, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), "3354213", 1, "Lighter", "Bob" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedBooks_LibrarianId",
                table: "BorrowedBooks",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_Users_Id",
                table: "Librarians",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Users_Id",
                table: "Readers",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBooks_Librarians_LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_Users_Id",
                table: "Librarians");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Users_Id",
                table: "Readers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BorrowedBooks_LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.DeleteData(
                table: "Librarians",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Librarians",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Readers",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Readers",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "BorrowedBooks");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Readers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Readers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LibrarianId",
                table: "Readers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Readers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Readers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Librarians",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Librarians",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Librarians",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Librarians",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Librarians",
                columns: new[] { "Id", "Email", "Login", "Password" },
                values: new object[,]
                {
                    { 1, "admin@gmail.com", "Admin", "1234" },
                    { 2, "admin1@gmail.com", "Admin1", "4567" }
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "Id", "Birthday", "DocumentNumber", "DocumentType", "Email", "LastName", "LibrarianId", "Login", "Name", "Password" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), "3354213", 1, "reader@gmail.com", "Lighter", null, "Reader", "Bob", "1234" },
                    { 2, new DateTime(1993, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "777789", 0, "rEAr@gmail.com", "Zeroph", null, "Reader1", "Alex", "1423" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Readers_LibrarianId",
                table: "Readers",
                column: "LibrarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Librarians_LibrarianId",
                table: "Readers",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id");
        }
    }
}
