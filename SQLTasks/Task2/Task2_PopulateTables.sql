DELETE FROM dbo.ClientContacts
DELETE FROM dbo.Clients

INSERT INTO dbo.Clients(ClientName) VALUES('John')
INSERT INTO dbo.Clients(ClientName) VALUES('Jill')
INSERT INTO dbo.Clients(ClientName) VALUES('Jane')
INSERT INTO dbo.Clients(ClientName) VALUES('Will')

DECLARE @ClientId AS bigint

SELECT @ClientId = Id FROM dbo.Clients WHERE ClientName = 'John'
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Email', 'john@mail.com')

SELECT @ClientId = Id FROM dbo.Clients WHERE ClientName = 'Jill'
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Email', 'jill@mail.com')

SELECT @ClientId = Id FROM dbo.Clients WHERE ClientName = 'Jane'
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Email', 'jane@mail.com')
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Phone', '+123456789')

SELECT @ClientId = Id FROM dbo.Clients WHERE ClientName = 'Will'
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Email', 'will@mail.com')
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Phone', '+987654321')
INSERT INTO dbo.ClientContacts(ClientId, ContactType, ContactValue) VALUES(@ClientId, 'Skype', 'WillOnSkype')
