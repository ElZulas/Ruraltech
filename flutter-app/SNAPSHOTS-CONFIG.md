# Configuración de Snapshots - Base de Datos Local

## IMPORTANTE: Seguridad

Las snapshots **NO deben tener acceso a la base de datos del servidor**.
Deben usar una base de datos local (SQLite) para evitar problemas de seguridad.

## Configuración para Snapshots

### 1. Crear un archivo de configuración para snapshots

Crea `lib/config/app_config.dart`:

```dart
class AppConfig {
  static const bool isSnapshot = bool.fromEnvironment('SNAPSHOT', defaultValue: false);
  static const String apiBaseUrl = isSnapshot 
    ? 'http://localhost' // No usado en snapshots
    : 'http://TU_IP:5002/api';
  
  static const bool useLocalDatabase = isSnapshot;
}
```

### 2. Modificar ApiService para snapshots

En `lib/services/api_service.dart`, agregar verificación:

```dart
import '../config/app_config.dart';

class ApiService {
  static const String baseUrl = AppConfig.useLocalDatabase 
    ? 'http://localhost' // No se usa en snapshots
    : 'http://localhost:5002/api';
  
  // En snapshots, todas las llamadas deben fallar o usar BD local
  static Future<Map<String, dynamic>> getAnimals() async {
    if (AppConfig.useLocalDatabase) {
      // Usar base de datos local en vez de API
      return await LocalDatabaseService.getAnimals();
    }
    // Código normal para versiones aprobadas...
  }
}
```

### 3. Crear servicio de base de datos local

Crea `lib/services/local_database_service.dart`:

```dart
import 'package:sqflite/sqflite.dart';
import 'package:path/path.dart';

class LocalDatabaseService {
  static Database? _database;
  
  static Future<Database> get database async {
    if (_database != null) return _database!;
    _database = await _initDatabase();
    return _database!;
  }
  
  static Future<Database> _initDatabase() async {
    String path = join(await getDatabasesPath(), 'cownect_snapshot.db');
    return await openDatabase(
      path,
      version: 1,
      onCreate: _onCreate,
    );
  }
  
  static Future<void> _onCreate(Database db, int version) async {
    // Crear tablas locales aquí
    // Estas son copias de las tablas del servidor pero solo locales
  }
  
  // Métodos para CRUD local...
}
```

### 4. Compilar snapshot con flag

```bash
flutter build apk --release --dart-define=SNAPSHOT=true
```

### 5. Agregar dependencia SQLite

En `pubspec.yaml`:
```yaml
dependencies:
  sqflite: ^2.3.0
  path: ^1.8.3
```

## Comportamiento Esperado

### Versiones Aprobadas (PreAlpha/Alpha/Beta)
- ✅ Acceso completo a la API del servidor
- ✅ Conexión a Supabase PostgreSQL
- ✅ Todas las funcionalidades disponibles

### Snapshots
- ❌ NO tienen acceso a la API del servidor
- ✅ Usan SQLite local
- ✅ Datos de prueba locales
- ✅ Solo para pruebas de UI y funcionalidades nuevas
- ⚠️ Los datos NO se sincronizan con el servidor

## Notas de Seguridad

1. **Nunca** permitas que las snapshots se conecten al servidor
2. Las snapshots deben tener un flag de compilación que las identifique
3. Verifica que `AppConfig.useLocalDatabase` esté en `true` para snapshots
4. Todas las llamadas a `ApiService` deben verificar este flag primero
5. Los datos en snapshots son temporales y se pierden al desinstalar

## Implementación Futura

Cuando implementes snapshots:
1. Crea el servicio de BD local
2. Modifica todos los servicios para verificar el flag
3. Compila con `--dart-define=SNAPSHOT=true`
4. Prueba que NO pueda conectarse al servidor
5. Copia el APK a `releases/snapshots/snapshot1.apk`
