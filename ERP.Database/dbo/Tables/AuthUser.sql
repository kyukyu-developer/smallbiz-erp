CREATE TABLE [dbo].[AuthUser] (
    [Id]           VARCHAR (50)   NOT NULL,
    [Username]     NVARCHAR (100) NOT NULL,
    [FirstName]    NVARCHAR (100) NULL,
    [LastName]     NVARCHAR (100) NULL,
    [Email]        NVARCHAR (200) NOT NULL,
    [PasswordHash] NVARCHAR (MAX) NOT NULL,
    [Role]         NVARCHAR (50)  DEFAULT (N'User') NOT NULL,
    [Active]       BIT            DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    NVARCHAR (50)  NULL,
    [UpdatedAt]    DATETIME2 (7)  NULL,
    [UpdatedBy]    NVARCHAR (50)  NULL,
    [LastAction]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_AuthUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AuthUser_Email]
    ON [dbo].[AuthUser]([Email] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AuthUser_Username]
    ON [dbo].[AuthUser]([Username] ASC);

