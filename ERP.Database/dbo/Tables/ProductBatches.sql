CREATE TABLE [dbo].[ProductBatches] (
    [Id]              VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [WarehouseId]     VARCHAR (50)    NOT NULL,
    [BatchNo]         VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [ManufactureDate] DATETIME2 (7)   NULL,
    [ExpiryDate]      DATETIME2 (7)   NULL,
    [Active]          BIT             CONSTRAINT [DF_ProductBatches_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   CONSTRAINT [DF_ProductBatches_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProductBatches] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductBatches_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_ProductBatches_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductBatches_ProductId]
    ON [dbo].[ProductBatches]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductBatches_WarehouseId]
    ON [dbo].[ProductBatches]([WarehouseId] ASC);

