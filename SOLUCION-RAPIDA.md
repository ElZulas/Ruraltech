# Solución Rápida - Paso a Paso

## ⚠️ PROBLEMA DETECTADO
El servidor está escuchando solo en `localhost` (10.234.89.228), por eso tu celular no puede conectarse.

## SOLUCIÓN EN 3 PASOS

### Paso 1: Permitir Puerto en Firewall (IMPORTANTE)

**Opción A: Script Automático (Más Fácil)**
1. Click derecho en PowerShell
2. Selecciona **"Ejecutar como administrador"**
3. Navega a la carpeta del proyecto:
   ```powershell
   cd "C:\Users\mufas\OneDrive\Escritorio\CowNect"
   ```
4. Ejecuta:
   ```powershell
   .\PERMITIR-PUERTO-FIREWALL.ps1
   ```

**Opción B: Comando Directo**
1. Abre PowerShell como **Administrador**
2. Ejecuta:
   ```powershell
   New-NetFirewallRule -DisplayName "RuralTech API - Puerto 5002" -Direction Inbound -LocalPort 5002 -Protocol TCP -Action Allow
   ```

**Opción C: Manual (Interfaz Gráfica)**
1. Presiona `Windows + R`
2. Escribe: `wf.msc` y presiona Enter
3. Click en **"Reglas de entrada"** (lado izquierdo)
4. Click en **"Nueva regla..."** (lado derecho)
5. Selecciona **"Puerto"** → Siguiente
6. Selecciona **"TCP"** y escribe `5002` → Siguiente
7. Selecciona **"Permitir la conexión"** → Siguiente
8. Marca **TODAS** las opciones (Dominio, Privada, Pública) → Siguiente
9. Nombre: `RuralTech API - Puerto 5002` → Finalizar

---

### Paso 2: Detener y Reiniciar el Servidor

1. **Detén el servidor actual:**
   - Ve a la ventana donde está corriendo
   - Presiona `Ctrl + C`

2. **Reinicia el servidor:**
   ```powershell
   .\run-api.ps1
   ```

3. **Verifica que diga:**
   ```
   Now listening on: http://10.234.89.228:5002
   ```
   
   Si dice `10.234.89.228` o `localhost`, el cambio no se aplicó. Detén y vuelve a iniciar.

---

### Paso 3: Probar desde tu Celular

1. **Obtén tu IP local:**
   ```powershell
   ipconfig | findstr IPv4
   ```
   Busca la IP de tu red local (ej: `10.234.89.228`)

2. **Desde tu celular (misma WiFi):**
   - Abre el navegador
   - Ve a: `http://TU_IP:5002/swagger`
   - Ejemplo: `http://10.234.89.228:5002/swagger`

3. **Si ves Swagger, ¡funciona!**
   - Ahora prueba la app `test3.apk`
   - Debería poder iniciar sesión

---

## Verificar que Todo Está Bien

Ejecuta este script para verificar:
```powershell
.\VERIFICAR-CONEXION.ps1
```

Te dirá:
- ✅ Tu IP local
- ✅ Si el servidor está corriendo
- ✅ Si el firewall está configurado
- ✅ La URL para acceder desde tu celular

---

## Si Aún No Funciona

1. **Verifica que estés en la misma red WiFi:**
   - Tu PC y celular deben estar en la misma red
   - No uses datos móviles

2. **Verifica la IP:**
   - La IP puede cambiar si te desconectas de WiFi
   - Ejecuta `ipconfig | findstr IPv4` de nuevo

3. **Prueba desactivar el firewall temporalmente:**
   - Solo para probar, no recomendado permanentemente
   - Si funciona sin firewall, el problema es la configuración del firewall

4. **Reinicia el router WiFi:**
   - A veces ayuda con problemas de red

---

## Tu IP Actual

Según la verificación, tu IP local es: **10.234.89.228**

Entonces desde tu celular deberías acceder a:
**http://10.234.89.228:5002/swagger**
