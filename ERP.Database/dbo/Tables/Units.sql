CREATE TABLE [dbo].[Units] (
    [Id]         VARCHAR (50)  CONSTRAINT [DF__Units__Id__17F790F9] DEFAULT (N'') NOT NULL,
    [Name]       VARCHAR (50)  CONSTRAINT [DF__Units__Name__160F4887] DEFAULT (N'') NOT NULL,
    [Symbol]     VARCHAR (50)  CONSTRAINT [DF__Units__Symbol__17036CC0] DEFAULT (N'') NOT NULL,
    [Active]     BIT           CONSTRAINT [DF__Units__Active__14270015] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]  DATETIME2 (7) CONSTRAINT [DF__Units__CreatedAt__151B244E] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (50) NULL,
    [UpdatedAt]  DATETIME2 (7) NULL,
    [UpdatedBy]  NVARCHAR (50) NULL,
    [LastAction] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Unit_Active]
    ON [dbo].[Units]([Active] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Unit_Name]
    ON [dbo].[Units]([Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Unit_Symbol]
    ON [dbo].[Units]([Symbol] ASC);

