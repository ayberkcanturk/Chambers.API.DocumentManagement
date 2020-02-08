#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["src/Chambers.API.DocumentManagement/Chambers.API.DocumentManagement.csproj", "src/Chambers.API.DocumentManagement/"]
RUN dotnet restore "src/Chambers.API.DocumentManagement/Chambers.API.DocumentManagement.csproj"
COPY . .
WORKDIR "/src/src/Chambers.API.DocumentManagement"
RUN dotnet build "Chambers.API.DocumentManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chambers.API.DocumentManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chambers.API.DocumentManagement.dll"]