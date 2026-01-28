# Cómo Ver los Logs del Error 500

## 🔍 Para Ver el Error Real

El error 500 ahora está capturado y mostrará el mensaje real en la consola del servidor.

### Pasos:

1. **Ve a la ventana de PowerShell donde está corriendo el backend**
2. **Busca mensajes que digan:**
   ```
   Error en login: [mensaje del error]
   Stack trace: [detalles técnicos]
   ```

3. **El error más común es:**
   - **Problema de conexión a Supabase** → Verifica que la conexión esté bien
   - **Campo null** → Algún campo requerido está null
   - **Problema con BCrypt** → Error al verificar la contraseña

## 🔧 Soluciones Comunes

### Si el error es de conexión a la base de datos:

Verifica que `appsettings.json` tenga la conexión correcta a Supabase.

### Si el error es con la contraseña:

Puede ser que el usuario tenga un PasswordHash null o inválido. El código ahora verifica esto.

### Si el error persiste:

Copia el mensaje completo del error de la consola y compártelo para poder ayudarte mejor.

## 📝 Nota

El servidor ahora tiene mejor manejo de errores y te dirá exactamente qué está fallando en la consola.
