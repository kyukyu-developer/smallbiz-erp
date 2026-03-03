CREATE TABLE [dbo].[Sales] (
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
    [Active]        BIT             CONSTRAINT [DF_Sales_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]     DATETIME2 (7)   CONSTRAINT [DF_Sales_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     NVARCHAR (50)   NULL,
    [UpdatedAt]     DATETIME2 (7)   NULL,
    [UpdatedBy]     NVARCHAR (50)   NULL,
    [LastAction]    NVARCHAR (50)   NULL,
    CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Sales_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id]),
    CONSTRAINT [FK_Sales_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Sales_CustomerId]
    ON [dbo].[Sales]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Sales_WarehouseId]
    ON [dbo].[Sales]([WarehouseId] ASC);

