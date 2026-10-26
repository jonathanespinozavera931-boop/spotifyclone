# Etapa de compilación: Usamos la imagen de .NET 10.0
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App
COPY . ./
# Aquí quitamos "SpotifyApp/" del inicio de la ruta
RUN dotnet restore "SpotifyApi/SpotifyApi.csproj"
RUN dotnet publish "SpotifyApi/SpotifyApi.csproj" -c Release -o out

# Etapa de ejecución: Usamos la imagen de ASP.NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /App
COPY --from=build /App/out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SpotifyApi.dll"]