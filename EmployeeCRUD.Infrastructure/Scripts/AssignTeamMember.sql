CREATE PROCEDURE AssignTeamMember
    @ProjectId UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO EmployeeProjects (ProjectsId, TeamMemberId)
    VALUES (@ProjectId, @EmployeeId);

    SELECT 
        p.Id AS ProjectId,
        p.ProjectName,
        p.Status,
        e.EmpName AS TeamMember
    FROM Projects p
    LEFT JOIN EmployeeProjects ep ON p.Id = ep.ProjectsId
    LEFT JOIN Employees e ON ep.TeamMemberId = e.Id
    WHERE p.Id = @ProjectId;
END;