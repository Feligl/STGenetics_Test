USE STGenetics
GO

CREATE PROCEDURE upsFindAnimalById(@id int) AS
BEGIN
	SELECT * FROM Animals WHERE AnimalId = @id
END 

