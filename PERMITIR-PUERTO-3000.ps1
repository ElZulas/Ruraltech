# Script para permitir el puerto 3000 en el firewall
# DEBES EJECUTAR ESTE SCRIPT COMO ADMINISTRADOR

Write-Host "=== CONFIGURANDO FIREWALL PARA PUERTO 3000 ===" -ForegroundColor Cyan
Write-Host ""

# Verificar si se ejecuta como administrador
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "ERROR: Este script debe ejecutarse como Administrador" -ForegroundColor Red
    Write-Host ""
    Write-Host "Para ejecutarlo:" -ForegroundColor Yellow
    Write-Host "1. Click derecho en PowerShell" -ForegroundColor Gray
    Write-Host "2. Selecciona 'Ejecutar como administrador'" -ForegroundColor Gray
    Write-Host "3. Navega a esta carpeta y ejecuta: .\PERMITIR-PUERTO-3000.ps1" -ForegroundColor Gray
    Write-Host ""
    pause
    exit
}

Write-Host "Creando regla de firewall para el puerto 3000..." -ForegroundColor Yellow

try {
    # Verificar si ya existe una regla
    $existingRule = Get-NetFirewallRule | Where-Object {$_.DisplayName -eq "RuralTech Frontend - Puerto 3000"}
    
    if ($existingRule) {
        Write-Host "La regla ya existe. Eliminando la anterior..." -ForegroundColor Yellow
        Remove-NetFirewallRule -DisplayName "RuralTech Frontend - Puerto 3000"
    }
    
    # Crear nueva regla
    New-NetFirewallRule -DisplayName "RuralTech Frontend - Puerto 3000" `
                       -Direction Inbound `
                       -LocalPort 3000 `
                       -Protocol TCP `
                       -Action Allow `
                       -Profile Domain,Private,Public
    
    Write-Host ""
    Write-Host "OK Regla de firewall creada exitosamente!" -ForegroundColor Green
    Write-Host ""
    Write-Host "El puerto 3000 ahora esta permitido en el firewall." -ForegroundColor Cyan
    Write-Host ""
    
} catch {
    Write-Host ""
    Write-Host "ERROR al crear la regla: $_" -ForegroundColor Red
    Write-Host ""
}

pause
