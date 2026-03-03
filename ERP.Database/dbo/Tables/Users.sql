CREATE TABLE [dbo].[Users] (
    [Id]           VARCHAR (50)   NOT NULL,
    [Username]     NVARCHAR (100) NOT NULL,
    [FirstName]    NVARCHAR (100) NULL,
    [LastName]     NVARCHAR (100) NULL,
    [Email]        NVARCHAR (200) NOT NULL,
    [PasswordHash] NVARCHAR (MAX) NOT NULL,
    [Role]         NVARCHAR (50)  CONSTRAINT [DF__Users__Role__412EB0B6] DEFAULT (N'User') NOT NULL,
    [Active]       BIT            CONSTRAINT [DF_Users_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  CONSTRAINT [DF_Users_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    NVARCHAR (50)  NULL,
    [UpdatedAt]    DATETIME2 (7)  NULL,
    [UpdatedBy]    NVARCHAR (50)  NULL,
    [LastAction]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]
    ON [dbo].[Users]([Email] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username]
    ON [dbo].[Users]([Username] ASC);

