#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR /src
#COPY ["WebLn.csproj", ""]
#RUN dotnet restore "./WebLn.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "WebLn.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "WebLn.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WebLn.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY /app/publish .
ENTRYPOINT ["dotnet", "WebLn.dll"]