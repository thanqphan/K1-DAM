using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAM.DAM.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatetblPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId_FolderId_FileId",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "FolderId",
                table: "Permissions",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "Permissions",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_EntityId",
                table: "Permissions",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_EntityId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "FolderId",
                table: "Permissions",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId_FolderId_FileId",
                table: "Permissions",
                columns: new[] { "UserId", "FolderId", "FileId" },
                unique: true,
                filter: "[FileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Files_FileId",
                table: "Permissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Folders_FolderId",
                table: "Permissions",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
