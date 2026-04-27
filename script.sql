CREATE DATABASE PROJETOLOJA;

USE PROJETOLOJA;

CREATE TABLE Usuarios(
 Id INT PRIMARY KEY auto_increment,
 Nome Varchar(40) NOT NULL,
 Email Varchar(100) NOT NULL,
 Senha Varchar(250) NOT NULL,
 Nivel Varchar(40) NOT NULL
);


CREATE TABLE Produtos(
Id INT PRIMARY KEY auto_increment,
Nome VARCHAR(50) NOT NULL,
Preco DECIMAL(10,2) NOT NULL,
DataCadastro DATETIME DEFAULT current_timestamp
);


INSERT INTO Usuarios(Nome,Email,Senha,Nivel) VALUES('Administrador','admin@email.com','$2a$12$Wr.qcmJjoLC1eNTC3G7iRetehmfd6mdmaegDND3f1hlMsNwAQXoz6','Admin');

INSERT INTO Usuarios(Nome,Email,Senha,Nivel) VALUES('Usuario','usuario@email.com','123456','Operador');

SELECT * FROM Usuarios;
SELECT * FROM Produtos;
