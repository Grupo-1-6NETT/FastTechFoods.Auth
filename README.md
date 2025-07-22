# Fast Tech Foods - Auth

Este é um microsserviço desenvolvido em .NET Core 8 para gerenciar o cadastro de Usuários. 
Este projeto é parte da solução para o Hackathon da Fase 5 do curso de pós graduação 6NETT na FIAP.

## Índice
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Projeto](#configuração-do-projeto)
- [Eventos](#eventos)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

## Pré-requisitos

- [Docker](https://www.docker.com/get-started/) e [Docker Compose](https://docs.docker.com/compose/install/) (necessário para executar o projeto)
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (somente para executar local)

## Configuração do Projeto

**1. Clone o repositório:**

   ```bash
   git https://github.com/Grupo-1-6NETT/FastTechFoods.Auth.git
   cd FastTechFoods.Auth
   ```

**2. Adicione configurações necessárias**

Adicione as credenciais do Postgres no appsettings do ambiente que estiver rodando.  
Por exempo, em ambiente de desenvolvimento, adicione a propriedade em `appsettings.Development.json`.  

```json
 "ConnectionStrings": {
    "DefaultConnection": "Host=batcave-server;Database=batbase;Username=alfred;Password=******;SSL Mode=Require;Trust Server Certificate=true"
  }
``` 

Adicione as credenciais do RabbitMQ

``` json
  "RabbitMQ": {
    "Hostname": "rabbitmq",
    "Username": "guest",
    "Password": "guest"    
  },
```


**3. Crie a imagem Docker:**

```bash
docker-compose up --build
```

---
## Eventos

### Cliente

|Evento|Descrição|
|---|---|
|CadastrarClienteAsync|Adiciona um Cliente na base de dados|
|AtualizarClienteAsync|Atualiza dados um Cliente na base de dados|
|RemoverClienteAsync|Remove o Cliente na base de dados com o ID informado|

### Funcionário

|Evento|Descrição|
|---|---|
|CadastrarFuncionarioAsync|Adiciona um Funcionário na base de dados|
|RemoverFuncionarioAsync|Remove o Funcionário na base de dados com o ID informado|

### Token

|Evento|Descrição|
|---|---|
|GetFuncionarioToken|Gera um token de autenticação para o e-mail e senha informados|
|GetClienteCpfToken|Gera um token de autenticação para o CPF e senha informados|
|GetClienteEmailToken|Gera um token de autenticação para o email e senha informados|

---
## Tecnologias Utilizadas
- **ASP.NET Core 8** - Framework principal para desenvolvimento do serviço
- **Entity Framework Core** - ORM para manipulação do banco de dados
- **Postgres** - Banco de dados
- **RabbitMQ** - Message Broker
- **MassTransit** - Transporte de mensagens
- **Docker** - Criação de conteiners
