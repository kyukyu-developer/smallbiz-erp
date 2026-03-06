CREATE TABLE [dbo].[Product]
(
    [Id] VARCHAR(50) NOT NULL,
    [Code] NVARCHAR(50) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,

    [Group_Id] VARCHAR(50),
    [Category_Id] VARCHAR(50),
    [Brand_Id] VARCHAR(50),
    [Base_Unit_Id] VARCHAR(50),

    [Track_Inventory] BIT DEFAULT 0 NOT NULL,
    [Has_Variant] BIT DEFAULT 0 NOT NULL,
    [Has_Serial_Number] BIT DEFAULT 0 NOT NULL,
    [Has_Batch_Number] BIT DEFAULT 0 NOT NULL,

    [Reorder_Level] INT NULL,
    [Allow_Negative_Stock] BIT DEFAULT 0 NOT NULL,

    [Active] BIT DEFAULT 1 NOT NULL,
    [CreatedAt] DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
    [UpdatedAt] DATETIME2 NULL,
    [CreatedBy] NVARCHAR(50),
    [UpdatedBy] NVARCHAR(50),
    [LastAction] NVARCHAR(50),

    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id]),

    -- Foreign Keys
    CONSTRAINT [FK_Product_ProductGroup]
        FOREIGN KEY ([Group_Id]) REFERENCES [dbo].[Product_Group]([Id]),

    CONSTRAINT [FK_Product_Category]
        FOREIGN KEY ([Category_Id]) REFERENCES [dbo].[Categories]([Id]),

    CONSTRAINT [FK_Product_Brand]
        FOREIGN KEY ([Brand_Id]) REFERENCES [dbo].[Brands]([Id]),

    CONSTRAINT [FK_Product_Unit]
        FOREIGN KEY ([Base_Unit_Id]) REFERENCES [dbo].[Units]([Id])
);
GO


-- =========================================
-- Indexes
-- =========================================

-- Product Code (usually unique)
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_Code]
ON [dbo].[Product] ([Code]);
GO

-- Product Name search
CREATE NONCLUSTERED INDEX [IX_Product_Name]
ON [dbo].[Product] ([Name]);
GO

-- Foreign key indexes
CREATE NONCLUSTERED INDEX [IX_Product_GroupId]
ON [dbo].[Product] ([Group_Id]);
GO

CREATE NONCLUSTERED INDEX [IX_Product_CategoryId]
ON [dbo].[Product] ([Category_Id]);
GO

CREATE NONCLUSTERED INDEX [IX_Product_BrandId]
ON [dbo].[Product] ([Brand_Id]);
GO

CREATE NONCLUSTERED INDEX [IX_Product_BaseUnitId]
ON [dbo].[Product] ([Base_Unit_Id]);
GO

-- Filter active products
CREATE NONCLUSTERED INDEX [IX_Product_Active]
ON [dbo].[Product] ([Active]);
GO

-- Date filtering
CREATE NONCLUSTERED INDEX [IX_Product_CreatedAt]
ON [dbo].[Product] ([CreatedAt]);
GO

CREATE NONCLUSTERED INDEX [IX_Product_UpdatedAt]
ON [dbo].[Product] ([UpdatedAt]);
GO

-- ERP optimized indexes
CREATE NONCLUSTERED INDEX [IX_Product_Group_Active]
ON [dbo].[Product] ([Group_Id], [Active]);
GO

CREATE NONCLUSTERED INDEX [IX_Product_Category_Active]
ON [dbo].[Product] ([Category_Id], [Active]);
GO

-- Inventory filtering
CREATE NONCLUSTERED INDEX [IX_Product_TrackInventory]
ON [dbo].[Product] ([Track_Inventory]);
GO