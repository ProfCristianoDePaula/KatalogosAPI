# ESTÁGIO 1: A BASE (Robô de Produção - Levinho)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# ESTÁGIO 2: O BUILDER (Robô de Compilação - Pesado, com o SDK completo)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos de configuração de cada projeto (A nossa Arquitetura Limpa)
COPY ["Katalogos.Api/Katalogos.Api.csproj", "Katalogos.Api/"]
COPY ["Katalogos.Application/Katalogos.Application.csproj", "Katalogos.Application/"]
COPY ["Katalogos.Domain/Katalogos.Domain.csproj", "Katalogos.Domain/"]
COPY ["Katalogos.Infrastructure/Katalogos.Infrastructure.csproj", "Katalogos.Infrastructure/"]

# Restaura as dependências (Baixa os pacotes)
RUN dotnet restore "Katalogos.Api/Katalogos.Api.csproj"

# Copia todo o resto do código fonte para dentro do container
COPY . .

# Compila o projeto
WORKDIR "/src/Katalogos.Api"
RUN dotnet build "Katalogos.Api.csproj" -c Release -o /app/build

# ESTÁGIO 3: A PUBLICAÇÃO (Prepara os arquivos finais)
FROM build AS publish
RUN dotnet publish "Katalogos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ESTÁGIO 4: O PRODUTO FINAL (Junta a base leve com os arquivos publicados)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# O Comando que liga a nossa API quando o container iniciar
ENTRYPOINT ["dotnet", "Katalogos.Api.dll"]