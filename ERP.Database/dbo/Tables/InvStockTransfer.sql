CREATE TABLE [dbo].[InvStockTransfer] (
    [Id]              VARCHAR (50)    NOT NULL,
    [TransferNo]      VARCHAR (50)    NOT NULL,
    [FromWarehouseId] VARCHAR (50)    NOT NULL,
    [ToWarehouseId]   VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [TransferDate]    DATETIME2 (7)   NOT NULL,
    [Status]          INT             NOT NULL,
    [Notes]           NVARCHAR (MAX)  NULL,
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_InvStockTransfer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvStockTransfer_InvWarehouse] FOREIGN KEY ([FromWarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_InvStockTransfer_InvWarehouse1] FOREIGN KEY ([ToWarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_InvStockTransfer_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockTransfer_FromWarehouseId]
    ON [dbo].[InvStockTransfer]([FromWarehouseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockTransfer_ProductId]
    ON [dbo].[InvStockTransfer]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockTransfer_ToWarehouseId]
    ON [dbo].[InvStockTransfer]([ToWarehouseId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_InvStockTransfer_TransferNo]
    ON [dbo].[InvStockTransfer]([TransferNo] ASC);

