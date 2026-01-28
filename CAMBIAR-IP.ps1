# Script para cambiar la IP en todos los archivos necesarios
# Uso: .\CAMBIAR-IP.ps1 -NuevaIP "192.168.1.100"

param(
    [Parameter(Mandatory=$true)]
    [string]$NuevaIP
)

Write-Host "=== CAMBIANDO IP A: $NuevaIP ===" -ForegroundColor Cyan
Write-Host ""

$cambios = @()

# 1. Cambiar IP en api_config.dart
$apiConfigPath = "flutter-app\lib\config\api_config.dart"
if (Test-Path $apiConfigPath) {
    $contenido = Get-Content $apiConfigPath -Raw
    if ($contenido -match "static const String devServerIp = '[^']*'") {
        $contenido = $contenido -replace "static const String devServerIp = '[^']*'", "static const String devServerIp = '$NuevaIP'"
        Set-Content $apiConfigPath -Value $contenido -NoNewline
        $cambios += "OK flutter-app/lib/config/api_config.dart"
    }
}

# 2. Cambiar IP en URLS-ACCESO.md
$urlsPath = "URLS-ACCESO.md"
if (Test-Path $urlsPath) {
    $contenido = Get-Content $urlsPath -Raw
    $contenido = $contenido -replace "\d+\.\d+\.\d+\.\d+", $NuevaIP
    Set-Content $urlsPath -Value $contenido -NoNewline
    $cambios += "OK URLS-ACCESO.md"
}

# 3. Cambiar IP en SOLUCION-RAPIDA.md
$solucionPath = "SOLUCION-RAPIDA.md"
if (Test-Path $solucionPath) {
    $contenido = Get-Content $solucionPath -Raw
    $contenido = $contenido -replace "\d+\.\d+\.\d+\.\d+", $NuevaIP
    Set-Content $solucionPath -Value $contenido -NoNewline
    $cambios += "OK SOLUCION-RAPIDA.md"
}

# 4. Cambiar IP en iniciar-servidor.ps1
$servidorPath = "iniciar-servidor.ps1"
if (Test-Path $servidorPath) {
    $contenido = Get-Content $servidorPath -Raw
    $contenido = $contenido -replace "\d+\.\d+\.\d+\.\d+", $NuevaIP
    Set-Content $servidorPath -Value $contenido -NoNewline
    $cambios += "OK iniciar-servidor.ps1"
}

# 5. Cambiar IP en iniciar-frontend.ps1
$frontendPath = "iniciar-frontend.ps1"
if (Test-Path $frontendPath) {
    $contenido = Get-Content $frontendPath -Raw
    $contenido = $contenido -replace "\d+\.\d+\.\d+\.\d+", $NuevaIP
    Set-Content $frontendPath -Value $contenido -NoNewline
    $cambios += "OK iniciar-frontend.ps1"
}

Write-Host "Archivos actualizados:" -ForegroundColor Green
foreach ($cambio in $cambios) {
    Write-Host "  $cambio" -ForegroundColor Gray
}
Write-Host ""

Write-Host "=== IMPORTANTE ===" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Si cambiaste la IP, RECOMPILA la app Flutter:" -ForegroundColor Cyan
Write-Host "   cd flutter-app" -ForegroundColor Gray
Write-Host "   flutter build apk --release" -ForegroundColor Gray
Write-Host "   Copy-Item build\app\outputs\flutter-apk\app-release.apk releases\prealpha\Cownect-PreAlpha.apk -Force" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Nueva URL de la Landing Page:" -ForegroundColor Cyan
Write-Host "   http://$NuevaIP:3000" -ForegroundColor Green
Write-Host ""
Write-Host "3. Nueva URL del API:" -ForegroundColor Cyan
Write-Host "   http://$NuevaIP:5002" -ForegroundColor Green
Write-Host ""
Write-Host "4. Nueva URL de Swagger:" -ForegroundColor Cyan
Write-Host "   http://$NuevaIP:5002/swagger" -ForegroundColor Green
Write-Host ""
