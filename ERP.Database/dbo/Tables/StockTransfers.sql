CREATE TABLE [dbo].[StockTransfers] (
    [Id]              VARCHAR (50)    NOT NULL,
    [TransferNo]      VARCHAR (50)    NOT NULL,
    [FromWarehouseId] VARCHAR (50)    NOT NULL,
    [ToWarehouseId]   VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [TransferDate]    DATETIME2 (7)   NOT NULL,
    [Status]          INT             NOT NULL,
    [Notes]           NVARCHAR (MAX)  NULL,
    [Active]          BIT             CONSTRAINT [DF_StockTransfers_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   CONSTRAINT [DF_StockTransfers_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_StockTransfers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockTransfers_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_StockTransfers_Warehouses] FOREIGN KEY ([FromWarehouseId]) REFERENCES [dbo].[Warehouses] ([Id]),
    CONSTRAINT [FK_StockTransfers_Warehouses1] FOREIGN KEY ([ToWarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_StockTransfers_FromWarehouseId]
    ON [dbo].[StockTransfers]([FromWarehouseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StockTransfers_ProductId]
    ON [dbo].[StockTransfers]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StockTransfers_ToWarehouseId]
    ON [dbo].[StockTransfers]([ToWarehouseId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_StockTransfers_TransferNo]
    ON [dbo].[StockTransfers]([TransferNo] ASC);

