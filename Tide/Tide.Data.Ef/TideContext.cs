using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models;
using Tide.Data.Models.Capabilities;
using Tide.Data.Models.Charts.Anomalies;
using Tide.Data.Models.Domains;
using Tide.Data.Models.Duties;
using Tide.Data.Models.FA;
using Tide.Data.Models.Issues;
using Tide.Data.Models.Nato;
using Tide.Data.Models.Objectives;
using Tide.Data.Models.Standards;
using Tide.Data.Models.Tcs;
using Tide.Data.Models.Tts;

namespace Tide.Data.Ef
{
    public class TideContext : DbContext
    {
        public TideContext() : base() { }
        public TideContext(DbContextOptions<TideContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(1000);
        }

        public DbSet<Nation> Nations { get; set; } = null!;
        public DbSet<Standard> Standards { get; set; } = null!;
        public DbSet<Capability> Capabilities { get; set; } = null!;
        public DbSet<CapabilityCycle> CapabilityCicles { get; set; } = null!;
        public DbSet<Objective> Objectives { get; set; } = null!;
        public DbSet<ObjectiveCycle> ObjectiveCycles { get; set; } = null!;
        public DbSet<FocusArea> FocusAreas { get; set; } = null!;
        public DbSet<FocusAreaCycle> FocusAreaCycles { get; set; } = null!;
        public DbSet<OperationalDomain> OperationalDomains { get; set; } = null!;
        public DbSet<Duty> Duties { get; set; } = null!;
        public DbSet<IssueCategory> Issues { get; set; } = null!;
        public DbSet<TestCaseParticipant> Participants { get; set; } = null!;
        public DbSet<TestCase> Tests { get; set; } = null!;
        public DbSet<TestTemplate> Templates { get; set; } = null!;
        public DbSet<TestTemplateCycle> TemplateCycles { get; set; } = null!;
        public DbSet<TestTemplateResult> TemplateResults { get; set; } = null!;
        public DbSet<TestTemplateDescription> TemplateDescriptions { get; set; } = null!;
        public DbSet<Ndpp> Ndpps { get; set; } = null!;

        public DbSet<TtYearAnomaly> TtYearAnomalies { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OperationalDomainCapabilityMap>().HasOne(x => x.Domain).WithMany(x => x.Capabilities).HasForeignKey(x => x.DomainId);
            builder.Entity<OperationalDomainCapabilityMap>().HasOne(x => x.Capability).WithMany(x => x.Domains).HasForeignKey(x => x.CapabilityId);
            builder.Entity<CapabilityFaMap>().HasOne(x => x.Capability).WithMany(x => x.Fas).HasForeignKey(x => x.CapabilityId);
            builder.Entity<CapabilityFaMap>().HasOne(x => x.FocusArea).WithMany(x => x.Capabilities).HasForeignKey(x => x.FaId);
            builder.Entity<DutyCapabilityMap>().HasOne(x => x.Capability).WithMany(x => x.Duties).HasForeignKey(x => x.CapabilityId);
            builder.Entity<DutyCapabilityMap>().HasOne(x => x.Duty).WithMany(x => x.Capabilities).HasForeignKey(x => x.DutyId);
            builder.Entity<IssueTestCaseMap>().HasOne(x => x.Test).WithMany(x => x.Issues).HasForeignKey(x => x.TestId);
            builder.Entity<IssueTestCaseMap>().HasOne(x => x.Issue).WithMany(x => x.Tests).HasForeignKey(x => x.IssueId);
            builder.Entity<ObjectiveFaMap>().HasOne(x => x.FocusArea).WithMany(x => x.Objectives).HasForeignKey(x => x.FaId);
            builder.Entity<ObjectiveFaMap>().HasOne(x => x.Objective).WithMany(x => x.Fas).HasForeignKey(x => x.ObjectiveId);
            builder.Entity<StandardCapabilityMap>().HasOne(x => x.Standard).WithMany(x => x.Capabilities).HasForeignKey(x => x.StandardId);
            builder.Entity<StandardCapabilityMap>().HasOne(x => x.Capability).WithMany(x => x.Standards).HasForeignKey(x => x.CapabilityId);
            builder.Entity<StandardObjectiveMap>().HasOne(x => x.Standard).WithMany(x => x.Objectives).HasForeignKey(x => x.StandardId);
            builder.Entity<StandardObjectiveMap>().HasOne(x => x.Objective).WithMany(x => x.Standards).HasForeignKey(x => x.ObjectiveId);
            builder.Entity<StandardTtMap>().HasOne(x => x.Standard).WithMany(x => x.Templates).HasForeignKey(x => x.StandardId);
            builder.Entity<StandardTtMap>().HasOne(x => x.TestTemplate).WithMany(x => x.Standards).HasForeignKey(x => x.TestTemplateId);
            builder.Entity<TestCaseParticipant>().HasOne(x => x.Test).WithMany(x => x.Participants).HasForeignKey(x => x.TestId);
            builder.Entity<TestCaseParticipant>().HasOne(x => x.Capability).WithMany(x => x.Tests).HasForeignKey(x => x.CapabilityId);
            builder.Entity<ObjectiveCapabilityMap>().HasOne(x => x.Objective).WithMany(x => x.Capabilities).HasForeignKey(x => x.ObjectiveId);
            builder.Entity<ObjectiveCapabilityMap>().HasOne(x => x.Capability).WithMany(x => x.Objectives).HasForeignKey(x => x.CapabilityId);
            builder.Entity<ObjectiveTtMap>().HasOne(x => x.Objective).WithMany(x => x.Templates).HasForeignKey(x => x.ObjectiveId);
            builder.Entity<ObjectiveTtMap>().HasOne(x => x.Template).WithMany(x => x.Objectives).HasForeignKey(x => x.TemplateId);
            builder.Entity<ObjectiveTcMap>().HasOne(x => x.Objective).WithMany(x => x.Tests).HasForeignKey(x => x.ObjectiveId);
            builder.Entity<ObjectiveTcMap>().HasOne(x => x.Test).WithMany(x => x.Objectives).HasForeignKey(x => x.TestId);
            builder.Entity<TestTemplateCycle>().HasOne(x => x.Diffusion).WithMany(x => x.Duplicates).HasForeignKey(x => x.DiffusionId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TestCase>().HasOne(x => x.Template).WithMany(x => x.Tests).HasForeignKey(x => x.TemplateId);

            builder.Entity<TtYearAnomaly>().HasOne(x => x.Objective).WithMany().HasForeignKey(x => x.ObjectiveId);
            builder.Entity<TtYearAnomaly>().HasOne(x => x.Template).WithMany().HasForeignKey(x => x.TemplateId);
            builder.Entity<TtYearAnomaly>().HasOne(x => x.Fa).WithMany().HasForeignKey(x => x.FaId);

            builder.Entity<CapabilityDescription>().HasOne(x => x.Capability).WithOne(x => x.Description).HasForeignKey<CapabilityDescription>(x => x.CapabilityId);
            builder.Entity<ObjectiveDescription>().HasOne(x => x.Objective).WithOne(x => x.Description).HasForeignKey<ObjectiveDescription>(x => x.ObjectiveId);
            builder.Entity<TestTemplateDescription>().HasOne(x => x.Template).WithOne(x => x.Description).HasForeignKey<TestTemplateDescription>(x => x.TemplateId);
            builder.Entity<TestTemplateResult>().HasOne(x => x.Template).WithOne(x => x.Result).HasForeignKey<TestTemplateResult>(x => x.TemplateId);
        }
    }
}
