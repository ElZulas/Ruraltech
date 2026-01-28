# Cómo Iniciar el Servidor Correctamente

## ⚠️ IMPORTANTE: Para que la App Móvil Funcione

El servidor debe escuchar en **todas las interfaces de red** (0.0.0.0), no solo en localhost.

## Opción 1: Usar el Script (Recomendado)

```powershell
.\run-api.ps1
```

El script ahora está configurado para escuchar en `0.0.0.0:5002`, lo que permite conexiones desde:
- Tu PC: `http://localhost:5002`
- Dispositivos móviles en la misma red: `http://TU_IP:5002`

## Opción 2: Manualmente

```powershell
cd "src\RuralTech.API"
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --urls "http://0.0.0.0:5002"
```

## Verificar que Funciona

1. **Desde tu PC:** Abre `http://localhost:5002/swagger`
2. **Desde tu teléfono (misma WiFi):** Abre `http://TU_IP:5002/swagger`

Si ambos funcionan, el servidor está configurado correctamente.

## Obtener tu IP Local

```powershell
ipconfig | findstr IPv4
```

Busca la IP de tu red local (ej: `192.168.1.64`)

## Nota sobre Firewall

Si el teléfono no puede conectarse, verifica el firewall de Windows:
- El puerto 5002 debe estar permitido
- O desactiva temporalmente el firewall para probar
