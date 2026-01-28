# Solucionar Error de Conexión en la App

## Error que Aparece

Cuando intentas iniciar sesión o registrarte, aparece un error como:
- `Connection timed out`
- `SocketSoftware caused connection abort`
- `No se pudo conectar al servidor`

## Soluciones (en orden de probabilidad)

### 1. Verificar que el Servidor API Esté Corriendo

**En tu computadora, ejecuta:**
```powershell
cd "src\RuralTech.API"
dotnet run --urls "http://localhost:5002"
```

O usa el script:
```powershell
.\run-api.ps1
```

**Debes ver algo como:**
```
Now listening on: http://localhost:5002
```

### 2. Verificar que Estés en la Misma Red WiFi

- Tu computadora y tu teléfono deben estar en la **misma red WiFi**
- Si estás usando datos móviles en el teléfono, no funcionará
- Si cambiaste de red WiFi, necesitas actualizar la IP

### 3. Verificar la IP del Servidor

**Obtén tu IP actual:**
```powershell
ipconfig | findstr IPv4
```

**Busca la IP de tu red local** (ej: `192.168.1.64`)

**Si la IP cambió:**
1. Edita `flutter-app/lib/config/api_config.dart`
2. Cambia `devServerIp = 'TU_NUEVA_IP'`
3. Recompila: `flutter build apk --release`
4. Copia a tests: `Copy-Item "build\app\outputs\flutter-apk\app-release.apk" "tests\testX.apk"`

### 4. Verificar el Firewall de Windows

El firewall puede estar bloqueando el puerto 5002.

**Permitir el puerto en el firewall:**
1. Abre "Firewall de Windows Defender"
2. Click en "Configuración avanzada"
3. Click en "Reglas de entrada" > "Nueva regla"
4. Selecciona "Puerto" > Siguiente
5. TCP > Puerto específico: `5002` > Siguiente
6. Permitir la conexión > Siguiente
7. Marca todas las opciones > Siguiente
8. Nombre: "RuralTech API" > Finalizar

### 5. Probar la Conexión desde el Teléfono

**Abre un navegador en tu teléfono** (misma red WiFi) y ve a:
```
http://TU_IP:5002/swagger
```

Si puedes ver Swagger, el servidor está accesible. Si no, hay un problema de red.

### 6. Verificar que el Servidor Escuche en Todas las Interfaces

El servidor debe escuchar en `0.0.0.0` o tu IP local, no solo en `localhost`.

**En `run-api.ps1` o al ejecutar:**
```powershell
dotnet run --urls "http://0.0.0.0:5002"
```

O para escuchar en tu IP específica:
```powershell
dotnet run --urls "http://192.168.1.64:5002"
```

## test3.apk - Mejoras

La versión `test3.apk` incluye:
- ✅ Timeout aumentado a 30 segundos
- ✅ Mensajes de error más claros
- ✅ Mejor detección de problemas de red
- ✅ Instrucciones en el mensaje de error

## Si Nada Funciona

1. **Reinicia el router WiFi** (a veces ayuda)
2. **Desactiva temporalmente el firewall** para probar
3. **Usa un hotspot de tu teléfono** y conecta tu PC a él
4. **Verifica que no haya VPN activa** que pueda interferir

## Para Producción

Cuando publiques la app, usa un servidor en la nube (Railway, Render, etc.) y cambia:
```dart
static const String environment = 'production';
static const String productionBaseUrl = 'https://tu-servidor.com/api';
```

Así no dependerás de tener tu PC prendida.
