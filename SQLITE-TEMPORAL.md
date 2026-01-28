# Usando SQLite Temporalmente

## ✅ Solución Aplicada

He configurado el proyecto para usar **SQLite local** en vez de Supabase porque no se puede conectar a Supabase desde tu red.

## 🔄 Cambios Realizados

1. **Conexión de Supabase deshabilitada** en `appsettings.json`
2. **SQLite activado automáticamente** - El código detecta que no hay conexión de Supabase y usa SQLite
3. **Base de datos local:** `ruraltech_temp.db` (se crea automáticamente)

## 🚀 Cómo Funciona Ahora

- **Base de datos:** SQLite local (archivo `ruraltech_temp.db` en la carpeta del proyecto)
- **Datos:** Se guardan localmente en tu PC
- **Ventaja:** Funciona sin internet
- **Desventaja:** Los datos solo están en tu PC (no se sincronizan con Supabase)

## 📝 Para Volver a Usar Supabase

Cuando tengas conexión a Supabase de nuevo:

1. **Edita `src/RuralTech.API/appsettings.json`**
2. **Pon la conexión de Supabase de nuevo:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=db.vaeufppoexhsmrlgjnai.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=a.h.a.v'.s.m_34;SSL Mode=Require;Trust Server Certificate=true"
     }
   }
   ```
3. **Reinicia el backend**

## ✅ Prueba Ahora

1. **Espera a que el backend termine de iniciar** (30-60 segundos)
2. **Intenta iniciar sesión desde tu celular**
3. **Si no tienes usuario, créalo desde Swagger o la Landing Page**

El login debería funcionar ahora porque usa SQLite local que no requiere conexión a internet.

## ⚠️ Nota Importante

- Los datos en SQLite son **temporales** y solo están en tu PC
- Cuando vuelvas a usar Supabase, necesitarás crear los usuarios de nuevo
- SQLite es solo para desarrollo/pruebas
