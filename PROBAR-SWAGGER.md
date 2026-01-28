# Cómo Probar Swagger desde tu Celular

## ✅ IP Actualizada

Tu nueva IP es: **10.3.0.235**

## 🔍 Verificar que el Servidor Esté Corriendo

**Ejecuta en PowerShell:**
```powershell
netstat -an | findstr "5002" | findstr "LISTENING"
```

**Debe mostrar:**
```
TCP    0.0.0.0:5002           0.0.0.0:0              LISTENING
```

Si muestra `127.0.0.1:5002` o `[::1]:5002`, el servidor solo escucha en localhost. Reinícialo con:
```powershell
.\run-api.ps1
```

## 📱 URL para Acceder desde tu Celular

**Swagger:**
```
http://10.3.0.235:5002/swagger
```

**API Directa:**
```
http://10.3.0.235:5002/api
```

## ⚠️ Si No Funciona

### 1. Verificar que Estés en la Misma Red

- Tu PC y celular deben estar en la **misma red WiFi**
- No uses datos móviles
- Verifica la IP de tu PC:
  ```powershell
  ipconfig | findstr IPv4
  ```
  Debe mostrar `10.3.0.235`

### 2. Verificar el Firewall

**Permite el puerto 5002:**
```powershell
# Como Administrador
New-NetFirewallRule -DisplayName "RuralTech API - Puerto 5002" -Direction Inbound -LocalPort 5002 -Protocol TCP -Action Allow
```

O ejecuta:
```powershell
.\PERMITIR-PUERTO-FIREWALL.ps1
```

### 3. Probar con Ping

Desde el celular, descarga una app de ping y prueba:
```
ping 10.3.0.235
```

Si el ping funciona pero Swagger no, es un problema del servidor o firewall.

### 4. Probar desde tu PC Primero

Abre en tu navegador:
```
http://localhost:5002/swagger
```

Si funciona en tu PC pero no en el celular, es un problema de red o firewall.

### 5. Reiniciar el Servidor

1. Detén el servidor (Ctrl+C en la ventana donde está corriendo)
2. Reinícialo:
   ```powershell
   .\run-api.ps1
   ```
3. Verifica que diga: `Now listening on: http://0.0.0.0:5002`

## 🎯 Checklist

- [ ] Servidor corriendo en `0.0.0.0:5002`
- [ ] Firewall permite puerto 5002
- [ ] Celular en la misma red WiFi
- [ ] IP correcta: `10.3.0.235`
- [ ] Swagger funciona en PC: `http://localhost:5002/swagger`

## 💡 Solución Rápida

Si nada funciona, prueba desactivar temporalmente el firewall:
1. Abre "Firewall de Windows Defender"
2. Desactiva temporalmente
3. Prueba Swagger desde el celular
4. Si funciona, vuelve a activar el firewall y configura la regla correctamente
