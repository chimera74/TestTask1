SELECT
	Id, Sd, MIN(Ed) AS Ed
FROM
	(SELECT
		S.Id AS Id,
		S.Dt AS Sd,
		E.Dt AS Ed
	FROM dbo.Dates S, dbo.Dates E
	WHERE
		S.Id = E.Id
		AND S.Dt < E.Dt
	) AS Subquery
GROUP BY Id, Sd