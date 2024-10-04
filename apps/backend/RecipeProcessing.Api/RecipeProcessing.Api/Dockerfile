FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5047

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RecipeProcessing.Api/RecipeProcessing.Api.csproj", "RecipeProcessing.Api/"]
COPY ["RecipeProcessing.Infrastructure/RecipeProcessing.Infrastructure.csproj", "RecipeProcessing.Infrastructure/"]
COPY ["RecipeProcessing.Core/RecipeProcessing.Core.csproj", "RecipeProcessing.Core/"] 
RUN dotnet restore "RecipeProcessing.Api/RecipeProcessing.Api.csproj"

COPY RecipeProcessing.Api/. RecipeProcessing.Api/
COPY RecipeProcessing.Infrastructure/. RecipeProcessing.Infrastructure/
COPY RecipeProcessing.Core/. RecipeProcessing.Core/ 

RUN dotnet build "RecipeProcessing.Api/RecipeProcessing.Api.csproj" -c Release -o /app/build
RUN dotnet publish "RecipeProcessing.Api/RecipeProcessing.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RecipeProcessing.Api.dll"]