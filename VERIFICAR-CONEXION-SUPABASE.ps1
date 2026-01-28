# Script para verificar la conexion a Supabase
Write-Host "=== VERIFICANDO CONEXION A SUPABASE ===" -ForegroundColor Cyan
Write-Host ""

$hostName = "db.vaeufppoexhsmrlgjnai.supabase.co"

Write-Host "Intentando conectar a Supabase..." -ForegroundColor Yellow
Write-Host "Host: $hostName" -ForegroundColor Gray
Write-Host ""

# Verificar si podemos hacer ping al host
try {
    $ping = Test-Connection -ComputerName $hostName -Count 2 -ErrorAction Stop
    Write-Host "OK El host responde al ping" -ForegroundColor Green
} catch {
    Write-Host "ERROR No se puede alcanzar el host de Supabase" -ForegroundColor Red
    Write-Host "Verifica tu conexion a internet" -ForegroundColor Yellow
    exit
}

Write-Host ""
Write-Host "=== VERIFICACIONES ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Verifica que tu conexion a internet funcione" -ForegroundColor Yellow
Write-Host "2. Verifica que el firewall no este bloqueando PostgreSQL (puerto 5432)" -ForegroundColor Yellow
Write-Host "3. Verifica que Supabase este funcionando:" -ForegroundColor Yellow
Write-Host "   https://vaeufppoexhsmrlgjnai.supabase.co" -ForegroundColor Cyan
Write-Host ""
Write-Host "4. Si el problema persiste, verifica las credenciales en:" -ForegroundColor Yellow
Write-Host "   src\RuralTech.API\appsettings.json" -ForegroundColor Cyan
Write-Host ""
Write-Host "5. Verifica los logs del backend para ver el error exacto" -ForegroundColor Yellow
Write-Host ""
