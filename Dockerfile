FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

EXPOSE 80/tcp 

WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish ./EmployeeAPI.Business/EmployeeAPI.Business.csproj -c Release -o out

#RUN dotnet tool install --version 8.0.14 --global dotnet-ef
#ENV PATH="$PATH:/root/.dotnet/tools"
#
#RUN dotnet ef database update --project ./EmployeeAPI.DataLibrary/EmployeeAPI.DataLibrary.csproj --startup-project ./EmployeeAPI.Business/

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .


ENTRYPOINT ["dotnet", "EmployeeAPI.Business.dll"]