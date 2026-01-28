# Configurar IP del Servidor en la App

## Problema
Cuando pruebas la app en un dispositivo Android físico o emulador, `localhost` se refiere al dispositivo mismo, no a tu computadora donde está corriendo el servidor.

## Solución

### 1. Obtener tu IP Local

**Windows:**
```powershell
ipconfig | findstr IPv4
```

**Linux/Mac:**
```bash
ifconfig
# o
ip addr
```

Busca la IP de tu red local (ej: `192.168.1.64`)

### 2. Configurar en la App

Edita el archivo `lib/config/api_config.dart`:

```dart
static const String serverIp = 'TU_IP_LOCAL'; // Ejemplo: '192.168.1.64'
```

### 3. Recompilar

```bash
flutter build apk --release
```

### 4. Copiar a tests/

```powershell
Copy-Item "build\app\outputs\flutter-apk\app-release.apk" "tests\test2.apk"
```

## Notas Importantes

- **Emulador Android**: Usa `10.0.2.2` en vez de tu IP local
- **Dispositivo físico**: Usa tu IP local (ej: `192.168.1.64`)
- **Misma computadora**: Si pruebas en web o desktop, puedes usar `localhost`
- Si cambias de red WiFi, necesitas actualizar la IP

## IP Actual Configurada

- IP: `192.168.1.64`
- Puerto: `5002`
- URL completa: `http://192.168.1.64:5002/api`
