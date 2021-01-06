CREATE TABLE [dbo].[Cliente] (
    [IdCliente]   INT           IDENTITY (1, 1) NOT NULL,
    [nomeCliente] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdCliente] ASC)
);

