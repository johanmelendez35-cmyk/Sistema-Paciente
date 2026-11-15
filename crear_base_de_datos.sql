-- Ejecutar este script completo en SQL Server Management Studio (SSMS)
-- antes de correr el programa.

CREATE DATABASE RegistroPacientesDB;
GO

USE RegistroPacientesDB;
GO

CREATE TABLE Pacientes (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(50)  NOT NULL,
    Apellido    NVARCHAR(50)  NOT NULL,
    Cedula      NVARCHAR(11)  NOT NULL UNIQUE,
    Edad        INT           NOT NULL,
    Telefono    NVARCHAR(15)  NOT NULL,
    Direccion   NVARCHAR(100) NOT NULL,
    Diagnostico NVARCHAR(200) NOT NULL,
    TieneSeguro BIT           NOT NULL DEFAULT 0
);
GO
