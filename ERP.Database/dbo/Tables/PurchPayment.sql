CREATE TABLE [dbo].[PurchPayment] (
    [Id]                VARCHAR (50)    NOT NULL,
    [PaymentNumber]     NVARCHAR (30)   NOT NULL,
    [PurchaseInvoiceId] VARCHAR (50)    NOT NULL,
    [Amount]            DECIMAL (18, 2) NOT NULL,
    [PaymentDate]       DATETIME2 (7)   NOT NULL,
    [PaymentMethod]     INT             NOT NULL DEFAULT 0,
    [ReferenceNumber]   NVARCHAR (100)  NULL,
    [Notes]             NVARCHAR (MAX)  NULL,
    [Active]            BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]         DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         NVARCHAR (50)   NULL,
    [UpdatedAt]         DATETIME2 (7)   NULL,
    [UpdatedBy]         NVARCHAR (50)   NULL,
    [LastAction]        NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchPayment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchPayment_PurchInvoice] FOREIGN KEY ([PurchaseInvoiceId]) REFERENCES [dbo].[PurchInvoice] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PurchPayment_PaymentNumber]
    ON [dbo].[PurchPayment]([PaymentNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchPayment_PurchaseInvoiceId]
    ON [dbo].[PurchPayment]([PurchaseInvoiceId] ASC);
