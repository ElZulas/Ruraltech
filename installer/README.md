# Instalador de Windows para RuralTech

Este instalador crea una aplicación desktop completa con estructura de carpetas en el escritorio.

## Requisitos

1. **Inno Setup** instalado (descarga desde: https://jrsoftware.org/isdl.php)
2. La aplicación Electron compilada en `../electron-app/dist/win-unpacked/`

## Compilar el Instalador

1. **Primero, compila la aplicación Electron:**
   ```powershell
   cd ..\electron-app
   npm run build:win
   ```

2. **Luego, compila el instalador:**
   - Abre `setup.iss` en Inno Setup Compiler
   - Haz clic en "Build" > "Compile"
   - O ejecuta desde la línea de comandos:
     ```powershell
     "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" setup.iss
     ```

3. El instalador estará en `dist/RuralTech-Setup.exe`

## Lo que Hace el Instalador

- Instala la aplicación en `C:\Program Files\RuralTech\`
- Crea un acceso directo en el escritorio
- Crea una carpeta `RuralTech` en el escritorio con:
  - `Documentos/` - Para documentos del usuario
  - `Reportes/` - Para reportes generados
  - `Backups/` - Para backups de la base de datos
- Agrega entrada en el menú de inicio
- Opcional: Icono en la barra de tareas

## Personalización

Edita `setup.iss` para:
- Cambiar el nombre de la aplicación
- Modificar las carpetas creadas
- Cambiar la ubicación de instalación
- Agregar más archivos o configuraciones

## Notas

- El instalador requiere permisos de administrador
- La aplicación se puede desinstalar desde "Agregar o quitar programas"
- Las carpetas en el escritorio se crean automáticamente durante la instalación
