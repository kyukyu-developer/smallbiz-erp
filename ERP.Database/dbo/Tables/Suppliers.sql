CREATE TABLE [dbo].[Suppliers] (
    [Id]              VARCHAR (50)  NOT NULL,
    [Code]            VARCHAR (50)  NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    [ContactPerson]   VARCHAR (50)  NULL,
    [Email]           VARCHAR (50)  NULL,
    [Phone]           VARCHAR (50)  NULL,
    [Address]         VARCHAR (50)  NULL,
    [City]            VARCHAR (50)  NULL,
    [Country]         VARCHAR (50)  NULL,
    [TaxNumber]       VARCHAR (50)  NULL,
    [PaymentTermDays] INT           NULL,
    [Active]          BIT           CONSTRAINT [DF_Suppliers_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7) CONSTRAINT [DF_Suppliers_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50) NULL,
    [UpdatedAt]       DATETIME2 (7) NULL,
    [UpdatedBy]       NVARCHAR (50) NULL,
    [LastAction]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

