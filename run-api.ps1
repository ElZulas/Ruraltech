# Script para ejecutar el API de RuralTech
$ErrorActionPreference = "Stop"

Write-Host "Iniciando RuralTech API..." -ForegroundColor Green
Write-Host ""

cd "src\RuralTech.API"

# Configurar entorno
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Ejecutar API
Write-Host "Ejecutando dotnet run..." -ForegroundColor Yellow
dotnet run --urls "http://localhost:5002"
