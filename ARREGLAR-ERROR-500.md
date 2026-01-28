# Arreglar Error 500 al Iniciar Sesión

## 🔍 Problema

El error 500 ocurre cuando intentas iniciar sesión desde la landing page en tu celular.

## ✅ Solución Aplicada

1. **Proxy de Vite corregido:** Ahora usa `localhost:5002` (el proxy funciona desde el servidor de Vite)
2. **Manejo de errores mejorado:** El backend ahora captura y muestra errores específicos

## 🚀 Pasos para que Funcione

### 1. Asegúrate de que AMBOS servidores estén corriendo

**Backend (API):**
```powershell
.\run-api.ps1
```
Debe mostrar: `Now listening on: http://0.0.0.0:5002`

**Frontend (Landing Page):**
```powershell
.\iniciar-frontend.ps1
```
Debe mostrar: `Network: http://TU_IP:3000/`

### 2. Verifica que ambos estén corriendo

```powershell
netstat -an | findstr "5002|3000" | findstr "LISTENING"
```

Debe mostrar AMBOS puertos:
```
TCP    0.0.0.0:3000           0.0.0.0:0              LISTENING
TCP    0.0.0.0:5002           0.0.0.0:0              LISTENING
```

### 3. Reinicia el Frontend

**IMPORTANTE:** El frontend necesita reiniciarse para que tome los cambios del proxy.

1. Detén el frontend (Ctrl+C en la ventana donde está corriendo)
2. Reinícialo:
   ```powershell
   .\iniciar-frontend.ps1
   ```

### 4. Prueba desde tu Celular

- Abre: `http://TU_IP:3000`
- Intenta iniciar sesión

## 🔍 Si Aún Da Error 500

### Ver los Logs del Backend

Ve a la ventana de PowerShell donde está corriendo el backend y busca mensajes como:
```
Error en login: [mensaje del error]
```

Esto te dirá exactamente qué está fallando.

### Posibles Causas

1. **Backend no está corriendo** → Inícialo con `.\run-api.ps1`
2. **Problema de conexión a Supabase** → Verifica la conexión en `appsettings.json`
3. **Usuario no existe o contraseña incorrecta** → Debería dar 401, no 500
4. **Campo null en la base de datos** → El código ahora verifica esto

## 📝 Nota Importante

El proxy de Vite funciona así:
- Cuando accedes desde `http://TU_IP:3000` en tu celular
- El frontend hace peticiones a `/api/auth/login`
- Vite redirige esas peticiones a `http://localhost:5002/api/auth/login`
- Por eso el proxy debe usar `localhost`, no la IP externa
