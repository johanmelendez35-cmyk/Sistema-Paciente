IF DB_ID('ClinicaCalderonDB') IS NULL
    CREATE DATABASE ClinicaCalderonDB;
GO

USE ClinicaCalderonDB;
GO

IF OBJECT_ID('dbo.Patients') IS NULL
    CREATE TABLE Patients (
        Id           INT IDENTITY(1,1) PRIMARY KEY,
        FirstName    NVARCHAR(50)  NOT NULL,
        LastName     NVARCHAR(50)  NOT NULL,
        NationalId   NVARCHAR(11)  NOT NULL UNIQUE,
        Age          INT           NOT NULL,
        Phone        NVARCHAR(15)  NOT NULL,
        Address      NVARCHAR(100) NOT NULL,
        Diagnosis    NVARCHAR(200) NOT NULL,
        HasInsurance BIT           NOT NULL DEFAULT 0
    );
GO
