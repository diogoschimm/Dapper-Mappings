CREATE TABLE [dbo].[Venda] (
    [IdVenda]    INT             IDENTITY (1, 1) NOT NULL,
    [ValorTotal] DECIMAL (18, 2) NOT NULL,
    [IdCliente]  INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([IdVenda] ASC)
);

