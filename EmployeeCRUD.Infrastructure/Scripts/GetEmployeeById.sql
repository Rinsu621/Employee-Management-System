CREATE OR ALTER PROCEDURE GetAllEmployeeById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT 
        e.Id,
        e.EmpName,
        e.Email,
        e.Phone,
        d.DeptName AS DepartmentName,
        e.CreatedAt
    FROM Employees e
    LEFT JOIN Departments d ON e.DepartmentId = d.Id
    WHERE e.Id = @Id
END
