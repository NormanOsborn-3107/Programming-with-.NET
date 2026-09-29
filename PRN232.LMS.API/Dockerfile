# ── Stage 1: Build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution & project files first (layer caching for NuGet restore)
COPY PRN232.LMS.sln ./
COPY PRN232.LMS.API/PRN232.LMS.API.csproj                   PRN232.LMS.API/
COPY PRN232.LMS.Services/PRN232.LMS.Services.csproj         PRN232.LMS.Services/
COPY PRN232.LMS.Repositories/PRN232.LMS.Repositories.csproj PRN232.LMS.Repositories/

RUN dotnet restore PRN232.LMS.sln

# Copy the rest of the source code
COPY . .

# Publish the API project
RUN dotnet publish PRN232.LMS.API/PRN232.LMS.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Stage 2: Runtime ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PRN232.LMS.API.dll"]
