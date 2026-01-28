# Solución Rápida: Error de Conexión a Base de Datos

## 🔍 El Problema

No se puede conectar a Supabase (la base de datos en la nube).

## ✅ Soluciones Rápidas

### 1. Verificar Internet

Abre tu navegador y ve a:
```
https://vaeufppoexhsmrlgjnai.supabase.co
```

Si no carga, hay un problema de internet.

### 2. Verificar Firewall (Puerto 5432)

El firewall puede estar bloqueando la conexión saliente a PostgreSQL.

**Permitir el puerto:**
```powershell
# Como Administrador
New-NetFirewallRule -DisplayName "PostgreSQL Outbound - Puerto 5432" -Direction Outbound -LocalPort 5432 -Protocol TCP -Action Allow
```

### 3. Ver Logs del Backend

**Ve a la ventana de PowerShell donde está corriendo el backend** y busca mensajes como:
```
Error conectando a la base de datos: [mensaje del error]
```

Esto te dirá exactamente qué está fallando.

### 4. Verificar Credenciales

Las credenciales están en `src/RuralTech.API/appsettings.json`.

**Verifica que la contraseña sea correcta:** `a.h.a.v'.s.m_34`

### 5. Probar Conexión

Ejecuta:
```powershell
.\VERIFICAR-CONEXION-SUPABASE.ps1
```

## 🔧 Solución Temporal: Usar SQLite

Si Supabase no está disponible temporalmente, puedes usar SQLite:

1. **Edita `src/RuralTech.API/appsettings.json`**
2. **Comenta o borra la conexión de Supabase:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": ""
     }
   }
   ```
3. **Reinicia el backend**

El código automáticamente usará SQLite si no hay conexión de Supabase.

**Nota:** SQLite es solo para desarrollo. Los datos se guardan localmente en `ruraltech_temp.db`.

## 📋 Checklist

- [ ] Internet funciona
- [ ] Supabase Dashboard accesible
- [ ] Firewall permite puerto 5432 (salida)
- [ ] Credenciales correctas
- [ ] Backend reiniciado

## 💡 Próximos Pasos

1. Verifica los logs del backend para ver el error exacto
2. Si es problema de internet/firewall, arregla eso
3. Si Supabase está caído, usa SQLite temporalmente
4. Cuando Supabase funcione de nuevo, vuelve a poner la conexión
