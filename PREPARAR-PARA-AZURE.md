# Preparar el Proyecto para Deploy en Azure

## 🎯 Objetivo

Cuando hagas deploy en Azure, **NO necesitarás cambiar IPs manualmente**. Todo funcionará automáticamente con URLs públicas.

## 📋 Cambios Necesarios para Azure

### 1. Frontend React (Landing Page)

**Archivo:** `client/vite.config.ts`

Cuando hagas deploy, el frontend se conectará automáticamente a la API en Azure. Solo necesitas:

```typescript
// En producción, usar la URL de Azure
const apiUrl = import.meta.env.PROD 
  ? 'https://tu-api.azurewebsites.net/api'
  : 'http://localhost:5002/api';
```

O mejor aún, usar variables de entorno:
```typescript
const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5002/api';
```

### 2. App Flutter

**Archivo:** `flutter-app/lib/config/api_config.dart`

Ya está configurado para producción:
```dart
static const String environment = 'production';
static const String productionBaseUrl = 'https://TU_SERVIDOR_EN_LA_NUBE.com/api';
```

Solo cambia:
- `environment = 'production'`
- `productionBaseUrl = 'https://tu-api.azurewebsites.net/api'`

### 3. API Backend (.NET)

**Archivo:** `src/RuralTech.API/appsettings.json`

Azure usará variables de entorno automáticamente. Solo asegúrate de configurar en Azure Portal:
- `ConnectionStrings__DefaultConnection` = Tu string de Supabase
- `Jwt__Key` = Tu clave JWT
- `ASPNETCORE_ENVIRONMENT` = Production

## 🚀 Proceso de Deploy en Azure

### Paso 1: Crear App Service en Azure

1. Ve a [Azure Portal](https://portal.azure.com)
2. Crea un nuevo "App Service" (Web App)
3. Selecciona:
   - Runtime: .NET 8
   - OS: Windows o Linux
   - Plan: Básico (gratis para empezar)

### Paso 2: Configurar Variables de Entorno

En Azure Portal, ve a tu App Service → Configuration → Application settings:

```
ConnectionStrings__DefaultConnection = Host=db.vaeufppoexhsmrlgjnai.supabase.co;Port=5432;...
Jwt__Key = RuralTechSecretKeyForJWTTokenGeneration2024SecureKey
Jwt__Issuer = RuralTech
Jwt__Audience = RuralTechUsers
ASPNETCORE_ENVIRONMENT = Production
```

### Paso 3: Deploy del Backend

**Opción A: Desde Visual Studio**
1. Click derecho en el proyecto `RuralTech.API`
2. Publish → Azure → App Service
3. Selecciona tu App Service
4. Publish

**Opción B: Desde GitHub Actions (CI/CD)**
- Configura un workflow que haga deploy automático

**Opción C: Desde Azure CLI**
```bash
az webapp deploy --resource-group tu-grupo --name tu-app --src-path ./src/RuralTech.API
```

### Paso 4: Actualizar Frontend

1. Cambia la URL del API en `client/vite.config.ts` o usa variable de entorno
2. Deploy del frontend a Azure Static Web Apps o Azure App Service

### Paso 5: Actualizar App Flutter

1. Cambia `api_config.dart`:
   ```dart
   static const String environment = 'production';
   static const String productionBaseUrl = 'https://tu-api.azurewebsites.net/api';
   ```
2. Recompila: `flutter build apk --release`
3. Sube a `releases/prealpha/` o `releases/beta/`

## ✅ Ventajas de Azure

- ✅ **URLs públicas**: No necesitas IPs locales
- ✅ **Siempre disponible**: 24/7 sin tener tu PC prendida
- ✅ **HTTPS automático**: Certificados SSL gratuitos
- ✅ **Escalable**: Puede manejar más usuarios
- ✅ **Variables de entorno**: Configuración segura

## 🔄 Mientras Tanto (Desarrollo Local)

### Para Cambiar de Red (Escuela, Casa, etc.)

**Opción 1: Script Automático (Recomendado)**
```powershell
.\OBTENER-IP.ps1
```
Este script detecta tu IP automáticamente y te pregunta si quieres actualizar todo.

**Opción 2: Manual**
```powershell
.\CAMBIAR-IP.ps1 -NuevaIP "192.168.1.100"
```

**Opción 3: Solo Cambiar en api_config.dart**
Si solo quieres probar la app Flutter:
1. Edita `flutter-app/lib/config/api_config.dart`
2. Cambia `devServerIp = 'TU_NUEVA_IP'`
3. Recompila: `flutter build apk --release`

## 📝 Checklist Antes de Deploy

- [ ] Backend configurado con variables de entorno
- [ ] Frontend configurado para usar URL de producción
- [ ] App Flutter configurada con `environment = 'production'`
- [ ] CORS configurado para permitir tu dominio
- [ ] Base de datos (Supabase) accesible desde Azure
- [ ] Pruebas realizadas en entorno de producción

## 🎉 Después del Deploy

Una vez en Azure:
- ✅ No necesitarás cambiar IPs nunca más
- ✅ La app funcionará desde cualquier lugar
- ✅ URLs públicas y estables
- ✅ HTTPS automático

## 💡 Recomendación

**Para mañana en la escuela:**
1. Ejecuta `.\OBTENER-IP.ps1` cuando llegues
2. O dame la IP y ejecuto `.\CAMBIAR-IP.ps1 -NuevaIP "TU_IP"`
3. Recompila la app si es necesario
4. Listo para probar

**Para producción:**
- Haz deploy en Azure lo antes posible
- Así no tendrás que cambiar IPs nunca más
- Todo funcionará con URLs públicas
