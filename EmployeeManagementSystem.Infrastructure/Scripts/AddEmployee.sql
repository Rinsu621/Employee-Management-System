CREATE OR ALTER PROCEDURE AddEmployee
@EmpName NVARCHAR(100),
@Email NVARCHAR(100),
@Phone NVARCHAR(15),
@RoleName NVARCHAR(50),
@DepartmentId UNIQUEIDENTIFIER=null,
@DefaultPassword NVARCHAR(100)

AS
BEGIN
SET NOCOUNT ON;
BEGIN TRY
BEGIN TRANSACTION
DECLARE @EmployeeId UNIQUEIDENTIFIER = NEWID();
        DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
        DECLARE @Now DATETIME = GETDATE();
        DECLARE @NormalizedEmail NVARCHAR(256) = UPPER(@Email);
        DECLARE @NormalizedUserName NVARCHAR(256) = UPPER(@Email);
        DECLARE @RoleId NVARCHAR(450);

SELECT @RoleId = Id FROM AspNetRoles WHERE NormalizedName = UPPER(@RoleName);

--Inserting into employees
INSERT INTO Employees(Id, EmpName, Email, Phone, DepartmentId, CreatedAt, UpdatedAt) VALUES(@EmployeeId, @EmpName, @Email, @Phone, @DepartmentId, @Now, @Now);

--Inserting into AspNetUsers
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, EmployeeId)
        VALUES (@UserId, @Email, @NormalizedUserName, @Email, @NormalizedEmail, 0, HASHBYTES('SHA2_256', @DefaultPassword), NEWID(), NEWID(),0,0,1,0, @EmployeeId);

INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);

COMMIT TRANSACTION;
SELECT 
e.Id ,
e.EmpName,
e.Email,
e.Phone,
d.DeptName AS DepartmentName,
@RoleName AS Role,
e.CreatedAt,
e.UpdatedAt
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id
WHERE e.Id = @EmployeeId;
END TRY
BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            THROW;
    END CATCH
END;