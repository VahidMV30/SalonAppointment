FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Bootstrapper/SalonAppointment.Api/SalonAppointment.Api.csproj", "src/Bootstrapper/SalonAppointment.Api/"]
COPY ["src/Modules/Identity/SalonAppointment.Identity.Api/SalonAppointment.Identity.Api.csproj", "src/Modules/Identity/SalonAppointment.Identity.Api/"]
COPY ["src/Modules/Identity/SalonAppointment.Identity.Infrastructure/SalonAppointment.Identity.Infrastructure.csproj", "src/Modules/Identity/SalonAppointment.Identity.Infrastructure/"]
COPY ["src/Shared/SalonAppointment.Shared.Infrastructure/SalonAppointment.Shared.Infrastructure.csproj", "src/Shared/SalonAppointment.Shared.Infrastructure/"]
COPY ["src/Modules/Identity/SalonAppointment.Identity.Domain/SalonAppointment.Identity.Domain.csproj", "src/Modules/Identity/SalonAppointment.Identity.Domain/"]
COPY ["src/Shared/SalonAppointment.Shared.Domain/SalonAppointment.Shared.Domain.csproj", "src/Shared/SalonAppointment.Shared.Domain/"]
COPY ["src/Modules/Identity/SalonAppointment.Identity.Application/SalonAppointment.Identity.Application.csproj", "src/Modules/Identity/SalonAppointment.Identity.Application/"]
COPY ["src/Shared/SalonAppointment.Shared.Api/SalonAppointment.Shared.Api.csproj", "src/Shared/SalonAppointment.Shared.Api/"]
RUN dotnet restore "src/Bootstrapper/SalonAppointment.Api/SalonAppointment.Api.csproj"
COPY . .
WORKDIR "/src/src/Bootstrapper/SalonAppointment.Api"
RUN dotnet build "./SalonAppointment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SalonAppointment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalonAppointment.Api.dll"]
