using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LorenisClothes.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "Talla",
                table: "Productos",
                newName: "TallasTexto");

            migrationBuilder.AlterColumn<string>(
                name: "ImagenUrl",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TallasTexto",
                table: "Productos",
                newName: "Talla");

            migrationBuilder.AlterColumn<string>(
                name: "ImagenUrl",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
