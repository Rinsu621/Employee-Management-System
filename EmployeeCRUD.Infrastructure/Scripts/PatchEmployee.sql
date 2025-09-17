CREATE or ALTER PROCEDURE PatchEmployee
	@Id UNIQUEIDENTIFIER,
	@EmpName VARCHAR(100) = NULL,
	@Email VARCHAR(100) = NULL,
	@Phone VARCHAR(15) = NULL
	As
	BEGIN
	SET NOCOUNT ON;

	UPDATE Employees
	SET 
		EmpName = COALESCE(@EmpName, EmpName),
		Email = COALESCE(@Email, Email),
		Phone = COALESCE(@Phone, Phone),
		UpdatedAt = GETDATE()
	WHERE Id = @Id;

	SELECT Id, EmpName, Email, Phone, CreatedAt, UpdatedAt FROM Employees
	WHERE Id = @Id;
	END