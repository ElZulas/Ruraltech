# Opciones de Hosting para la API

## Resumen Rápido

Cuando publiques la app, necesitas hostear tu API .NET en la nube para que esté disponible 24/7 sin tener tu PC prendida.

## Opciones Gratuitas/Baratas

### 1. Railway (⭐ Recomendado)
- **Costo**: Gratis hasta cierto límite, luego ~$5-20/mes
- **Ventajas**: 
  - Super fácil de usar
  - Soporta .NET automáticamente
  - Despliegue con un comando
  - URL automática: `https://tu-app.railway.app`
- **Cómo**: https://railway.app
- **Comando**: `railway up` y listo

### 2. Render
- **Costo**: Plan gratuito disponible, luego ~$7-25/mes
- **Ventajas**:
  - Muy estable
  - Buena documentación
  - URL: `https://tu-app.onrender.com`
- **Cómo**: https://render.com

### 3. Fly.io
- **Costo**: Gratis para empezar, luego ~$5-15/mes
- **Ventajas**:
  - Muy rápido
  - Global (múltiples regiones)
  - URL: `https://tu-app.fly.dev`
- **Cómo**: https://fly.io

### 4. Azure App Service (Microsoft)
- **Costo**: Plan gratuito, luego ~$10-30/mes
- **Ventajas**:
  - Excelente para .NET (es de Microsoft)
  - Muy confiable
  - URL: `https://tu-app.azurewebsites.net`
- **Cómo**: https://azure.microsoft.com

## Pasos Básicos (Ejemplo con Railway)

1. **Crear cuenta en Railway**
2. **Conectar tu repo de GitHub**
3. **Railway detecta automáticamente que es .NET**
4. **Configurar variables de entorno:**
   - `ConnectionStrings__DefaultConnection` = Tu string de Supabase
   - `Jwt__Key` = Tu clave JWT
5. **Deploy automático**
6. **Obtener URL**: `https://tu-app.railway.app`

## Configurar la App

Una vez tengas tu URL de producción:

1. Edita `flutter-app/lib/config/api_config.dart`:
   ```dart
   static const String environment = 'production';
   static const String productionBaseUrl = 'https://tu-app.railway.app/api';
   ```

2. Recompila:
   ```bash
   flutter build apk --release
   ```

3. Listo! La app usará tu servidor en la nube

## Base de Datos

✅ Tu base de datos ya está en Supabase (en la nube)
✅ No necesitas hacer nada extra
✅ Solo asegúrate de que el servidor tenga las credenciales correctas

## Costo Total Estimado

- **Hosting API**: $0-20/mes (depende del tráfico)
- **Supabase**: Gratis hasta cierto límite, luego ~$25/mes
- **Total**: ~$0-45/mes para empezar

## Recomendación

Para empezar, usa **Railway** porque:
- Es gratis para desarrollo
- Super fácil de configurar
- Funciona perfecto con .NET
- Puedes escalar cuando crezcas
