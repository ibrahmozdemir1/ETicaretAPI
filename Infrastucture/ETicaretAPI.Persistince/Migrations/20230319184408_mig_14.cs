using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretAPI.Persistince.Migrations
{
    public partial class mig_14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Baskets_BasketId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BasketId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Baskets_Id",
                table: "Orders",
                column: "Id",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Baskets_Id",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BasketId",
                table: "Orders",
                column: "BasketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Baskets_BasketId",
                table: "Orders",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
