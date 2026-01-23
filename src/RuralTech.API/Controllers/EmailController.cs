using Microsoft.AspNetCore.Mvc;
using RuralTech.Core.DTOs;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IConfiguration configuration, ILogger<EmailController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("send-confirmation")]
    public async Task<IActionResult> SendConfirmationEmail([FromBody] ConfirmationEmailDto dto)
    {
        try
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:Username"];
            var smtpPassword = _configuration["Email:Password"];
            var fromEmail = _configuration["Email:FromEmail"] ?? smtpUser;

            // Si no hay configuración de email, usar modo desarrollo (solo log)
            if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogInformation("=== EMAIL DE CONFIRMACIÓN (MODO DESARROLLO) ===");
                _logger.LogInformation($"Para: {dto.Email}");
                _logger.LogInformation($"Asunto: Bienvenido a RuralTech - Confirma tu registro");
                _logger.LogInformation($"Contraseña temporal: {dto.TempPassword}");
                _logger.LogInformation("=============================================");

                return Ok(new { message = "Email simulado enviado (modo desarrollo)", email = dto.Email });
            }

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail ?? "noreply@ruraltech.com", "RuralTech"),
                Subject = "Bienvenido a RuralTech - Confirma tu registro",
                Body = GenerateEmailBody(dto),
                IsBodyHtml = true
            };

            mailMessage.To.Add(dto.Email);

            await client.SendMailAsync(mailMessage);

            return Ok(new { message = "Email enviado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email");
            // En desarrollo, simular éxito
            _logger.LogInformation("=== EMAIL DE CONFIRMACIÓN (SIMULADO) ===");
            _logger.LogInformation($"Para: {dto.Email}");
            _logger.LogInformation($"Contraseña temporal: {dto.TempPassword}");
            
            return Ok(new { message = "Email simulado (error en configuración)", email = dto.Email });
        }
    }

    private string GenerateEmailBody(ConfirmationEmailDto dto)
    {
        var body = new StringBuilder();
        body.AppendLine("<!DOCTYPE html>");
        body.AppendLine("<html>");
        body.AppendLine("<head>");
        body.AppendLine("<meta charset='UTF-8'>");
        body.AppendLine("<style>");
        body.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
        body.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
        body.AppendLine(".header { background: linear-gradient(135deg, #2563eb 0%, #10b981 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }");
        body.AppendLine(".content { background: #f9fafb; padding: 30px; border-radius: 0 0 10px 10px; }");
        body.AppendLine(".credentials { background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #10b981; }");
        body.AppendLine(".button { display: inline-block; background: linear-gradient(135deg, #2563eb 0%, #10b981 100%); color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }");
        body.AppendLine("</style>");
        body.AppendLine("</head>");
        body.AppendLine("<body>");
        body.AppendLine("<div class='container'>");
        body.AppendLine("<div class='header'>");
        body.AppendLine("<h1>¡Bienvenido a RuralTech!</h1>");
        body.AppendLine("</div>");
        body.AppendLine("<div class='content'>");
        body.AppendLine($"<p>Hola <strong>{dto.FullName}</strong>,</p>");
        body.AppendLine("<p>Gracias por registrarte en RuralTech. Tu cuenta ha sido creada exitosamente.</p>");
        body.AppendLine("<div class='credentials'>");
        body.AppendLine("<h3>Tus credenciales de acceso:</h3>");
        body.AppendLine($"<p><strong>Email:</strong> {dto.Email}</p>");
        body.AppendLine($"<p><strong>Contraseña temporal:</strong> <code style='background: #f3f4f6; padding: 4px 8px; border-radius: 4px;'>{dto.TempPassword}</code></p>");
        body.AppendLine("<p><small>⚠️ Por seguridad, cambia tu contraseña después de iniciar sesión.</small></p>");
        body.AppendLine("</div>");
        body.AppendLine("<p>Puedes acceder a la aplicación desde:</p>");
        body.AppendLine("<a href='https://ruraltech.app/app' class='button'>Acceder a la App</a>");
        body.AppendLine("<p>O descarga nuestra aplicación móvil para una mejor experiencia.</p>");
        body.AppendLine("<hr style='border: none; border-top: 1px solid #e5e7eb; margin: 30px 0;'>");
        body.AppendLine("<p style='font-size: 12px; color: #6b7280;'>Si no solicitaste este registro, puedes ignorar este email.</p>");
        body.AppendLine("</div>");
        body.AppendLine("</div>");
        body.AppendLine("</body>");
        body.AppendLine("</html>");

        return body.ToString();
    }
}
