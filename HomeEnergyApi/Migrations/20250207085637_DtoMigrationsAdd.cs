using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEnergyApi.Migrations
{
    /// <inheritdoc />
    public partial class DtoMigrationsAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvidedUtilities",
                table: "UtilityProviders");

            migrationBuilder.DropColumn(
                name: "HasSolar",
                table: "HomeUsageDatas");

            migrationBuilder.AddColumn<string>(
                name: "ProvidedUtility",
                table: "UtilityProviders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MonthlyElectricUsage",
                table: "HomeUsageDatas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvidedUtility",
                table: "UtilityProviders");

            migrationBuilder.AddColumn<string>(
                name: "ProvidedUtilities",
                table: "UtilityProviders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "MonthlyElectricUsage",
                table: "HomeUsageDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSolar",
                table: "HomeUsageDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
