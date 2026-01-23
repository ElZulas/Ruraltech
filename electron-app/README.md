# RuralTech Desktop App (Electron)

Aplicación desktop nativa construida con Electron que reutiliza el código React del frontend.

## Instalación

```bash
npm install
```

## Desarrollo

```bash
# Asegúrate de que el frontend esté corriendo en http://localhost:5173
npm start
```

## Compilar para Producción

### Windows
```bash
npm run build:win
```

### macOS
```bash
npm run build:mac
```

### Linux
```bash
npm run build:linux
```

Los ejecutables estarán en la carpeta `dist/`.

## Requisitos Previos

1. El API debe estar corriendo en `http://localhost:5000`
2. El frontend debe estar compilado o corriendo en desarrollo en `http://localhost:5173`

## Notas

- En desarrollo, la app se conecta al servidor de desarrollo de Vite
- En producción, carga los archivos estáticos desde `../client/dist/`
- Asegúrate de compilar el frontend antes de compilar Electron: `cd ../client && npm run build`
