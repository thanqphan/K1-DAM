using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAM.DAM.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatetblPermissionfield : Migration
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
                name: "IX_Permissions_FileId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_FolderId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Permissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "Permissions",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "Permissions",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FileId",
                table: "Permissions",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FolderId",
                table: "Permissions",
                column: "FolderId");

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
    }
}
