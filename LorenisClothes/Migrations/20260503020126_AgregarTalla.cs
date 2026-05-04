using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LorenisClothes.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTalla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Talla",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Talla",
                table: "Productos");
        }
    }
}
