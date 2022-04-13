SELECT
	MIN(Clients.ClientName) as ClientName,
	COUNT(ClientContacts.ClientId) as ContactCount
FROM dbo.ClientContacts
JOIN Clients ON Clients.Id = ClientContacts.ClientId
GROUP BY ClientId
