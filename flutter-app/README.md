# RuralTech Mobile App (Flutter)

Aplicación móvil Android construida con Flutter.

## Requisitos Previos

1. Flutter SDK instalado (versión 3.0.0 o superior)
2. Android Studio con Android SDK
3. Un dispositivo Android o emulador

## Instalación

```bash
cd flutter-app
flutter pub get
```

## Configuración

Edita `lib/services/api_service.dart` y actualiza la URL del API:

```dart
static const String baseUrl = 'http://TU_IP_LOCAL:5000/api';
```

Para desarrollo, usa tu IP local (ej: `http://192.168.1.100:5000/api`)

## Ejecutar en Desarrollo

```bash
flutter run
```

## Compilar APK para Producción

```bash
flutter build apk --release
```

El APK estará en: `build/app/outputs/flutter-apk/app-release.apk`

## Compilar AAB (para Google Play Store)

```bash
flutter build appbundle --release
```

El AAB estará en: `build/app/outputs/bundle/release/app-release.aab`

## Estructura del Proyecto

```
lib/
├── main.dart              # Punto de entrada
├── screens/               # Pantallas de la app
│   ├── login_screen.dart
│   └── dashboard_screen.dart
└── services/              # Servicios
    └── api_service.dart   # Cliente HTTP para el API
```

## Características

- ✅ Login y registro
- ✅ Dashboard con lista de animales
- ✅ Conexión al API de .NET
- ✅ Almacenamiento local de tokens
- ✅ Interfaz moderna con Material Design
