# Cómo Publicar la App en Producción

## El Problema

Cuando publiques la app, no puedes tener tu computadora prendida siempre. Necesitas un servidor en la nube que esté disponible 24/7.

## Solución: Hosting en la Nube

Tienes varias opciones para hostear tu API .NET:

### Opciones Recomendadas (Gratis o Baratas)

#### 1. **Railway** (Recomendado - Fácil)
- ✅ Gratis para empezar
- ✅ Fácil de configurar
- ✅ Soporta .NET automáticamente
- ✅ URL: `https://tu-app.railway.app`
- 📖 Guía: https://docs.railway.app

#### 2. **Render**
- ✅ Plan gratuito disponible
- ✅ Soporta .NET
- ✅ URL: `https://tu-app.onrender.com`
- 📖 Guía: https://render.com/docs

#### 3. **Fly.io**
- ✅ Plan gratuito
- ✅ Muy rápido
- ✅ URL: `https://tu-app.fly.dev`
- 📖 Guía: https://fly.io/docs

#### 4. **Azure App Service** (Microsoft)
- ✅ Plan gratuito para empezar
- ✅ Excelente para .NET
- ✅ URL: `https://tu-app.azurewebsites.net`
- 📖 Guía: https://docs.microsoft.com/azure/app-service

### Pasos para Publicar

#### Paso 1: Preparar el API para Producción

1. **Actualizar appsettings.json para producción:**
   - Asegúrate de que tenga las credenciales de Supabase
   - Configura CORS para permitir tu dominio de producción

2. **Configurar variables de entorno en el hosting:**
   - `ConnectionStrings__DefaultConnection` = Tu string de conexión de Supabase
   - `Jwt__Key` = Tu clave JWT
   - `ASPNETCORE_ENVIRONMENT` = Production

#### Paso 2: Desplegar el API

**Ejemplo con Railway:**
```bash
# Instalar Railway CLI
npm i -g @railway/cli

# Login
railway login

# Inicializar proyecto
railway init

# Desplegar
railway up
```

#### Paso 3: Configurar la App Flutter para Producción

1. **Edita `flutter-app/lib/config/api_config.dart`:**
   ```dart
   static const String environment = 'production';
   static const String productionBaseUrl = 'https://tu-app.railway.app/api';
   ```

2. **Recompila la app:**
   ```bash
   flutter build apk --release
   ```

3. **Copia a releases/:**
   ```bash
   Copy-Item "build\app\outputs\flutter-apk\app-release.apk" "releases\beta\Cownect-Beta.apk"
   ```

## Configuración de CORS

Asegúrate de que tu API permita las peticiones desde la app móvil. En `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

## Base de Datos

Tu base de datos ya está en Supabase (en la nube), así que no hay problema. Solo asegúrate de:
- ✅ Las credenciales estén correctas en el servidor de producción
- ✅ El servidor pueda conectarse a Supabase (no debería haber problema)

## Costos Estimados

- **Railway**: Gratis hasta cierto límite, luego ~$5-20/mes
- **Render**: Gratis con limitaciones, luego ~$7-25/mes
- **Fly.io**: Gratis para empezar, luego ~$5-15/mes
- **Azure**: Plan gratuito disponible, luego ~$10-30/mes

## Ventajas de Hosting en la Nube

✅ Tu servidor está disponible 24/7
✅ No necesitas tener tu PC prendida
✅ Escalable (puede manejar más usuarios)
✅ Más seguro (certificados SSL automáticos)
✅ Backups automáticos en algunos servicios

## Notas Importantes

- Cuando cambies a producción, actualiza `environment = 'production'` en `api_config.dart`
- La app automáticamente usará la URL de producción
- Asegúrate de que el servidor tenga las variables de entorno correctas
- Prueba la conexión antes de publicar la app
