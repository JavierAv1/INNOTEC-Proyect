# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 As build
WORKDIR /webapp

EXPOSE 80
EXPOSE 8080

# Copiar todos los archivos del proyecto
COPY . .

# Restaurar las dependencias y construir
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /webapp
VOLUME /root/.aspnet/DataProtection-Keys
COPY --from=build /out .
ENTRYPOINT ["dotnet", "PL-INNOTEC-Proyect.dll"]

ENV ASPNETCORE_ENVIRONMENT Development
