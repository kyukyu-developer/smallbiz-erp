CREATE TABLE [dbo].[SalesInvoice] (
    [Id]            VARCHAR (50)    NOT NULL,
    [InvoiceNumber] VARCHAR (50)    NOT NULL,
    [SaleDate]      DATETIME2 (7)   NOT NULL,
    [CustomerId]    VARCHAR (50)    NOT NULL,
    [WarehouseId]   VARCHAR (50)    NULL,
    [SubTotal]      DECIMAL (18, 2) NOT NULL,
    [TotalDiscount] DECIMAL (18, 2) NULL,
    [TotalTax]      DECIMAL (18, 2) NULL,
    [TotalAmount]   DECIMAL (18, 2) NOT NULL,
    [PaidAmount]    DECIMAL (18, 2) NULL,
    [PaymentStatus] INT             NOT NULL,
    [Status]        INT             NOT NULL,
    [DueDate]       DATETIME2 (7)   NULL,
    [Notes]         NVARCHAR (MAX)  NULL,
    [Active]        BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]     DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     NVARCHAR (50)   NULL,
    [UpdatedAt]     DATETIME2 (7)   NULL,
    [UpdatedBy]     NVARCHAR (50)   NULL,
    [LastAction]    NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesInvoice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesInvoice_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_SalesInvoice_SalesCustomer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[SalesCustomer] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoice_CustomerId]
    ON [dbo].[SalesInvoice]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoice_WarehouseId]
    ON [dbo].[SalesInvoice]([WarehouseId] ASC);

