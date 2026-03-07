CREATE TABLE [dbo].[AuthRefreshToken] (
    [Id]        VARCHAR (50)   NOT NULL,
    [Token]     NVARCHAR (500) NOT NULL,
    [UserId]    NVARCHAR (50)  NOT NULL,
    [ExpiresAt] DATETIME2 (7)  NOT NULL,
    [CreatedAt] DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [IsRevoked] BIT            NOT NULL,
    CONSTRAINT [PK_AuthRefreshToken] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AuthRefreshToken_ExpiresAt]
    ON [dbo].[AuthRefreshToken]([ExpiresAt] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthRefreshToken_Token]
    ON [dbo].[AuthRefreshToken]([Token] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthRefreshToken_UserId]
    ON [dbo].[AuthRefreshToken]([UserId] ASC);

