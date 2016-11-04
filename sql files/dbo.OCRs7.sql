CREATE TABLE [dbo].[Records] (
    [recordId]              INT            IDENTITY (1, 1) NOT NULL,
    [internalCaseId]        INT            NOT NULL,
    [departmentId]          INT            NOT NULL,
    [facilityId]            INT            NOT NULL,
    [ocrId]                 INT            NULL,
    [recordReferenceNumber] NVARCHAR (MAX) NULL,
    [pageNumber]            NVARCHAR (MAX) NULL,
    [recordEntryDate]       DATETIME       NOT NULL,
    [provider]              NVARCHAR (MAX) NULL,
    [memo]                  NVARCHAR (MAX) NULL,
    [noteDate]              NVARCHAR (MAX) NULL,
    [serviceDate]           DATETIME       NOT NULL,
    [noteSubjective]        NVARCHAR (MAX) NULL,
    [history]               NVARCHAR (MAX) NULL,
    [noteObjective]         NVARCHAR (MAX) NULL,
    [noteAssessment]        NVARCHAR (MAX) NULL,
    [notePlan]              NVARCHAR (MAX) NULL,
    [medications]           NVARCHAR (MAX) NULL,
    [age]                   NVARCHAR (MAX) NULL,
    [DOB]                   NVARCHAR (MAX) NULL,
    [allergies]             NVARCHAR (MAX) NULL,
    [vitalSigns]            NVARCHAR (MAX) NULL,
    [providerFirstName]     NVARCHAR (MAX) NULL,
    [providerLastName]      NVARCHAR (MAX) NULL,
    [fileContent]           NVARCHAR (MAX) NULL,
    [diagnosis]             NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Records] PRIMARY KEY CLUSTERED ([recordId] ASC),
    CONSTRAINT [FK_dbo.Records_dbo.Departments_departmentId] FOREIGN KEY ([departmentId]) REFERENCES [dbo].[Departments] ([departmentId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Records_dbo.Facilities_facilityId] FOREIGN KEY ([facilityId]) REFERENCES [dbo].[Facilities] ([facilityId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Records_dbo.InternalCaseNumbers_internalCaseId] FOREIGN KEY ([internalCaseId]) REFERENCES [dbo].[InternalCaseNumbers] ([internalCaseId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Records_dbo.OCRs_ocrId] FOREIGN KEY ([ocrId]) REFERENCES [dbo].[OCRs] ([ocrId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_internalCaseId]
    ON [dbo].[Records]([internalCaseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_departmentId]
    ON [dbo].[Records]([departmentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_facilityId]
    ON [dbo].[Records]([facilityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ocrId]
    ON [dbo].[Records]([ocrId] ASC);

