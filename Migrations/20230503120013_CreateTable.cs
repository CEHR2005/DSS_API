using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSS_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleItems_UserItems_UserId",
                table: "ArticleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentItems_ArticleItems_ArticleId",
                table: "CommentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentItems_UserItems_UserId",
                table: "CommentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserItems",
                table: "UserItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentItems",
                table: "CommentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleItems",
                table: "ArticleItems");

            migrationBuilder.RenameTable(
                name: "UserItems",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "CommentItems",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "ArticleItems",
                newName: "Article");

            migrationBuilder.RenameIndex(
                name: "IX_CommentItems_UserId",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentItems_ArticleId",
                table: "Comment",
                newName: "IX_Comment_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleItems_UserId",
                table: "Article",
                newName: "IX_Article_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Article",
                table: "Article",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_User_UserId",
                table: "Article",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_User_UserId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_UserId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Article",
                table: "Article");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserItems");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "CommentItems");

            migrationBuilder.RenameTable(
                name: "Article",
                newName: "ArticleItems");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "CommentItems",
                newName: "IX_CommentItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ArticleId",
                table: "CommentItems",
                newName: "IX_CommentItems_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Article_UserId",
                table: "ArticleItems",
                newName: "IX_ArticleItems_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserItems",
                table: "UserItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentItems",
                table: "CommentItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleItems",
                table: "ArticleItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleItems_UserItems_UserId",
                table: "ArticleItems",
                column: "UserId",
                principalTable: "UserItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentItems_ArticleItems_ArticleId",
                table: "CommentItems",
                column: "ArticleId",
                principalTable: "ArticleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentItems_UserItems_UserId",
                table: "CommentItems",
                column: "UserId",
                principalTable: "UserItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
