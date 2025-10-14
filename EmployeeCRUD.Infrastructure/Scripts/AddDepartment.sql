CREATE OR ALTER PROCEDURE AddDepartment
    @Id UNIQUEIDENTIFIER OUTPUT,
    @DeptName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Id = NEWID();

    DECLARE @Now DATETIME = GETDATE();

    INSERT INTO Departments (Id, DeptName, CreatedAt, UpdatedAt)
    VALUES (@Id, @DeptName, @Now, @Now);

     SELECT 
        Id,
        DeptName AS Name
    FROM Departments
    WHERE Id = @Id;
END
