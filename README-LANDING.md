# RuralTech - Página de Landing y Registro

## Características Implementadas

### 1. Página de Landing (`/`)
- Diseño moderno con gradientes de color
- Sección hero con información sobre la aplicación
- Tarjetas de características principales
- Formulario de registro integrado

### 2. Sistema de Registro
- Formulario completo con validación
- Generación automática de contraseña temporal
- Envío de email de confirmación (con credenciales)
- Redirección a la aplicación después del registro

### 3. Sistema de Email
- Endpoint: `POST /api/email/send-confirmation`
- Envía email HTML con credenciales de acceso
- Modo desarrollo: muestra credenciales en logs si no hay configuración SMTP
- Configuración en `appsettings.json`:
  ```json
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": "587",
    "Username": "tu-email@gmail.com",
    "Password": "tu-contraseña-app",
    "FromEmail": "tu-email@gmail.com"
  }
  ```

### 4. Aplicación PWA (Progressive Web App)
- Manifest.json configurado para instalación
- Service Worker para funcionamiento offline
- Instalable en dispositivos móviles y desktop
- Accesible desde `/app/dashboard`

### 5. Base de Datos de Prueba
- Seeder automático con datos de ejemplo:
  - Usuario demo: `demo@ruraltech.com` / `Demo123!`
  - 3 animales de ejemplo (Luna, Toro Max, Esperanza)
  - Historial de peso para cada animal
  - Vacunas registradas
  - 3 productos en el marketplace

## Cómo Usar

### 1. Configurar Email (Opcional)
Si quieres enviar emails reales, edita `appsettings.json` y agrega tus credenciales SMTP.

Para Gmail:
- Necesitas una "Contraseña de aplicación" (no tu contraseña normal)
- Ve a: Configuración de Google > Seguridad > Contraseñas de aplicaciones

### 2. Ejecutar la Aplicación

**Backend (API):**
```powershell
cd "RuralTech App\src\RuralTech.API"
dotnet run
```

**Frontend:**
```powershell
cd "RuralTech App\client"
npm run dev
```

### 3. Probar el Registro

1. Ve a `http://localhost:5173` (página de landing)
2. Completa el formulario de registro
3. Revisa los logs del API para ver las credenciales (si no hay email configurado)
4. Inicia sesión con las credenciales recibidas

### 4. Usar Datos de Prueba

Puedes iniciar sesión directamente con:
- Email: `demo@ruraltech.com`
- Contraseña: `Demo123!`

Este usuario ya tiene animales, vacunas y productos de ejemplo.

## Rutas

- `/` - Página de landing y registro
- `/login` - Página de inicio de sesión
- `/register` - Página de registro alterna
- `/app/dashboard` - Dashboard principal (requiere autenticación)
- `/app/animals` - Gestión de animales
- `/app/marketplace` - Marketplace
- `/app/alerts` - Alertas de vacunas

## Instalación como PWA

### En Chrome/Edge (Desktop):
1. Abre la aplicación en el navegador
2. Haz clic en el ícono de instalación en la barra de direcciones
3. La app se instalará como aplicación de escritorio

### En Android (Chrome):
1. Abre la aplicación en Chrome
2. Menú > "Agregar a pantalla de inicio"
3. La app aparecerá como una aplicación instalada

### En iOS (Safari):
1. Abre la aplicación en Safari
2. Compartir > "Agregar a pantalla de inicio"
3. La app aparecerá en tu pantalla de inicio
