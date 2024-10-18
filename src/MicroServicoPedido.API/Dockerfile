# Etapa de construção
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copia todos os arquivos do diretório src (sem o src na frente)
COPY . .

# Configura o diretório de trabalho para o projeto da API
WORKDIR /app/MicroServicoPedido.API

# Restaura as dependências
RUN dotnet restore

# Publica o projeto
RUN dotnet publish -c Release -o /app/out

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia os arquivos publicados da etapa anterior
COPY --from=build-env /app/out .

# Define o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "MicroServicoPedido.API.dll"]
