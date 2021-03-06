using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TimeReg_Api.Migrations
{
    public partial class create_user_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create table in db
            migrationBuilder.CreateTable(
              name: "users",
              columns: table => new
              {
                  // Define columns
                  id = table.Column<long>(type: "bigint", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  email = table.Column<string>(type: "text", nullable: true),
                  password = table.Column<string>(type: "text", nullable: true),
                  firstname = table.Column<string>(type: "text", nullable: true),
                  lastname = table.Column<string>(type: "text", nullable: true),
                  telephone = table.Column<string>(type: "text", nullable: true),
                  isadmin = table.Column<bool>(type: "bool", nullable: false),

              },
              // Add constraints if necessary
              constraints: table =>
              {
                  table.PrimaryKey("PK_User", x => x.id);
                  table.UniqueConstraint("Unique_email", x => x.email);
              }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
