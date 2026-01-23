# RuralTech - Aplicación Nativa

## Configuración Completa

### 1. Configurar Supabase

Sigue las instrucciones en `SUPABASE-SETUP.md` para configurar tu base de datos.

### 2. Instalar dependencias

**Backend:**
```powershell
cd "RuralTech App\src\RuralTech.API"
dotnet restore
```

**Frontend:**
```powershell
cd "RuralTech App\client"
npm install
```

**Electron (App Desktop):**
```powershell
cd "RuralTech App\electron-app"
npm install
```

### 3. Configurar variables de entorno

Edita `RuralTech App/src/RuralTech.API/appsettings.json` con tus credenciales de Supabase.

### 4. Ejecutar en desarrollo

**Terminal 1 - API:**
```powershell
cd "RuralTech App\src\RuralTech.API"
dotnet run
```

**Terminal 2 - Frontend:**
```powershell
cd "RuralTech App\client"
npm run dev
```

**Terminal 3 - Electron App:**
```powershell
cd "RuralTech App\electron-app"
npm start
```

### 5. Compilar aplicación para producción

**Compilar Frontend:**
```powershell
cd "RuralTech App\client"
npm run build
```

**Compilar Electron App:**

**Windows:**
```powershell
cd "RuralTech App\electron-app"
npm run build:win
```

**macOS:**
```powershell
cd "RuralTech App\electron-app"
npm run build:mac
```

**Linux:**
```powershell
cd "RuralTech App\electron-app"
npm run build:linux
```

Los ejecutables estarán en `RuralTech App/electron-app/dist/`

## Estructura del Proyecto

```
RuralTech App/
├── src/
│   ├── RuralTech.API/          # Backend .NET con Supabase
│   ├── RuralTech.Core/          # Entidades y DTOs
│   └── RuralTech.Infrastructure/# Acceso a datos
├── client/                      # Frontend React
├── electron-app/                # Aplicación Electron (Desktop)
│   ├── main.js                  # Proceso principal
│   ├── preload.js               # Script de precarga
│   └── package.json             # Configuración Electron
└── README-APP-NATIVA.md         # Este archivo
```

## Características

- ✅ Base de datos en Supabase (PostgreSQL)
- ✅ Aplicación desktop nativa con Electron
- ✅ Reutiliza todo el código React existente
- ✅ Instalable en Windows, macOS y Linux
- ✅ Funciona offline (con Service Worker)
- ✅ API REST completa con .NET

## Próximos Pasos

Para crear una aplicación móvil nativa (Android/iOS), puedes usar:

1. **React Native** - Reutiliza lógica pero requiere reescribir componentes
2. **Ionic** - Usa el mismo código web pero compila nativo
3. **Capacitor** - Similar a Ionic, muy fácil de integrar

¿Quieres que configuremos también una app móvil?
