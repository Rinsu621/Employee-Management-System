CREATE  PROCEDURE UpdateEmployee
    @Id UNIQUEIDENTIFIER,
    @EmpName VARCHAR(100),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @UpdatedAt DATETIME = NULL
AS
BEGIN
    UPDATE Employees
    SET
        EmpName=@EmpName,
        Email=@Email,
        Phone=@Phone,
        UpdatedAt=GETDATE()
    WHERE Id=@Id;

    SELECT Id, EmpName, Email, Phone, CreatedAt, UpdatedAt
    FROM Employees
    WHERE Id=@Id;
END