# 1. Используем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj всех проектов и восстанавливаем зависимости
COPY ["Booking.Api/Booking.Api.csproj", "Booking.Api/"]
COPY ["Booking.Application/Booking.Application.csproj", "Booking.Application/"]
COPY ["Booking.Domain/Booking.Domain.csproj", "Booking.Domain/"]
COPY ["Booking.Infrastructure/Booking.Infrastructure.csproj", "Booking.Infrastructure/"]
RUN dotnet restore "Booking.Api/Booking.Api.csproj"

# Копируем весь код
COPY . .

# Сборка и публикация проекта
WORKDIR "/src/Booking.Api"
RUN dotnet publish "Booking.Api.csproj" -c Release -o /app/publish

# 2. Используем runtime-образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Запуск приложения
ENTRYPOINT ["dotnet", "Booking.Api.dll"]