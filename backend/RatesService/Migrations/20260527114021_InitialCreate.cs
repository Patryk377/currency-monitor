using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RatesService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Mid = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Code_EffectiveDate",
                table: "ExchangeRates",
                columns: new[] { "Code", "EffectiveDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRates");
        }
    }
}
