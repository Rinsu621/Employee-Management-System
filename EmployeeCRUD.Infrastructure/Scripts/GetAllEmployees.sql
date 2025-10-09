CREATE OR ALTER PROCEDURE GetAllEmployees
    @Page INT,
    @PageSize INT,
    @Role NVARCHAR(256) = NULL,
    @DepartmentId UNIQUEIDENTIFIER = NULL,
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL,
    @SearchTerm NVARCHAR(200) = NULL,
    @SortKey NVARCHAR(50) = 'CreatedAt',
    @SortAsc BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserEmails TABLE (Email NVARCHAR(256));

    IF @Role IS NOT NULL
    BEGIN
        INSERT INTO @UserEmails (Email)
        SELECT u.Email
        FROM AspNetUsers u
        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
        INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
        WHERE r.Name = @Role;
    END
    ELSE IF @SearchTerm IS NOT NULL
    BEGIN
        INSERT INTO @UserEmails (Email)
        SELECT u.Email
        FROM AspNetUsers u
        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
        INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
        WHERE LOWER(r.Name) LIKE '%' + LOWER(@SearchTerm) + '%';
    END

    IF OBJECT_ID('tempdb..#FilteredEmployees') IS NOT NULL DROP TABLE #FilteredEmployees;

    SELECT 
        e.Id,
        e.EmpName,
        e.Email,
        e.Phone,
        d.DeptName AS DepartmentName,
        e.CreatedAt
    INTO #FilteredEmployees
    FROM Employees e
    LEFT JOIN Departments d ON e.DepartmentId = d.Id
    WHERE
        (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
        AND (@FromDate IS NULL OR e.CreatedAt >= @FromDate)
        AND (@ToDate IS NULL OR e.CreatedAt <= @ToDate)
        AND (
            @SearchTerm IS NULL 
            OR LOWER(e.EmpName) LIKE '%' + LOWER(@SearchTerm) + '%'
            OR LOWER(e.Email) LIKE '%' + LOWER(@SearchTerm) + '%'
            OR LOWER(d.DeptName) LIKE '%' + LOWER(@SearchTerm) + '%'
            OR CAST(e.Id AS NVARCHAR(50)) LIKE '%' + LOWER(@SearchTerm) + '%'
            OR EXISTS (SELECT 1 FROM @UserEmails ue WHERE ue.Email = e.Email)
        )
        AND (
            @Role IS NULL 
            OR EXISTS (SELECT 1 FROM @UserEmails ue WHERE ue.Email = e.Email)
        );

   DECLARE @sql NVARCHAR(MAX) = N'
        SELECT 
            f.Id,
            f.EmpName,
            f.Email,
            f.Phone,
            f.DepartmentName,
            f.CreatedAt,
            (SELECT TOP 1 r.Name
             FROM AspNetUserRoles ur
             INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
             INNER JOIN AspNetUsers u ON u.Id = ur.UserId
             WHERE u.Email = f.Email) AS Role
        FROM #FilteredEmployees f
        ORDER BY ' + QUOTENAME(@SortKey) + CASE WHEN @SortAsc = 1 THEN ' ASC' ELSE ' DESC' END + '
        OFFSET ' + CAST((@Page - 1) * @PageSize AS NVARCHAR(10)) + ' ROWS
        FETCH NEXT ' + CAST(@PageSize AS NVARCHAR(10)) + ' ROWS ONLY;
    ';
    EXEC sp_executesql @sql;

    SELECT COUNT(*) AS TotalCount
    FROM #FilteredEmployees;

	DROP TABLE #FilteredEmployees;
END
