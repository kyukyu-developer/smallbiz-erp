CREATE TABLE [dbo].[RefreshTokens] (
    [Id]        VARCHAR (50)   NOT NULL,
    [Token]     NVARCHAR (500) NOT NULL,
    [UserId]    NVARCHAR (50)  NOT NULL,
    [ExpiresAt] DATETIME2 (7)  NOT NULL,
    [CreatedAt] DATETIME2 (7)  CONSTRAINT [DF__RefreshTo__Creat__4E88ABD4] DEFAULT (getutcdate()) NOT NULL,
    [IsRevoked] BIT            CONSTRAINT [DF__RefreshTo__IsRev__4F7CD00D] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_ExpiresAt]
    ON [dbo].[RefreshTokens]([ExpiresAt] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_Token]
    ON [dbo].[RefreshTokens]([Token] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId]
    ON [dbo].[RefreshTokens]([UserId] ASC);

