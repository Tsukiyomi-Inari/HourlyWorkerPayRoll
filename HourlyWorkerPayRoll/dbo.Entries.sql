CREATE TABLE [dbo].[Entries] (
    [EntryId]   INT             IDENTITY (0, 1) NOT NULL,
    [FirstName] TEXT            NOT NULL,
    [LastName]  TEXT            NOT NULL,
    [Messages]  INT             NOT NULL,
    [Pay]       MONEY NOT NULL,
    [EntryDate] DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([EntryId] ASC)
);

