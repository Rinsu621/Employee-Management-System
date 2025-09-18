CREATE OR ALTER PROCEDURE AddProject
    @Id UNIQUEIDENTIFIER = NULL,
    @ProjectName NVARCHAR(100),
    @Description NVARCHAR(500),
    @StartDate DATETIME,
    @EndDate DATETIME = NULL,
    @Budget DECIMAL(18, 2),
    @Status NVARCHAR(50),
    @ClientName NVARCHAR(100) = NULL,
    @IsArchived BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF @Id IS NULL
        SET @Id = NEWID();

   INSERT INTO Projects 
    (Id, ProjectName, Description, StartDate, EndDate, Budget, Status, ClientName, IsArchived, CreatedAt, UpdatedAt)
VALUES 
    (@Id, @ProjectName, @Description, @StartDate, @EndDate, @Budget, @Status, @ClientName, 0, GETDATE(), GETDATE());


    SELECT 
        p.Id,
        p.ProjectName,
        p.Description,
        p.StartDate,
        p.EndDate,
        p.Budget,
        p.Status,
        p.ClientName,
        p.IsArchived, 
        d.DeptName AS DepartmentName,
        pm.EmpName AS ProjectManagerName,
        '' AS TeamMembers 
    FROM Projects p
    LEFT JOIN Employees pm ON p.ProjectManagerId = pm.Id
    LEFT JOIN Departments d ON p.DepartmentId = d.Id
    WHERE p.Id = @Id;
END
