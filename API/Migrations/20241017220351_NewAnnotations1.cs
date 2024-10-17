using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class NewAnnotations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_t_e_serie_ser_serie",
                schema: "public",
                table: "t_e_serie_ser",
                newName: "IX_t_e_serie_ser_ser_titre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_t_e_serie_ser_ser_titre",
                schema: "public",
                table: "t_e_serie_ser",
                newName: "ix_t_e_serie_ser_serie");
        }
    }
}
