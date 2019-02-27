using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Product.WebApi.Migrations
{
    public partial class RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Products",
                rowVersion: true,
                nullable: true);

            migrationBuilder.Sql(
                @"
        CREATE TRIGGER SetProductRowVersionOnUpdate
        AFTER UPDATE ON Products
        BEGIN
            UPDATE Products
            SET RowVersion = randomblob(8)
            WHERE rowid = NEW.rowid;
        END
        ");
            migrationBuilder.Sql(
                @"
        CREATE TRIGGER SetProductRowVersionOnInsert
        AFTER INSERT ON Products
        BEGIN
            UPDATE Products
            SET RowVersion = randomblob(8)
            WHERE rowid = NEW.rowid;
        END
        ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Products");
        }
    }
}
