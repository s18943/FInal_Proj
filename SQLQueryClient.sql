-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2020-06-21 10:41:49.025

-- tables
-- Table: Banner
CREATE TABLE Banner (
    IdAdvertisement int  NOT NULL,
    Name int  NOT NULL,
    Price decimal(6,2)  NOT NULL,
    Area decimal(6,2)  NOT NULL,
    IdCampaign int  NOT NULL,
    CONSTRAINT Banner_pk PRIMARY KEY  (IdAdvertisement)
);

-- Table: Building
CREATE TABLE Building (
    IdBuilding int  NOT NULL,
    Street nvarchar(100)  NOT NULL,
    StreetNumber int  NOT NULL,
    City nvarchar(100)  NOT NULL,
    Height decimal(6,2)  NOT NULL,
    CONSTRAINT Building_pk PRIMARY KEY  (IdBuilding)
);

-- Table: Campaign
CREATE TABLE Campaign (
    IdCampaign int  NOT NULL,
    IdClient int  NOT NULL,
    StartDate date  NOT NULL,
    EndDate date  NOT NULL,
    PricePerSquareMeter decimal(6,2)  NOT NULL,
    FromIdBuilding int  NOT NULL,
    ToIdBuilding int  NOT NULL,
    CONSTRAINT Campaign_pk PRIMARY KEY  (IdCampaign)
);

-- Table: Client
CREATE TABLE Client (
    IdClient int  NOT NULL,
    FirstName nvarchar(100)  NOT NULL,
    LastName nvarchar(100)  NOT NULL,
    Email nvarchar(100)  NOT NULL,
    Phone nvarchar(100)  NOT NULL,
    Login nvarchar(100)  NOT NULL,
    Password nvarchar(100)  NOT NULL,
    
    CONSTRAINT Client_pk PRIMARY KEY  (IdClient)
);

-- foreign keys
-- Reference: Banner_Campaign (table: Banner)
ALTER TABLE Banner ADD CONSTRAINT Banner_Campaign
    FOREIGN KEY (IdCampaign)
    REFERENCES Campaign (IdCampaign);

-- Reference: Campaign_Client (table: Campaign)
ALTER TABLE Campaign ADD CONSTRAINT Campaign_Client
    FOREIGN KEY (IdClient)
    REFERENCES Client (IdClient);

-- Reference: FromBuilding (table: Campaign)
ALTER TABLE Campaign ADD CONSTRAINT FromBuilding
    FOREIGN KEY (FromIdBuilding)
    REFERENCES Building (IdBuilding);

-- Reference: ToBuilding (table: Campaign)
ALTER TABLE Campaign ADD CONSTRAINT ToBuilding
    FOREIGN KEY (ToIdBuilding)
    REFERENCES Building (IdBuilding);

-- End of file.



