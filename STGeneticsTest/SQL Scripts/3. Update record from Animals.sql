USE STGenetics
GO

CREATE PROCEDURE upsUpdateAnimal(
@id int,
@name varchar(MAX) = NULL, 
@breed varchar(MAX) = NULL,
@birthdate date = NULL,
@sex varchar(1) = NULL,
@price float = NULL,
@status bit = NULL
)
AS
BEGIN
	UPDATE Animals SET 
	Name = IsNull(@name, Name),
	Breed = IsNull(@breed, Breed),
	BirthDate = IsNull(@birthdate, BirthDate),
	Sex = IsNull(@sex,Sex),
	Price = IsNull(@price, Price),
	Status = IsNull(@status, Status)
	WHERE AnimalId = @id

	SELECT * FROM Animals WHERE AnimalId = @id
END