CREATE OR ALTER PROCEDURE AddSalary
    @Id UNIQUEIDENTIFIER OUTPUT,
    @EmployeeId UNIQUEIDENTIFIER,
    @BasicSalary DECIMAL(18,2),
    @Conveyance DECIMAL(18,2),
    @Tax DECIMAL(18,2),
    @PF DECIMAL(18,2),
    @ESI DECIMAL(18,2),
    @CreatedAt DATETIME OUTPUT,
    @UpdatedAt DATETIME OUTPUT,
    @SalaryDate DATETIME
AS
BEGIN 
SET NOCOUNT ON;
SET @ID= NEWID();
DECLARE  @Now DATETIME= GETDATE();

INSERT INTO Salaries (Id, EmployeeId, BasicSalary, Conveyance, Tax, PF, ESI, CreatedAt, UpdatedAt, SalaryDate) OUTPUT(inserted.Id) VALUES (@Id,@EmployeeId, @BasicSalary, @Conveyance,@Tax,@PF,@ESI,@Now,@Now,@SalaryDate)
END