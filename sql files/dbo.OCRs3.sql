CREATE TABLE [dbo].[OCRs] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [documentId]       INT            NOT NULL,
    [documentFilename] NVARCHAR (MAX) NULL,
    [documentText]     NVARCHAR (MAX) NULL,
    [documentSections] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_dbo.OCRs] PRIMARY KEY CLUSTERED ([id] ASC)
);

