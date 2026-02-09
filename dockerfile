FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR app

COPY . .

RUN dotnet build PersonalAccount.sln
RUN dotnet publish --output ./publish PersonalAccount.sln

WORKDIR publish
ENTRYPOINT ["dotnet", "PersonalAccount.Console.dll"]