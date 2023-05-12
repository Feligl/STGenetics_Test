USE STGenetics
CREATE TABLE Animals
	(
	AnimalId int NOT NULL IDENTITY (1, 1) PRIMARY KEY,
	Name varchar(MAX) NOT NULL,
	Breed varchar(MAX) NOT NULL,
	BirthDate date NOT NULL,
	Sex varchar(1) NOT NULL CONSTRAINT Sex_Constraint CHECK (Sex='M' OR Sex='F'),
	Price int NOT NULL,
	Status bit NOT NULL,
	)