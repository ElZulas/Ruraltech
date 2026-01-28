cd "src\RuralTech.API"
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "Iniciando RuralTech API..." -ForegroundColor Green
Write-Host ""
Write-Host "El servidor estara disponible en:" -ForegroundColor Cyan
Write-Host "  - http://localhost:5002 (desde esta PC)" -ForegroundColor Cyan
Write-Host "  - http://10.234.89.228:5002 (desde dispositivos en la misma red)" -ForegroundColor Cyan
Write-Host ""
Write-Host "Presiona Ctrl+C para detener el servidor" -ForegroundColor Yellow
Write-Host ""
dotnet run --urls "http://10.234.89.228:5002"
