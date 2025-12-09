# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore/publish API
COPY . .
RUN dotnet restore "./AutoroversApi/AutoroversApi.csproj"
RUN dotnet publish "./AutoroversApi/AutoroversApi.csproj" -c Release -o /app/publish

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# ASP.NET official images listen on 8080 by default
EXPOSE 8080

ENTRYPOINT ["dotnet", "AutoroversApi.dll"]
