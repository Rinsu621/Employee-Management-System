CREATE PROCEDURE PatchProject
    @Id UNIQUEIDENTIFIER,
    @ProjectName VARCHAR(100) = NULL,
    @Description VARCHAR(500) = NULL,
    @ClientName NVARCHAR(100) = NULL,
    @Status NVARCHAR(50) = NULL,
    @Budget DECIMAL(18,2) = NULL,
    @EndDate DATETIME = NULL,
    @ProjectManagerId UNIQUEIDENTIFIER = NULL,
    @IsArchived BIT = NULL,
    @TeamMembersIds NVARCHAR(MAX) = NULL  

AS
BEGIN
SET NOCOUNT ON;

UPDATE Projects
SET
 ProjectName = COALESCE(@ProjectName, ProjectName),
        Description = COALESCE(@Description, Description),
        ClientName = COALESCE(@ClientName, ClientName),
        Status = COALESCE(@Status, Status),
        Budget = COALESCE(@Budget, Budget),
        EndDate = COALESCE(@EndDate, EndDate),
        ProjectManagerId = COALESCE(@ProjectManagerId, ProjectManagerId),
        IsArchived = COALESCE(@IsArchived, IsArchived)
    WHERE Id = @Id;

    IF @TeamMembersIds IS NOT NULL
    BEGIN
    INSERT INTO EmployeeProjects(ProjectsId, TeamMemberId)
    SELECT @Id, CAST(value AS UNIQUEIDENTIFIER)
    FROM STRING_SPLIT(@TeamMembersIds,',')
    WHERE NOT EXISTS(
    SELECT 1 FROM EmployeeProjects
    WHERE ProjectsId=@Id AND TeamMemberId=CAST(value AS UNIQUEIDENTIFIER)
    );
    END
    END
