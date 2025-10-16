CREATE OR ALTER PROCEDURE AddDepartment
    @Id UNIQUEIDENTIFIER OUTPUT,
    @DeptName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY

    SET @Id = NEWID();

    INSERT INTO Departments (Id, DeptName, CreatedAt, UpdatedAt)
        OUTPUT inserted.Id, inserted.DeptName AS Name
        VALUES (@Id, @DeptName, GETDATE(), GETDATE());

    COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
    END CATCH;
END
