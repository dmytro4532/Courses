FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Courses/Directory.Packages.props", "Courses/"]
COPY ["Courses/Directory.Build.props", "Courses/"]
COPY ["Courses/src/Courses.API/Courses.API.csproj", "Courses/src/Courses.API/"]
COPY ["Courses/src/Courses.Application/Courses.Application.csproj", "Courses/src/Courses.Application/"]
COPY ["Courses/src/Courses.Domain/Courses.Domain.csproj", "Courses/src/Courses.Domain/"]
COPY ["Courses/src/Shared/Shared.csproj", "Courses/src/Shared/"]
COPY ["Courses/src/Courses.Infrastructure/Courses.Infrastructure.csproj", "Courses/src/Courses.Infrastructure/"]
RUN dotnet restore "./Courses/src/Courses.API/Courses.API.csproj"
COPY . .
WORKDIR "/src/Courses/src/Courses.API"
RUN dotnet build "./Courses.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Courses.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courses.API.dll"]