USE STGenetics
GO

CREATE PROCEDURE upsInsertAnimal(
@name varchar(MAX), 
@breed varchar(MAX),
@birthdate date,
@sex varchar(1),
@price float,
@status bit
)
AS
BEGIN
	DECLARE @result table(id int)
	INSERT INTO Animals(Name,Breed,BirthDate,Sex,Price,Status) 
	OUTPUT INSERTED.AnimalId into @result
	VALUES(@name,@breed,@birthdate,@sex,@price,@status)
	SELECT * FROM @result
END