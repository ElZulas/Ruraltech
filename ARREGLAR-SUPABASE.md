# Arreglar Conexión a Supabase

## ✅ Cambio Revertido

He restaurado la conexión a Supabase en `appsettings.json`.

## 🔍 Problema

No se puede conectar a Supabase desde tu red. Posibles causas:

1. **Firewall bloqueando puerto 5432 (salida)**
2. **Red de la escuela bloquea conexiones externas**
3. **Supabase temporalmente no disponible**

## ✅ Soluciones

### 1. Permitir Puerto 5432 en Firewall (Salida)

```powershell
# Como Administrador
New-NetFirewallRule -DisplayName "PostgreSQL Outbound - Puerto 5432" -Direction Outbound -LocalPort 5432 -Protocol TCP -Action Allow
```

### 2. Verificar que Supabase Esté Funcionando

Abre en tu navegador:
```
https://vaeufppoexhsmrlgjnai.supabase.co
```

Si no carga, Supabase puede estar caído o tu red lo bloquea.

### 3. Ver Logs del Backend

Ve a la ventana de PowerShell donde está corriendo el backend y busca el error exacto de conexión.

### 4. Si la Red de la Escuela Bloquea Supabase

Algunas redes escolares bloquean conexiones externas. En ese caso:
- Usa tu hotspot móvil
- O espera a estar en otra red (casa, etc.)

## 📝 Estado Actual

- ✅ Conexión a Supabase restaurada en `appsettings.json`
- ✅ Backend reiniciado
- ⚠️ Si sigue sin conectar, revisa los logs del backend para ver el error exacto

## 🔧 Ver Error Exacto

El backend ahora muestra el error exacto en la consola. Revisa la ventana de PowerShell donde está corriendo para ver qué está fallando.
