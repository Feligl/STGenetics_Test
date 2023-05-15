USE STGenetics
GO

CREATE TABLE Purchases
	(
	PurchaseId int NOT NULL IDENTITY (1, 1) PRIMARY KEY,
	TotalOrderPrice float NOT NULL,
	Freight int NOT NULL,
	)

CREATE TABLE PurchaseDetails
	(
	AnimalId int NOT NULL,
	PurchaseId int NOT NULL,
	UnitPrice float NOT NULL,
	QuantitySold int NOT NULL,
	CONSTRAINT PK_Animal_Purchase PRIMARY KEY (AnimalId, PurchaseId),
	CONSTRAINT FK_Animal_Id FOREIGN KEY (AnimalId) REFERENCES Animals(AnimalId),
	CONSTRAINT FK_Purchase_Id FOREIGN KEY (PurchaseId) REFERENCES Purchases(PurchaseId)
	)