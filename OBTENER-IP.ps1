# Script para obtener tu IP local automáticamente
Write-Host "=== OBTENIENDO TU IP LOCAL ===" -ForegroundColor Cyan
Write-Host ""

$ipv4 = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object {
    $_.IPAddress -like "192.168.*" -or 
    $_.IPAddress -like "10.*" -or 
    $_.IPAddress -like "172.16.*" -or
    $_.IPAddress -like "172.17.*" -or
    $_.IPAddress -like "172.18.*" -or
    $_.IPAddress -like "172.19.*" -or
    $_.IPAddress -like "172.20.*" -or
    $_.IPAddress -like "172.21.*" -or
    $_.IPAddress -like "172.22.*" -or
    $_.IPAddress -like "172.23.*" -or
    $_.IPAddress -like "172.24.*" -or
    $_.IPAddress -like "172.25.*" -or
    $_.IPAddress -like "172.26.*" -or
    $_.IPAddress -like "172.27.*" -or
    $_.IPAddress -like "172.28.*" -or
    $_.IPAddress -like "172.29.*" -or
    $_.IPAddress -like "172.30.*" -or
    $_.IPAddress -like "172.31.*"
} | Select-Object -First 1).IPAddress

if ($ipv4) {
    Write-Host "Tu IP Local es: $ipv4" -ForegroundColor Green
    Write-Host ""
    Write-Host "URLs:" -ForegroundColor Yellow
    Write-Host "  Landing Page: http://$ipv4:3000" -ForegroundColor Cyan
    Write-Host "  API:          http://$ipv4:5002" -ForegroundColor Cyan
    Write-Host "  Swagger:       http://$ipv4:5002/swagger" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "¿Quieres actualizar todos los archivos con esta IP? (S/N)" -ForegroundColor Yellow
    $respuesta = Read-Host
    if ($respuesta -eq "S" -or $respuesta -eq "s" -or $respuesta -eq "Y" -or $respuesta -eq "y") {
        Write-Host ""
        Write-Host "Actualizando archivos..." -ForegroundColor Yellow
        & ".\CAMBIAR-IP.ps1" -NuevaIP $ipv4
    }
} else {
    Write-Host "No se encontró una IP local válida" -ForegroundColor Red
    Write-Host ""
    Write-Host "Ejecuta manualmente:" -ForegroundColor Yellow
    Write-Host "  ipconfig | findstr IPv4" -ForegroundColor Gray
}
