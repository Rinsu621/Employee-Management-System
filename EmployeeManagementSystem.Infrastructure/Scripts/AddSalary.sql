CREATE OR ALTER PROCEDURE AddSalary
    @Id UNIQUEIDENTIFIER OUTPUT,
    @EmployeeId UNIQUEIDENTIFIER,
    @BasicSalary DECIMAL(18,2),
    @Conveyance DECIMAL(18,2),
    @Tax DECIMAL(18,2),
    @PF DECIMAL(18,2),
    @ESI DECIMAL(18,2),
    @PaymentMode NVARCHAR(50),
    @CreatedBy UNIQUEIDENTIFIER,
    @SalaryDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SET @Id = NEWID();
    DECLARE @Now DATETIME = GETDATE();

    INSERT INTO Salaries 
        (Id, EmployeeId, BasicSalary, Conveyance, Tax, PF, ESI, PaymentMode, Status, CreatedBy, CreatedAt, SalaryDate) OUTPUT(inserted.Id)
    VALUES 
        (@Id, @EmployeeId, @BasicSalary, @Conveyance, @Tax, @PF, @ESI, @PaymentMode, 'Pending', @CreatedBy, @Now, @SalaryDate)

END
