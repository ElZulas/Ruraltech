class ApiConfig {
  // ============================================
  // CONFIGURACIÓN DE ENTORNO
  // ============================================
  
  // Cambia esto según el entorno:
  // - 'development' = Para desarrollo local (localhost o IP local)
  // - 'production' = Para producción (servidor en la nube)
  static const String environment = 'development';
  
  // ============================================
  // CONFIGURACIÓN DE DESARROLLO
  // ============================================
  // Si estás probando en emulador Android, usa: 10.0.2.2
  // Si estás probando en dispositivo físico, usa tu IP local (ej: 192.168.1.XXX)
  static const String devServerIp = '10.234.89.228'; // Cambia esta IP si cambias de red
  static const int devServerPort = 5002;
  
  // ============================================
  // CONFIGURACIÓN DE PRODUCCIÓN
  // ============================================
  // Cuando publiques la app, configura aquí la URL de tu servidor en la nube
  // Ejemplos de hosting:
  // - Railway: https://tu-app.railway.app
  // - Render: https://tu-app.onrender.com
  // - Heroku: https://tu-app.herokuapp.com
  // - Azure: https://tu-app.azurewebsites.net
  // - AWS: https://tu-app.amazonaws.com
  // - Tu propio servidor: https://api.cownect.com
  static const String productionBaseUrl = 'https://TU_SERVIDOR_EN_LA_NUBE.com/api';
  
  // ============================================
  // URL FINAL (NO MODIFICAR)
  // ============================================
  static String get baseUrl {
    if (environment == 'production') {
      return productionBaseUrl;
    } else {
      // Desarrollo: usar IP local o localhost
      return 'http://$devServerIp:$devServerPort/api';
    }
  }
  
  // Para debugging
  static String get currentConfig {
    return 'Environment: $environment\n'
           'Base URL: $baseUrl';
  }
}
