CREATE TABLE [dbo].[OCRs] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [documentId]       NVARCHAR (MAX) NULL,
    [documentFilename] NVARCHAR (MAX) NULL,
    [documentText]     NVARCHAR (MAX) NULL,
    [documentSections] NVARCHAR (MAX) NULL,
    [ocrId] INT NOT NULL, 
    CONSTRAINT [PK_dbo.OCRs] PRIMARY KEY CLUSTERED ([id] ASC)
);

