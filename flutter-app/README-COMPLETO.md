# Cownect - App Flutter Completa

## Estructura de la App

### Pantallas Implementadas
- ✅ **LoginScreen** - Inicio de sesión con diseño similar a landing page
- ✅ **RegisterScreen** - Registro de usuarios con validaciones
- ✅ **DashboardScreen** - Dashboard principal con navegación
- ✅ **ColaboradoresScreen** - Gestión completa de colaboradores (CRUD)
- ✅ **UPPsScreen** - Pantalla base para UPPs
- ✅ **AnimalsScreen** - Pantalla base para Animales
- ✅ **BovinosScreen** - Pantalla base para Bovinos

### Servicios Implementados
- ✅ **ApiService** - Autenticación (login/registro)
- ✅ **UPPService** - CRUD de UPPs
- ✅ **ColaboradoresService** - CRUD completo de colaboradores
- ✅ **BovinosService** - CRUD de bovinos
- ✅ **AnimalsService** - CRUD de animales con peso, vacunas y tratamientos

## Diseño

La app sigue el mismo diseño de la landing page:
- **Colores**: Negro y blanco principalmente
- **Estilo**: Elegante, minimalista, con bordes negros
- **Tipografía**: Bold para títulos, normal para contenido
- **Iconos**: Material Icons en negro

## Funcionalidades

### Autenticación
- Login con email y contraseña
- Registro completo con validaciones:
  - Nombre completo (mínimo 2 palabras)
  - Email válido
  - Fecha de nacimiento (mayor de 18 años)
  - Contraseña (mínimo 8 caracteres, 1 número, 1 símbolo)
  - Teléfono y ubicación opcionales

### Colaboradores
- Listar colaboradores por UPP
- Agregar colaborador:
  - Nombre alias
  - PIN (4-6 dígitos)
  - Teléfono (opcional)
  - Rol (ENCARGADO, OPERARIO, VETERINARIO)
- Cambiar estatus (ACTIVO/SUSPENDIDO)
- Eliminar colaborador

### Navegación
- Bottom Navigation Bar con 5 secciones:
  - Inicio (Dashboard)
  - UPPs
  - Animales
  - Bovinos
  - Colaboradores

## Configuración

### URL del API
Edita `lib/services/api_service.dart`:
```dart
static const String baseUrl = 'http://TU_IP:5002/api';
```

Para desarrollo local en dispositivo físico, usa tu IP local:
```dart
static const String baseUrl = 'http://192.168.1.100:5002/api';
```

## Compilar APK

```bash
cd flutter-app
flutter pub get
flutter build apk --release
```

El APK estará en:
```
build/app/outputs/flutter-apk/app-release.apk
```

## Próximos Pasos

Las siguientes pantallas están creadas pero necesitan implementación completa:
- UPPs (crear, editar, eliminar)
- Animales (CRUD completo)
- Bovinos (CRUD completo)
- Infraestructura
- Lotes de Aves
- Marketplace
- Alertas

## Notas

- Todos los cambios se reflejan en la base de datos de Supabase
- La app requiere conexión a internet para funcionar
- Los tokens se guardan localmente usando SharedPreferences
- El diseño es responsive y funciona en diferentes tamaños de pantalla
