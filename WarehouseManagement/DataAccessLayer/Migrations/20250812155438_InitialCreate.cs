using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "warehouse");

            migrationBuilder.CreateTable(
                name: "client",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    archive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receipt_document",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("receipt_document_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resource",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    archive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("resource_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unit_measurement",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    archive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("unit_measurement_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "snipment_document",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    sign = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("snipment_document_pkey", x => x.id);
                    table.ForeignKey(
                        name: "snipment_document_client_id_fkey",
                        column: x => x.client_id,
                        principalSchema: "warehouse",
                        principalTable: "client",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "balance",
                schema: "warehouse",
                columns: table => new
                {
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    count = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("balance_pkey", x => new { x.resource_id, x.unit_id });
                    table.ForeignKey(
                        name: "balance_resource_id_fkey",
                        column: x => x.resource_id,
                        principalSchema: "warehouse",
                        principalTable: "resource",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "balance_unit_id_fkey",
                        column: x => x.unit_id,
                        principalSchema: "warehouse",
                        principalTable: "unit_measurement",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "receipt_resource",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    document_id = table.Column<int>(type: "integer", nullable: false),
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("receipt_resource_pkey", x => x.id);
                    table.ForeignKey(
                        name: "receipt_resource_document_id_fkey",
                        column: x => x.document_id,
                        principalSchema: "warehouse",
                        principalTable: "receipt_document",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "receipt_resource_resource_id_fkey",
                        column: x => x.resource_id,
                        principalSchema: "warehouse",
                        principalTable: "resource",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "receipt_resource_unit_id_fkey",
                        column: x => x.unit_id,
                        principalSchema: "warehouse",
                        principalTable: "unit_measurement",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "snipment_resource",
                schema: "warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    document_id = table.Column<int>(type: "integer", nullable: false),
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("snipment_resource_pkey", x => x.id);
                    table.ForeignKey(
                        name: "snipment_resource_document_id_fkey",
                        column: x => x.document_id,
                        principalSchema: "warehouse",
                        principalTable: "snipment_document",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "snipment_resource_resource_id_fkey",
                        column: x => x.resource_id,
                        principalSchema: "warehouse",
                        principalTable: "resource",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "snipment_resource_unit_id_fkey",
                        column: x => x.unit_id,
                        principalSchema: "warehouse",
                        principalTable: "unit_measurement",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "balance_id_unique",
                schema: "warehouse",
                table: "balance",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_balance_unit_id",
                schema: "warehouse",
                table: "balance",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "client_name_idx",
                schema: "warehouse",
                table: "client",
                column: "name")
                .Annotation("Npgsql:StorageParameter:deduplicate_items", "true");

            migrationBuilder.CreateIndex(
                name: "receipt_document_number_idx",
                schema: "warehouse",
                table: "receipt_document",
                column: "number")
                .Annotation("Npgsql:StorageParameter:deduplicate_items", "true");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_resource_document_id",
                schema: "warehouse",
                table: "receipt_resource",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_resource_resource_id",
                schema: "warehouse",
                table: "receipt_resource",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_resource_unit_id",
                schema: "warehouse",
                table: "receipt_resource",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "resource_name_idx",
                schema: "warehouse",
                table: "resource",
                column: "name")
                .Annotation("Npgsql:StorageParameter:deduplicate_items", "true");

            migrationBuilder.CreateIndex(
                name: "IX_snipment_document_client_id",
                schema: "warehouse",
                table: "snipment_document",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "snipment_document_number_idx",
                schema: "warehouse",
                table: "snipment_document",
                column: "number")
                .Annotation("Npgsql:StorageParameter:deduplicate_items", "true");

            migrationBuilder.CreateIndex(
                name: "IX_snipment_resource_document_id",
                schema: "warehouse",
                table: "snipment_resource",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_snipment_resource_resource_id",
                schema: "warehouse",
                table: "snipment_resource",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_snipment_resource_unit_id",
                schema: "warehouse",
                table: "snipment_resource",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "unit_measurement_name_idx",
                schema: "warehouse",
                table: "unit_measurement",
                column: "name")
                .Annotation("Npgsql:StorageParameter:deduplicate_items", "true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "balance",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "receipt_resource",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "snipment_resource",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "receipt_document",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "snipment_document",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "resource",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "unit_measurement",
                schema: "warehouse");

            migrationBuilder.DropTable(
                name: "client",
                schema: "warehouse");
        }
    }
}
