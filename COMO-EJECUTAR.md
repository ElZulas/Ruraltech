# Cómo Ejecutar los Comandos

## 📍 Dónde Ejecutar

Todos los comandos se ejecutan en **PowerShell** desde la carpeta raíz del proyecto:
```
C:\Users\mufas\OneDrive\Escritorio\CowNect
```

## 🚀 Pasos para Iniciar Todo

### Paso 1: Abrir PowerShell

1. Presiona `Windows + X`
2. Selecciona **"Windows PowerShell"** o **"Terminal"**
3. O busca "PowerShell" en el menú de inicio

### Paso 2: Navegar a la Carpeta del Proyecto

```powershell
cd "C:\Users\mufas\OneDrive\Escritorio\CowNect"
```

### Paso 3: Iniciar el Backend (API)

**Opción A: Usar el Script (Recomendado)**
```powershell
.\run-api.ps1
```

**Opción B: Manualmente**
```powershell
cd "src\RuralTech.API"
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --urls "http://0.0.0.0:5002"
```

**Deja esta ventana abierta** - El servidor seguirá corriendo aquí.

### Paso 4: Iniciar el Frontend (Landing Page)

**Abre una NUEVA ventana de PowerShell** (deja la anterior corriendo el backend):

1. Presiona `Windows + X` → PowerShell (otra vez)
2. Navega al proyecto:
   ```powershell
   cd "C:\Users\mufas\OneDrive\Escritorio\CowNect"
   ```
3. Ejecuta:
   ```powershell
   .\iniciar-frontend.ps1
   ```

O manualmente:
```powershell
cd client
npm run dev
```

**Deja esta ventana también abierta** - El frontend seguirá corriendo aquí.

## 📱 Resultado

Ahora tendrás:
- ✅ Backend corriendo en `http://localhost:5002`
- ✅ Frontend corriendo en `http://localhost:3000`
- ✅ Accesible desde tu celular en `http://TU_IP:3000` y `http://TU_IP:5002`

## 🛑 Para Detener Todo

En cada ventana de PowerShell donde esté corriendo algo:
- Presiona `Ctrl + C`
- O simplemente cierra la ventana

## 🔧 Scripts Útiles

### Cambiar IP Automáticamente
```powershell
.\OBTENER-IP.ps1
```

### Cambiar IP Manualmente
```powershell
.\CAMBIAR-IP.ps1 -NuevaIP "192.168.1.100"
```

### Verificar Conexión
```powershell
.\VERIFICAR-CONEXION.ps1
```

### Permitir Puerto en Firewall (Como Administrador)
```powershell
.\PERMITIR-PUERTO-FIREWALL.ps1
.\PERMITIR-PUERTO-3000.ps1
```

## ⚠️ Notas Importantes

1. **Dos Ventanas Separadas**: Necesitas 2 ventanas de PowerShell:
   - Una para el backend (puerto 5002)
   - Otra para el frontend (puerto 3000)

2. **No Cierres las Ventanas**: Mientras estén corriendo, no cierres las ventanas de PowerShell.

3. **Permisos de Administrador**: Algunos scripts (como los de firewall) necesitan ejecutarse como Administrador:
   - Click derecho en PowerShell
   - "Ejecutar como administrador"

4. **Misma Carpeta**: Todos los scripts deben ejecutarse desde la carpeta raíz del proyecto:
   ```
   C:\Users\mufas\OneDrive\Escritorio\CowNect
   ```

## 🎯 Resumen Visual

```
PowerShell Ventana 1          PowerShell Ventana 2
─────────────────────         ─────────────────────
cd CowNect                     cd CowNect
.\run-api.ps1                 .\iniciar-frontend.ps1
                               (o: cd client && npm run dev)
     │                              │
     │ Backend corriendo            │ Frontend corriendo
     │ Puerto 5002                  │ Puerto 3000
     │                              │
     └──────────────────────────────┘
              │
              └─> Accesible desde celular
```

## 💡 Tip Pro

Puedes crear accesos directos en el escritorio para iniciar más rápido:
1. Click derecho en el escritorio → Nuevo → Acceso directo
2. Ubicación: `powershell.exe -NoExit -Command "cd 'C:\Users\mufas\OneDrive\Escritorio\CowNect'; .\run-api.ps1"`
3. Nombre: "Iniciar Backend"
4. Repite para el frontend
