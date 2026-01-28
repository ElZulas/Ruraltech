# Cómo Iniciar los Servidores para que Funcione el Login

## ⚠️ PROBLEMA

Si no puedes iniciar sesión desde tu celular, probablemente los servidores **NO están corriendo**.

## ✅ SOLUCIÓN: Iniciar Ambos Servidores

### Paso 1: Iniciar el Backend (API)

**Abre PowerShell y ejecuta:**
```powershell
cd "C:\Users\mufas\OneDrive\Escritorio\CowNect"
.\run-api.ps1
```

**Debe mostrar:**
```
Now listening on: http://0.0.0.0:5002
```

**Deja esta ventana abierta** - El servidor seguirá corriendo aquí.

---

### Paso 2: Iniciar el Frontend (Landing Page)

**Abre OTRA ventana de PowerShell** (deja la anterior corriendo):

```powershell
cd "C:\Users\mufas\OneDrive\Escritorio\CowNect"
.\iniciar-frontend.ps1
```

**Debe mostrar:**
```
VITE ready in XXX ms
➜  Local:   http://localhost:3000/
➜  Network: http://10.3.0.235:3000/
```

**Deja esta ventana también abierta** - El frontend seguirá corriendo aquí.

---

### Paso 3: Verificar que Ambos Estén Corriendo

**En una tercera ventana de PowerShell, ejecuta:**
```powershell
netstat -an | findstr "5002|3000" | findstr "LISTENING"
```

**Debe mostrar:**
```
TCP    0.0.0.0:3000           0.0.0.0:0              LISTENING
TCP    0.0.0.0:5002           0.0.0.0:0              LISTENING
```

Si ves ambos puertos, **todo está bien configurado**.

---

## 📱 Probar desde tu Celular

1. **Abre el navegador en tu celular** (misma red WiFi)
2. **Ve a:** `http://10.3.0.235:3000`
3. **Intenta iniciar sesión**

---

## 🔍 Si Aún No Funciona

### Verificar Firewall

**Permite ambos puertos:**
```powershell
# Como Administrador
.\PERMITIR-PUERTO-FIREWALL.ps1
.\PERMITIR-PUERTO-3000.ps1
```

### Verificar Red

- Tu celular y PC deben estar en la **misma red WiFi**
- No uses datos móviles
- Algunas redes escolares bloquean conexiones entre dispositivos

### Ver Mensajes de Error

Si ves un error específico al intentar iniciar sesión:
- **"No se pudo conectar al servidor"** → El backend no está corriendo
- **"Email o contraseña incorrectos"** → Las credenciales están mal
- **Error de CORS** → Problema de configuración del servidor

---

## 🛑 Para Detener los Servidores

En cada ventana de PowerShell donde esté corriendo algo:
- Presiona `Ctrl + C`
- O simplemente cierra la ventana

---

## 📋 Checklist Rápido

- [ ] Backend corriendo (`.\run-api.ps1`)
- [ ] Frontend corriendo (`.\iniciar-frontend.ps1`)
- [ ] Ambos puertos escuchando (verificar con `netstat`)
- [ ] Firewall permite puertos 3000 y 5002
- [ ] Celular en la misma red WiFi
- [ ] Probar desde celular: `http://10.3.0.235:3000`

---

## 💡 Tip

Si tienes problemas, ejecuta este script para verificar todo:
```powershell
.\VERIFICAR-CONEXION.ps1
```
