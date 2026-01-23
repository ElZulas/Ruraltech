using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuralTech.Core.DTOs;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UPPsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UPPsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUPPs()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var upps = await _context.UPPs
            .Where(u => u.OwnerId == userId)
            .Select(u => new UPPDto
            {
                Id = u.Id,
                OwnerId = u.OwnerId,
                ClavePGN = u.ClavePGN,
                NombrePredio = u.NombrePredio,
                PropietarioLegal = u.PropietarioLegal,
                CodigoQRAcceso = u.CodigoQRAcceso,
                EstadoMX = u.EstadoMX,
                Latitude = u.Latitude,
                Longitude = u.Longitude,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                AnimalCount = u.Animals.Count
            })
            .ToListAsync();

        return Ok(upps);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUPP(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var upp = await _context.UPPs
            .Where(u => u.Id == id && u.OwnerId == userId)
            .Select(u => new UPPDto
            {
                Id = u.Id,
                OwnerId = u.OwnerId,
                ClavePGN = u.ClavePGN,
                NombrePredio = u.NombrePredio,
                PropietarioLegal = u.PropietarioLegal,
                CodigoQRAcceso = u.CodigoQRAcceso,
                EstadoMX = u.EstadoMX,
                Latitude = u.Latitude,
                Longitude = u.Longitude,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                AnimalCount = u.Animals.Count
            })
            .FirstOrDefaultAsync();

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada" });
        }

        return Ok(upp);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUPP([FromBody] CreateUPPDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Validar ClavePGN: alfanumérico, sin espacios
        if (string.IsNullOrWhiteSpace(dto.ClavePGN) || 
            !Regex.IsMatch(dto.ClavePGN, @"^[A-Za-z0-9]+$") ||
            dto.ClavePGN.Contains(" "))
        {
            return BadRequest(new { message = "La Clave PGN debe ser alfanumérica y no contener espacios" });
        }

        if (dto.ClavePGN.Length > 20)
        {
            return BadRequest(new { message = "La Clave PGN no puede exceder 20 caracteres" });
        }

        // Validar EstadoMX: debe ser exactamente 2 caracteres
        if (string.IsNullOrWhiteSpace(dto.EstadoMX) || dto.EstadoMX.Length != 2)
        {
            return BadRequest(new { message = "El estado debe ser una clave INEGI de 2 caracteres" });
        }

        // Validar que la ClavePGN sea única
        if (await _context.UPPs.AnyAsync(u => u.ClavePGN == dto.ClavePGN))
        {
            return BadRequest(new { message = "La Clave PGN ya está registrada" });
        }

        // Generar código QR de acceso único
        string codigoQR;
        do
        {
            codigoQR = $"RANCH-{RandomString(6).ToUpper()}";
        } while (await _context.UPPs.AnyAsync(u => u.CodigoQRAcceso == codigoQR));

        var upp = new UPP
        {
            Id = Guid.NewGuid(),
            OwnerId = userId,
            ClavePGN = dto.ClavePGN.ToUpper(),
            NombrePredio = dto.NombrePredio,
            PropietarioLegal = dto.PropietarioLegal,
            CodigoQRAcceso = codigoQR,
            EstadoMX = dto.EstadoMX,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            CreatedAt = DateTime.UtcNow
        };

        _context.UPPs.Add(upp);
        await _context.SaveChangesAsync();

        return Ok(new UPPDto
        {
            Id = upp.Id,
            OwnerId = upp.OwnerId,
            ClavePGN = upp.ClavePGN,
            NombrePredio = upp.NombrePredio,
            PropietarioLegal = upp.PropietarioLegal,
            CodigoQRAcceso = upp.CodigoQRAcceso,
            EstadoMX = upp.EstadoMX,
            Latitude = upp.Latitude,
            Longitude = upp.Longitude,
            CreatedAt = upp.CreatedAt,
            AnimalCount = 0
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUPP(Guid id, [FromBody] CreateUPPDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == id && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada" });
        }

        // Validar ClavePGN si cambió
        if (upp.ClavePGN != dto.ClavePGN.ToUpper())
        {
            if (string.IsNullOrWhiteSpace(dto.ClavePGN) || 
                !Regex.IsMatch(dto.ClavePGN, @"^[A-Za-z0-9]+$") ||
                dto.ClavePGN.Contains(" "))
            {
                return BadRequest(new { message = "La Clave PGN debe ser alfanumérica y no contener espacios" });
            }

            if (await _context.UPPs.AnyAsync(u => u.ClavePGN == dto.ClavePGN.ToUpper() && u.Id != id))
            {
                return BadRequest(new { message = "La Clave PGN ya está registrada" });
            }

            upp.ClavePGN = dto.ClavePGN.ToUpper();
        }

        // Validar EstadoMX
        if (string.IsNullOrWhiteSpace(dto.EstadoMX) || dto.EstadoMX.Length != 2)
        {
            return BadRequest(new { message = "El estado debe ser una clave INEGI de 2 caracteres" });
        }

        upp.NombrePredio = dto.NombrePredio;
        upp.PropietarioLegal = dto.PropietarioLegal;
        upp.EstadoMX = dto.EstadoMX;
        upp.Latitude = dto.Latitude;
        upp.Longitude = dto.Longitude;
        upp.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new UPPDto
        {
            Id = upp.Id,
            OwnerId = upp.OwnerId,
            ClavePGN = upp.ClavePGN,
            NombrePredio = upp.NombrePredio,
            PropietarioLegal = upp.PropietarioLegal,
            CodigoQRAcceso = upp.CodigoQRAcceso,
            EstadoMX = upp.EstadoMX,
            Latitude = upp.Latitude,
            Longitude = upp.Longitude,
            CreatedAt = upp.CreatedAt,
            UpdatedAt = upp.UpdatedAt,
            AnimalCount = upp.Animals.Count
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUPP(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == id && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada" });
        }

        // Verificar que no tenga animales asociados
        if (upp.Animals.Any())
        {
            return BadRequest(new { message = "No se puede eliminar una UPP que tiene animales asociados" });
        }

        _context.UPPs.Remove(upp);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
