#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Construction_Contractor/Construction_Contractor.csproj", "Construction_Contractor/"]
RUN dotnet restore "Construction_Contractor/Construction_Contractor.csproj"
COPY . .
WORKDIR "/src/Construction_Contractor"
RUN dotnet build "Construction_Contractor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Construction_Contractor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Construction_Contractor.dll"]