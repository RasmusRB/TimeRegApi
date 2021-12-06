using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TimeReg_Api.Migrations
{
    public partial class create_activity_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.CreateTable(
              name: "activities",
              columns: table => new
              {
                  id = table.Column<long>(type: "bigint", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  activity = table.Column<string>(type: "text", nullable: true),
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Activity", x => x.id);
              }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
