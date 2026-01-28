# Script para reiniciar todo correctamente
Write-Host "=== REINICIANDO SERVIDORES ===" -ForegroundColor Cyan
Write-Host ""

# Detener procesos existentes
Write-Host "Deteniendo procesos existentes..." -ForegroundColor Yellow
Get-Process | Where-Object {$_.ProcessName -eq "node"} | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process | Where-Object {$_.ProcessName -like "*dotnet*"} | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

Write-Host "OK Procesos detenidos" -ForegroundColor Green
Write-Host ""

# Verificar que los puertos estén libres
$puerto5002 = netstat -ano | findstr "5002" | findstr "LISTENING"
$puerto3000 = netstat -ano | findstr "3000" | findstr "LISTENING"

if ($puerto5002) {
    Write-Host "ADVERTENCIA: Puerto 5002 aún en uso" -ForegroundColor Red
    Write-Host "  $puerto5002" -ForegroundColor Gray
}

if ($puerto3000) {
    Write-Host "ADVERTENCIA: Puerto 3000 aún en uso" -ForegroundColor Red
    Write-Host "  $puerto3000" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=== INSTRUCCIONES ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Inicia el Backend en una ventana:" -ForegroundColor Yellow
Write-Host "   .\run-api.ps1" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Inicia el Frontend en otra ventana:" -ForegroundColor Yellow
Write-Host "   .\iniciar-frontend.ps1" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Verifica que ambos estén corriendo:" -ForegroundColor Yellow
Write-Host "   netstat -an | findstr '5002|3000' | findstr 'LISTENING'" -ForegroundColor Cyan
Write-Host ""
Write-Host "4. Prueba desde tu celular:" -ForegroundColor Yellow
Write-Host "   http://10.3.0.235:3000" -ForegroundColor Green
Write-Host "   http://10.3.0.235:5002/swagger" -ForegroundColor Green
Write-Host ""
