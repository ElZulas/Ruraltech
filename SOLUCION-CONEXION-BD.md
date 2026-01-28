# Solución: Error de Conexión a la Base de Datos

## 🔍 Problema

El error indica que no se puede conectar a Supabase (la base de datos en la nube).

## ✅ Soluciones

### 1. Verificar Conexión a Internet

Asegúrate de que tu PC tenga conexión a internet:
- Abre un navegador
- Ve a: `https://vaeufppoexhsmrlgjnai.supabase.co`
- Si no carga, hay un problema de internet

### 2. Verificar que Supabase Esté Funcionando

1. Ve al Dashboard de Supabase: `https://supabase.com/dashboard`
2. Selecciona tu proyecto
3. Ve a "Settings" > "Database"
4. Verifica que el estado sea "Active"

### 3. Verificar Credenciales

Las credenciales están en `src/RuralTech.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.vaeufppoexhsmrlgjnai.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=a.h.a.v'.s.m_34;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

**Verifica que:**
- El password sea correcto: `a.h.a.v'.s.m_34`
- El host sea correcto: `db.vaeufppoexhsmrlgjnai.supabase.co`

### 4. Verificar Firewall

El firewall puede estar bloqueando el puerto 5432 (PostgreSQL).

**Permitir el puerto:**
```powershell
# Como Administrador
New-NetFirewallRule -DisplayName "PostgreSQL - Puerto 5432" -Direction Outbound -LocalPort 5432 -Protocol TCP -Action Allow
```

### 5. Probar Conexión Manualmente

Ejecuta este script para verificar:
```powershell
.\VERIFICAR-CONEXION-SUPABASE.ps1
```

### 6. Ver Logs del Backend

Ve a la ventana de PowerShell donde está corriendo el backend y busca mensajes como:
```
Error conectando a la base de datos: [mensaje]
```

Esto te dirá exactamente qué está fallando.

## 🔧 Solución Temporal: Usar SQLite

Si Supabase no está disponible, puedes usar SQLite temporalmente:

1. **Comenta la conexión de Supabase** en `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": ""
     }
   }
   ```

2. **El código automáticamente usará SQLite** si no hay conexión de Supabase

3. **Reinicia el backend**

**Nota:** SQLite es solo para desarrollo. Para producción necesitas Supabase.

## 📋 Checklist

- [ ] Internet funciona
- [ ] Supabase Dashboard accesible
- [ ] Credenciales correctas en `appsettings.json`
- [ ] Firewall permite puerto 5432 (salida)
- [ ] Backend reiniciado después de cambios

## 💡 Si Nada Funciona

1. **Verifica en Supabase Dashboard** que el proyecto esté activo
2. **Intenta cambiar la contraseña** en Supabase y actualiza `appsettings.json`
3. **Usa SQLite temporalmente** para seguir trabajando
