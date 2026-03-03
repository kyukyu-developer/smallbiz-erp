CREATE TABLE [dbo].[Warehouses] (
    [Id]                VARCHAR (50)   NOT NULL,
    [Name]              NVARCHAR (50)  NOT NULL,
    [City]              NVARCHAR (50)  NULL,
    [BranchType]        NVARCHAR (20)  NOT NULL,
    [IsMainWarehouse]   BIT            NOT NULL,
    [ParentWarehouseId] NVARCHAR (50)  NULL,
    [IsUsedWarehouse]   BIT            CONSTRAINT [DF__Warehouse__IsUse__44FF419A] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [Location]          NVARCHAR (100) NULL,
    [Address]           NVARCHAR (255) NULL,
    [Country]           NVARCHAR (50)  NULL,
    [ContactPerson]     NVARCHAR (100) NULL,
    [Phone]             NVARCHAR (20)  NULL,
    [Active]            BIT            CONSTRAINT [DF__Warehouse__Activ__45F365D3] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]         DATETIME2 (7)  CONSTRAINT [DF__Warehouse__Creat__46E78A0C] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]         DATETIME2 (7)  NULL,
    [CreatedBy]         NVARCHAR (50)  NULL,
    [UpdatedBy]         NVARCHAR (50)  NULL,
    [LastAction]        NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Warehouses] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Warehouse_Active]
    ON [dbo].[Warehouses]([Active] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Warehouse_BranchType]
    ON [dbo].[Warehouses]([BranchType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Warehouse_IsMainWarehouse]
    ON [dbo].[Warehouses]([IsMainWarehouse] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Warehouse_Name_City]
    ON [dbo].[Warehouses]([Name] ASC, [City] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Warehouse_ParentWarehouseId]
    ON [dbo].[Warehouses]([ParentWarehouseId] ASC);

