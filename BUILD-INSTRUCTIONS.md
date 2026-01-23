# Instrucciones de Compilación Completa

## 1. Configurar Supabase

Sigue las instrucciones en `SUPABASE-SETUP-COMPLETO.md` para configurar la base de datos.

## 2. Compilar el API

```powershell
cd "RuralTech App\src\RuralTech.API"
dotnet build
dotnet run
```

El API estará disponible en `http://localhost:5000`

## 3. Compilar el Frontend Web

```powershell
cd "RuralTech App\client"
npm install
npm run build
```

Para desarrollo:
```powershell
npm run dev
```

## 4. Compilar Aplicación Android (Flutter)

```powershell
cd "RuralTech App\flutter-app"
flutter pub get
flutter build apk --release
```

El APK estará en: `flutter-app/build/app/outputs/flutter-apk/app-release.apk`

**Importante:** Antes de compilar, edita `lib/services/api_service.dart` y cambia la URL del API a tu IP local o servidor de producción.

## 5. Compilar Aplicación Desktop (Electron)

```powershell
cd "RuralTech App\electron-app"
npm install
npm run build:win
```

La aplicación estará en: `electron-app/dist/win-unpacked/`

## 6. Crear Instalador de Windows

1. **Instala Inno Setup** desde: https://jrsoftware.org/isdl.php

2. **Abre `installer/setup.iss` en Inno Setup Compiler**

3. **Compila el instalador:**
   - Build > Compile
   - O desde línea de comandos:
     ```powershell
     "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "RuralTech App\installer\setup.iss"
     ```

4. El instalador estará en: `installer/dist/RuralTech-Setup.exe`

## 7. Configurar Descargas en el API

Una vez compilados los instaladores, colócalos en:

- **PC Installer:** `installer/dist/RuralTech-Setup.exe`
- **Android APK:** `flutter-app/build/app/outputs/flutter-apk/app-release.apk`

El API servirá estos archivos desde los endpoints:
- `GET /api/download/pc` - Descarga el instalador de Windows
- `GET /api/download/android` - Descarga el APK de Android

## Orden de Compilación Recomendado

1. ✅ Configurar Supabase
2. ✅ Compilar y ejecutar el API
3. ✅ Compilar el frontend web
4. ✅ Compilar la app Flutter (Android)
5. ✅ Compilar la app Electron (Desktop)
6. ✅ Crear el instalador de Windows
7. ✅ Probar las descargas desde la página de landing

## Verificar que Todo Funciona

1. **API:** `http://localhost:5000/swagger`
2. **Frontend Web:** `http://localhost:5173`
3. **Registro:** Completa el formulario en la landing page
4. **Descargas:** Después del registro, prueba descargar PC y Android
5. **Instalación:** Instala ambas aplicaciones y verifica que se conecten al API

## Troubleshooting

### El API no inicia
- Verifica la connection string de Supabase
- Asegúrate de que las tablas existan en Supabase
- Revisa los logs del API

### Las descargas no funcionan
- Verifica que los archivos existan en las rutas esperadas
- Revisa los logs del API para ver errores
- Asegúrate de que los archivos se hayan compilado correctamente

### La app Flutter no se conecta al API
- Verifica que el API esté corriendo
- Actualiza la URL en `api_service.dart` con tu IP local
- Verifica que el dispositivo/emulador tenga acceso a la red

### El instalador no funciona
- Verifica que Electron se haya compilado correctamente
- Asegúrate de que la ruta en `setup.iss` sea correcta
- Revisa los logs de Inno Setup
