CREATE TABLE Owner (
    OwnerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Phone NVARCHAR(15),
    Email NVARCHAR(100)
);

CREATE TABLE Pet (
    PetId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50),
    Type NVARCHAR(30),
    BirthDate DATE,
    OwnerId INT,
    FOREIGN KEY (OwnerId) REFERENCES Owner(OwnerId)
);

CREATE TABLE Service (
    ServiceId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Price DECIMAL(10,2),
    Description NVARCHAR(255)
);

CREATE TABLE PetService (
    PetId INT,
    ServiceId INT,
    ServiceDate DATE,
    Note NVARCHAR(255),
    PRIMARY KEY (PetId, ServiceId, ServiceDate),
    FOREIGN KEY (PetId) REFERENCES Pet(PetId),
    FOREIGN KEY (ServiceId) REFERENCES Service(ServiceId)
);

CREATE TABLE Appointment (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
    PetId INT,
    AppointmentDate DATETIME,
    Reason NVARCHAR(255),
    FOREIGN KEY (PetId) REFERENCES Pet(PetId)
);
