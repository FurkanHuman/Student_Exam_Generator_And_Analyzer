# syntax=docker/dockerfile:1

# Base image
FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

EXPOSE 8080
EXPOSE 8085

ENV LANG=tr_TR.UTF-8 \
    LANGUAGE=tr_TR:tr \
    LC_ALL=tr_TR.UTF-8 \
    ASPNETCORE_URLS="http://+:8080;https://+:8085" \
    ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx

RUN mkdir /https
WORKDIR /app

# Build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY ["Src/SES/BlazorWebUI/BlazorWebUI/BlazorWebUI.csproj", "Src/SES/BlazorWebUI/BlazorWebUI/"]
COPY ["Src/SES/Application/Application.csproj", "Src/SES/Application/"]
COPY ["Src/SES/Domain/Domain.csproj", "Src/SES/Domain/"]
COPY ["Src/SES/Infrastructure/Infrastructure.csproj", "Src/SES/Infrastructure/"]
COPY ["Src/SES/Persistence/Persistence.csproj", "Src/SES/Persistence/"]

RUN dotnet restore "Src/SES/BlazorWebUI/BlazorWebUI/BlazorWebUI.csproj" --arch $TARGETARCH

COPY . .
WORKDIR "/src/Src/SES/BlazorWebUI/BlazorWebUI"
RUN dotnet build "./BlazorWebUI.csproj" -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish "./BlazorWebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false --arch $TARGETARCH

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BlazorWebUI.dll"]
