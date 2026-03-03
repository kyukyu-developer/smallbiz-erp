CREATE TABLE [dbo].[Purchases] (
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
    [Active]              BIT             CONSTRAINT [DF_Purchases_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]           DATETIME2 (7)   CONSTRAINT [DF_Purchases_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           NVARCHAR (50)   NULL,
    [UpdatedAt]           DATETIME2 (7)   NULL,
    [UpdatedBy]           NVARCHAR (50)   NULL,
    [LastAction]          NVARCHAR (50)   NULL,
    CONSTRAINT [PK_Purchases] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Purchases_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([Id]),
    CONSTRAINT [FK_Purchases_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Purchases_SupplierId]
    ON [dbo].[Purchases]([SupplierId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Purchases_WarehouseId]
    ON [dbo].[Purchases]([WarehouseId] ASC);

