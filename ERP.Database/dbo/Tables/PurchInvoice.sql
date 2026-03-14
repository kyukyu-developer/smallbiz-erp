CREATE TABLE [dbo].[PurchInvoice] (
    [Id]                  VARCHAR (50)    NOT NULL,
    [PurchaseOrderNumber] NVARCHAR (MAX)  NOT NULL,
    [PurchaseDate]        DATETIME2 (7)   NOT NULL,
    [SupplierId]          VARCHAR (50)    NOT NULL,
    [WarehouseId]         VARCHAR (50)    NULL,
    [SubTotal]            DECIMAL (18, 2) NOT NULL,
    [TotalDiscount]       DECIMAL (18, 2) NULL,
    [TotalTax]            DECIMAL (18, 2) NULL,
    [TotalAmount]         DECIMAL (18, 2) NOT NULL,
    [PaidAmount]          DECIMAL (18, 2) NULL,
    [PaymentStatus]       INT             NOT NULL,
    [Status]              INT             NOT NULL,
    [ExpectedDate]        DATETIME2 (7)   NULL,
    [ReceivedDate]        DATETIME2 (7)   NULL,
    [Notes]               NVARCHAR (MAX)  NULL,
    [Active]              BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]           DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           NVARCHAR (50)   NULL,
    [UpdatedAt]           DATETIME2 (7)   NULL,
    [UpdatedBy]           NVARCHAR (50)   NULL,
    [LastAction]          NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchInvoice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchInvoice_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_PurchInvoice_PurchSupplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[PurchSupplier] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PurchInvoice_SupplierId]
    ON [dbo].[PurchInvoice]([SupplierId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchInvoice_WarehouseId]
    ON [dbo].[PurchInvoice]([WarehouseId] ASC);

