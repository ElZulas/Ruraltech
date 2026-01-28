# Solución: Error al Iniciar Sesión desde el Celular

## Problema

El login no funciona desde el celular porque:
1. El proxy de Vite está usando `localhost:5002` (no funciona desde el celular)
2. CORS no permite conexiones desde la IP del celular
3. Los servidores pueden no estar corriendo

## Soluciones Aplicadas

### ✅ 1. Proxy de Vite Actualizado

El proxy ahora usa la IP real `10.3.0.235:5002` en vez de `localhost:5002`.

### ✅ 2. CORS Actualizado

CORS ahora permite conexiones desde:
- `http://localhost:3000` (desde tu PC)
- `http://10.3.0.235:3000` (desde tu celular)

## Pasos para que Funcione

### 1. Reiniciar el Backend

**Detén el servidor actual** (Ctrl+C) y reinícialo:
```powershell
.\run-api.ps1
```

**Debe mostrar:**
```
Now listening on: http://0.0.0.0:5002
```

### 2. Reiniciar el Frontend

**Detén el frontend actual** (Ctrl+C) y reinícialo:
```powershell
.\iniciar-frontend.ps1
```

**Debe mostrar:**
```
VITE ready in XXX ms
➜  Local:   http://localhost:3000/
➜  Network: http://10.3.0.235:3000/
```

### 3. Verificar que Ambos Estén Corriendo

```powershell
netstat -an | findstr "5002\|3000" | findstr "LISTENING"
```

**Debe mostrar:**
```
TCP    0.0.0.0:3000           0.0.0.0:0              LISTENING
TCP    0.0.0.0:5002           0.0.0.0:0              LISTENING
```

### 4. Probar desde el Celular

**Landing Page:**
```
http://10.3.0.235:3000
```

**Swagger:**
```
http://10.3.0.235:5002/swagger
```

## Si Aún No Funciona

### Verificar Firewall

**Permite ambos puertos:**
```powershell
# Como Administrador
.\PERMITIR-PUERTO-FIREWALL.ps1
.\PERMITIR-PUERTO-3000.ps1
```

### Verificar Red

- Tu celular y PC deben estar en la **misma red WiFi**
- No uses datos móviles
- Algunas redes escolares bloquean conexiones entre dispositivos

### Probar desde tu PC Primero

1. Abre `http://localhost:3000` en tu PC
2. Intenta iniciar sesión
3. Si funciona en PC pero no en celular, es problema de red/firewall

## Checklist

- [ ] Backend corriendo en `0.0.0.0:5002`
- [ ] Frontend corriendo en `0.0.0.0:3000`
- [ ] Proxy de Vite usando `10.3.0.235:5002`
- [ ] CORS permite `http://10.3.0.235:3000`
- [ ] Firewall permite puertos 3000 y 5002
- [ ] Celular en la misma red WiFi
