# Script para iniciar ambos servidores correctamente
Write-Host "=== INICIANDO SERVIDORES ===" -ForegroundColor Cyan
Write-Host ""

# Detener procesos antiguos
Write-Host "Deteniendo procesos antiguos..." -ForegroundColor Yellow
Get-Process | Where-Object {$_.ProcessName -eq "node"} | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process | Where-Object {$_.ProcessName -like "*dotnet*"} | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 3

# Obtener IP actual
$ip = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object {$_.IPAddress -like "10.*"} | Select-Object -First 1).IPAddress
if (-not $ip) {
    $ip = (ipconfig | findstr IPv4 | Select-Object -First 1) -replace '.*: ', ''
    $ip = $ip.Trim()
}

Write-Host "IP detectada: $ip" -ForegroundColor Green
Write-Host ""

# Iniciar Backend
Write-Host "Iniciando Backend..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-ExecutionPolicy", "Bypass", "-Command", "cd '$PWD\src\RuralTech.API'; `$env:ASPNETCORE_ENVIRONMENT='Development'; Write-Host 'Backend iniciando...' -ForegroundColor Green; dotnet run --urls 'http://0.0.0.0:5002'"

Start-Sleep -Seconds 5

# Iniciar Frontend
Write-Host "Iniciando Frontend..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-ExecutionPolicy", "Bypass", "-Command", "cd '$PWD\client'; Write-Host 'Frontend iniciando...' -ForegroundColor Green; npm run dev"

Start-Sleep -Seconds 10

# Verificar
Write-Host ""
Write-Host "=== VERIFICANDO ===" -ForegroundColor Cyan
$puertos = netstat -ano | findstr "5002\|3000" | findstr "LISTENING"
if ($puertos) {
    Write-Host "OK Servidores corriendo:" -ForegroundColor Green
    $puertos | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
} else {
    Write-Host "ADVERTENCIA: Los puertos no están escuchando todavía" -ForegroundColor Yellow
    Write-Host "Espera unos segundos más..." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== URLs ===" -ForegroundColor Cyan
Write-Host "Landing Page: http://$ip:3000" -ForegroundColor Green
Write-Host "Swagger: http://$ip:5002/swagger" -ForegroundColor Green
Write-Host ""
Write-Host "Abre estas URLs desde tu celular (misma red WiFi)" -ForegroundColor Yellow
Write-Host ""
