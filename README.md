# BancoAPI

A **BancoAPI** é uma API RESTful que permite a gestão de usuários, carteiras e transações financeiras. A API foi construída utilizando ASP.NET Core, EF Core e PostgreSQL, e segue boas práticas de design de software.

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

  - **DELETE /api/user/cpf**
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
  ***Se não encontrado ou erro:***

    "InvalidUserId", "O ID informado é inválido."
    "UserNotFound", "Não foi possivel remover usuário com ID fornecido, usuário não encontrado."


  
  



