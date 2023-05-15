USE STGenetics
GO

CREATE PROCEDURE upsInsertPurchaseDetail(
@animalId int, 
@purchaseId int,
@unitPrice float,
@quantitySold int
)
AS
BEGIN
	SET NOCOUNT OFF
	INSERT INTO PurchaseDetail(AnimalId,PurchaseId,UnitPrice,QuantitySold)
	VALUES(@animalId,@purchaseId,@unitPrice,@quantitySold)
	SELECT @@ROWCOUNT
END