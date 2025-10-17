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

    WITH FilteredEmployees AS (
        SELECT 
            e.Id,
            e.EmpName,
            e.Email,
            e.Phone,
            d.DeptName AS DepartmentName,
            e.CreatedAt,
            r.Name AS Role
        FROM Employees e
        LEFT JOIN Departments d ON e.DepartmentId = d.Id
        LEFT JOIN AspNetUsers u ON u.EmployeeId = e.Id
		LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id
		LEFT JOIN AspNetRoles r ON r.Id = ur.RoleId
 
        WHERE
            (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
            AND (@FromDate IS NULL OR e.CreatedAt >= @FromDate)
            AND (@ToDate IS NULL OR e.CreatedAt <= @ToDate)
            AND (
                @SearchTerm IS NULL 
                OR e.EmpName LIKE '%' + @SearchTerm + '%'
                OR e.Email LIKE '%' + @SearchTerm + '%'
				OR e.Phone LIKE '%' + @SearchTerm + '%'
                OR d.DeptName LIKE '%' + @SearchTerm + '%'
				OR r.Name LIKE '%' + @SearchTerm + '%'
                OR CAST(e.Id AS NVARCHAR(50)) LIKE '%' + @SearchTerm + '%'
            )
            AND (@Role IS NULL OR r.Name = @Role)
    )
    SELECT *
    INTO #PagedEmployees
    FROM FilteredEmployees;

    CREATE INDEX IX_PagedEmployees_CreatedAt ON #PagedEmployees (CreatedAt);

    DECLARE @sql NVARCHAR(MAX) = N'
        SELECT *
        FROM #PagedEmployees
        ORDER BY ' + QUOTENAME(@SortKey) + CASE WHEN @SortAsc = 1 THEN ' ASC' ELSE ' DESC' END + '
        OFFSET ' + CAST((@Page - 1) * @PageSize AS NVARCHAR(10)) + ' ROWS
        FETCH NEXT ' + CAST(@PageSize AS NVARCHAR(10)) + ' ROWS ONLY;
    ';

    EXEC sp_executesql @sql;

    SELECT COUNT(*) AS TotalCount FROM #PagedEmployees;

    DROP TABLE #PagedEmployees;
END;
