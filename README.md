# MinimalApi

Este repositório contém um projeto desenvolvido como parte do Bootcamp Avanade - Back-end com .NET e IA da DIO, focado no aprendizado e aplicação de ASP.NET Minimal APIs.

## 📋 Sobre o Projeto

O objetivo deste projeto é explorar e demonstrar a construção de APIs de forma concisa e eficiente utilizando as Minimal APIs do ASP.NET. Ele aborda conceitos fundamentais para o desenvolvimento de back-ends modernos, incluindo autenticação, gerenciamento de dados e testes.

## 🚀 Funcionalidades Implementadas

- **Criação de Projeto**: Configuração inicial de um projeto Minimal API
- **Definição de Entidades**: Modelagem de dados para o projeto
- **Autenticação e Autorização**:
  - Sistema de login com Entity Framework
  - Tabela de administradores com seed para usuário padrão
  - Geração de tokens JWT
  - Endpoints protegidos para administradores
- **Gerenciamento de Veículos**:
  - Operações CRUD completas (Create, Read, Update, Delete)
  - Endpoints específicos para gestão de veículos
- **Documentação**: Configuração do Swagger com suporte a autenticação JWT
- **Testes**:
  - Testes de unidade para o modelo de Administrador
  - Testes de persistência
  - Testes de request
- **Deployment**: Configuração para deploy da aplicação

## 🛠️ Tecnologias Utilizadas

- ASP.NET Core Minimal APIs
- C#
- .NET 8 (LTS)
- Entity Framework Core
- JWT (JSON Web Tokens)
- Swagger/OpenAPI
- xUnit (para testes)

## 📦 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- IDE/Editor de texto (Visual Studio, VS Code, Rider, etc.)
- Git

## 🏃‍♂️ Como Executar o Projeto

1. **Clone o repositório**:
```bash
git clone https://github.com/seu-usuario/minimalapi.git
cd minimalapi
