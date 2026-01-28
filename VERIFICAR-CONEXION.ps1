# Script para verificar que todo este configurado correctamente
Write-Host "=== VERIFICACION DE CONFIGURACION ===" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar IP local
Write-Host "1. Tu IP Local:" -ForegroundColor Yellow
$ipv4 = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object {$_.IPAddress -like "192.168.*" -or $_.IPAddress -like "10.*" -or $_.IPAddress -like "172.*"} | Select-Object -First 1).IPAddress
if ($ipv4) {
    Write-Host "   OK IP encontrada: $ipv4" -ForegroundColor Green
    Write-Host "   URL para acceder desde tu celular: http://$ipv4:5002/swagger" -ForegroundColor Cyan
} else {
    Write-Host "   ERROR No se encontro IP local" -ForegroundColor Red
}
Write-Host ""

# 2. Verificar si el puerto 5002 esta en uso
Write-Host "2. Estado del Puerto 5002:" -ForegroundColor Yellow
$port = netstat -an | findstr "5002"
if ($port) {
    Write-Host "   OK Puerto 5002 esta en uso:" -ForegroundColor Green
    Write-Host "   $port" -ForegroundColor Gray
    if ($port -like "*0.0.0.0:5002*" -or $port -like "*LISTENING*") {
        Write-Host "   OK El servidor esta escuchando" -ForegroundColor Green
    }
    if ($port -like "*127.0.0.1:5002*") {
        Write-Host "   ADVERTENCIA: El servidor solo escucha en localhost!" -ForegroundColor Red
        Write-Host "   Debes reiniciarlo con: .\run-api.ps1" -ForegroundColor Yellow
    }
} else {
    Write-Host "   ERROR Puerto 5002 NO esta en uso" -ForegroundColor Red
    Write-Host "   Ejecuta .\run-api.ps1 para iniciar el servidor" -ForegroundColor Yellow
}
Write-Host ""

# 3. Verificar reglas de firewall
Write-Host "3. Reglas de Firewall para Puerto 5002:" -ForegroundColor Yellow
try {
    $firewallRule = Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*5002*" -or $_.DisplayName -like "*RuralTech*"}
    if ($firewallRule) {
        Write-Host "   OK Regla de firewall encontrada:" -ForegroundColor Green
        $firewallRule | ForEach-Object {
            Write-Host "     - $($_.DisplayName) - Estado: $($_.Enabled)" -ForegroundColor Gray
        }
    } else {
        Write-Host "   ERROR No se encontro regla de firewall" -ForegroundColor Red
        Write-Host "   Ejecuta este comando como Administrador:" -ForegroundColor Yellow
        Write-Host "   New-NetFirewallRule -DisplayName 'RuralTech API - Puerto 5002' -Direction Inbound -LocalPort 5002 -Protocol TCP -Action Allow" -ForegroundColor Cyan
    }
} catch {
    Write-Host "   No se pudo verificar firewall (necesitas permisos de administrador)" -ForegroundColor Yellow
}
Write-Host ""

# 4. Instrucciones finales
Write-Host "=== INSTRUCCIONES ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Asegurate de que el servidor este corriendo:" -ForegroundColor Yellow
Write-Host "   .\run-api.ps1" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Desde tu celular (misma WiFi):" -ForegroundColor Yellow
if ($ipv4) {
    Write-Host "   Abre: http://$ipv4:5002/swagger" -ForegroundColor Cyan
} else {
    Write-Host "   Abre: http://TU_IP:5002/swagger" -ForegroundColor Cyan
}
Write-Host ""
Write-Host "3. Si no funciona, verifica:" -ForegroundColor Yellow
Write-Host "   - El servidor esta corriendo" -ForegroundColor Gray
Write-Host "   - El firewall permite el puerto 5002" -ForegroundColor Gray
Write-Host "   - Tu celular esta en la misma red WiFi" -ForegroundColor Gray
Write-Host "   - La IP es correcta" -ForegroundColor Gray
Write-Host ""
