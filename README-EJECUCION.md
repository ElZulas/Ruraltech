# Cómo Ejecutar RuralTech

## Backend API (.NET)

### Opción 1: Usando PowerShell Script
```powershell
.\run-api.ps1
```

### Opción 2: Manualmente
```powershell
cd "src\RuralTech.API"
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run --urls "http://localhost:5002"
```

### Opción 3: Desde Visual Studio o Rider
1. Abre `RuralTech.sln`
2. Establece `RuralTech.API` como proyecto de inicio
3. Presiona F5

## Frontend React

```powershell
cd client
npm install  # Solo la primera vez
npm run dev
```

## URLs

- **API HTTP**: http://localhost:5002
- **API Swagger**: http://localhost:5002/swagger
- **Frontend**: http://localhost:3000

## Nota Importante

⚠️ **Usa HTTP (puerto 5002), NO HTTPS (puerto 5001)** para evitar problemas con certificados SSL de desarrollo.

Si ves errores de conexión, asegúrate de:
1. Que el API esté corriendo (deberías ver logs en la consola)
2. Que uses `http://` y no `https://`
3. Que el puerto 5002 no esté bloqueado por firewall
