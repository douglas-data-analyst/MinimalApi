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

### 1. **Clone o repositório**:
```bash
git clone https://github.com/seu-usuario/minimalapi.git
cd minimalapi
```

### 2. **Restaure as dependências**:
```bash
dotnet restore
```

### 3. **Configure o banco de dados**:
Edite o arquivo `appsettings.json` e ajuste a connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MinimalApiDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-super-segura-aqui-2024",
    "Issuer": "MinimalApi",
    "Audience": "MinimalApiUsers"
  }
}
```

### 4. **Execute as migrações do Entity Framework**:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. **Execute a aplicação**:
```bash
dotnet run
```

### 6. **Acesse a aplicação**:
- **API Principal**: https://localhost:7000
- **Swagger/OpenAPI**: https://localhost:7000/swagger
- **Health Checks**: https://localhost:7000/health

## 🔐 Autenticação

### Para acessar os endpoints protegidos:

1. **Faça login** no endpoint `POST /api/admin/login`:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

2. **Utilize o token JWT** retornado no header das requisições:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Exemplo de requisição autenticada:
```bash
curl -X GET https://localhost:7000/api/veiculos \
  -H "Authorization: Bearer {seu-token-jwt}" \
  -H "Content-Type: application/json"
```

## 🧪 Testes

### Executar testes unitários:
```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"

# Executar com cobertura detalhada
dotnet test --logger "console;verbosity=detailed"

# Gerar relatório de cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Executar testes de integração:
```bash
cd MinimalApi.Tests
dotnet test
```

## 📊 Health Checks

Verifique o status da aplicação:
```bash
curl https://localhost:7000/health
```

## 🐛 Debug

### Para executar em modo desenvolvimento:
```bash
dotnet run --environment Development
```

### Para visualizar logs detalhados:
```bash
dotnet run --log-level Information
```

## 📦 Build para produção

### Criar build otimizado:
```bash
dotnet publish -c Release -o ./publish
```

### Executar em produção:
```bash
cd publish
dotnet MinimalApi.dll
```

## 🗄️ Configuração do Banco de Dados

### Usando SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MinimalApiDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=true;"
  }
}
```

### Usando SQLite:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=minimalapi.db"
  }
}
```

### Comandos úteis do Entity Framework:
```bash
# Criar nova migration
dotnet ef migrations add AddNewFeature

# Remover última migration
dotnet ef migrations remove

# Listar migrations
dotnet ef migrations list

# Aplicar migration específica
dotnet ef database update 20240101000000_InitialCreate
```

## 🔧 Variáveis de Ambiente

Crie um arquivo `appsettings.Development.json` para configurações de desenvolvimento:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 📁 Estrutura do Projeto

```
MinimalApi/
├── Controllers/          # Endpoints da API
├── Models/              # Entidades e DTOs
├── Data/                # Contexto do Entity Framework
├── Services/            # Lógica de negócio
├── Migrations/          # Migrações do banco de dados
├── Tests/               # Projeto de testes
├── appsettings.json     # Configurações da aplicação
└── Program.cs           # Configuração principal da API
```

## 🤝 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para:

1. Fazer um fork do projeto
2. Criar uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abrir um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👥 Autor

Desenvolvido como parte do Bootcamp Avanade - Back-end com .NET e IA da [Digital Innovation One](https://www.dio.me/).

---

**📞 Suporte**: Em caso de dúvidas, abra uma issue no repositório ou entre em contato através dos canais da DIO.

**🔄 Atualizações**: Este projeto é mantido regularmente com atualizações de segurança e novas funcionalidades.

**⭐ Se este projeto foi útil, deixe uma estrela no repositório!**
```
