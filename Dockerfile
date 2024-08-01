# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["EventMatcha.BackgroundService/EventMatcha.BackgroundService.csproj", "EventMatcha.BackgroundService/"]
COPY ["EventMatcha.BackgroundServiceCore/EventMatcha.BackgroundServiceCore.csproj", "EventMatcha.BackgroundServiceCore/"]

# If you have other project dependencies, copy them similarly
# COPY ["EventMatcha.Shared/EventMatcha.Shared.csproj", "EventMatcha.Shared/"] 

# Restore dependencies for the main project
RUN dotnet restore "EventMatcha.BackgroundService/EventMatcha.BackgroundService.csproj"

# Copy the remaining files and build the app
COPY . .
WORKDIR "/src/EventMatcha.BackgroundService"
RUN dotnet build "EventMatcha.BackgroundService.csproj" -c Release -o /app/build
RUN dotnet publish "EventMatcha.BackgroundService.csproj" -c Release -o /app/publish

# Use the official .NET runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EventMatcha.BackgroundService.dll"]
