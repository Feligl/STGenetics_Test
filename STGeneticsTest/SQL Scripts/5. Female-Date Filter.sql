USE STGenetics
GO

CREATE PROCEDURE upsSexCountFilter(@id int) AS
BEGIN
    SELECT AnimalId, Name, BirthDate, Status
    FROM Animals
    WHERE Sex = 'F' 
        AND BirthDate <= DATEADD(YEAR, -2, GETDATE())
    ORDER BY Name;
END 

-- Possible Optimization. Not implemented
--CREATE NONCLUSTERED INDEX IX_FemaleDateFilter ON Animals (Sex, Status, BirthDate) INCLUDE (Name);