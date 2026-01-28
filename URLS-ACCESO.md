# URLs para Acceder desde tu Celular

## 📱 Landing Page (Frontend React)

**URL:** `http://10.234.89.228:3000`

Abre esta URL en el navegador de tu celular (debe estar en la misma red WiFi que tu PC).

---

## 🔧 API Backend

**URL:** `http://10.234.89.228:5002`

**Swagger (Documentación API):** `http://10.234.89.228:5002/swagger`

---

## 📥 Descargar App Android

**URL de Descarga:** `http://10.234.89.228:5002/api/download/android`

O desde la Landing Page, después de iniciar sesión, ve a la página de descarga.

---

## ⚠️ IMPORTANTE

- Tu celular y tu PC deben estar en la **misma red WiFi**
- No uses datos móviles en tu celular
- Si cambias de red WiFi, la IP puede cambiar (ejecuta `ipconfig | findstr IPv4` para obtener la nueva IP)
- **El frontend debe estar corriendo** - Ejecuta `.\iniciar-frontend.ps1` o `cd client && npm run dev`
- **El backend debe estar corriendo** - Ejecuta `.\run-api.ps1`
- **Ambos puertos deben estar permitidos en el firewall** (3000 y 5002)

---

## 📋 Resumen de URLs

| Servicio | URL |
|----------|-----|
| Landing Page | `http://10.234.89.228:3000` |
| API Backend | `http://10.234.89.228:5002` |
| Swagger | `http://10.234.89.228:5002/swagger` |
| Descargar Android (PreAlpha) | `http://10.234.89.228:5002/api/download/android` |

---

## 🚀 Estado Actual

✅ Servidor API corriendo en `10.234.89.228:5002` (accesible desde `10.234.89.228:5002`)  
✅ Frontend React corriendo en puerto `3000`  
✅ PreAlpha APK disponible en `releases/prealpha/Cownect-PreAlpha.apk`  
✅ Endpoint de descarga configurado
