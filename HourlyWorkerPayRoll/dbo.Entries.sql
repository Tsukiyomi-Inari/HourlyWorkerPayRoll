DROP TABLE IF EXISTS [dbo].[Entries];

CREATE TABLE [dbo].[Entries] (
    [EntryId]   INT             IDENTITY (0, 1) NOT NULL,
    [FirstName] TEXT            NOT NULL,
    [LastName]  TEXT            NOT NULL,
    [Messages]  INT             NOT NULL,
    [Pay]       MONEY NOT NULL,
    [EntryDate] DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([EntryId] ASC)
);

 INSERT INTO [dbo].[Entries] ([FirstName],[LastName],[Messages], [Pay], [EntryDate] )
VALUES ('Kat', 'Bellman', 640, $400, GETDATE());

SELECT *FROM [dbo].[Entries];