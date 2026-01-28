# Script para iniciar el frontend React
Write-Host "Iniciando Frontend React..." -ForegroundColor Green
Write-Host ""

cd "client"

# Verificar que node_modules existe
if (-not (Test-Path "node_modules")) {
    Write-Host "Instalando dependencias..." -ForegroundColor Yellow
    npm install
    Write-Host ""
}

Write-Host "El frontend estara disponible en:" -ForegroundColor Cyan
Write-Host "  - http://localhost:3000 (desde esta PC)" -ForegroundColor Cyan
Write-Host "  - http://10.234.89.228:3000 (desde dispositivos en la misma red)" -ForegroundColor Cyan
Write-Host ""
Write-Host "Presiona Ctrl+C para detener el servidor" -ForegroundColor Yellow
Write-Host ""

npm run dev
