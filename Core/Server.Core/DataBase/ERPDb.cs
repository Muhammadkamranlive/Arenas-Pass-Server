using System;
using Server.Domain;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Server.Domain.PassTransmission;
using Server.Domain.DigitalPass.Transaction;
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

        
        public virtual DbSet<PasswordResetDomain>  PasswordResetDomains                       { get; set; }
        public virtual DbSet<CONTACTDETAILS>       CONTACTDETAILs                             { get; set; }
        public virtual DbSet<EmergencyContacts>    EmergencyContacts                          { get; set; }
        public virtual DbSet<Case>                 Cases                                      { get; set; }
        public virtual DbSet<CaseComment>          CaseComments                               { get; set; }
        public virtual DbSet<Asset>                Assets                                     { get; set; }
        public virtual DbSet<Attachments>          Attachments                                { get; set; }
        public virtual DbSet<HRNotes>              HRNotes                                    { get; set; }
        public virtual DbSet<NOTIFICATIONS>        NOTIFICATIONs                              { get; set; }
        public virtual DbSet<CandidateInfo>        CandidateInfos                             { get; set; }
        public virtual DbSet<Education>            Educations                                 { get; set; }
        public virtual DbSet<JobExperience>        JobExperiences                             { get; set; }
        public virtual DbSet<Personal>             Personals                                  { get; set; }
        public virtual DbSet<ProfessionalLicense>  ProfessionalLicenses                       { get; set; }
        public virtual DbSet<GENERALTASK>          GENERALTASKs                               { get; set; }
        public virtual DbSet<Dependent>            Dependents                                 { get; set; }
        public virtual DbSet<ZoomMeetings>         ZoomMeetings                               { get; set; }
        public virtual DbSet<WebPages>             WebPages                                   { get; set; }
        public virtual DbSet<BlogPage>             BlogPages                                  { get; set; }
        public virtual DbSet<Trainings>            Trainings                                  { get; set; }
        public virtual DbSet<ContactPage>          ContactPages                               { get; set; }
        public virtual DbSet<ArenasTenants>        PelicanHRMTenants                          { get; set; }
        public virtual DbSet<AdminLogs>            Logs                                       { get; set; }
        public virtual DbSet<Designations>         Designations                               { get; set; }
        public virtual DbSet<Chat>                 Chats                                      { get; set; }
        public virtual DbSet<WalletPass>           WalletPasses                               { get; set; }
        public virtual DbSet<Apple_Pass_Account>   Apple_Pass_Accounts                        { get; set; }
        public virtual DbSet<Transaction_No>       Transaction_No                             { get; set; }
        public virtual DbSet<TenantApiHitsHistory> TenantApiHitsHistories                     { get; set; }
        public virtual DbSet<TenantKeyHistory>     TenantKeyHistories                         { get; set; }
        public virtual DbSet<TenantLicenes>        TenantLicenes                              { get; set; }
        public virtual DbSet<UserVoucher>          UserVouchers                               { get; set; }
        public virtual DbSet<Account_Transaction>  Account_Transactions                       { get; set; }
        public virtual DbSet<Account_Balance>      Account_Balance                            { get; set; }
        public virtual DbSet<Pass_Transmission>    Pass_Transmission                          { get; set; }
        public virtual DbSet<Vault>                Vault                                  { get; set; }
        public virtual DbSet<ArenasBilling>        ArenasBillings                             { get; set; }
        public virtual DbSet<PaymentFeatureList>   PaymentFeatureLists                        { get; set; }
        public virtual DbSet<Payment_Plans>        Payment_Plans                              { get; set; }
        public virtual DbSet<TenantCharges>        TenantCharges                              { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<GiftCard>("GiftCard");
            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<Voucher>("Voucher");
            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<Coupon>("Coupon");

            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<EventTicket>("EventTicket");

            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<LoyaltyCard>("LoyaltyCard");

            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<MembershipCard>("MembershipCard");

            modelBuilder.Entity<WalletPass>().HasDiscriminator<string>("Type")
           .HasValue<BoardingPass>("BoardingPass");


            modelBuilder.Entity<PunchCard>().HasDiscriminator<string>("Type")
           .HasValue<PunchCard>("PunchCard");

           
            // Configure precision for Balance property
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

                var entity = modelBuilder.Entity(entityType.ClrType);

                // Apply decimal precision (18,2)
                foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                {
                    entity.Property(property.Name).HasColumnType("decimal(18,2)");
                }

                // Apply ValueGeneratedOnAdd for integer primary keys
                var intIdProperty = entityType.GetProperties()
                 .FirstOrDefault(p => p.Name == "Id" && p.ClrType == typeof(int));

                if (intIdProperty != null)
                {
                    entity.Property("Id").ValueGeneratedOnAdd();
                }

            }



        }



    }

}