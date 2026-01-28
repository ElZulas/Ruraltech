# Cómo Compilar la App Android

## Requisitos Previos

1. **Flutter SDK** instalado (versión 3.0.0 o superior)
2. **Android Studio** o Android SDK instalado
3. **Java JDK** 11 o superior

## Pasos para Compilar

### 1. Navegar a la carpeta de la app Flutter

```bash
cd flutter-app
```

### 2. Obtener dependencias

```bash
flutter pub get
```

### 3. Verificar configuración de Android

```bash
flutter doctor
```

Asegúrate de que Android toolchain esté configurado correctamente.

### 4. Compilar APK de release

```bash
flutter build apk --release
```

El APK se generará en:
```
flutter-app/build/app/outputs/flutter-apk/app-release.apk
```

### 5. (Opcional) Compilar App Bundle para Google Play

```bash
flutter build appbundle --release
```

El AAB se generará en:
```
flutter-app/build/app/outputs/bundle/release/app-release.aab
```

## Configuración de la App

### Cambiar el nombre de la app

Edita `pubspec.yaml`:
```yaml
name: ruraltech_mobile
version: 1.0.0+1
```

### Configurar permisos

Los permisos se configuran en `android/app/src/main/AndroidManifest.xml`

### Icono de la app

Coloca el icono en `assets/icon/icon.png` y ejecuta:
```bash
flutter pub run flutter_launcher_icons
```

## Notas

- El APK generado será compatible con Android 8.0 (API level 26) o superior
- El tamaño del APK será aproximadamente 20-30 MB
- Para distribuir en Google Play, usa el formato AAB (Android App Bundle)

## Ubicación del APK para el servidor

Una vez compilado, copia el APK a:
```
src/RuralTech.API/apps/Cownect-Android.apk
```

O el servidor buscará automáticamente en:
- `flutter-app/build/app/outputs/flutter-apk/app-release.apk`
- `flutter-app/build/app/outputs/apk/release/app-release.apk`
