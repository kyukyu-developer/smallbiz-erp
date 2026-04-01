CREATE TABLE [dbo].[PurchOrder] (
    [Id]            VARCHAR (50)    NOT NULL,
    [OrderNumber]   NVARCHAR (30)   NOT NULL,
    [OrderDate]     DATETIME2 (7)   NOT NULL,
    [SupplierId]    VARCHAR (50)    NOT NULL,
    [WarehouseId]   VARCHAR (50)    NOT NULL,
    [SubTotal]      DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [TotalDiscount] DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalTax]      DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalAmount]   DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [Status]        INT             NOT NULL DEFAULT 0,
    [ExpectedDate]  DATETIME2 (7)   NULL,
    [Notes]         NVARCHAR (MAX)  NULL,
    [Active]        BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]     DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     NVARCHAR (50)   NULL,
    [UpdatedAt]     DATETIME2 (7)   NULL,
    [UpdatedBy]     NVARCHAR (50)   NULL,
    [LastAction]    NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchOrder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchOrder_PurchSupplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[PurchSupplier] ([Id]),
    CONSTRAINT [FK_PurchOrder_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PurchOrder_OrderNumber]
    ON [dbo].[PurchOrder]([OrderNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchOrder_SupplierId]
    ON [dbo].[PurchOrder]([SupplierId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchOrder_WarehouseId]
    ON [dbo].[PurchOrder]([WarehouseId] ASC);
