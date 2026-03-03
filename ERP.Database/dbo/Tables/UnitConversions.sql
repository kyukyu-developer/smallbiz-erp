CREATE TABLE [dbo].[UnitConversions] (
    [Id]         VARCHAR (50)    NOT NULL,
    [ProductId]  VARCHAR (50)    NOT NULL,
    [FromUnitId] VARCHAR (50)    NOT NULL,
    [ToUnitId]   VARCHAR (50)    NOT NULL,
    [Factor]     DECIMAL (18, 6) NOT NULL,
    [Active]     BIT             CONSTRAINT [DF_UnitConversions_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]  DATETIME2 (7)   CONSTRAINT [DF_UnitConversions_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (50)   NULL,
    [UpdatedAt]  DATETIME2 (7)   NULL,
    [UpdatedBy]  NVARCHAR (50)   NULL,
    [LastAction] NVARCHAR (50)   NULL,
    CONSTRAINT [PK_UnitConversions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UnitConversions_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_UnitConversions_Units] FOREIGN KEY ([FromUnitId]) REFERENCES [dbo].[Units] ([Id]),
    CONSTRAINT [FK_UnitConversions_Units1] FOREIGN KEY ([ToUnitId]) REFERENCES [dbo].[Units] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_UnitConversions_FromUnitId]
    ON [dbo].[UnitConversions]([FromUnitId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UnitConversions_ProductId_FromUnitId_ToUnitId]
    ON [dbo].[UnitConversions]([ProductId] ASC, [FromUnitId] ASC, [ToUnitId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UnitConversions_ToUnitId]
    ON [dbo].[UnitConversions]([ToUnitId] ASC);

