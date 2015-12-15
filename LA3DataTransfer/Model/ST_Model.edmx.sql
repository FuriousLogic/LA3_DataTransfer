
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/12/2013 07:15:39
-- Generated from EDMX file: C:\Users\Barry\Documents\Visual Studio 2012\Projects\LA3DataTransfer\LA3DataTransfer\Model\ST_Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LoanArrangerStaging_ef];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ST_Collector]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ST_Collector];
GO
IF OBJECT_ID(N'[dbo].[ST_Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ST_Account];
GO
IF OBJECT_ID(N'[dbo].[ST_Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ST_Customer];
GO
IF OBJECT_ID(N'[dbo].[ST_Payment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ST_Payment];
GO
IF OBJECT_ID(N'[dbo].[Parameters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Parameters];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ST_Collector'
CREATE TABLE [dbo].[ST_Collector] (
    [collector_id] int  NOT NULL,
    [notes] nvarchar(max)  NULL,
    [name] nvarchar(40)  NULL
);
GO

-- Creating table 'ST_Account'
CREATE TABLE [dbo].[ST_Account] (
    [account_id] int  NOT NULL,
    [cust_id] int  NULL,
    [ret_price] decimal(19,4)  NULL,
    [tot_amount] decimal(19,4)  NULL,
    [period] smallint  NULL,
    [payment] decimal(19,4)  NULL,
    [left_to_pay] decimal(19,4)  NULL,
    [overdue_amount] decimal(19,4)  NULL,
    [last_checked] datetime  NULL,
    [next_payment] datetime  NULL,
    [posted] bit  NOT NULL,
    [posted_date] datetime  NULL,
    [monthly] bit  NOT NULL,
    [printed_form] bit  NOT NULL,
    [pay_off] nvarchar(50)  NULL,
    [pay_off_amount] decimal(19,4)  NULL,
    [notes] nvarchar(max)  NULL
);
GO

-- Creating table 'ST_Customer'
CREATE TABLE [dbo].[ST_Customer] (
    [cust_id] int  NOT NULL,
    [forename] nvarchar(30)  NULL,
    [surname] nvarchar(20)  NULL,
    [address] nvarchar(max)  NULL,
    [notes] nvarchar(max)  NULL,
    [collector_id] int  NULL,
    [week_day] int  NULL,
    [max_loan] float  NULL,
    [cust_date] datetime  NULL,
    [debt] decimal(19,4)  NULL,
    [telephone] nvarchar(30)  NULL,
    [start_date] datetime  NULL,
    [Locked] bit  NOT NULL
);
GO

-- Creating table 'ST_Payment'
CREATE TABLE [dbo].[ST_Payment] (
    [Payment_id] int  NOT NULL,
    [account_id] int  NOT NULL,
    [date] datetime  NULL,
    [amount] decimal(19,4)  NULL,
    [Note] nvarchar(50)  NULL,
    [Sundry] bit  NOT NULL,
    [Left_to_pay] float  NULL
);
GO

-- Creating table 'Parameters'
CREATE TABLE [dbo].[Parameters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] int  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [collector_id] in table 'ST_Collector'
ALTER TABLE [dbo].[ST_Collector]
ADD CONSTRAINT [PK_ST_Collector]
    PRIMARY KEY CLUSTERED ([collector_id] ASC);
GO

-- Creating primary key on [account_id] in table 'ST_Account'
ALTER TABLE [dbo].[ST_Account]
ADD CONSTRAINT [PK_ST_Account]
    PRIMARY KEY CLUSTERED ([account_id] ASC);
GO

-- Creating primary key on [cust_id] in table 'ST_Customer'
ALTER TABLE [dbo].[ST_Customer]
ADD CONSTRAINT [PK_ST_Customer]
    PRIMARY KEY CLUSTERED ([cust_id] ASC);
GO

-- Creating primary key on [Payment_id], [account_id] in table 'ST_Payment'
ALTER TABLE [dbo].[ST_Payment]
ADD CONSTRAINT [PK_ST_Payment]
    PRIMARY KEY CLUSTERED ([Payment_id], [account_id] ASC);
GO

-- Creating primary key on [Id] in table 'Parameters'
ALTER TABLE [dbo].[Parameters]
ADD CONSTRAINT [PK_Parameters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------