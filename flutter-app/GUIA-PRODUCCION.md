# Guía Rápida: Cambiar a Producción

## Cuando Tengas tu Servidor en la Nube

### 1. Edita `lib/config/api_config.dart`

```dart
static const String environment = 'production'; // Cambiar de 'development' a 'production'
static const String productionBaseUrl = 'https://tu-servidor.com/api'; // Tu URL de producción
```

### 2. Recompila la App

```bash
flutter build apk --release
```

### 3. Copia a Releases

```bash
Copy-Item "build\app\outputs\flutter-apk\app-release.apk" "releases\beta\Cownect-Beta.apk"
```

## Ejemplo de URLs de Producción

- Railway: `https://cownect-api.railway.app/api`
- Render: `https://cownect-api.onrender.com/api`
- Azure: `https://cownect-api.azurewebsites.net/api`

## Verificar Configuración

La app automáticamente detectará el entorno y usará la URL correcta:
- **Development**: `http://192.168.1.64:5002/api` (tu PC local)
- **Production**: `https://tu-servidor.com/api` (servidor en la nube)
