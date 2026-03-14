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

CREATE TABLE [ProdCategory] (
    [Id] varchar(50) NOT NULL,
    [Code] varchar(20) NOT NULL,
    [Name] varchar(50) NOT NULL,
    [ParentCategoryId] varchar(50) NULL,
    [Description] nvarchar(max) NULL,
    [IsUsed] bit NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdCategory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProdCategory_ParentProdCategory] FOREIGN KEY ([ParentCategoryId]) REFERENCES [ProdCategory] ([Id])
);
GO

CREATE TABLE [SalesCustomer] (
    [Id] varchar(50) NOT NULL,
    [Code] varchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [ContactPerson] nvarchar(50) NULL,
    [Email] varchar(50) NULL,
    [Phone] varchar(50) NULL,
    [Address] varchar(50) NULL,
    [City] varchar(50) NULL,
    [Country] varchar(50) NULL,
    [TaxNumber] varchar(50) NULL,
    [CreditLimit] decimal(18,2) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_SalesCustomer] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AuthRefreshToken] (
    [Id] varchar(50) NOT NULL,
    [Token] nvarchar(500) NOT NULL,
    [UserId] nvarchar(50) NOT NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [IsRevoked] bit NOT NULL,
    CONSTRAINT [PK_AuthRefreshToken] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PurchSupplier] (
    [Id] varchar(50) NOT NULL,
    [Code] varchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [ContactPerson] varchar(50) NULL,
    [Email] varchar(50) NULL,
    [Phone] varchar(50) NULL,
    [Address] varchar(50) NULL,
    [City] varchar(50) NULL,
    [Country] varchar(50) NULL,
    [TaxNumber] varchar(50) NULL,
    [PaymentTermDays] int NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_PurchSupplier] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProdUnit] (
    [Id] varchar(50) NOT NULL DEFAULT '',
    [Name] varchar(50) NOT NULL DEFAULT '',
    [Symbol] varchar(50) NOT NULL DEFAULT '',
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdUnit] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AuthUser] (
    [Id] varchar(50) NOT NULL,
    [Username] nvarchar(100) NOT NULL,
    [FirstName] nvarchar(100) NULL,
    [LastName] nvarchar(100) NULL,
    [Email] nvarchar(200) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(50) NOT NULL DEFAULT N'User',
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_AuthUser] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [InvWarehouse] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [City] nvarchar(50) NULL,
    [BranchType] nvarchar(20) NOT NULL,
    [IsMainWarehouse] bit NOT NULL,
    [ParentWarehouseId] nvarchar(50) NULL,
    [IsUsedWarehouse] bit NOT NULL DEFAULT CAST(1 AS bit),
    [Location] nvarchar(100) NULL,
    [Address] nvarchar(255) NULL,
    [Country] nvarchar(50) NULL,
    [ContactPerson] nvarchar(100) NULL,
    [Phone] nvarchar(20) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_InvWarehouse] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProdItem] (
    [Id] varchar(50) NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [CategoryId] varchar(50) NOT NULL,
    [BaseUnitId] varchar(50) NOT NULL,
    [MinimumStock] decimal(18,2) NULL,
    [MaximumStock] decimal(18,2) NULL,
    [ReorderLevel] decimal(18,2) NULL,
    [Barcode] nvarchar(100) NULL,
    [IsBatchTracked] bit NOT NULL,
    [IsSerialTracked] bit NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProdItem_ProdCategory] FOREIGN KEY ([CategoryId]) REFERENCES [ProdCategory] ([Id]),
    CONSTRAINT [FK_ProdItem_ProdUnit] FOREIGN KEY ([BaseUnitId]) REFERENCES [ProdUnit] ([Id])
);
GO

CREATE TABLE [PurchInvoice] (
    [Id] varchar(50) NOT NULL,
    [PurchaseOrderNumber] nvarchar(max) NOT NULL,
    [PurchaseDate] datetime2 NOT NULL,
    [SupplierId] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NULL,
    [SubTotal] decimal(18,2) NOT NULL,
    [TotalDiscount] decimal(18,2) NULL,
    [TotalTax] decimal(18,2) NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [PaidAmount] decimal(18,2) NULL,
    [PaymentStatus] int NOT NULL,
    [Status] int NOT NULL,
    [ExpectedDate] datetime2 NULL,
    [ReceivedDate] datetime2 NULL,
    [Notes] nvarchar(max) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_PurchInvoice] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PurchInvoice_PurchSupplier] FOREIGN KEY ([SupplierId]) REFERENCES [PurchSupplier] ([Id]),
    CONSTRAINT [FK_PurchInvoice_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [SalesInvoice] (
    [Id] varchar(50) NOT NULL,
    [InvoiceNumber] varchar(50) NOT NULL,
    [SaleDate] datetime2 NOT NULL,
    [CustomerId] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NULL,
    [SubTotal] decimal(18,2) NOT NULL,
    [TotalDiscount] decimal(18,2) NULL,
    [TotalTax] decimal(18,2) NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [PaidAmount] decimal(18,2) NULL,
    [PaymentStatus] int NOT NULL,
    [Status] int NOT NULL,
    [DueDate] datetime2 NULL,
    [Notes] nvarchar(max) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_SalesInvoice] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesInvoice_SalesCustomer] FOREIGN KEY ([CustomerId]) REFERENCES [SalesCustomer] ([Id]),
    CONSTRAINT [FK_SalesInvoice_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [ProdBatch] (
    [Id] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NOT NULL,
    [BatchNo] varchar(50) NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [ManufactureDate] datetime2 NULL,
    [ExpiryDate] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdBatch] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProdBatch_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_ProdBatch_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [ProdSerial] (
    [Id] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NOT NULL,
    [SerialNo] varchar(50) NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [ManufactureDate] datetime2 NULL,
    [ExpiryDate] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdSerial] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProdSerial_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_ProdSerial_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [ProdUnitPrice] (
    [Id] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [UnitId] varchar(50) NOT NULL,
    [SalePrice] decimal(18,2) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdUnitPrice] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProdUnitPrice_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_ProdUnitPrice_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [ProdUnit] ([Id])
);
GO

CREATE TABLE [InvStockAdjustment] (
    [Id] varchar(50) NOT NULL,
    [AdjustmentNo] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [AdjustmentQuantity] decimal(18,2) NOT NULL,
    [Reason] nvarchar(max) NOT NULL,
    [AdjustmentDate] datetime2 NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_InvStockAdjustment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InvStockAdjustment_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_InvStockAdjustment_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [InvStockTransfer] (
    [Id] varchar(50) NOT NULL,
    [TransferNo] varchar(50) NOT NULL,
    [FromWarehouseId] varchar(50) NOT NULL,
    [ToWarehouseId] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [TransferDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [Notes] nvarchar(max) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_InvStockTransfer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InvStockTransfer_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_InvStockTransfer_InvWarehouse] FOREIGN KEY ([FromWarehouseId]) REFERENCES [InvWarehouse] ([Id]),
    CONSTRAINT [FK_InvStockTransfer_InvWarehouse1] FOREIGN KEY ([ToWarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [ProdUnitConversion] (
    [Id] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [FromUnitId] varchar(50) NOT NULL,
    [ToUnitId] varchar(50) NOT NULL,
    [Factor] decimal(18,6) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_ProdUnitConversion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProdUnitConversion_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_ProdUnitConversion_ProdUnit] FOREIGN KEY ([FromUnitId]) REFERENCES [ProdUnit] ([Id]),
    CONSTRAINT [FK_ProdUnitConversion_ProdUnit1] FOREIGN KEY ([ToUnitId]) REFERENCES [ProdUnit] ([Id])
);
GO

CREATE TABLE [InvWarehouseStock] (
    [Id] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [AvailableQuantity] decimal(18,2) NOT NULL,
    [ReservedQuantity] decimal(18,2) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_InvWarehouseStock] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InvWarehouseStock_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_InvWarehouseStock_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE TABLE [PurchItem] (
    [Id] varchar(50) NOT NULL,
    [PurchaseId] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [UnitId] varchar(50) NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [UnitCost] decimal(18,2) NOT NULL,
    [DiscountPercent] decimal(18,2) NULL,
    [DiscountAmount] decimal(18,2) NULL,
    [TaxPercent] decimal(18,2) NULL,
    [TaxAmount] decimal(18,2) NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Notes] nvarchar(max) NULL,
    [BatchId] varchar(50) NULL,
    [SerialId] varchar(50) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_PurchItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PurchItem_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [ProdBatch] ([Id]),
    CONSTRAINT [FK_PurchItem_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [ProdSerial] ([Id]),
    CONSTRAINT [FK_PurchItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_PurchItem_PurchInvoice] FOREIGN KEY ([PurchaseId]) REFERENCES [PurchInvoice] ([Id]),
    CONSTRAINT [FK_PurchItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [ProdUnit] ([Id])
);
GO

CREATE TABLE [SalesInvoiceItem] (
    [Id] varchar(50) NOT NULL,
    [SaleId] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [UnitId] varchar(50) NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [DiscountPercent] decimal(18,2) NULL,
    [DiscountAmount] decimal(18,2) NULL,
    [TaxPercent] decimal(18,2) NULL,
    [TaxAmount] decimal(18,2) NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Notes] nvarchar(max) NULL,
    [BatchId] varchar(50) NULL,
    [SerialId] varchar(50) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_SalesInvoiceItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [ProdBatch] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [ProdSerial] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_SalesInvoice] FOREIGN KEY ([SaleId]) REFERENCES [SalesInvoice] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [ProdUnit] ([Id])
);
GO

CREATE TABLE [InvStockMovement] (
    [Id] varchar(50) NOT NULL,
    [ProductId] varchar(50) NOT NULL,
    [WarehouseId] varchar(50) NOT NULL,
    [MovementType] varchar(50) NOT NULL,
    [ReferenceType] int NOT NULL,
    [ReferenceId] varchar(50) NOT NULL,
    [BaseQuantity] decimal(18,2) NOT NULL,
    [BatchId] varchar(50) NULL,
    [SerialId] varchar(50) NULL,
    [MovementDate] datetime2 NOT NULL,
    [Notes] nvarchar(max) NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((getutcdate())),
    [CreatedBy] nvarchar(50) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(50) NULL,
    [LastAction] nvarchar(50) NULL,
    CONSTRAINT [PK_InvStockMovement] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InvStockMovement_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [ProdBatch] ([Id]),
    CONSTRAINT [FK_InvStockMovement_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [ProdSerial] ([Id]),
    CONSTRAINT [FK_InvStockMovement_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [ProdItem] ([Id]),
    CONSTRAINT [FK_InvStockMovement_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [InvWarehouse] ([Id])
);
GO

CREATE INDEX [IX_ProdCategory_ParentCategoryId] ON [ProdCategory] ([ParentCategoryId]);
GO

CREATE INDEX [IX_ProdBatch_ProductId] ON [ProdBatch] ([ProductId]);
GO

CREATE INDEX [IX_ProdBatch_WarehouseId] ON [ProdBatch] ([WarehouseId]);
GO

CREATE INDEX [IX_ProdItem_BaseUnitId] ON [ProdItem] ([BaseUnitId]);
GO

CREATE INDEX [IX_ProdItem_CategoryId] ON [ProdItem] ([CategoryId]);
GO

CREATE UNIQUE INDEX [IX_ProdItem_Code] ON [ProdItem] ([Code]);
GO

CREATE INDEX [IX_ProdItem_Name] ON [ProdItem] ([Name]);
GO

CREATE INDEX [IX_ProdSerial_ProductId] ON [ProdSerial] ([ProductId]);
GO

CREATE INDEX [IX_ProdSerial_WarehouseId] ON [ProdSerial] ([WarehouseId]);
GO

CREATE INDEX [IX_ProdUnitPrice_ProductId] ON [ProdUnitPrice] ([ProductId]);
GO

CREATE INDEX [IX_ProdUnitPrice_UnitId] ON [ProdUnitPrice] ([UnitId]);
GO

CREATE INDEX [IX_PurchItem_BatchId] ON [PurchItem] ([BatchId]);
GO

CREATE INDEX [IX_PurchItem_ProductId] ON [PurchItem] ([ProductId]);
GO

CREATE INDEX [IX_PurchItem_PurchaseId] ON [PurchItem] ([PurchaseId]);
GO

CREATE INDEX [IX_PurchItem_SerialId] ON [PurchItem] ([SerialId]);
GO

CREATE INDEX [IX_PurchItem_UnitId] ON [PurchItem] ([UnitId]);
GO

CREATE INDEX [IX_PurchInvoice_SupplierId] ON [PurchInvoice] ([SupplierId]);
GO

CREATE INDEX [IX_PurchInvoice_WarehouseId] ON [PurchInvoice] ([WarehouseId]);
GO

CREATE INDEX [IX_AuthRefreshToken_ExpiresAt] ON [AuthRefreshToken] ([ExpiresAt]);
GO

CREATE INDEX [IX_AuthRefreshToken_Token] ON [AuthRefreshToken] ([Token]);
GO

CREATE INDEX [IX_AuthRefreshToken_UserId] ON [AuthRefreshToken] ([UserId]);
GO

CREATE INDEX [IX_SalesInvoice_CustomerId] ON [SalesInvoice] ([CustomerId]);
GO

CREATE INDEX [IX_SalesInvoice_WarehouseId] ON [SalesInvoice] ([WarehouseId]);
GO

CREATE INDEX [IX_SalesInvoiceItem_BatchId] ON [SalesInvoiceItem] ([BatchId]);
GO

CREATE INDEX [IX_SalesInvoiceItem_ProductId] ON [SalesInvoiceItem] ([ProductId]);
GO

CREATE INDEX [IX_SalesInvoiceItem_SaleId] ON [SalesInvoiceItem] ([SaleId]);
GO

CREATE INDEX [IX_SalesInvoiceItem_SerialId] ON [SalesInvoiceItem] ([SerialId]);
GO

CREATE INDEX [IX_SalesInvoiceItem_UnitId] ON [SalesInvoiceItem] ([UnitId]);
GO

CREATE INDEX [IX_InvStockAdjustment_ProductId] ON [InvStockAdjustment] ([ProductId]);
GO

CREATE INDEX [IX_InvStockAdjustment_WarehouseId] ON [InvStockAdjustment] ([WarehouseId]);
GO

CREATE INDEX [IX_InvStockMovement_BatchId] ON [InvStockMovement] ([BatchId]);
GO

CREATE INDEX [IX_InvStockMovement_ProductId] ON [InvStockMovement] ([ProductId]);
GO

CREATE INDEX [IX_InvStockMovement_SerialId] ON [InvStockMovement] ([SerialId]);
GO

CREATE INDEX [IX_InvStockMovement_WarehouseId] ON [InvStockMovement] ([WarehouseId]);
GO

CREATE INDEX [IX_InvStockTransfer_FromWarehouseId] ON [InvStockTransfer] ([FromWarehouseId]);
GO

CREATE INDEX [IX_InvStockTransfer_ProductId] ON [InvStockTransfer] ([ProductId]);
GO

CREATE INDEX [IX_InvStockTransfer_ToWarehouseId] ON [InvStockTransfer] ([ToWarehouseId]);
GO

CREATE UNIQUE INDEX [IX_InvStockTransfer_TransferNo] ON [InvStockTransfer] ([TransferNo]);
GO

CREATE INDEX [IX_ProdUnitConversion_FromUnitId] ON [ProdUnitConversion] ([FromUnitId]);
GO

CREATE UNIQUE INDEX [IX_ProdUnitConversion_ProductId_FromUnitId_ToUnitId] ON [ProdUnitConversion] ([ProductId], [FromUnitId], [ToUnitId]);
GO

CREATE INDEX [IX_ProdUnitConversion_ToUnitId] ON [ProdUnitConversion] ([ToUnitId]);
GO

CREATE INDEX [IX_ProdUnit_Active] ON [ProdUnit] ([Active]);
GO

CREATE INDEX [IX_ProdUnit_Name] ON [ProdUnit] ([Name]);
GO

CREATE UNIQUE INDEX [IX_ProdUnit_Symbol] ON [ProdUnit] ([Symbol]);
GO

CREATE UNIQUE INDEX [IX_AuthUser_Email] ON [AuthUser] ([Email]);
GO

CREATE UNIQUE INDEX [IX_AuthUser_Username] ON [AuthUser] ([Username]);
GO

CREATE INDEX [IX_InvWarehouse_Active] ON [InvWarehouse] ([Active]);
GO

CREATE INDEX [IX_InvWarehouse_BranchType] ON [InvWarehouse] ([BranchType]);
GO

CREATE INDEX [IX_InvWarehouse_IsMainWarehouse] ON [InvWarehouse] ([IsMainWarehouse]);
GO

CREATE INDEX [IX_InvWarehouse_Name_City] ON [InvWarehouse] ([Name], [City]);
GO

CREATE INDEX [IX_InvWarehouse_ParentWarehouseId] ON [InvWarehouse] ([ParentWarehouseId]);
GO

CREATE INDEX [IX_InvWarehouseStock_ProductId] ON [InvWarehouseStock] ([ProductId]);
GO

CREATE INDEX [IX_InvWarehouseStock_WarehouseId] ON [InvWarehouseStock] ([WarehouseId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260306114844_InitialCreate', N'8.0.11');
GO

COMMIT;
GO
