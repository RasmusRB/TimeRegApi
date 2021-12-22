using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TimeReg_Api.Migrations
{
    public partial class create_timeregistration_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "timeregistration",
                columns: table => new 
                {
                    timereg_id = table.Column<long>(type: "bigint", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timereg_created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    timereg_start = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    timereg_end = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    timereg_comment = table.Column<string>(type: "text", nullable: true),
                    activity_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_TimeReg", x => x.timereg_id);
                }
            );

            // Adding foreign keys
            migrationBuilder.AddForeignKey(
                name: "fk_activity_id",             // name of foreign key
                table: "activities",                // table it refers to
                column: "id",                       // name in other table
                principalTable: "activities",       // table which it is constrained to
                principalColumn: "id",              // column in that table
                onDelete: ReferentialAction.Cascade // If record in parent table is deleted => Will delete record in child table
            );

            migrationBuilder.AddForeignKey(
                name: "fk_user_id",                 
                table: "users",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Timeregistration");
        }
    }
}
