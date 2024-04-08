using Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Server.Core
{
    public class ERPDb : IdentityDbContext<ApplicationUser, CustomRole, string>
    {
        public ERPDb
        (
            DbContextOptions<ERPDb> dbContextOptions
        ) : base(dbContextOptions)
        {

           


        }
        public virtual DbSet<PasswordResetDomain> PasswordResetDomains { get; set; }
        public virtual DbSet<CONTACTDETAILS> CONTACTDETAILs { get; set; }
        public virtual DbSet<EmergencyContacts> EmergencyContacts { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseComment> CaseComments { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Attachments> Attachments { get; set; }
        public virtual DbSet<HRNotes> HRNotes { get; set; }
        public virtual DbSet<NOTIFICATIONS> NOTIFICATIONs { get; set; }
        public virtual DbSet<CandidateInfo> CandidateInfos { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<JobExperience> JobExperiences { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<ProfessionalLicense> ProfessionalLicenses { get; set; }
        public virtual DbSet<GENERALTASK> GENERALTASKs { get; set; }
        public virtual DbSet<Dependent> Dependents { get; set; }
        public virtual DbSet<ZoomMeetings> ZoomMeetings { get; set; }
        public virtual DbSet<WebPages> WebPages { get; set; }
        public virtual DbSet<BlogPage> BlogPages { get; set; }
        public virtual DbSet<Trainings> Trainings { get; set; }
        public virtual DbSet<ContactPage> ContactPages { get; set; }
        public virtual DbSet<PelicanHRMTenant> PelicanHRMTenants { get; set; }
        public virtual DbSet<AdminLogs> Logs            { get; set; }
        public virtual DbSet<Designations> Designations { get; set; }
        public virtual DbSet<Chat> Chats     { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogPage>()
               .Property(w => w.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<ContactPage>()
               .Property(w => w.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<WebPages>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Trainings>()
               .Property(w => w.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<PelicanHRMTenant>()
               .Property(w => w.CompanyId)
               .ValueGeneratedOnAdd();
           
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {

                modelBuilder.Entity<CustomRole>(entity =>
                {
                    entity.Property(r => r.Permissions).HasMaxLength(255);
                });

                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null && idProperty.ClrType == typeof(Guid))
                {

                    modelBuilder.Entity(entityType.Name)
                        .HasKey("Id")
                        .HasName($"PK_{entityType.Name}_Id");


                    modelBuilder.Entity(entityType.Name)
                        .Property<Guid>("Id")
                        .ValueGeneratedOnAdd();
                    modelBuilder.Entity<Asset>()
                   .Property(a => a.Price)
                   .HasPrecision(18, 2);
                }
            }



        }



    }

}