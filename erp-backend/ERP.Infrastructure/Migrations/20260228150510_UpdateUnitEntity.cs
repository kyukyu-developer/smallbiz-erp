using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUnitEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop all FK constraints that reference Units.Id
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_BaseUnitId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductUnitPrices_Units_UnitId",
                table: "ProductUnitPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitConversions_Units_FromUnitId",
                table: "UnitConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitConversions_Units_ToUnitId",
                table: "UnitConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_Units_UnitId",
                table: "PurchaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItems_Units_UnitId",
                table: "SalesItems");

            // Step 2: Add new columns FIRST — keeps the table non-empty when we drop old columns
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Units",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastAction",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Units",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Units",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            // Step 3: Drop old UnitName — new columns already exist so table is non-empty
            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "Units");

            // Step 4: Drop PK — required before dropping the PK column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            // Step 5: Drop the old int IDENTITY Id, add new nvarchar(50) Id
            // SQL Server cannot ALTER an IDENTITY PK column — must drop and re-add
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // Step 6: Restore PK
            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            // Step 7: Change FK columns to nvarchar(50) (FKs already dropped so ALTER works)
            migrationBuilder.AlterColumn<string>(
                name: "ToUnitId",
                table: "UnitConversions",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FromUnitId",
                table: "UnitConversions",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UnitId",
                table: "SalesItems",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UnitId",
                table: "PurchaseItems",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UnitId",
                table: "ProductUnitPrices",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "BaseUnitId",
                table: "Products",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Step 8: Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Unit_Active",
                table: "Units",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Name",
                table: "Units",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Symbol",
                table: "Units",
                column: "Symbol",
                unique: true);

            // Step 9: Re-add FK constraints (both sides are now nvarchar(50))
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_BaseUnitId",
                table: "Products",
                column: "BaseUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductUnitPrices_Units_UnitId",
                table: "ProductUnitPrices",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitConversions_Units_FromUnitId",
                table: "UnitConversions",
                column: "FromUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitConversions_Units_ToUnitId",
                table: "UnitConversions",
                column: "ToUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_Units_UnitId",
                table: "PurchaseItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItems_Units_UnitId",
                table: "SalesItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop FK constraints
            migrationBuilder.DropForeignKey(name: "FK_Products_Units_BaseUnitId", table: "Products");
            migrationBuilder.DropForeignKey(name: "FK_ProductUnitPrices_Units_UnitId", table: "ProductUnitPrices");
            migrationBuilder.DropForeignKey(name: "FK_UnitConversions_Units_FromUnitId", table: "UnitConversions");
            migrationBuilder.DropForeignKey(name: "FK_UnitConversions_Units_ToUnitId", table: "UnitConversions");
            migrationBuilder.DropForeignKey(name: "FK_PurchaseItems_Units_UnitId", table: "PurchaseItems");
            migrationBuilder.DropForeignKey(name: "FK_SalesItems_Units_UnitId", table: "SalesItems");

            // Step 2: Drop indexes
            migrationBuilder.DropIndex(name: "IX_Unit_Active", table: "Units");
            migrationBuilder.DropIndex(name: "IX_Unit_Name", table: "Units");
            migrationBuilder.DropIndex(name: "IX_Unit_Symbol", table: "Units");

            // Step 3: Restore FK columns back to int
            migrationBuilder.AlterColumn<int>(
                name: "ToUnitId", table: "UnitConversions", type: "int",
                nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "FromUnitId", table: "UnitConversions", type: "int",
                nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId", table: "SalesItems", type: "int",
                nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId", table: "PurchaseItems", type: "int",
                nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId", table: "ProductUnitPrices", type: "int",
                nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "BaseUnitId", table: "Products", type: "int",
                nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)");

            // Step 4: Add UnitName back (keep table non-empty while we swap Id)
            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Step 5: Drop PK
            migrationBuilder.DropPrimaryKey(name: "PK_Units", table: "Units");

            // Step 6: Drop nvarchar(50) Id, re-add int IDENTITY Id
            migrationBuilder.DropColumn(name: "Id", table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Step 7: Restore PK
            migrationBuilder.AddPrimaryKey(name: "PK_Units", table: "Units", column: "Id");

            // Step 8: Drop new audit + business columns
            migrationBuilder.DropColumn(name: "Active", table: "Units");
            migrationBuilder.DropColumn(name: "CreatedAt", table: "Units");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "Units");
            migrationBuilder.DropColumn(name: "LastAction", table: "Units");
            migrationBuilder.DropColumn(name: "Name", table: "Units");
            migrationBuilder.DropColumn(name: "Symbol", table: "Units");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Units");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "Units");

            // Step 9: Restore FK constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_BaseUnitId", table: "Products",
                column: "BaseUnitId", principalTable: "Units", principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductUnitPrices_Units_UnitId", table: "ProductUnitPrices",
                column: "UnitId", principalTable: "Units", principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitConversions_Units_FromUnitId", table: "UnitConversions",
                column: "FromUnitId", principalTable: "Units", principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitConversions_Units_ToUnitId", table: "UnitConversions",
                column: "ToUnitId", principalTable: "Units", principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_Units_UnitId", table: "PurchaseItems",
                column: "UnitId", principalTable: "Units", principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItems_Units_UnitId", table: "SalesItems",
                column: "UnitId", principalTable: "Units", principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
