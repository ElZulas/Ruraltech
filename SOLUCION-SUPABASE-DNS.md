# Solución: Problema de DNS con Supabase

## 🔍 Problema Detectado

No se puede resolver el nombre del host de Supabase (`db.vaeufppoexhsmrlgjnai.supabase.co`).

Esto puede ser porque:
1. **La red de la escuela bloquea DNS externos**
2. **Problema de DNS en tu PC**
3. **Supabase requiere conexión HTTPS primero**

## ✅ Soluciones

### 1. Cambiar DNS Temporalmente

**Usa DNS de Google (8.8.8.8):**

1. Abre "Configuración de red" en Windows
2. Ve a tu conexión WiFi
3. Propiedades > Protocolo de Internet versión 4 (TCP/IPv4)
4. Usar las siguientes direcciones de servidor DNS:
   - DNS preferido: `8.8.8.8`
   - DNS alternativo: `8.8.4.4`
5. Aceptar y reiniciar el backend

### 2. Verificar que Puedas Acceder a Supabase

Abre en tu navegador:
```
https://vaeufppoexhsmrlgjnai.supabase.co
```

Si no carga, la red de la escuela puede estar bloqueando Supabase.

### 3. Usar Hotspot Móvil

Si la red de la escuela bloquea Supabase:
- Conecta tu PC a tu hotspot móvil
- Intenta de nuevo

### 4. Ver Logs del Backend

Ve a la ventana de PowerShell donde está corriendo el backend (`.\run-api.ps1`).

Busca mensajes como:
```
Error conectando a la base de datos: [mensaje]
```

Esto te dirá el error exacto.

## 📝 Estado Actual

- ✅ Conexión a Supabase restaurada en `appsettings.json`
- ✅ Backend reiniciado
- ⚠️ Problema de resolución DNS detectado

## 🔧 Próximos Pasos

1. **Cambia el DNS a 8.8.8.8** (solución más probable)
2. **Reinicia el backend** después de cambiar DNS
3. **Verifica los logs** para ver si conecta

Si después de cambiar DNS sigue sin funcionar, la red de la escuela probablemente está bloqueando Supabase y necesitarás usar tu hotspot móvil.
