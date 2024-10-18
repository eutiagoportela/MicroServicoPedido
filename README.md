# Microserviço de Cálculo de Embalagem

Este repositório faz o seguinte processo: 

A API recebe uma lista de pedidos. Cada pedido contém uma lista de produtos, cada um com suas dimensões (altura, largura, comprimento). A API processa cada pedido e determinar a melhor forma de embalar os produtos, selecionando uma ou mais caixas para cada pedido e especificando quais produtos vão em cada caixa.

Caixas Disponíveis:

As caixas pré-fabricadas (altura, largura, comprimento):

Caixa 1: 30 x 40 x 80
Caixa 2: 80 x 50 x 40
Caixa 3: 50 x 80 x 60

Entrada e saída do endpoint:
Entrada de exemplo do endpoint: 
{
  "pedidos": [
    {
      "pedido_id": 4,
      "produtos": [
       {
          "produto_id": "Mouse Gamer",
          "dimensoes": {"altura": 5, "largura": 8, "comprimento": 12}
        },
        {
          "produto_id": "Teclado Mecânico",
          "dimensoes": {"altura": 4, "largura": 45, "comprimento": 15}
        }
      ]
    }
  ]
}

Exemplo de saida do endpoint com a entrada:
{
  "pedidos": [
    {
      "pedido_id": 4,
      "caixas": [
        {
          "caixa_id": "Caixa 3",
          "produtos": [
            "Mouse Gamer",
            "Teclado Mecânico"
          ]
        }
      ]
    }
  ]
}

###Passos para Instalação via Docker
Após clonar o projeto, navegue até a pasta src do projeto usando o prompt de comando ou o PowerShell. Por exemplo: cd H:\TESTE\FINAL\MicroServicoPedido\src<br/>
Abra o Docker Desktop e execute o seguinte comando: docker-compose -f src\docker-compose.yml up --build<br/>
Após criar o container, siga estes passos para utilização:<br/>
  http://localhost:8080/health para testar com resposta "Healthy"<br/>
  http://localhost:8080/api/usuarios/register e o JSON por exemplo: { "NomeUsuario": "usuario1","Senha": "senha123"} para registrar um usuário<br/>
  http://localhost:8080/api/auth/login e o JSON por exemplo: {"usuarioNome": "usuario1","senha": "senha123"} para obter o Token de acesso<br/>
  http://localhost:8080/api/pedidos/calcular com o uso do Token e o JSON de entrada fornecido anteriormente, para obter o resultado esperado.<br/>
