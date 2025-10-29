CREATE OR ALTER PROCEDURE GetSeniorAccountantsEmails
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.Email
    FROM Employees e
    INNER JOIN AspNetUsers u ON e.Id = u.EmployeeId
    INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
    INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
    INNER JOIN Departments d ON e.DepartmentId = d.Id
    WHERE e.Position = 'SeniorAccountant'
      AND d.DeptName = 'Finance';
END
GO