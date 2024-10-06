using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFeaturedToProduct_AndEditTrackingNumberName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrakcingNumber",
                table: "OrderHeaders",
                newName: "TrackingNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "TrackingNumber",
                table: "OrderHeaders",
                newName: "TrakcingNumber");
        }
    }
}
