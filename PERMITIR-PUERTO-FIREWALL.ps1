# Script para permitir el puerto 5002 en el firewall
# DEBES EJECUTAR ESTE SCRIPT COMO ADMINISTRADOR

Write-Host "=== CONFIGURANDO FIREWALL ===" -ForegroundColor Cyan
Write-Host ""

# Verificar si se ejecuta como administrador
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "ERROR: Este script debe ejecutarse como Administrador" -ForegroundColor Red
    Write-Host ""
    Write-Host "Para ejecutarlo:" -ForegroundColor Yellow
    Write-Host "1. Click derecho en PowerShell" -ForegroundColor Gray
    Write-Host "2. Selecciona 'Ejecutar como administrador'" -ForegroundColor Gray
    Write-Host "3. Navega a esta carpeta y ejecuta: .\PERMITIR-PUERTO-FIREWALL.ps1" -ForegroundColor Gray
    Write-Host ""
    pause
    exit
}

Write-Host "Creando regla de firewall para el puerto 5002..." -ForegroundColor Yellow

try {
    # Verificar si ya existe una regla
    $existingRule = Get-NetFirewallRule | Where-Object {$_.DisplayName -eq "RuralTech API - Puerto 5002"}
    
    if ($existingRule) {
        Write-Host "La regla ya existe. Eliminando la anterior..." -ForegroundColor Yellow
        Remove-NetFirewallRule -DisplayName "RuralTech API - Puerto 5002"
    }
    
    # Crear nueva regla
    New-NetFirewallRule -DisplayName "RuralTech API - Puerto 5002" `
                       -Direction Inbound `
                       -LocalPort 5002 `
                       -Protocol TCP `
                       -Action Allow `
                       -Profile Domain,Private,Public
    
    Write-Host ""
    Write-Host "OK Regla de firewall creada exitosamente!" -ForegroundColor Green
    Write-Host ""
    Write-Host "El puerto 5002 ahora esta permitido en el firewall." -ForegroundColor Cyan
    Write-Host ""
    
} catch {
    Write-Host ""
    Write-Host "ERROR al crear la regla: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Puedes crear la regla manualmente:" -ForegroundColor Yellow
    Write-Host "1. Abre 'Firewall de Windows Defender con seguridad avanzada'" -ForegroundColor Gray
    Write-Host "2. Click en 'Reglas de entrada' > 'Nueva regla...'" -ForegroundColor Gray
    Write-Host "3. Selecciona 'Puerto' > TCP > Puerto 5002" -ForegroundColor Gray
    Write-Host "4. Permite la conexion > Todas las opciones > Finalizar" -ForegroundColor Gray
    Write-Host ""
}

pause
