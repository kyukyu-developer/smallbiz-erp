CREATE TABLE [dbo].[InvWarehouse] (
    [Id]                VARCHAR (50)   NOT NULL,
    [Name]              NVARCHAR (50)  NOT NULL,
    [City]              NVARCHAR (50)  NULL,
    [BranchType]        NVARCHAR (20)  NOT NULL,
    [IsMainWarehouse]   BIT            NOT NULL,
    [ParentWarehouseId] NVARCHAR (50)  NULL,
    [IsUsedWarehouse]   BIT            DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [Location]          NVARCHAR (100) NULL,
    [Address]           NVARCHAR (255) NULL,
    [Country]           NVARCHAR (50)  NULL,
    [ContactPerson]     NVARCHAR (100) NULL,
    [Phone]             NVARCHAR (20)  NULL,
    [Active]            BIT            DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]         DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]         DATETIME2 (7)  NULL,
    [CreatedBy]         NVARCHAR (50)  NULL,
    [UpdatedBy]         NVARCHAR (50)  NULL,
    [LastAction]        NVARCHAR (50)  NULL,
    CONSTRAINT [PK_InvWarehouse] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouse_Active]
    ON [dbo].[InvWarehouse]([Active] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouse_BranchType]
    ON [dbo].[InvWarehouse]([BranchType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouse_IsMainWarehouse]
    ON [dbo].[InvWarehouse]([IsMainWarehouse] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouse_Name_City]
    ON [dbo].[InvWarehouse]([Name] ASC, [City] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouse_ParentWarehouseId]
    ON [dbo].[InvWarehouse]([ParentWarehouseId] ASC);

