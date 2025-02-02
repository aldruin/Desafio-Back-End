# BancoAPI

A **BancoAPI** é uma API REST desenvolvida em ASP.NET Core para facilitar a gestão de usuários, carteiras e transações financeiras. Ela foi projetada seguindo boas práticas de desenvolvimento e arquitetura, utilizando Entity Framework Core para a persistência de dados e PostgreSQL como banco de dados.

**Padrões:**

    Repository Pattern
    Notification Pattern
    Arquitetura em Camadas
    EF Core Code First
    DTOs
    Validators com Fluent Validation
    Injeção de Dependência
    Princípios S.O.L.I.D

------------------------------------------------------------------------------------------------


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

------------------------------------------------------------------------------------------------


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

### 2. **Carteiras**

- ** GET api/wallet/id
- ** GET api/wallet/cpf
- ** GET api/wallet/email

  ***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)
  Este endpoint atualiza dados do usuário logado.

  **Resposta:**

      ```json
          [
              {
                  "success": true,
                  "data": {
                    "id": "17001c54-a50a-4284-ace8-ab196232501f",
                    "userId": "a290badc-e1ce-44b0-8059-dc29c17e27ee",
                    "balance": 100,
                    "transactionsId": [
                      "0a502f3e-e6f3-4e2f-acd0-52ca80d8687e",
                      "c528a0f9-b3ef-4acc-afee-4faec7cce1af",
                      "5375693f-fe68-4dc7-b675-41e8dceedf1e"
                    ]
                  },
                  "notifications": [
                    {
                      "action": "WalletFound",
                      "message": "Carteira de usuário obtida com sucesso!"
                    }
                  ]
                }    
            ]                
  **Se não encontrado ou erro:**

      "FailToCreateWallet", "Erro ao criar Carteira de Usuario, userId invalido, erro interno no servidor."
      "InvalidUserId", "ID de Usuário não encontrado ou inválido."
      "WalletNotFound", "Nenhuma carteira encontrada com ID de usuário fornecido."
      "FailToGetWallet", "Erro ao obter carteira de usuário, informe um CPF válido."
      "WalletNotFound", "Nenhuma carteira encontrada com CPF de usuário fornecido."
      "FailToGetWallet", "Erro ao obter carteira de usuário, informe um Email válido."
      "WalletNotFound", "Nenhuma carteira encontrada com Email de usuário fornecido."

  - **Métodos de WalletService:
 
    **UpdateBalanceAsync:**
    Esse método por padrão pertence ao WalletService e é chamado quando ocorre uma Transaction.
    A transaction passa o tipo de operação e carteira para que seja atualizado o saldo e persistido no banco de dados. 

  ------------------------------------------------------------------------------------------------

### 3. **Transações**

-** POST api/transaction/deposit

Este endpoint atualiza o saldo da carteira do usuário, realizando um depósito.

***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)

**Corpo da Requisição:**

    ```json
        [
            {
              "value": 100
            }
        ]

Tanto originWalletId como destinationWalletId são obtidos pelo Claim do usuário logado, garantindo que o deposito irá para a conta do usuário.

**Resposta:**

    ```json
        [
            {
              "success": true,
              "data": {
                "id": "3d0ef122-e44b-49cd-a156-082d5ef07f77",
                "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                "destinationWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                "value": 100,
                "transactionDate": "2025-02-02T19:03:22.9582234Z"
              },
              "notifications": [
                {
                  "action": "TransactionOK",
                  "message": "Transação realizada com sucesso!"
                },
                {
                  "action": "WalletBalanceUpdated",
                  "message": "Deposito realizado com sucesso!"
                }
              ]
            }
        ]

**Se não encontrado ou erro:**

    "InvalidTransaction", "Erro ao criar transação, Valor deve ser maior do que zero."
    "InvalidWallet", "Wallet ID do usuário não encontrado ou inválido."
    "DepositFailed", "Falha ao atualizar o saldo da carteira de destino."
    "DepositTransactionFailed", $"Erro ao realizar transação de deposito: {ErrorMessage}"

------------------------------------------------------------------------------------------------

- POST api/transaction/transfer

Este endpoint transfere o saldo da carteira do usuário para outro usuário, realizando uma transferência.

***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)

Este endpoint utiliza o E-mail como 'chave' para obter o destinatário da transação.

**Corpo da Requisição:**

    ```json
        [
            {
              "value": 100,
              "cpf": "02112345658"
            }
        ]

**Resposta:**

    ```json
        [
            {
              "success": true,
              "data": {
                "id": "a7483d4b-584a-42b5-a2a0-7650542fb0a1",
                "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                "destinationWalletId": "7e181ee0-1045-47aa-bdb5-bb2d61c80753",
                "value": 100,
                "transactionDate": "2025-02-02T19:35:31.3978212Z",
                "cpf": null
              },
              "notifications": [
                {
                  "action": "TransactionOK",
                  "message": "Transação realizada com sucesso!"
                },
                {
                  "action": "WalletBalanceUpdated",
                  "message": "Transferencia realizada com sucesso!"
                }
              ]
            }
        ]   

**Se erro ou não encontrado:**

    "CpfInvalido", "O CPF é obrigatório e deve estar no formato 000.000.000-00 ou 00000000000."
    "ValueMustBeValuable", "O valor não pode ser nulo. Valor precisa ser maior do que zero."
    "TransferenceFailed", "Falha ao atualizar o saldo da carteira de destino."
    "FailToGetWallet", "Falha ao obter carteira com CPF fornecido. Carteira de usuário não encontrada."

------------------------------------------------------------------------------------------------

- GET api/transaction/transactions
    
Este endpoint possui um filtro opcional de Data, para obter as transações da data escolhida.
Caso não insira nenhuma data, irá retornar todas as transações do usuário logado no sistema.

***Autenticação:*** Bearer token no cabeçalho da requisição (token JWT do usuário logado)

**Requisição com data deverá ser feita via Query ou Swagger**

**Exemplo de requisição:**

    https://localhost:7000/api/transaction/transactions?date=2024-08-06

**Resposta:**

    ```json
        [
            {
              "success": true,
              "data": [
                {
                  "id": "0a502f3e-e6f3-4e2f-acd0-52ca80d8687e",
                  "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "destinationWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "value": 100,
                  "transactionDate": "2025-01-30T00:52:22.468343Z",
                  "cpf": null
                },
                {
                  "id": "c528a0f9-b3ef-4acc-afee-4faec7cce1af",
                  "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "destinationWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "value": 100,
                  "transactionDate": "2025-01-31T00:29:43.08232Z",
                  "cpf": null
                },
                {
                  "id": "5375693f-fe68-4dc7-b675-41e8dceedf1e",
                  "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "destinationWalletId": "b8e58193-61b1-40e6-8611-6f95130c5d97",
                  "value": 100,
                  "transactionDate": "2025-02-01T01:36:54.723034Z",
                  "cpf": null
                },
                {
                  "id": "4a69cf84-4cd2-46d2-b01c-b43f0fc2bec4",
                  "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "destinationWalletId": "7e181ee0-1045-47aa-bdb5-bb2d61c80753",
                  "value": 69.2,
                  "transactionDate": "2025-01-20T00:54:08.260613Z",
                  "cpf": null
                },
                {
                  "id": "3d0ef122-e44b-49cd-a156-082d5ef07f77",
                  "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "destinationWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "value": 100,
                  "transactionDate": "2025-02-02T19:03:22.958223Z",
                  "cpf": null
                },
                {
                  "id": "a7483d4b-584a-42b5-a2a0-7650542fb0a1",
                  "originWalletId": "17001c54-a50a-4284-ace8-ab196232501f",
                  "destinationWalletId": "7e181ee0-1045-47aa-bdb5-bb2d61c80753",
                  "value": 100,
                  "transactionDate": "2025-02-02T19:35:31.397821Z",
                  "cpf": null
                }
              ],
              "notifications": [
                {
                  "action": "Success",
                  "message": "Transações obtidas com sucesso."
                }
              ]
            }
        ]


**Se erro ou não encontrado:**

    "InvalidUserId", "Não foi possivel obter user id do usuário logado"
    "InvalidUserWallet", "Não foi possivel obter carteira do usuário logado"
    "NoTransactions", "Nenhuma transação encontrada para os critérios especificados."
    
------------------------------------------------------------------------------------------------

###Contribuição

Sinta-se a vontade para contribuir!
- Faça um fork do projeto.
- Crie um branch (git checkout -b feature/nova-funcionalidade).
- Faça commit das mudanças (git commit -m 'Adiciona nova funcionalidade').
- Faça o push para o branch (git push origin feature/nova-funcionalidade).
- Abra um Pull Request.

------------------------------------------------------------------------------------------------

Este projeto foi desenvolvido para o desafio back-end utilizando C#!

contato: aldruinsouza@outlook.com

grato por ler até aqui!
