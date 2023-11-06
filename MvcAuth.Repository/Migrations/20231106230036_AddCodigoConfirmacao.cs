using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcAuth.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoConfirmacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodigoConfirmacao",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoConfirmacao",
                table: "Usuario");
        }
    }
}
