CREATE TABLE [dbo].[SalesPayment] (
    [Id]              VARCHAR (50)    NOT NULL,
    [PaymentNumber]   NVARCHAR (30)   NOT NULL,
    [SalesInvoiceId]  VARCHAR (50)    NOT NULL,
    [Amount]          DECIMAL (18, 2) NOT NULL,
    [PaymentDate]     DATETIME2 (7)   NOT NULL,
    [PaymentMethod]   INT             NOT NULL DEFAULT 0,
    [ReferenceNumber] NVARCHAR (100)  NULL,
    [Notes]           NVARCHAR (MAX)  NULL,
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesPayment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesPayment_SalesInvoice] FOREIGN KEY ([SalesInvoiceId]) REFERENCES [dbo].[SalesInvoice] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SalesPayment_PaymentNumber]
    ON [dbo].[SalesPayment]([PaymentNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesPayment_SalesInvoiceId]
    ON [dbo].[SalesPayment]([SalesInvoiceId] ASC);
