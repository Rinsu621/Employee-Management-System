CREATE OR ALTER PROCEDURE AddEmployee
    @Id UNIQUEIDENTIFIER = NULL,
    @EmpName VARCHAR(100),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @DepartmentId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @Id IS NULL
            SET @Id = NEWID();

        DECLARE @Now DATETIME = GETDATE();

        INSERT INTO Employees (Id, EmpName, Email, Phone, DepartmentId, CreatedAt, UpdatedAt)
        VALUES (@Id, @EmpName, @Email, @Phone, @DepartmentId, @Now, @Now);

        COMMIT TRANSACTION;

        SELECT
            e.Id, 
            e.EmpName, 
            e.Email, 
            e.Phone, 
            d.DeptName AS DepartmentName,
            e.CreatedAt,
            e.UpdatedAt
        FROM Employees e
        LEFT JOIN Departments d
            ON e.DepartmentId = d.Id
        WHERE e.Id = @Id;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW; 
    END CATCH
END
