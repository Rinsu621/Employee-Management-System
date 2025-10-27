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

    IF OBJECT_ID('tempdb..#UserEmails') IS NOT NULL DROP TABLE #UserEmails;

    CREATE TABLE #UserEmails (Email NVARCHAR(256) PRIMARY KEY);

    IF @Role IS NOT NULL
    BEGIN
        INSERT INTO #UserEmails (Email)
        SELECT u.Email
        FROM AspNetUsers u
        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
        INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
        WHERE r.Name = @Role;
    END
    ELSE IF @SearchTerm IS NOT NULL
    BEGIN
        INSERT INTO #UserEmails (Email)
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
            OR EXISTS (SELECT 1 FROM #UserEmails ue WHERE ue.Email = e.Email)
        )
        AND (
            @Role IS NULL 
            OR EXISTS (SELECT 1 FROM #UserEmails ue WHERE ue.Email = e.Email)
        );

    CREATE INDEX IX_FilteredEmployees_Email ON #FilteredEmployees (Email);

    ;WITH OrderedEmployees AS (
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
             WHERE u.Email = f.Email) AS Role,
            ROW_NUMBER() OVER (
                ORDER BY 
                    CASE WHEN @SortKey = 'EmpName' AND @SortAsc = 1 THEN f.EmpName END ASC,
                    CASE WHEN @SortKey = 'EmpName' AND @SortAsc = 0 THEN f.EmpName END DESC,
                    CASE WHEN @SortKey = 'Email' AND @SortAsc = 1 THEN f.Email END ASC,
                    CASE WHEN @SortKey = 'Email' AND @SortAsc = 0 THEN f.Email END DESC,
                    CASE WHEN @SortKey = 'Phone' AND @SortAsc = 1 THEN f.Phone END ASC,
                    CASE WHEN @SortKey = 'Phone' AND @SortAsc = 0 THEN f.Phone END DESC,
                    CASE WHEN @SortKey = 'DepartmentName' AND @SortAsc = 1 THEN f.DepartmentName END ASC,
                    CASE WHEN @SortKey = 'DepartmentName' AND @SortAsc = 0 THEN f.DepartmentName END DESC,
                    CASE WHEN @SortKey = 'CreatedAt' AND @SortAsc = 1 THEN f.CreatedAt END ASC,
                    CASE WHEN @SortKey = 'CreatedAt' AND @SortAsc = 0 THEN f.CreatedAt END DESC
            ) AS RowNum
        FROM #FilteredEmployees f
    )
    SELECT 
        Id, EmpName, Email, Phone, DepartmentName, CreatedAt, Role
    FROM OrderedEmployees
    WHERE RowNum BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize);

    SELECT COUNT(*) AS TotalCount FROM #FilteredEmployees;

    DROP TABLE #FilteredEmployees;
    DROP TABLE #UserEmails;
END
