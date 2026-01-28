# Crear Usuario de Prueba para Login

## Problema

El error 500 puede ser porque no hay usuarios en la base de datos o el usuario que intentas usar no existe.

## Solución: Crear Usuario de Prueba

### Opción 1: Desde Swagger

1. Abre Swagger: `http://localhost:5002/swagger` (o `http://10.234.89.228:5002/swagger` desde tu celular)
2. Ve al endpoint `POST /api/auth/register`
3. Usa estos datos:
   ```json
   {
     "email": "test@test.com",
     "password": "Test123!",
     "fullName": "Usuario Prueba",
     "dateOfBirth": "2000-01-01",
     "phone": null,
     "location": null
   }
   ```
4. Ejecuta el endpoint
5. Ahora puedes iniciar sesión con:
   - Email: `test@test.com`
   - Contraseña: `Test123!`

### Opción 2: Desde la Landing Page

1. Ve a la página de registro: `http://10.234.89.228:3000/register`
2. Llena el formulario con:
   - Nombre Completo: `Usuario Prueba`
   - Email: `test@test.com`
   - Fecha de Nacimiento: `01/01/2000`
   - Contraseña: `Test123!`
3. Regístrate
4. Luego inicia sesión con esas credenciales

### Opción 3: Usuario Demo (si existe)

El código tiene un seeder que crea un usuario demo:
- Email: `demo@ruraltech.com`
- Contraseña: `Demo123!`

Prueba iniciar sesión con esas credenciales.

## Verificar que el Usuario Existe

Si quieres verificar qué usuarios hay en la base de datos, puedes:
1. Ir a Supabase Dashboard
2. Ver la tabla `Users`
3. Verificar que haya usuarios registrados

## Nota sobre Contraseñas

Las contraseñas deben cumplir:
- Mínimo 8 caracteres
- Al menos 1 número
- Al menos 1 símbolo (!@#$%^&*(),.?":{}|<>)
