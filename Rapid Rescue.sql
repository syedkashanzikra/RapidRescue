USE RapidRescue;
GO

-- Table: Roles
CREATE TABLE Roles (
    Role_Id int IDENTITY(1,1) PRIMARY KEY,
    RoleName nvarchar(MAX) NOT NULL,
    Status bit NOT NULL
);
GO

-- Table: Users
CREATE TABLE Users (
    User_id int IDENTITY(1,1) PRIMARY KEY,
    FirstName nvarchar(50) NOT NULL,
    LastName nvarchar(50) NOT NULL,
    Email nvarchar(100) NOT NULL,
    Password nvarchar(100) NOT NULL,
    RememberToken nvarchar(100) NOT NULL,
    Role_Id int NOT NULL,
    IsActive bit NOT NULL,
    CreatedAt datetime2 NOT NULL,
    UpdatedAt datetime2 NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (Role_Id) REFERENCES Roles(Role_Id) ON DELETE CASCADE
);
GO

-- Table: DriverInfo
CREATE TABLE DriverInfo (
    DriverId int IDENTITY(1,1) PRIMARY KEY,
    PhoneNumber nvarchar(15) NOT NULL,
    LicenseNumber nvarchar(100) NOT NULL,
    LicenseExpiryDate datetime2 NOT NULL,
    Address nvarchar(200) NOT NULL,
    VehicleAssigned nvarchar(100) NOT NULL,
    DateOfHire datetime2 NOT NULL,
    IsActive bit NOT NULL,
    User_id int NOT NULL,
    CreatedAt datetime2 NOT NULL,
    UpdatedAt datetime2 NOT NULL,
    Latitude float NULL,
    Longitude float NULL,
    CONSTRAINT FK_DriverInfo_Users FOREIGN KEY (User_id) REFERENCES Users(User_id) ON DELETE CASCADE
);
GO

-- Table: Ambulances
CREATE TABLE Ambulances (
    AmbulanceId int IDENTITY(1,1) PRIMARY KEY,
    VehicleNumber nvarchar(50) NOT NULL,
    EquipmentLevel nvarchar(50) NOT NULL,
    DriverId int NOT NULL,
    CONSTRAINT FK_Ambulances_DriverInfo FOREIGN KEY (DriverId) REFERENCES DriverInfo(DriverId) ON DELETE CASCADE
);
GO

-- Table: EMTs
CREATE TABLE EMTs (
    EMT_Id int IDENTITY(1,1) PRIMARY KEY,
    User_id int NOT NULL,
    CertificationNumber nvarchar(100) NOT NULL,
    CertificationExpiryDate datetime2 NOT NULL,
    ContactNumber nvarchar(MAX) NOT NULL,
    LicenseNumber nvarchar(100) NOT NULL,
    IsAvailable bit NOT NULL,
    Address nvarchar(200) NOT NULL,
    CreatedAt datetime2 NOT NULL,
    UpdatedAt datetime2 NOT NULL,
    CONSTRAINT FK_EMTs_Users FOREIGN KEY (User_id) REFERENCES Users(User_id) ON DELETE CASCADE
);
GO

-- Table: PatientsInfo
CREATE TABLE PatientsInfo (
    Patient_Id int IDENTITY(1,1) PRIMARY KEY,
    User_id int NOT NULL,
    MobileNumber nvarchar(15) NOT NULL,
    Situation nvarchar(255) NOT NULL,
    PickupLocation nvarchar(255) NOT NULL,
    RequestedTime datetime2 NOT NULL,
    AdditionalDetails nvarchar(255) NOT NULL,
    Gender nvarchar(MAX) NOT NULL,
    IsEmergency bit NOT NULL,
    CONSTRAINT FK_PatientsInfo_Users FOREIGN KEY (User_id) REFERENCES Users(User_id) ON DELETE CASCADE
);
GO

-- Table: Requests
CREATE TABLE Requests (
    RequestId int IDENTITY(1,1) PRIMARY KEY,
    DriverId int NOT NULL,
    PatientLatitude float NOT NULL,
    PatientLongitude float NOT NULL,
    RequestedAt datetime2 NOT NULL,
    DriverStatus nvarchar(MAX) NOT NULL,
    CONSTRAINT FK_Requests_DriverInfo FOREIGN KEY (DriverId) REFERENCES DriverInfo(DriverId) ON DELETE CASCADE
);
GO

-- Table: Contact
CREATE TABLE Contact (
    Id int IDENTITY(1,1) PRIMARY KEY,
    FullName nvarchar(100) NOT NULL,
    Email nvarchar(100) NOT NULL,
    Phone nvarchar(15) NOT NULL,
    Subject nvarchar(200) NOT NULL,
    Message nvarchar(500) NOT NULL,
    SubmittedOn datetime2 NOT NULL
);
GO
