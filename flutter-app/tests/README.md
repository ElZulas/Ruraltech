# Carpeta de Pruebas - APKs

Esta carpeta es para guardar los APKs compilados durante las pruebas.

## Convención de Nombres

Guarda los APKs con nombres secuenciales:
- `test1.apk` - Primera prueba
- `test2.apk` - Segunda prueba
- `test3.apk` - Tercera prueba
- Y así sucesivamente...

## Cómo Compilar y Guardar

1. Compila el APK:
```bash
cd flutter-app
flutter build apk --release
```

2. Copia el APK a esta carpeta con el nombre correspondiente:
```bash
# Windows PowerShell
Copy-Item "build\app\outputs\flutter-apk\app-release.apk" "tests\test1.apk"

# O manualmente desde el explorador de archivos
```

## Notas

- Cada vez que hagas cambios significativos, compila un nuevo APK y guárdalo con el siguiente número
- Esto te permite comparar versiones y hacer rollback si algo sale mal
- Los APKs aquí NO se suben al servidor automáticamente, son solo para pruebas locales
