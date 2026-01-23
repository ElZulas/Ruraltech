using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RuralTech.Core.DTOs;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using RuralTech.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        // Validar que el usuario sea mayor de 18 años
        var age = DateTime.UtcNow.Year - dto.DateOfBirth.Year;
        if (DateTime.UtcNow.DayOfYear < dto.DateOfBirth.DayOfYear) age--;
        
        if (age < 18)
        {
            return BadRequest(new { message = "Debes ser mayor de 18 años para registrarte como Propietario Legal" });
        }

        // Validar formato de email
        if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return BadRequest(new { message = "El formato del email no es válido" });
        }

        // Validar que el nombre completo tenga al menos 2 palabras
        var nameWords = dto.FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (nameWords.Length < 2)
        {
            return BadRequest(new { message = "El nombre completo debe tener al menos 2 palabras" });
        }

        // Validar longitud máxima del nombre completo
        if (dto.FullName.Length > 150)
        {
            return BadRequest(new { message = "El nombre completo no puede exceder 150 caracteres" });
        }

        // Validar contraseña: mínimo 8 caracteres, al menos 1 número y 1 símbolo
        if (dto.Password.Length < 8 || 
            !Regex.IsMatch(dto.Password, @"\d") ||
            !Regex.IsMatch(dto.Password, @"[!@#$%^&*(),.?\"":{}|<>]"))
        {
            return BadRequest(new { message = "La contraseña debe tener mínimo 8 caracteres, al menos 1 número y 1 símbolo" });
        }

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(new { message = "El email ya está registrado" });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = PasswordHasher.HashPassword(dto.Password),
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth,
            Phone = dto.Phone,
            Location = dto.Location,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return Ok(new
        {
            token,
            user = new
            {
                id = user.Id,
                email = user.Email,
                fullName = user.FullName,
                dateOfBirth = user.DateOfBirth,
                phone = user.Phone,
                location = user.Location
            }
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // Permitir login con email o teléfono
        var user = await _context.Users.FirstOrDefaultAsync(u => 
            u.Email == dto.Email || u.Phone == dto.Email);

        if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        var token = GenerateJwtToken(user);

        return Ok(new
        {
            token,
            user = new
            {
                id = user.Id,
                email = user.Email,
                fullName = user.FullName,
                dateOfBirth = user.DateOfBirth,
                phone = user.Phone,
                location = user.Location
            }
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var userId = Guid.Parse(userIdString);
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = user.Id,
            email = user.Email,
            fullName = user.FullName,
            dateOfBirth = user.DateOfBirth,
            phone = user.Phone,
            location = user.Location
        });
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "RuralTechSecretKeyForJWTTokenGeneration2024");
        var issuer = _configuration["Jwt:Issuer"] ?? "RuralTech";
        var audience = _configuration["Jwt:Audience"] ?? "RuralTechUsers";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("colaborador/login")]
    [AllowAnonymous]
    public async Task<IActionResult> ColaboradorLogin([FromBody] ColaboradorLoginDto dto)
    {
        // Validar PIN
        if (string.IsNullOrWhiteSpace(dto.Pin) || !Regex.IsMatch(dto.Pin, @"^\d{4,6}$"))
        {
            return BadRequest(new { message = "El PIN debe ser numérico y tener entre 4 y 6 dígitos" });
        }

        var colaborador = await _context.Colaboradores
            .Include(c => c.UPP)
            .FirstOrDefaultAsync(c => c.UPPId == dto.UPPId);

        if (colaborador == null)
        {
            return Unauthorized(new { message = "UPP o PIN incorrecto" });
        }

        // Verificar estatus
        if (colaborador.Estatus != EstatusColaborador.ACTIVO)
        {
            return Unauthorized(new { message = "El colaborador está suspendido" });
        }

        // Verificar PIN
        if (!PasswordHasher.VerifyPassword(dto.Pin, colaborador.PinAccesoHash))
        {
            return Unauthorized(new { message = "UPP o PIN incorrecto" });
        }

        // Generar token JWT para colaborador
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "RuralTechSecretKeyForJWTTokenGeneration2024");
        var issuer = _configuration["Jwt:Issuer"] ?? "RuralTech";
        var audience = _configuration["Jwt:Audience"] ?? "RuralTechUsers";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, colaborador.Id.ToString()),
            new Claim(ClaimTypes.Name, colaborador.NombreAlias),
            new Claim("UPPId", colaborador.UPPId.ToString()),
            new Claim("Rol", colaborador.Rol.ToString()),
            new Claim("TipoUsuario", "Colaborador")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new 
        { 
            token = tokenString, 
            colaborador = new 
            { 
                id = colaborador.Id, 
                nombreAlias = colaborador.NombreAlias, 
                rol = colaborador.Rol.ToString(),
                uppId = colaborador.UPPId,
                nombreUPP = colaborador.UPP.NombrePredio
            } 
        });
    }
}
