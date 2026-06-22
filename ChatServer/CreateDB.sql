IF DB_ID('ChatAppDB') IS NULL
BEGIN
    CREATE DATABASE ChatAppDB;
END
GO

USE ChatAppDB;
GO

IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(256) NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NULL,
        IsEmailVerified BIT NOT NULL DEFAULT 0,
        AvatarBase64 NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID('Messages', 'U') IS NULL
BEGIN
    CREATE TABLE Messages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Sender NVARCHAR(50) NOT NULL,
        Receiver NVARCHAR(50) NULL,
        Content NVARCHAR(MAX) NOT NULL,
        SendTime DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID('OnlineUsers', 'U') IS NULL
BEGIN
    CREATE TABLE OnlineUsers (
        Username NVARCHAR(50) NOT NULL PRIMARY KEY,
        ServerPort INT NOT NULL,
        LastSeen DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID('FileTransfers', 'U') IS NULL
BEGIN
    CREATE TABLE FileTransfers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Sender NVARCHAR(50) NOT NULL,
        Receiver NVARCHAR(50) NOT NULL,
        FileName NVARCHAR(255) NOT NULL,
        Base64Data NVARCHAR(MAX) NOT NULL,
        SendTime DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID('Rooms', 'U') IS NULL
BEGIN
    CREATE TABLE Rooms (
        RoomId CHAR(6) NOT NULL PRIMARY KEY,
        RoomName NVARCHAR(100) NOT NULL,
        OwnerUsername NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID('RoomMembers', 'U') IS NULL
BEGIN
    CREATE TABLE RoomMembers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RoomId CHAR(6) NOT NULL,
        Username NVARCHAR(50) NOT NULL,
        JoinedAt DATETIME NOT NULL DEFAULT GETDATE(),

        CONSTRAINT UQ_RoomMembers UNIQUE (RoomId, Username)
    );
END
GO
IF OBJECT_ID('RoomMessages', 'U') IS NULL
BEGIN
    CREATE TABLE RoomMessages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RoomId CHAR(6) NOT NULL,
        Sender NVARCHAR(50) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        SendTime DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO