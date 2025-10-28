CREATE OR ALTER   PROCEDURE [dbo].[GetSalaries]
    @Year INT = NULL,
    @Month INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Year IS NULL SET @Year = YEAR(GETDATE());
    IF @Month IS NULL SET @Month = MONTH(GETDATE());

   SELECT 
   s.Id,
    e.EmpName,
    s.BasicSalary,
    s.Conveyance,
    s.Tax,
    s.PF,
    s.ESI,
    s.PaymentMode,
    s.Status,
	 s.CreatedBy,
	 createdEmployee.EmpName AS CreatedByName,
        s.ActionBy,
		 actionEmployee.EmpName AS ActionByName,
        s.ActionAt,
    (s.BasicSalary + s.Conveyance) AS GrossSalary,
    (s.BasicSalary + s.Conveyance - (s.Tax + s.PF + s.ESI)) AS NetSalary,
    r.Name AS Role,
    s.SalaryDate
FROM Salaries s
LEFT JOIN EmployeeManagementSystem.dbo.Employees e 
    ON s.EmployeeId = e.Id

	 -- Join for CreatedBy name
    LEFT JOIN EmployeeManagementSystem.dbo.AspNetUsers createdUser
        ON s.CreatedBy = createdUser.Id
    LEFT JOIN EmployeeManagementSystem.dbo.Employees createdEmployee
        ON createdUser.EmployeeId = createdEmployee.Id


		-- Join for ActionBy name
    LEFT JOIN EmployeeManagementSystem.dbo.AspNetUsers actionUser
        ON s.ActionBy = actionUser.Id
    LEFT JOIN EmployeeManagementSystem.dbo.Employees actionEmployee
        ON actionUser.EmployeeId = actionEmployee.Id

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
