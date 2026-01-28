# Script para ejecutar el API de RuralTech
$ErrorActionPreference = "Stop"

Write-Host "Iniciando RuralTech API..." -ForegroundColor Green
Write-Host ""

cd "src\RuralTech.API"

# Configurar entorno
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Ejecutar API
# IMPORTANTE: Usar 0.0.0.0 para que escuche en todas las interfaces de red
# Esto permite que dispositivos móviles en la misma red WiFi se conecten
Write-Host "Ejecutando dotnet run..." -ForegroundColor Yellow
Write-Host "El servidor estará disponible en:" -ForegroundColor Cyan
Write-Host "  - http://localhost:5002 (desde esta PC)" -ForegroundColor Cyan
Write-Host "  - http://TU_IP_LOCAL:5002 (desde dispositivos en la misma red)" -ForegroundColor Cyan
Write-Host ""
dotnet run --urls "http://0.0.0.0:5002"
