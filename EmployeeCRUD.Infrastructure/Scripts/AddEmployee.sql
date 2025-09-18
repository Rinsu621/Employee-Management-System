CREATE OR ALTER PROCEDURE AddEmployee
    @Id UNIQUEIDENTIFIER = NULL,
    @EmpName VARCHAR(100),
    @Email VARCHAR(100),
    @Phone VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Id IS NULL
        SET @Id = NEWID();

    DECLARE @Now DATETIME = GETDATE();
    INSERT INTO Employees (Id, EmpName, Email, Phone, CreatedAt, UpdatedAt)
    VALUES (@Id, @EmpName, @Email, @Phone, @Now, @Now);

    SELECT Id, EmpName, Email, Phone, CreatedAt, UpdatedAt
    FROM Employees
    WHERE Id = @Id;
END
