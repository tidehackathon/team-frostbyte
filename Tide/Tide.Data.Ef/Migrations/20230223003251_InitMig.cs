using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tide.Data.Ef.Migrations
{
    public partial class InitMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Duties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FocusAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objectives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationalDomains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalDomains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Standards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FocusAreaCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    FocusAreaId = table.Column<int>(type: "int", nullable: false),
                    Interoperability = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusAreaCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FocusAreaCycles_FocusAreas_FocusAreaId",
                        column: x => x.FocusAreaId,
                        principalTable: "FocusAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Capabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Capabilities_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InteroperabilityScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Scope = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveCycles_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardTtMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    TestTemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardTtMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandardTtMap_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardTtMap_Templates_TestTemplateId",
                        column: x => x.TestTemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TestTemplateId = table.Column<int>(type: "int", nullable: false),
                    DiffusionId = table.Column<int>(type: "int", nullable: true),
                    Similarity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiffusionSimilarity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TestsCount = table.Column<int>(type: "int", nullable: false),
                    Anomaly = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateCycles_TemplateCycles_DiffusionId",
                        column: x => x.DiffusionId,
                        principalTable: "TemplateCycles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemplateCycles_Templates_TestTemplateId",
                        column: x => x.TestTemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preconditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateDescriptions_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Success = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Limited = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interoperability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateResults_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CapabilityCicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SuccessRate = table.Column<int>(type: "int", nullable: false),
                    FailureRate = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CurrentInteroperability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaseInteroperability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapabilityCicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapabilityCicles_Capabilities_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "Capabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveDescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveDescription_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveFaMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveFaMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveFaMap_FocusAreaCycles_FaId",
                        column: x => x.FaId,
                        principalTable: "FocusAreaCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveFaMap_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardObjectiveMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardObjectiveMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandardObjectiveMap_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardObjectiveMap_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveTtMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveTtMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveTtMap_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveTtMap_TemplateCycles_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "TemplateCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    Shortfall = table.Column<bool>(type: "bit", nullable: false),
                    ParticipantsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_TemplateCycles_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "TemplateCycles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TtYearAnomalies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaId = table.Column<int>(type: "int", nullable: false),
                    FaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TtYearAnomalies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TtYearAnomalies_FocusAreas_FaId",
                        column: x => x.FaId,
                        principalTable: "FocusAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TtYearAnomalies_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TtYearAnomalies_TemplateCycles_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "TemplateCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CapabilityDescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapabilityDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapabilityDescription_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CapabilityFaMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaId = table.Column<int>(type: "int", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapabilityFaMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapabilityFaMap_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapabilityFaMap_FocusAreaCycles_FaId",
                        column: x => x.FaId,
                        principalTable: "FocusAreaCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DutyCapabilityMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DutyId = table.Column<int>(type: "int", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyCapabilityMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DutyCapabilityMap_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyCapabilityMap_Duties_DutyId",
                        column: x => x.DutyId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveCapabilityMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CapabilityId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    InteroperabilityScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveCapabilityMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveCapabilityMap_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveCapabilityMap_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationalDomainCapabilityMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalDomainCapabilityMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationalDomainCapabilityMap_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationalDomainCapabilityMap_OperationalDomains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "OperationalDomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardCapabilityMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false),
                    InteroperabilityScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardCapabilityMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandardCapabilityMap_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardCapabilityMap_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueTestCaseMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTestCaseMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueTestCaseMap_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueTestCaseMap_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveTcMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveTcMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveTcMap_ObjectiveCycles_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "ObjectiveCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveTcMap_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    CapabilityId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_CapabilityCicles_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CapabilityCicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capabilities_NationId",
                table: "Capabilities",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityCicles_CapabilityId",
                table: "CapabilityCicles",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityDescription_CapabilityId",
                table: "CapabilityDescription",
                column: "CapabilityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityFaMap_CapabilityId",
                table: "CapabilityFaMap",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityFaMap_FaId",
                table: "CapabilityFaMap",
                column: "FaId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyCapabilityMap_CapabilityId",
                table: "DutyCapabilityMap",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyCapabilityMap_DutyId",
                table: "DutyCapabilityMap",
                column: "DutyId");

            migrationBuilder.CreateIndex(
                name: "IX_FocusAreaCycles_FocusAreaId",
                table: "FocusAreaCycles",
                column: "FocusAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTestCaseMap_IssueId",
                table: "IssueTestCaseMap",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTestCaseMap_TestId",
                table: "IssueTestCaseMap",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveCapabilityMap_CapabilityId",
                table: "ObjectiveCapabilityMap",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveCapabilityMap_ObjectiveId",
                table: "ObjectiveCapabilityMap",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveCycles_ObjectiveId",
                table: "ObjectiveCycles",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveDescription_ObjectiveId",
                table: "ObjectiveDescription",
                column: "ObjectiveId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveFaMap_FaId",
                table: "ObjectiveFaMap",
                column: "FaId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveFaMap_ObjectiveId",
                table: "ObjectiveFaMap",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveTcMap_ObjectiveId",
                table: "ObjectiveTcMap",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveTcMap_TestId",
                table: "ObjectiveTcMap",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveTtMap_ObjectiveId",
                table: "ObjectiveTtMap",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveTtMap_TemplateId",
                table: "ObjectiveTtMap",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalDomainCapabilityMap_CapabilityId",
                table: "OperationalDomainCapabilityMap",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalDomainCapabilityMap_DomainId",
                table: "OperationalDomainCapabilityMap",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CapabilityId",
                table: "Participants",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_TestId",
                table: "Participants",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardCapabilityMap_CapabilityId",
                table: "StandardCapabilityMap",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardCapabilityMap_StandardId",
                table: "StandardCapabilityMap",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardObjectiveMap_ObjectiveId",
                table: "StandardObjectiveMap",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardObjectiveMap_StandardId",
                table: "StandardObjectiveMap",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTtMap_StandardId",
                table: "StandardTtMap",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTtMap_TestTemplateId",
                table: "StandardTtMap",
                column: "TestTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCycles_DiffusionId",
                table: "TemplateCycles",
                column: "DiffusionId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCycles_TestTemplateId",
                table: "TemplateCycles",
                column: "TestTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDescriptions_TemplateId",
                table: "TemplateDescriptions",
                column: "TemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateResults_TemplateId",
                table: "TemplateResults",
                column: "TemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TemplateId",
                table: "Tests",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TtYearAnomalies_FaId",
                table: "TtYearAnomalies",
                column: "FaId");

            migrationBuilder.CreateIndex(
                name: "IX_TtYearAnomalies_ObjectiveId",
                table: "TtYearAnomalies",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_TtYearAnomalies_TemplateId",
                table: "TtYearAnomalies",
                column: "TemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CapabilityDescription");

            migrationBuilder.DropTable(
                name: "CapabilityFaMap");

            migrationBuilder.DropTable(
                name: "DutyCapabilityMap");

            migrationBuilder.DropTable(
                name: "IssueTestCaseMap");

            migrationBuilder.DropTable(
                name: "ObjectiveCapabilityMap");

            migrationBuilder.DropTable(
                name: "ObjectiveDescription");

            migrationBuilder.DropTable(
                name: "ObjectiveFaMap");

            migrationBuilder.DropTable(
                name: "ObjectiveTcMap");

            migrationBuilder.DropTable(
                name: "ObjectiveTtMap");

            migrationBuilder.DropTable(
                name: "OperationalDomainCapabilityMap");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "StandardCapabilityMap");

            migrationBuilder.DropTable(
                name: "StandardObjectiveMap");

            migrationBuilder.DropTable(
                name: "StandardTtMap");

            migrationBuilder.DropTable(
                name: "TemplateDescriptions");

            migrationBuilder.DropTable(
                name: "TemplateResults");

            migrationBuilder.DropTable(
                name: "TtYearAnomalies");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "FocusAreaCycles");

            migrationBuilder.DropTable(
                name: "OperationalDomains");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "CapabilityCicles");

            migrationBuilder.DropTable(
                name: "Standards");

            migrationBuilder.DropTable(
                name: "ObjectiveCycles");

            migrationBuilder.DropTable(
                name: "FocusAreas");

            migrationBuilder.DropTable(
                name: "TemplateCycles");

            migrationBuilder.DropTable(
                name: "Capabilities");

            migrationBuilder.DropTable(
                name: "Objectives");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Nations");
        }
    }
}
