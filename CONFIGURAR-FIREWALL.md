# Configurar Firewall de Windows para Permitir el Puerto 5002

## Método 1: Desde la Interfaz Gráfica (Más Fácil)

### Paso 1: Abrir Firewall de Windows
1. Presiona `Windows + R`
2. Escribe: `wf.msc` y presiona Enter
3. Se abrirá "Firewall de Windows Defender con seguridad avanzada"

### Paso 2: Crear Regla de Entrada
1. En el panel izquierdo, click en **"Reglas de entrada"**
2. En el panel derecho, click en **"Nueva regla..."**

### Paso 3: Configurar la Regla
1. **Tipo de regla:** Selecciona **"Puerto"** → Siguiente
2. **Protocolo y puertos:**
   - Selecciona **"TCP"**
   - Selecciona **"Puertos locales específicos"**
   - Escribe: `5002`
   - Siguiente
3. **Acción:**
   - Selecciona **"Permitir la conexión"**
   - Siguiente
4. **Perfil:**
   - Marca **TODAS** las opciones:
     - ☑ Dominio
     - ☑ Privada
     - ☑ Pública
   - Siguiente
5. **Nombre:**
   - Nombre: `RuralTech API - Puerto 5002`
   - Descripción: `Permite conexiones al servidor API de RuralTech desde dispositivos en la red local`
   - Finalizar

### Paso 4: Verificar
- Deberías ver la nueva regla en la lista de "Reglas de entrada"
- Asegúrate de que esté **Habilitada** (columna "Estado")

---

## Método 2: Desde PowerShell (Rápido)

Abre PowerShell como **Administrador** y ejecuta:

```powershell
New-NetFirewallRule -DisplayName "RuralTech API - Puerto 5002" -Direction Inbound -LocalPort 5002 -Protocol TCP -Action Allow
```

---

## Método 3: Desactivar Firewall Temporalmente (Solo para Probar)

⚠️ **Solo para pruebas, no recomendado para uso permanente**

1. Presiona `Windows + R`
2. Escribe: `firewall.cpl` y presiona Enter
3. Click en **"Activar o desactivar Firewall de Windows Defender"**
4. Desactiva el firewall para redes privadas y públicas
5. Prueba la conexión
6. **IMPORTANTE:** Vuelve a activar el firewall después de probar

---

## Verificar que Funciona

### 1. Verificar que el Servidor Está Corriendo
```powershell
.\run-api.ps1
```

Deberías ver:
```
Now listening on: http://0.0.0.0:5002
```

### 2. Verificar desde tu PC
Abre en el navegador: `http://localhost:5002/swagger`

### 3. Obtener tu IP Local
```powershell
ipconfig | findstr IPv4
```

Busca la IP de tu red local (ej: `192.168.1.64`)

### 4. Verificar desde tu Celular
- Conecta tu celular a la **misma red WiFi** que tu PC
- Abre el navegador en tu celular
- Ve a: `http://TU_IP:5002/swagger` (ej: `http://192.168.1.64:5002/swagger`)

Si ves Swagger, el firewall está configurado correctamente.

---

## Solución de Problemas

### Si aún no funciona después de configurar el firewall:

1. **Verifica que el servidor esté corriendo:**
   ```powershell
   netstat -an | findstr 5002
   ```
   Deberías ver algo como: `TCP    0.0.0.0:5002    0.0.0.0:0    LISTENING`

2. **Verifica que estés en la misma red:**
   - Tu PC y celular deben estar en la misma WiFi
   - No uses datos móviles en el celular

3. **Verifica la IP:**
   - La IP puede cambiar si te desconectas y vuelves a conectar
   - Ejecuta `ipconfig | findstr IPv4` de nuevo

4. **Prueba con ping desde el celular:**
   - Descarga una app de ping en tu celular
   - Haz ping a la IP de tu PC
   - Si el ping funciona, el problema no es el firewall

5. **Reinicia el servidor:**
   - Detén el servidor (Ctrl+C)
   - Vuelve a ejecutar `.\run-api.ps1`

---

## Nota Importante

Si cambias de red WiFi, la IP de tu PC puede cambiar. Actualiza la IP en `flutter-app/lib/config/api_config.dart` si es necesario.
