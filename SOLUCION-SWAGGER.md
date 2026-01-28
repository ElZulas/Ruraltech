# Solución: Swagger No Carga en el Celular

## Problema
El Swagger no carga desde el celular en `http://10.3.0.235:5002/swagger`

## Soluciones

### 1. Verificar que el Servidor Esté Corriendo

**Ejecuta en PowerShell:**
```powershell
.\run-api.ps1
```

**Debe mostrar:**
```
Now listening on: http://0.0.0.0:5002
```

### 2. Verificar que Escuche en Todas las Interfaces

**Ejecuta:**
```powershell
netstat -an | findstr "5002" | findstr "LISTENING"
```

**Debe mostrar:**
```
TCP    0.0.0.0:5002           0.0.0.0:0              LISTENING
```

Si muestra `127.0.0.1:5002` o `[::1]:5002`, el servidor solo escucha en localhost. Reinícialo.

### 3. Verificar el Firewall

**Permite el puerto 5002:**
```powershell
# Como Administrador
New-NetFirewallRule -DisplayName "RuralTech API - Puerto 5002" -Direction Inbound -LocalPort 5002 -Protocol TCP -Action Allow
```

O ejecuta:
```powershell
.\PERMITIR-PUERTO-FIREWALL.ps1
```

### 4. Probar desde el Celular

**URLs para probar:**
- Swagger: `http://10.3.0.235:5002/swagger`
- API Health: `http://10.3.0.235:5002/api`

### 5. Verificar que Estés en la Misma Red

- Tu PC y celular deben estar en la **misma red WiFi**
- No uses datos móviles
- Verifica que la IP de tu PC sea realmente `10.3.0.235`:
  ```powershell
  ipconfig | findstr IPv4
  ```

### 6. Probar con Ping

Desde el celular, descarga una app de ping y prueba:
```
ping 10.3.0.235
```

Si el ping funciona pero Swagger no, es un problema del servidor o firewall.

## Verificación Rápida

Ejecuta este script para verificar todo:
```powershell
.\VERIFICAR-CONEXION.ps1
```

## Si Aún No Funciona

1. **Reinicia el servidor:**
   - Detén el servidor (Ctrl+C)
   - Ejecuta `.\run-api.ps1` de nuevo

2. **Desactiva temporalmente el firewall** para probar:
   - Si funciona sin firewall, el problema es la configuración del firewall
   - Vuelve a activarlo después

3. **Verifica que no haya VPN activa** que pueda interferir

4. **Prueba desde otro dispositivo** para descartar problemas del celular
