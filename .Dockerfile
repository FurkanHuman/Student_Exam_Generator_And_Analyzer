# 🔹 Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# 🔹 Open HTTP ve HTTPS ports
EXPOSE 8080
EXPOSE 8085

ENV LANG=tr_TR.UTF-8 \
    LANGUAGE=tr_TR:tr \
    LC_ALL=tr_TR.UTF-8

# 🔹 Certificate directory
ENV ASPNETCORE_URLS="http://+:8080;https://+:8085"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=supersecret

RUN mkdir /https

WORKDIR /app

# 🔹 Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Src/SES/BlazorWebUI/BlazorWebUI/BlazorWebUI.csproj", "Src/SES/BlazorWebUI/BlazorWebUI/"]
COPY ["Src/SES/Application/Application.csproj", "Src/SES/Application/"]
COPY ["Src/SES/Domain/Domain.csproj", "Src/SES/Domain/"]
COPY ["Src/SES/Infrastructure/Infrastructure.csproj", "Src/SES/Infrastructure/"]
COPY ["Src/SES/Persistence/Persistence.csproj", "Src/SES/Persistence/"]
RUN dotnet restore "./Src/SES/BlazorWebUI/BlazorWebUI/BlazorWebUI.csproj"

COPY . .
WORKDIR "/src/Src/SES/BlazorWebUI/BlazorWebUI"
RUN dotnet build "./BlazorWebUI.csproj" -c Release -o /app/build

# 🔹 Publish stage
FROM build AS publish
RUN dotnet publish "./BlazorWebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 🔹 Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 🔹 The certificate will be mounted here, it should be connected from the host instead of COPY To mount the certificate file externally: -v $(pwd)/cert.pfx:/https/cert.pfx:ro
ENTRYPOINT ["dotnet", "BlazorWebUI.dll"]