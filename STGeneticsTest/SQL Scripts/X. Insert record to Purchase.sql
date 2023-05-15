USE STGenetics
GO

CREATE PROCEDURE upsInsertPurchase(
@totalOrderPrice float,
@freight int
)
AS
BEGIN
	DECLARE @result table(id int)
	INSERT INTO Purchases(TotalOrderPrice,Freight)
	OUTPUT INSERTED.PurchaseId into @result
	VALUES(@totalOrderPrice,@freight)
	SELECT * FROM @result
END