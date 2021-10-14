CREATE DATABASE dashboard;
go

use dashboard;
go

CREATE TABLE Pedidos (id INT IDENTITY(1,1), data_criacao DATETIME, data_entrega DATETIME, endereco VARCHAR(100), nome VARCHAR(50), veiculo VARCHAR(100), placa VARCHAR(20), CONSTRAINT PK_PEDIDOS PRIMARY KEY (id));
GO

CREATE TABLE Produtos(id INT IDENTITY(1,1), nome VARCHAR(50), descricao VARCHAR(100), valor DECIMAL(12,2), CONSTRAINT PK_PRODUTOS PRIMARY KEY (id));
GO

CREATE TABLE Pedidos_Produtos(id_pedido INT, id_produto INT, 
                              CONSTRAINT PK_PEDIDO_PRODUTO PRIMARY KEY (id_pedido, id_produto), 
							  CONSTRAINT FK_PEDIDO FOREIGN KEY (id_pedido) REFERENCES Pedidos(id),
							  CONSTRAINT FK_PRODUTO FOREIGN KEY (id_produto) REFERENCES Produtos(id));
GO
