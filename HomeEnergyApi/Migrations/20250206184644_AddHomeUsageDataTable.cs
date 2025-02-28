using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEnergyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeUsageDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyElectricUsage",
                table: "Homes");

            migrationBuilder.CreateTable(
                name: "HomeUsageDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonthlyElectricUsage = table.Column<int>(type: "INTEGER", nullable: false),
                    HomeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeUsageDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeUsageDatas_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeUsageDatas_HomeId",
                table: "HomeUsageDatas",
                column: "HomeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeUsageDatas");

            migrationBuilder.AddColumn<int>(
                name: "MonthlyElectricUsage",
                table: "Homes",
                type: "INTEGER",
                nullable: true);
        }
    }
}
