IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Permissions] nvarchar(255) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [MiddleName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NOT NULL,
        [image] nvarchar(max) NULL,
        [isAdmin] bit NOT NULL,
        [isEmployee] bit NOT NULL,
        [defaultPassword] nvarchar(max) NULL,
        [EmployeeId] int NOT NULL,
        [CompanyName] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        [CompanyDesignation] nvarchar(max) NOT NULL,
        [LoginRestEnable] bit NOT NULL,
        [SubscriptionName] nvarchar(max) NOT NULL,
        [EmployeeSubscriptionName] nvarchar(max) NOT NULL,
        [EmployeePaymentSubscription] nvarchar(max) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Assets] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [ItemName] nvarchar(max) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
        [Manufacturer] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [ItemCode] nvarchar(max) NOT NULL,
        [ModelNo] nvarchar(max) NOT NULL,
        [SerialOrLicenseNo] nvarchar(max) NOT NULL,
        [ExpiryDate] datetime2 NULL,
        [WarrantyTill] datetime2 NULL,
        [Note] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Asset_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Attachments] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [DocType] nvarchar(max) NOT NULL,
        [DocPlacementType] nvarchar(max) NOT NULL,
        [DocName] nvarchar(max) NOT NULL,
        [DocumentUrl] nvarchar(max) NOT NULL,
        [SenderId] nvarchar(max) NULL,
        [ReceiverId] nvarchar(max) NULL,
        [Status] nvarchar(max) NULL,
        [state] nvarchar(max) NULL,
        [ExpDate] datetime2 NULL,
        [IssueDate] datetime2 NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Attachments_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [BlogPages] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [MetaDescription] nvarchar(max) NULL,
        [image] nvarchar(max) NULL,
        [userId] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.BlogPage_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [CandidateInfos] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [OrganizationName] nvarchar(max) NOT NULL,
        [BusinessType] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Website] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [ContactName] nvarchar(max) NOT NULL,
        [Position] nvarchar(max) NOT NULL,
        [BusinessEmail] nvarchar(max) NOT NULL,
        [PersonalEmail] nvarchar(max) NOT NULL,
        [MobilePhone] nvarchar(max) NOT NULL,
        [OfficePhone] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.CandidateInfo_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [CaseComments] (
        [Id] uniqueidentifier NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CaseId] uniqueidentifier NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.CaseComment_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Cases] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [CustomerId] nvarchar(max) NOT NULL,
        [AgentId] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Case_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Chats] (
        [Id] uniqueidentifier NOT NULL,
        [SenderId] nvarchar(max) NOT NULL,
        [ReceiverId] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Timestamp] datetime2 NOT NULL,
        [Role] nvarchar(max) NULL,
        CONSTRAINT [PK_Server.Domain.Chat_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [CONTACTDETAILs] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [HomePhone] nvarchar(max) NOT NULL,
        [MobilePhone] nvarchar(max) NOT NULL,
        [Carrier] nvarchar(max) NOT NULL,
        [PersonalEmail] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.CONTACTDETAILS_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [ContactPages] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [email] nvarchar(max) NOT NULL,
        [description] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        [createAt] datetime2 NULL,
        CONSTRAINT [PK_Server.Domain.ContactPage_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Dependents] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Relationship] nvarchar(max) NOT NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [HasSpecialNeeds] bit NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Dependent_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Designations] (
        [Id] uniqueidentifier NOT NULL,
        [Designation] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Designations_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Educations] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [SchoolName] nvarchar(max) NOT NULL,
        [Degree] nvarchar(max) NOT NULL,
        [FieldOfStudy] nvarchar(max) NOT NULL,
        [YearOfCompletion] int NOT NULL,
        [GPA] float NOT NULL,
        [Interests] nvarchar(max) NULL,
        [Notes] nvarchar(max) NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Education_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [EmergencyContacts] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Relationship] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [IsPrimary] bit NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.EmergencyContacts_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [GENERALTASKs] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [DueDate] datetime2 NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [Progress] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.GENERALTASK_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [HRNotes] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [HRId] nvarchar(max) NOT NULL,
        [Note] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.HRNotes_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [JobExperiences] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [PreviousCompany] nvarchar(max) NOT NULL,
        [PreviousCompanyAddress] nvarchar(max) NOT NULL,
        [JobTitle] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FromDate] datetime2 NOT NULL,
        [ToDate] datetime2 NOT NULL,
        [JobDescription] nvarchar(max) NULL,
        [ReasonForLeaving] nvarchar(max) NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.JobExperience_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Logs] (
        [Id] uniqueidentifier NOT NULL,
        [OperationType] nvarchar(max) NOT NULL,
        [EntityType] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Timestamp] datetime2 NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.AdminLogs_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [NOTIFICATIONs] (
        [Id] uniqueidentifier NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [Timestamp] datetime2 NOT NULL,
        [IsRead] bit NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [WorkflowStep] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.NOTIFICATIONS_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [PasswordResetDomains] (
        [Id] uniqueidentifier NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [OTP] nvarchar(max) NOT NULL,
        [ExpireTime] datetime2 NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.PasswordResetDomain_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [PelicanHRMTenants] (
        [Id] uniqueidentifier NOT NULL,
        [CompanyName] nvarchar(max) NOT NULL,
        [CompanyId] int NOT NULL IDENTITY,
        [PhoneNumber] nvarchar(max) NULL,
        [AddressLine1] nvarchar(max) NULL,
        [AddressLine2] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [State] nvarchar(max) NULL,
        [Country] nvarchar(max) NULL,
        [ZipCode] nvarchar(max) NULL,
        [Longitude] decimal(18,2) NULL,
        [Latitude] decimal(18,2) NULL,
        [isMailingAddress] nvarchar(max) NULL,
        [isPhysicalAddress] nvarchar(max) NULL,
        [whosisCompany] nvarchar(max) NULL,
        [noDomesticEmployee] int NULL,
        [noInterEmployee] int NULL,
        [noDomesticContractor] int NULL,
        [noInterContractor] int NULL,
        [EIN] nvarchar(max) NULL,
        [BussinessType] nvarchar(max) NULL,
        [LegalName] nvarchar(max) NULL,
        [FillingFormIRS] nvarchar(max) NULL,
        [Industry] nvarchar(max) NULL,
        [ProfileStatus] nvarchar(max) NOT NULL,
        [CompanyStatus] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Server.Domain.PelicanHRMTenant_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Personals] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [DOB] datetime2 NOT NULL,
        [Age] int NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [Race] nvarchar(max) NOT NULL,
        [Ethnicity] nvarchar(max) NOT NULL,
        [Nationality] nvarchar(max) NOT NULL,
        [Photo] nvarchar(max) NOT NULL,
        [HomePhone] nvarchar(max) NOT NULL,
        [MobilePhone] nvarchar(max) NOT NULL,
        [Carrier] nvarchar(max) NOT NULL,
        [PersonalEmail] nvarchar(max) NOT NULL,
        [BusinessEmail] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [State] nvarchar(max) NOT NULL,
        [ZipCode] nvarchar(max) NOT NULL,
        [birthcountry] nvarchar(max) NOT NULL,
        [Country] nvarchar(max) NOT NULL,
        [BestTimeToContact] nvarchar(max) NOT NULL,
        [HowDidYouHearAboutUs] nvarchar(max) NOT NULL,
        [Specialty] nvarchar(max) NOT NULL,
        [TypeOfEmployment] nvarchar(max) NOT NULL,
        [YearsOfExperience] int NOT NULL,
        [ComputerChartingSystemExperience] nvarchar(max) NOT NULL,
        [DesiredTravelArea] nvarchar(max) NOT NULL,
        [LocationPreference] nvarchar(max) NOT NULL,
        [Position] nvarchar(max) NULL,
        [AcceptTermsAndConditions] bit NOT NULL,
        [SNumber] nvarchar(max) NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Personal_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [ProfessionalLicenses] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [LicenseName] nvarchar(max) NOT NULL,
        [LicenseType] nvarchar(max) NOT NULL,
        [StateOfLicense] nvarchar(max) NOT NULL,
        [LicenseNumber] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [IssueDate] datetime2 NOT NULL,
        [ExpirationDate] datetime2 NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.ProfessionalLicense_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [Trainings] (
        [Id] uniqueidentifier NOT NULL,
        [userId] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [TrainingType] nvarchar(max) NOT NULL,
        [Hours] nvarchar(max) NOT NULL,
        [Credits] nvarchar(max) NOT NULL,
        [Date] datetime2 NOT NULL,
        [endDate] datetime2 NULL,
        [School] nvarchar(max) NULL,
        [Notes] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.Trainings_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [WebPages] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [MetaDescription] nvarchar(max) NULL,
        [image] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [MainCategory] nvarchar(max) NOT NULL,
        [SubCategory] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.WebPages_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [ZoomMeetings] (
        [Id] uniqueidentifier NOT NULL,
        [MeetingNumber] nvarchar(max) NOT NULL,
        [password] nvarchar(max) NOT NULL,
        [start_time] datetime2 NOT NULL,
        [duration] int NOT NULL,
        [timezone] nvarchar(max) NOT NULL,
        [created_at] datetime2 NOT NULL,
        [start_url] nvarchar(max) NOT NULL,
        [join_url] nvarchar(max) NOT NULL,
        [Topic] nvarchar(max) NOT NULL,
        [email] nvarchar(max) NOT NULL,
        [recieverId] nvarchar(max) NOT NULL,
        [senderId] nvarchar(max) NOT NULL,
        [Date] datetime2 NOT NULL,
        [description] nvarchar(max) NOT NULL,
        [meetingstatus] nvarchar(max) NOT NULL,
        [meetingBanner] nvarchar(max) NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_Server.Domain.ZoomMeetings_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240928192739_InitialModel')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240928192739_InitialModel', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [PK_Server.Domain.PelicanHRMTenant_Id];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'AddressLine2');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [AddressLine2];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'BussinessType');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [BussinessType];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'EIN');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [EIN];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'FillingFormIRS');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [FillingFormIRS];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'Industry');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [Industry];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'Latitude');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [Latitude];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'LegalName');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [LegalName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'Longitude');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [Longitude];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'noDomesticContractor');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [noDomesticContractor];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'noDomesticEmployee');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [noDomesticEmployee];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'noInterContractor');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [noInterContractor];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'noInterEmployee');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [noInterEmployee];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PelicanHRMTenants]') AND [c].[name] = N'whosisCompany');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [PelicanHRMTenants] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [PelicanHRMTenants] DROP COLUMN [whosisCompany];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    ALTER TABLE [PelicanHRMTenants] ADD CONSTRAINT [PK_Server.Domain.ArenasTenants_Id] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241015185534_arenastenant')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241015185534_arenastenant', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241110091121_Added_Gift_Card')
BEGIN
    CREATE TABLE [WalletPasses] (
        [Id] int NOT NULL IDENTITY,
        [Type] nvarchar(max) NOT NULL,
        [Logo] nvarchar(max) NOT NULL,
        [Background_Color] nvarchar(max) NOT NULL,
        [Foreground_Color] nvarchar(max) NOT NULL,
        [Label_Color] nvarchar(max) NOT NULL,
        [Organization_Name] nvarchar(max) NOT NULL,
        [Serial_Number] nvarchar(max) NOT NULL,
        [Localized_Name] nvarchar(max) NOT NULL,
        [Terms_And_Conditions] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Web_Service_URL] nvarchar(max) NOT NULL,
        [Authentication_Token] nvarchar(max) NOT NULL,
        [Apple_Pass] varbinary(max) NOT NULL,
        [Currency_Code] nvarchar(max) NULL,
        [Recipient_Name] nvarchar(max) NULL,
        [Sender_Name] nvarchar(max) NULL,
        [Message] nvarchar(max) NULL,
        [Barcode_Type] nvarchar(max) NULL,
        [Barcode_Format] nvarchar(max) NULL,
        [Expiration_Date] datetime2 NULL,
        [Relevant_Date] datetime2 NULL,
        [Balance] decimal(18,2) NULL,
        CONSTRAINT [PK_WalletPasses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241110091121_Added_Gift_Card')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241110091121_Added_Gift_Card', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241119072246_ApplePassAccount')
BEGIN
    CREATE TABLE [Apple_Pass_Accounts] (
        [Id] int NOT NULL IDENTITY,
        [Pass_Type_Identifier] nvarchar(max) NOT NULL,
        [Team_Identifier] nvarchar(max) NOT NULL,
        [TenantId] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [AccountType] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Apple_Pass_Accounts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241119072246_ApplePassAccount')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241119072246_ApplePassAccount', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241119120439_AddedTransaction_No')
BEGIN
    CREATE TABLE [Transaction_No] (
        [Id] int NOT NULL IDENTITY,
        [CreatedAt] datetime2 NOT NULL,
        [EntityType] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Transaction_No] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241119120439_AddedTransaction_No')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241119120439_AddedTransaction_No', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Web_Service_URL');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Web_Service_URL] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Terms_And_Conditions');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Terms_And_Conditions] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Logo');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Logo] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Localized_Name');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Localized_Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Description');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Description] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Authentication_Token');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Authentication_Token] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241121173209_AddedNullablesinWallet')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241121173209_AddedNullablesinWallet', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241122023015_AddedNullablesinWalletTenantId')
BEGIN
    ALTER TABLE [WalletPasses] ADD [TenantId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241122023015_AddedNullablesinWalletTenantId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241122023015_AddedNullablesinWalletTenantId', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    EXEC sp_rename N'[WalletPasses].[Logo]', N'Webiste', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WalletPasses]') AND [c].[name] = N'Apple_Pass');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [WalletPasses] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [WalletPasses] ALTER COLUMN [Apple_Pass] varbinary(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Additional_Benefits] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Address] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [AirlineCode] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [AirlineName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Amount] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [ArrivalAirportCode] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [ArrivalAirportName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [BaggageClaimInfo] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [BoardingTime] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Card_Holder_Title] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Card_holder_Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [ClassOfService] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Code_Type] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Currency_Sign] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Current_Punches] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [DepartureAirportCode] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [DepartureAirportName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [DepartureTime] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Discount_Percentage] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Effective_Date] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Email] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [EntryGate] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [EventName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [FlightNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [FrequentFlyerNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [GateNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [GiftCard_Currency_Code] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [GiftCard_Currency_Sign] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Is_Redeemed] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Issuer] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Logo_Text] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Logo_Url] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [LoyaltyCard_Reward_Details] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Member_Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Membership_Number] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Offer] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Offer_Code] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [PassengerName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Phone] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Points_Balance] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Privacy_Policy] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Program_Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Punch_Title] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Reward_Details] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [SeatInfo] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [SeatNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Status] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Terminal] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [TicketNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Ticket_Type] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [Total_Punches] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    ALTER TABLE [WalletPasses] ADD [VenueName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241123062308_AddedAllWalletPasses')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241123062308_AddedAllWalletPasses', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241124144657_addedDeveloperAreas')
BEGIN
    CREATE TABLE [TenantApiHitsHistories] (
        [Id] int NOT NULL IDENTITY,
        [privateKey] nvarchar(max) NOT NULL,
        [publicKey] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [TenantId] int NOT NULL,
        CONSTRAINT [PK_TenantApiHitsHistories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241124144657_addedDeveloperAreas')
BEGIN
    CREATE TABLE [TenantKeyHistories] (
        [Id] int NOT NULL IDENTITY,
        [privateKey] nvarchar(max) NOT NULL,
        [publicKey] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [TenantId] int NOT NULL,
        [Change_By] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TenantKeyHistories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241124144657_addedDeveloperAreas')
BEGIN
    CREATE TABLE [TenantLicenes] (
        [Id] int NOT NULL IDENTITY,
        [privateKey] nvarchar(max) NOT NULL,
        [publicKey] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [Update_At] datetime2 NOT NULL,
        [TenantId] int NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TenantLicenes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241124144657_addedDeveloperAreas')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241124144657_addedDeveloperAreas', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127012306_TransactionNoModified')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241127012306_TransactionNoModified', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127032630_AddedUserVoucher')
BEGIN
    CREATE TABLE [UserVouchers] (
        [Id] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Remarks] nvarchar(max) NOT NULL,
        [Uploaded_By] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdateAt] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_UserVouchers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127032630_AddedUserVoucher')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241127032630_AddedUserVoucher', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127033258_RemovedUpdatedAtFromUserVoucher')
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserVouchers]') AND [c].[name] = N'UpdateAt');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [UserVouchers] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [UserVouchers] DROP COLUMN [UpdateAt];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127033258_RemovedUpdatedAtFromUserVoucher')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241127033258_RemovedUpdatedAtFromUserVoucher', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127034801_AddedBatchNoToUserVoucher')
BEGIN
    ALTER TABLE [UserVouchers] ADD [BatchNo] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127034801_AddedBatchNoToUserVoucher')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241127034801_AddedBatchNoToUserVoucher', N'6.0.31');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127042305_AddedIdandUsernameToUserVoucher')
BEGIN
    EXEC sp_rename N'[UserVouchers].[Uploaded_By]', N'UploadedByUsername', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127042305_AddedIdandUsernameToUserVoucher')
BEGIN
    ALTER TABLE [UserVouchers] ADD [ProcessedByUserId] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127042305_AddedIdandUsernameToUserVoucher')
BEGIN
    ALTER TABLE [UserVouchers] ADD [ProcessedByUsername] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127042305_AddedIdandUsernameToUserVoucher')
BEGIN
    ALTER TABLE [UserVouchers] ADD [UploadedByUserId] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241127042305_AddedIdandUsernameToUserVoucher')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241127042305_AddedIdandUsernameToUserVoucher', N'6.0.31');
END;
GO

COMMIT;
GO

