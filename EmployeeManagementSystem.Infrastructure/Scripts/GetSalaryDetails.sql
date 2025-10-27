CREATE OR ALTER PROCEDURE GetSalaries
    @Year INT = NULL,
    @Month INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Year IS NULL SET @Year = YEAR(GETDATE());
    IF @Month IS NULL SET @Month = MONTH(GETDATE());

   SELECT 
    e.EmpName,
    s.BasicSalary,
    s.Conveyance,
    s.Tax,
    s.PF,
    s.ESI,
    s.PaymentMode,
    s.Status,
    (s.BasicSalary + s.Conveyance) AS GrossSalary,
    (s.BasicSalary + s.Conveyance - (s.Tax + s.PF + s.ESI)) AS NetSalary,
    r.Name AS Role,
    s.SalaryDate
FROM Salaries s
LEFT JOIN EmployeeManagementSystem.dbo.Employees e 
    ON s.EmployeeId = e.Id
LEFT JOIN EmployeeManagementSystem.dbo.AspNetUsers u
    ON e.Id = u.EmployeeId
LEFT JOIN EmployeeManagementSystem.dbo.AspNetUserRoles ur
    ON u.Id = ur.UserId
LEFT JOIN EmployeeManagementSystem.dbo.AspNetRoles r
    ON ur.RoleId = r.Id
WHERE YEAR(s.SalaryDate) = @Year
  AND MONTH(s.SalaryDate) = @Month
ORDER BY s.SalaryDate DESC;

END
