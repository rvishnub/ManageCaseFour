CREATE TABLE [dbo].[UserCaseJunctions] (
    [userCaseJunctionId] INT            IDENTITY (1, 1) NOT NULL,
    [caseId]             INT            NOT NULL,
    [Id]                 NVARCHAR (128) NULL,
    CONSTRAINT [PK_dbo.UserCaseJunctions] PRIMARY KEY CLUSTERED ([userCaseJunctionId] ASC),
    CONSTRAINT [FK_dbo.UserCaseJunctions_dbo.Cases_caseId] FOREIGN KEY ([caseId]) REFERENCES [dbo].[Cases] ([caseId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserCaseJunctions_dbo.AspNetUsers_id] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_caseId]
    ON [dbo].[UserCaseJunctions]([caseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[UserCaseJunctions]([Id] ASC);

