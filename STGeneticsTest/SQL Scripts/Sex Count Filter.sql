USE STGenetics
GO

CREATE PROCEDURE upsSexCountFilter(@id int) AS
BEGIN
	SELECT Sex, COUNT(Sex) As Count
	FROM Animals
	GROUP BY Sex
END 

-- Possible Optimization. Not implemented
--CREATE NONCLUSTERED INDEX IX_Animals_Sex ON Animals (Sex);