CREATE TABLE Clients (
	Id bigint NOT NULL IDENTITY PRIMARY KEY,
	ClientName nvarchar(200)
)

CREATE TABLE ClientContacts (
	Id bigint NOT NULL IDENTITY PRIMARY KEY,
	ClientId bigint NOT NULL FOREIGN KEY REFERENCES Clients(Id),
	ContactType nvarchar(255),
	ContactValue nvarchar(255)
)