# Backend.Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY src/backend/Oxigin.Attendance.API.sln ./
COPY src/backend/Oxigin.Attendance.API/ ./Oxigin.Attendance.API/
COPY src/backend/Oxigin.Attendance.Core/ ./Oxigin.Attendance.Core/
COPY src/backend/Oxigin.Attendance.Datastore/ ./Oxigin.Attendance.Datastore/
COPY src/backend/Oxigin.Attendance.Shared/ ./Oxigin.Attendance.Shared/
RUN dotnet restore ./Oxigin.Attendance.API.sln
RUN dotnet publish ./Oxigin.Attendance.API/Oxigin.Attendance.API.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
CMD ["dotnet", "Oxigin.Attendance.API.dll"]
