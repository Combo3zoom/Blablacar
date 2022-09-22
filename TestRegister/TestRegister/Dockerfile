FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestRegister/TestRegister.csproj", "TestRegister/"]
RUN dotnet restore "TestRegister/TestRegister.csproj"
COPY . .
WORKDIR "/src/TestRegister"
RUN dotnet build "TestRegister.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestRegister.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestRegister.dll"]
