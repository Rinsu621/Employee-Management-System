CREATE OR ALTER PROCEDURE UpdateSalaryStatus
    @Id UNIQUEIDENTIFIER,
    @Status NVARCHAR(50),
    @ActionBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Salaries
    SET Status = @Status,
        ActionBy = @ActionBy,
        ActionAt = GETDATE()
    WHERE Id = @Id;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS Success;
END
