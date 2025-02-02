# BancoAPI

A **BancoAPI** é uma API REST desenvolvida em ASP.NET Core para facilitar a gestão de usuários, carteiras e transações financeiras. Ela foi projetada seguindo boas práticas de desenvolvimento e arquitetura, utilizando Entity Framework Core para a persistência de dados e PostgreSQL como banco de dados.

## Como executar o projeto

### 1. Pré-requisitos
Certifique-se de ter os seguintes itens instalados:
- .NET SDK 8.0
- PostgreSQL

### 2. Clone o repositório

    git clone https://github.com/seu-usuario/BancoAPI.git  
    cd BancoAPI  

### 3. Configure a string de conexão
- No arquivo appsettings.json, configure a string de conexão com o banco de dados PostgreSQL

      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=bancoapi;Username=postgres;Password=admin"
      }
  
  ### 4. Aplique as migrações
  Antes de rodar a aplicação, aplique as migrações para criar as tabelas no banco de dados.

  **No terminal do projeto BancoApi.Api :**

      dotnet ef migrations add InitialMigration --project ../BancoApi.Infrastructure/BancoApi.Infrastructure.csproj

  ### 5. Execute o projeto

  Ao executar a primeira vez, irá popular o banco de dados através do SeedData.cs, que irá executar automaticamente pelo Program.cs

  Os usuários padrões são:

      Name = "João",
      LastName = "Silva",
      Cpf = "12345678901",
      Email = new Email { Value = "joao.silva@example.com" },
      Password = new Password { Value = "Senha123" }

      Name = "Maria",
      LastName = "Oliveira",
      Cpf = "23456789012",
      Email = new Email { Value = "maria.oliveira@example.com" },
      Password = new Password { Value = "Senha123" }

      Name = "Carlos",
      LastName = "Santos",
      Cpf = "34567890123",
      Email = new Email { Value = "carlos.santos@example.com" },
      Password = new Password { Value = "Senha123" }

  É importante utilizar um usuário criado, seja padrão ou não, para testar a aplicação, pois a maioria dos Endpoints utiliza a informação contida na Claim do Usuário logado.
  
  **Basta rodar a aplicação:**

      dotnet run

  **A API estará disponível em https://localhost:7000 ou http://localhost:5162.**


## Funcionalidades

- **Gerenciamento de Usuários**: Criação, leitura, atualização e exclusão de usuários.
- **Gerenciamento de Carteiras**: Criação, leitura e atualização de carteira de usuários.
- **Transações**: Realização de transações financeiras entre carteiras.
- **Seed Data**: Preenche automaticamente o banco de dados com dados iniciais durante o desenvolvimento, caso nao haja nada salvo no banco de dados.

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework para construção de APIs.
- **Entity Framework Core**: ORM utilizado para interagir com o banco de dados.
- **Swagger**: Para documentação interativa da API.
- **PostgreSQL**: Banco de dados relacional utilizado para armazenar os dados.

## Endpoints da API

### 1. **Usuários**

- **POST /api/user/create**
  
  Este endpoint cria um novo usuário no sistema

  **Corpo da requisição:**

  ```json
  [  
      {
          "name": "João",
          "lastName": "Silva",
          "cpf": "12345678901",
          "email": "joao.silva@example.com",
          "password": "Senha@123"
      }
  ]
.

  **Resposta:**
  Código de Status: 200 OK (sucesso)
  
    ```json
    [
        {
          "success": true,
          "data": {
            "id": "a9913126-771d-4312-bca1-4ad1b9edec82",
            "name": "Joao",
            "lastName": "Silva",
            "cpf": "12345678901",
            "email": "joao.silva@example.com",
            "password": null
          },
          "notifications": [
            {
              "action": "WalletCreated",
              "message": "Carteira de usuário criada com sucesso!"
            },
            {
              "action": "UserCreated",
              "message": "Usuário criado com sucesso!"
            }
          ]
        }
    ]

  ***Se houver erros durante a criação:***
  
    ```json
      [
          {
            "success": false,
            "errors": [
              {
                "action": "InvalidUser",
                "message": "E-mail de Usuário já cadastrado."
              }
            ]
          }
      ]
  **Outras mensagens de erro:**
  
        "CPF de Usuário já cadastrado."
        "A senha deve ter no mínimo 8 caracteres, uma letra, um caractere especial e um número"
        "Erro ao criar o usuário: {ErrorMessage}"
        "O ultimo nome é obrigatório"
        "O primeiro nome é obrigatório"
        "O CPF é obrigatório"
        "CPF inválido. O formato correto é 000.000.000-00 ou 00000000000."
        "O endereço de E-mail é obrigatório"
        "A senha é obrigatória"
------------------------------------------------------------------------------------------------

  - **GET /api/user/id**
  
    Este endpoint retorna os dados do usuário logado, identificado pelo seu token de autenticação.

  ***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)

  **Resposta:**
  Código de Status: 200 OK (sucesso)
  
    ```json
    [
      {
        "success": true,
        "data": {
          "id": "535c120c-23e2-4c74-a295-37e92b5469c2",
          "name": "Teste",
          "lastName": "User",
          "cpf": "05165052526",
          "email": "teste@teste.com",
          "password": null
        },
        "notifications": [
          {
            "action": "UserFound",
            "message": "Usuário com ID fornecido obtido com sucesso."
          }
        ]
      }
    ]

  ***Se não encontrado ou erro:***
  
    ```json
      [
          {
            "success": false,
            "errors": [
              {
                "action": "InvalidUserId",
                "message": "O ID informado é inválido."
              }
            ]
          }
      ]
  **Outras mensagens de erro:**
  
        "UserNotFound", "Não foi encontrado o usuário com ID fornecido"

  **Os metodos GET de Usuário possui O mesmo padrão para E-mail e Senha, com os endpoints:**
  - **GET /api/user/cpf**
  - **GET /api/user/email**
------------------------------------------------------------------------------------------------

  - **DELETE /api/user/id**
    Este endpoint remove o usuário logado, sua carteira e transações no banco de dados do sistema.

  ***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)
  
  **Resposta:**
  
    ```json
      [
        {
          "success": false,
          "errors": [
            {
              "action": "UserDeleted",
              "message": "Usuário deletado com sucesso."
            }
          ]
        }
      ]
  **Se não encontrado ou erro:**

    "InvalidUserId", "O ID informado é inválido."
    "UserNotFound", "Não foi possivel remover usuário com ID fornecido, usuário não encontrado."
  ------------------------------------------------------------------------------------------------

- **UPDATE /api/user/update**
    Este endpoint atualiza dados do usuário logado.

  ***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)

  **Corpo da requisição:**
  **OBS: enviar somente o ID de usuário e dados a serem atualizados.**
  
      ```json
      [
        {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string",
          "lastName": "string",
          "cpf": "string",
          "email": "string",
          "password": "string"
        }
      ]
  
  **Resposta:**
  
      ```json
        [
          {
            "success": true,
            "data": {
            "id": "a290badc-e1ce-44b0-8059-dc29c17e27ee",
            "name": "Aldruin",
            "lastName": "Souza",
            "cpf": "05198084956",
            "email": "teste@teste.com",
            "password": null
            },
            "notifications": [
              {
                "action": "UsuarioAtualizado",
                "message": "Os  dados de usuario foram atualizados com sucesso."
              }
            ]
          }
        ]
    
**Se não encontrado ou erro:**

    "Unauthorized", "O ID do Usuario logado não coincide com o ID passado."
    "NothingToUpdate", "Não foi encontrado nenhum dado para atualizar."
    "UserNotFound", "Não foi encontrado o usuário com ID fornecido"
    "InvalidUserId", "O ID informado é inválido ou nulo."
  ------------------------------------------------------------------------------------------------



  
  



