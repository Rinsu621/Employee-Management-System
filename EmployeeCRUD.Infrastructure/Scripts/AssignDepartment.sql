CREATE PROCEDURE AssignDepartment
@ProjectId UNIQUEIDENTIFIER,
@DepartmentId UNIQUEIDENTIFIER
As
BEGIN 
SET NOCOUNT ON;
UPDATE Projects
SET DepartmentId = @DepartmentId
WHERE Id = @ProjectId

SELECT p.ProjectName, d.DeptName 
FROM Projects p
LEFT JOIN Departments d ON p.DepartmentId = d.Id
WHERE p.Id = @ProjectId
END;