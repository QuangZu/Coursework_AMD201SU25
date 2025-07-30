using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShorteningService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls");

            migrationBuilder.RenameTable(
                name: "ShortUrls",
                newName: "URLs");

            migrationBuilder.AlterColumn<string>(
                name: "short_url",
                table: "URLs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "long_url",
                table: "URLs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_URLs",
                table: "URLs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_URLs",
                table: "URLs");

            migrationBuilder.RenameTable(
                name: "URLs",
                newName: "ShortUrls");

            migrationBuilder.AlterColumn<string>(
                name: "short_url",
                table: "ShortUrls",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "long_url",
                table: "ShortUrls",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls",
                column: "Id");
        }
    }
}
