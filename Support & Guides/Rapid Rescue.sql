-- Create Roles Table
CREATE TABLE Roles (
    Role_Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(MAX) NOT NULL,
    Status BIT NOT NULL
);

-- Create Users Table
CREATE TABLE Users (
    User_id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    RememberToken NVARCHAR(100) NOT NULL,
    Role_Id INT NOT NULL,
    IsActive BIT NOT NULL,
    CreatedAt DATETIME2(7) NOT NULL,
    UpdatedAt DATETIME2(7) NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (Role_Id) REFERENCES Roles(Role_Id)
);

-- Create Ambulances Table
CREATE TABLE Ambulances (
    AmbulanceId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleNumber NVARCHAR(50) NOT NULL,
    EquipmentLevel NVARCHAR(50) NOT NULL,
    DriverId INT NOT NULL,
    CONSTRAINT FK_Ambulances_DriverInfo FOREIGN KEY (DriverId) REFERENCES DriverInfo(DriverId)
);

-- Create Contact Table
CREATE TABLE Contact (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15) NOT NULL,
    Subject NVARCHAR(200) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    SubmittedOn DATETIME2(7) NOT NULL
);

-- Create DriverInfo Table
CREATE TABLE DriverInfo (
    DriverId INT IDENTITY(1,1) PRIMARY KEY,
    PhoneNumber NVARCHAR(15) NOT NULL,
    LicenseNumber NVARCHAR(100) NOT NULL,
    LicenseExpiryDate DATETIME2(7) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    VehicleAssigned NVARCHAR(100) NOT NULL,
    DateOfHire DATETIME2(7) NOT NULL,
    IsActive BIT NOT NULL,
    User_id INT NOT NULL,
    CreatedAt DATETIME2(7) NOT NULL,
    UpdatedAt DATETIME2(7) NOT NULL,
    Latitude FLOAT NULL,
    Longitude FLOAT NULL,
    CONSTRAINT FK_DriverInfo_Users FOREIGN KEY (User_id) REFERENCES Users(User_id)
);

-- Create EMTs Table
CREATE TABLE EMTs (
    EMT_Id INT IDENTITY(1,1) PRIMARY KEY,
    User_id INT NOT NULL,
    CertificationNumber NVARCHAR(100) NOT NULL,
    CertificationExpiryDate DATETIME2(7) NOT NULL,
    ContactNumber NVARCHAR(MAX) NOT NULL,
    LicenseNumber NVARCHAR(100) NOT NULL,
    IsAvailable BIT NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME2(7) NOT NULL,
    UpdatedAt DATETIME2(7) NOT NULL,
    CONSTRAINT FK_EMTs_Users FOREIGN KEY (User_id) REFERENCES Users(User_id)
);

-- Create Notifications Table
CREATE TABLE Notifications (
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,
    NotificationType NVARCHAR(MAX) NOT NULL,
    NotificationMessage NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2(7) NOT NULL
);

-- Create PatientsInfo Table
CREATE TABLE PatientsInfo (
    Patient_Id INT IDENTITY(1,1) PRIMARY KEY,
    User_id INT NOT NULL,
    MobileNumber NVARCHAR(15) NOT NULL,
    Situation NVARCHAR(255) NOT NULL,
    PickupLocation NVARCHAR(255) NOT NULL,
    RequestedTime DATETIME2(7) NOT NULL,
    AdditionalDetails NVARCHAR(255) NOT NULL,
    Gender NVARCHAR(MAX) NOT NULL,
    IsEmergency BIT NOT NULL,
    CONSTRAINT FK_PatientsInfo_Users FOREIGN KEY (User_id) REFERENCES Users(User_id)
);

-- Create Requests Table
CREATE TABLE Requests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    DriverId INT NOT NULL,
    PatientLatitude FLOAT NOT NULL,
    PatientLongitude FLOAT NOT NULL,
    RequestedAt DATETIME2(7) NOT NULL,
    DriverStatus NVARCHAR(MAX) NOT NULL,
    CanceledReason NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Requests_DriverInfo FOREIGN KEY (DriverId) REFERENCES DriverInfo(DriverId)
);
