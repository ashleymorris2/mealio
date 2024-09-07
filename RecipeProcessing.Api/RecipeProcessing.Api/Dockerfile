# Step 1: Base runtime image for ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

EXPOSE 5047

# Step 2: Use the .NET SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Define build arguments for configuration (Release/Debug) and environment
ARG BUILD_CONFIGURATION=Release

# Ensure the app listens on all interfaces
ENV ASPNETCORE_URLS=http://0.0.0.0:80

# Copy the project files and restore dependencies
COPY ["RecipeProcessing.Api/RecipeProcessing.Api.csproj", "./"]
COPY ["RecipeProcessing.Infrastructure/RecipeProcessing.Infrastructure.csproj", "RecipeProcessing.Infrastructure/"]
RUN dotnet restore "RecipeProcessing.Api.csproj"

# Copy all the source code
COPY . .

# Build the project using the specified configuration
RUN dotnet build "RecipeProcessing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Step 3: Publish the application
RUN dotnet publish "RecipeProcessing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Step 4: Final runtime image for ASP.NET Core
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "RecipeProcessing.Api.dll"]