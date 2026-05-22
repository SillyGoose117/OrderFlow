using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderFlow.Console.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AmountLeft",
                table: "Products",
                newName: "Stock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "AmountLeft");
        }
    }
}
