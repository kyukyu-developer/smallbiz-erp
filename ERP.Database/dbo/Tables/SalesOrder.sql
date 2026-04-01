CREATE TABLE [dbo].[SalesOrder] (
    [Id]            VARCHAR (50)    NOT NULL,
    [OrderNumber]   NVARCHAR (30)   NOT NULL,
    [OrderDate]     DATETIME2 (7)   NOT NULL,
    [CustomerId]    VARCHAR (50)    NOT NULL,
    [QuotationId]   VARCHAR (50)    NULL,
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
    CONSTRAINT [PK_SalesOrder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesOrder_SalesCustomer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[SalesCustomer] ([Id]),
    CONSTRAINT [FK_SalesOrder_SalesQuotation] FOREIGN KEY ([QuotationId]) REFERENCES [dbo].[SalesQuotation] ([Id]),
    CONSTRAINT [FK_SalesOrder_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SalesOrder_OrderNumber]
    ON [dbo].[SalesOrder]([OrderNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesOrder_CustomerId]
    ON [dbo].[SalesOrder]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesOrder_QuotationId]
    ON [dbo].[SalesOrder]([QuotationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesOrder_WarehouseId]
    ON [dbo].[SalesOrder]([WarehouseId] ASC);
