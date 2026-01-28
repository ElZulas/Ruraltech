# Script para probar conexion a Supabase
Write-Host "=== PROBANDO CONEXION A SUPABASE ===" -ForegroundColor Cyan
Write-Host ""

$hostName = "db.vaeufppoexhsmrlgjnai.supabase.co"

# Intentar resolver IPv4
Write-Host "Resolviendo direccion IP..." -ForegroundColor Yellow
try {
    $dnsResult = Resolve-DnsName $hostName -Type A -ErrorAction Stop
    $ipv4 = $dnsResult[0].IPAddress
    Write-Host "OK IPv4 encontrada: $ipv4" -ForegroundColor Green
} catch {
    Write-Host "ADVERTENCIA: No se encontro IPv4, solo IPv6" -ForegroundColor Yellow
    $ipv4 = $null
}

# Probar conexion al puerto 5432
Write-Host ""
Write-Host "Probando conexion al puerto 5432..." -ForegroundColor Yellow
if ($ipv4) {
    try {
        $test = Test-NetConnection -ComputerName $ipv4 -Port 5432 -WarningAction SilentlyContinue -ErrorAction Stop
        if ($test.TcpTestSucceeded) {
            Write-Host "OK Conexion al puerto 5432 exitosa!" -ForegroundColor Green
        } else {
            Write-Host "ERROR No se puede conectar al puerto 5432" -ForegroundColor Red
            Write-Host "El firewall o la red pueden estar bloqueando" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "ERROR: $_" -ForegroundColor Red
    }
} else {
    Write-Host "No se puede probar sin IPv4" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== RECOMENDACIONES ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Si no se puede conectar:" -ForegroundColor Yellow
Write-Host "1. Verifica que el firewall permita el puerto 5432 (salida)" -ForegroundColor Gray
Write-Host "2. La red de la escuela puede estar bloqueando PostgreSQL" -ForegroundColor Gray
Write-Host "3. Prueba con tu hotspot movil" -ForegroundColor Gray
Write-Host ""
