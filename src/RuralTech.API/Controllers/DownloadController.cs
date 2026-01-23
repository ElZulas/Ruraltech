using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DownloadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DownloadController> _logger;

    public DownloadController(IWebHostEnvironment environment, ILogger<DownloadController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    [HttpGet("pc")]
    public IActionResult DownloadPC()
    {
        try
        {
            // Ruta al instalador de Windows
            var installerPath = Path.Combine(_environment.ContentRootPath, "..", "..", "installer", "dist", "RuralTech-Setup.exe");
            
            if (!System.IO.File.Exists(installerPath))
            {
                // Si no existe, crear un archivo temporal o redirigir
                _logger.LogWarning($"Installer not found at {installerPath}");
                return NotFound(new { message = "Instalador no disponible. Por favor, compila el instalador primero." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(installerPath);
            return File(fileBytes, "application/x-msdownload", "RuralTech-Setup.exe");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading PC installer");
            return StatusCode(500, new { message = "Error al descargar el instalador" });
        }
    }

    [HttpGet("android")]
    public IActionResult DownloadAndroid()
    {
        try
        {
            // Ruta al APK de Android
            var apkPath = Path.Combine(_environment.ContentRootPath, "..", "..", "flutter-app", "build", "app", "outputs", "flutter-apk", "app-release.apk");
            
            if (!System.IO.File.Exists(apkPath))
            {
                // Si no existe, crear un archivo temporal o redirigir
                _logger.LogWarning($"APK not found at {apkPath}");
                return NotFound(new { message = "APK no disponible. Por favor, compila la aplicación Android primero." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(apkPath);
            return File(fileBytes, "application/vnd.android.package-archive", "RuralTech.apk");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading Android APK");
            return StatusCode(500, new { message = "Error al descargar el APK" });
        }
    }

    [HttpGet("instructions")]
    public IActionResult GetDownloadInstructions()
    {
        var instructions = new
        {
            web = "Accede desde tu navegador: https://ruraltech.app/app",
            android = "Descarga el APK desde el botón de descarga y instálalo en tu dispositivo Android",
            pc = "Descarga el instalador de Windows y ejecútalo para instalar la aplicación"
        };

        return Ok(instructions);
    }
}
