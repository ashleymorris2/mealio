# Step 1: Base runtime image for ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5047

# Step 2: Use the .NET SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Define build arguments for configuration (Release/Debug) and environment
ARG BUILD_CONFIGURATION=Release

# Copy the project files and restore dependencies
COPY ["RecipeProcessing.Api/RecipeProcessing.Api.csproj", "RecipeProcessing.Api/"]
COPY ["RecipeProcessing.Infrastructure/RecipeProcessing.Infrastructure.csproj", "RecipeProcessing.Infrastructure/"]
COPY ["RecipeProcessing.Core/RecipeProcessing.Core.csproj", "RecipeProcessing.Core/"]  
RUN dotnet restore "RecipeProcessing.Api/RecipeProcessing.Api.csproj"

# Copy all the source code
COPY RecipeProcessing.Api/. RecipeProcessing.Api/
COPY RecipeProcessing.Infrastructure/. RecipeProcessing.Infrastructure/
COPY RecipeProcessing.Core/. RecipeProcessing.Core/  

# Build the project using the specified configuration
RUN dotnet build "RecipeProcessing.Api/RecipeProcessing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Step 3: Publish the application
RUN dotnet publish "RecipeProcessing.Api/RecipeProcessing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Step 3a: Development mode (includes SDK and hot-reloading)
FROM build AS dev
WORKDIR /src
CMD ["dotnet", "watch", "run", "--project", "/src/RecipeProcessing.Api/RecipeProcessing.Api.csproj"]

# Step 3b: Final runtime image for ASP.NET Core
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Set the entry point to run the app
ENTRYPOINT ["dotnet", "RecipeProcessing.Api.dll"]