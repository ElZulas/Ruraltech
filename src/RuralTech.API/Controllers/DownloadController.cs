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
            // Buscar versiones aprobadas: beta -> alpha -> prealpha (la más estable primero)
            var releasePaths = new[]
            {
                Path.Combine(_environment.ContentRootPath, "..", "..", "flutter-app", "releases", "beta", "Cownect-Beta.apk"),
                Path.Combine(_environment.ContentRootPath, "..", "..", "flutter-app", "releases", "alpha", "Cownect-Alpha.apk"),
                Path.Combine(_environment.ContentRootPath, "..", "..", "flutter-app", "releases", "prealpha", "Cownect-PreAlpha.apk"),
            };

            string? apkPath = null;
            string? apkFileName = null;
            
            foreach (var path in releasePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    apkPath = path;
                    apkFileName = Path.GetFileName(path);
                    break;
                }
            }
            
            if (apkPath == null || !System.IO.File.Exists(apkPath))
            {
                _logger.LogWarning($"No approved release found. Searched: {string.Join(", ", releasePaths)}");
                return NotFound(new { message = "No hay versión aprobada disponible. Las versiones de prueba (test1.apk, test2.apk, etc.) no están disponibles para descarga pública." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(apkPath);
            _logger.LogInformation($"Serving approved release from: {apkPath}");
            return File(fileBytes, "application/vnd.android.package-archive", apkFileName ?? "Cownect-Android.apk");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading Android APK");
            return StatusCode(500, new { message = "Error al descargar el APK" });
        }
    }

    [HttpGet("android/snapshot")]
    public IActionResult DownloadAndroidSnapshot()
    {
        try
        {
            // Buscar snapshots: snapshot10 -> snapshot9 -> ... -> snapshot1 (el más reciente primero)
            var snapshotPaths = new List<string>();
            for (int i = 10; i >= 1; i--)
            {
                snapshotPaths.Add(Path.Combine(_environment.ContentRootPath, "..", "..", "flutter-app", "releases", "snapshots", $"snapshot{i}.apk"));
            }

            string? apkPath = null;
            string? apkFileName = null;
            
            foreach (var path in snapshotPaths)
            {
                if (System.IO.File.Exists(path))
                {
                    apkPath = path;
                    apkFileName = Path.GetFileName(path);
                    break;
                }
            }
            
            if (apkPath == null || !System.IO.File.Exists(apkPath))
            {
                _logger.LogWarning($"No snapshot found. Searched: {string.Join(", ", snapshotPaths)}");
                return NotFound(new { message = "No hay snapshots disponibles actualmente." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(apkPath);
            _logger.LogInformation($"Serving snapshot from: {apkPath}");
            return File(fileBytes, "application/vnd.android.package-archive", apkFileName ?? "Cownect-Snapshot.apk");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading Android Snapshot");
            return StatusCode(500, new { message = "Error al descargar el snapshot" });
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
