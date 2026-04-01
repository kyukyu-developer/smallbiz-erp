CREATE TABLE [dbo].[SalesQuotation] (
    [Id]              VARCHAR (50)    NOT NULL,
    [QuotationNumber] NVARCHAR (30)   NOT NULL,
    [QuotationDate]   DATETIME2 (7)   NOT NULL,
    [ValidUntil]      DATETIME2 (7)   NULL,
    [CustomerId]      VARCHAR (50)    NOT NULL,
    [SubTotal]        DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [TotalDiscount]   DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalTax]        DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalAmount]     DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [Status]          INT             NOT NULL DEFAULT 0,
    [Notes]           NVARCHAR (MAX)  NULL,
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesQuotation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesQuotation_SalesCustomer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[SalesCustomer] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SalesQuotation_QuotationNumber]
    ON [dbo].[SalesQuotation]([QuotationNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesQuotation_CustomerId]
    ON [dbo].[SalesQuotation]([CustomerId] ASC);
