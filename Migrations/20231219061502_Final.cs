using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceManageGroup.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "EmployeeSequence",
                schema: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "ManagerSequence",
                schema: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "RecruiterSequence",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "EmployeeDetails",
                columns: table => new
                {
                    employeeId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "CONCAT('23EM', RIGHT('00' + CAST(NEXT VALUE FOR EmployeeSequence AS VARCHAR(2)), 2))"),
                    employeeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    employeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeePassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeAge = table.Column<int>(type: "int", nullable: false),
                    employeeVacationStartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeVacationEndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeVacationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    employeeTrainingStartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeTrainingEndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeWorkingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeVacationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeTechnology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeTrainerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeProject = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDetails", x => x.employeeId);
                });

            migrationBuilder.CreateTable(
                name: "ManagerDetails",
                columns: table => new
                {
                    managerId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "CONCAT('23PM', RIGHT('00' + CAST(NEXT VALUE FOR ManagerSequence AS VARCHAR(2)), 2))"),
                    managerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    managerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    managerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    managerPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerDetails", x => x.managerId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDetails",
                columns: table => new
                {
                    projectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    projectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    projectStartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    projectEndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    projectLead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    projectType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDetails", x => x.projectId);
                });

            migrationBuilder.CreateTable(
                name: "RecruiterDetails",
                columns: table => new
                {
                    recruiterId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "CONCAT('23HR', RIGHT('00' + CAST(NEXT VALUE FOR RecruiterSequence AS VARCHAR(2)), 2))"),
                    recruiterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recruiterEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recruiterNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recruiterPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruiterDetails", x => x.recruiterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDetails");

            migrationBuilder.DropTable(
                name: "ManagerDetails");

            migrationBuilder.DropTable(
                name: "ProjectDetails");

            migrationBuilder.DropTable(
                name: "RecruiterDetails");

            migrationBuilder.DropSequence(
                name: "EmployeeSequence",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "ManagerSequence",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "RecruiterSequence",
                schema: "dbo");
        }
    }
}
