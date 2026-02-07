-- Create Database (run this separately if database doesn't exist)
-- CREATE DATABASE ApodDb;
-- GO

-- Use the database
USE ApodDb;
GO

-- Create APOD Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Apod')
BEGIN
    CREATE TABLE Apod (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Date DATE NOT NULL UNIQUE,
        Title NVARCHAR(500) NOT NULL,
        Explanation NVARCHAR(MAX) NOT NULL,
        Url NVARCHAR(1000) NOT NULL,
        HdUrl NVARCHAR(1000) NULL,
        MediaType NVARCHAR(50) NOT NULL,
        ServiceVersion NVARCHAR(20) NOT NULL,
        Copyright NVARCHAR(200) NULL,
        ThumbnailUrl NVARCHAR(1000) NULL,
        SavedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );

    PRINT 'Table Apod created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Apod already exists.';
END
GO

-- Create index on Date for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Apod_Date')
BEGIN
    CREATE INDEX IX_Apod_Date ON Apod(Date DESC);
    PRINT 'Index IX_Apod_Date created successfully.';
END
GO
