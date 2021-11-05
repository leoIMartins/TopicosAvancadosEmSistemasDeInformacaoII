using Microsoft.EntityFrameworkCore.Migrations;

namespace VendaIngressos.Data.Migrations
{
    public partial class inclusaodeatributo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Ingresso",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Ingresso");
        }
    }
}
