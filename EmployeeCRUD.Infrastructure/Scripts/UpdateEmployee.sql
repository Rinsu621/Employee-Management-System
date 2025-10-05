CREATE PROCEDURE UpdateEmployee
    @EmployeeId UNIQUEIDENTIFIER,
    @EmpName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(50),
    @DepartmentId UNIQUEIDENTIFIER = NULL,
    @Role NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Update Employee table
    UPDATE Employees
    SET EmpName = @EmpName,
        Email = @Email,
        Phone = @Phone,
        DepartmentId = @DepartmentId
    WHERE Id = @EmployeeId;

    -- 2. Update Identity user
    UPDATE AspNetUsers
    SET UserName = @Email,
        Email = @Email
    WHERE EmployeeId = @EmployeeId;

    -- 3. Get UserId
    DECLARE @UserId NVARCHAR(450);
    SELECT @UserId = Id FROM AspNetUsers WHERE EmployeeId = @EmployeeId;

    -- 4. Get RoleId
    DECLARE @RoleId NVARCHAR(450);
    SELECT @RoleId = Id FROM AspNetRoles WHERE Name = @Role;

    -- 5. If Role does not exist, create it
    IF @RoleId IS NULL
    BEGIN
        INSERT INTO AspNetRoles (Id, Name, NormalizedName)
        VALUES (NEWID(), @Role, UPPER(@Role));

        SELECT @RoleId = Id FROM AspNetRoles WHERE Name = @Role;
    END

    -- 6. Check current role
    DECLARE @CurrentRoleId NVARCHAR(450);
    SELECT @CurrentRoleId = RoleId 
    FROM AspNetUserRoles 
    WHERE UserId = @UserId;

    -- 7. Remove old role if different
    IF @CurrentRoleId IS NOT NULL AND @CurrentRoleId != @RoleId
    BEGIN
        DELETE FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @CurrentRoleId;
    END

    -- 8. Assign new role if not already assigned
    IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AspNetUserRoles(UserId, RoleId)
        VALUES (@UserId, @RoleId);
    END
END
