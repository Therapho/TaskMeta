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

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
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
GO

CREATE TABLE [Funds] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(200) NULL,
    [TargetDate] date NULL,
    [Allocation] int NULL,
    [TargetBalance] money NULL DEFAULT 0.0,
    [Balance] money NOT NULL,
    [Locked] bit NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK__tmp_ms_x__3214EC07A56E5A2D] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Status] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK__Status__3214EC070AA3A213] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TaskDefinitions] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(50) NOT NULL,
    [Value] money NOT NULL,
    [Sequence] int NOT NULL,
    [Active] bit NULL DEFAULT CAST(1 AS bit),
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK__TaskDefinition] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TaskWeeks] (
    [Id] int NOT NULL IDENTITY,
    [WeekStartDate] date NOT NULL,
    [StatusId] int NOT NULL,
    [Value] money NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_TaskWeeks] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TransactionCategories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nchar(10) NOT NULL,
    CONSTRAINT [PK__Transact__3214EC0735F1E75C] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [TransactionLog] (
    [Id] int NOT NULL IDENTITY,
    [Date] datetime NOT NULL DEFAULT ((getdate())),
    [Description] nvarchar(50) NOT NULL,
    [CategoryId] int NOT NULL,
    [Amount] money NOT NULL,
    [TargetUserId] nvarchar(450) NOT NULL,
    [CallingUserId] nvarchar(450) NOT NULL,
    [SourceFundId] int NULL,
    [TargetFundId] int NULL,
    [PreviousAmount] money NULL,
    CONSTRAINT [PK__tmp_ms_x__3214EC07B4738D25] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TransactionLog_SourceFund] FOREIGN KEY ([SourceFundId]) REFERENCES [Funds] ([Id]),
    CONSTRAINT [FK_TransactionLog_TargetFund] FOREIGN KEY ([TargetFundId]) REFERENCES [Funds] ([Id])
);
GO

CREATE TABLE [TaskActivities] (
    [Id] int NOT NULL IDENTITY,
    [Sequence] int NOT NULL,
    [TaskWeekId] int NOT NULL,
    [Complete] bit NOT NULL,
    [TaskDefinitionId] int NOT NULL,
    [Description] nvarchar(50) NOT NULL,
    [Value] money NOT NULL,
    [TaskDate] date NOT NULL,
    CONSTRAINT [PK__tmp_ms_x__3214EC0772780ACB] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaskActivities_TaskWeeks] FOREIGN KEY ([TaskWeekId]) REFERENCES [TaskWeeks] ([Id])
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_TaskActivities_TaskWeekId] ON [TaskActivities] ([TaskWeekId]);
GO

CREATE INDEX [IX_TransactionLog_SourceFundId] ON [TransactionLog] ([SourceFundId]);
GO

CREATE INDEX [IX_TransactionLog_TargetFundId] ON [TransactionLog] ([TargetFundId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240516105503_Create', N'8.0.5');
GO

COMMIT;
GO

