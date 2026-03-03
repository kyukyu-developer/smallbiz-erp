CREATE TABLE [dbo].[ProductSerials] (
    [Id]              VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [WarehouseId]     VARCHAR (50)    NOT NULL,
    [SerialNo]        VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [ManufactureDate] DATETIME2 (7)   NULL,
    [ExpiryDate]      DATETIME2 (7)   NULL,
    [Active]          BIT             CONSTRAINT [DF_ProductSerials_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   CONSTRAINT [DF_ProductSerials_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProductSerials_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductSerials_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_ProductSerials_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);

