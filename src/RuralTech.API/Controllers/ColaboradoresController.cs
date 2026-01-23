using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuralTech.Core.DTOs;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using RuralTech.Infrastructure.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ColaboradoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ColaboradoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("upp/{uppId}")]
    public async Task<IActionResult> GetColaboradoresByUPP(Guid uppId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == uppId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada" });
        }

        var colaboradores = await _context.Colaboradores
            .Where(c => c.UPPId == uppId)
            .Select(c => new ColaboradorDto
            {
                Id = c.Id,
                UPPId = c.UPPId,
                NombreUPP = c.UPP.NombrePredio,
                NombreAlias = c.NombreAlias,
                TelefonoContacto = c.TelefonoContacto,
                Rol = c.Rol.ToString(),
                Estatus = c.Estatus.ToString(),
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();

        return Ok(colaboradores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetColaborador(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var colaborador = await _context.Colaboradores
            .Include(c => c.UPP)
            .FirstOrDefaultAsync(c => c.Id == id && c.UPP.OwnerId == userId);

        if (colaborador == null)
        {
            return NotFound(new { message = "Colaborador no encontrado" });
        }

        return Ok(new ColaboradorDto
        {
            Id = colaborador.Id,
            UPPId = colaborador.UPPId,
            NombreUPP = colaborador.UPP.NombrePredio,
            NombreAlias = colaborador.NombreAlias,
            TelefonoContacto = colaborador.TelefonoContacto,
            Rol = colaborador.Rol.ToString(),
            Estatus = colaborador.Estatus.ToString(),
            CreatedAt = colaborador.CreatedAt,
            UpdatedAt = colaborador.UpdatedAt
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateColaborador([FromBody] CreateColaboradorDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == dto.UPPId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada" });
        }

        // Validar PIN: numérico de 4 a 6 dígitos
        if (string.IsNullOrWhiteSpace(dto.Pin) || 
            !Regex.IsMatch(dto.Pin, @"^\d{4,6}$"))
        {
            return BadRequest(new { message = "El PIN debe ser numérico y tener entre 4 y 6 dígitos" });
        }

        // Validar nombre alias
        if (string.IsNullOrWhiteSpace(dto.NombreAlias) || dto.NombreAlias.Length > 50)
        {
            return BadRequest(new { message = "El nombre alias es requerido y no puede exceder 50 caracteres" });
        }

        // Validar rol
        if (!Enum.TryParse<RolColaborador>(dto.Rol, true, out var rol))
        {
            return BadRequest(new { message = "Rol inválido. Valores permitidos: ENCARGADO, OPERARIO, VETERINARIO" });
        }

        // Encriptar PIN
        var pinHash = PasswordHasher.HashPassword(dto.Pin);

        var colaborador = new Colaborador
        {
            Id = Guid.NewGuid(),
            UPPId = dto.UPPId,
            NombreAlias = dto.NombreAlias,
            PinAccesoHash = pinHash,
            TelefonoContacto = dto.TelefonoContacto,
            Rol = rol,
            Estatus = EstatusColaborador.ACTIVO,
            CreatedAt = DateTime.UtcNow
        };

        _context.Colaboradores.Add(colaborador);
        await _context.SaveChangesAsync();

        return Ok(new ColaboradorDto
        {
            Id = colaborador.Id,
            UPPId = colaborador.UPPId,
            NombreUPP = upp.NombrePredio,
            NombreAlias = colaborador.NombreAlias,
            TelefonoContacto = colaborador.TelefonoContacto,
            Rol = colaborador.Rol.ToString(),
            Estatus = colaborador.Estatus.ToString(),
            CreatedAt = colaborador.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateColaborador(Guid id, [FromBody] CreateColaboradorDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var colaborador = await _context.Colaboradores
            .Include(c => c.UPP)
            .FirstOrDefaultAsync(c => c.Id == id && c.UPP.OwnerId == userId);

        if (colaborador == null)
        {
            return NotFound(new { message = "Colaborador no encontrado" });
        }

        // Validar PIN si se proporciona
        if (!string.IsNullOrWhiteSpace(dto.Pin))
        {
            if (!Regex.IsMatch(dto.Pin, @"^\d{4,6}$"))
            {
                return BadRequest(new { message = "El PIN debe ser numérico y tener entre 4 y 6 dígitos" });
            }
            colaborador.PinAccesoHash = PasswordHasher.HashPassword(dto.Pin);
        }

        // Validar nombre alias
        if (!string.IsNullOrWhiteSpace(dto.NombreAlias))
        {
            if (dto.NombreAlias.Length > 50)
            {
                return BadRequest(new { message = "El nombre alias no puede exceder 50 caracteres" });
            }
            colaborador.NombreAlias = dto.NombreAlias;
        }

        // Validar rol
        if (!string.IsNullOrWhiteSpace(dto.Rol))
        {
            if (!Enum.TryParse<RolColaborador>(dto.Rol, true, out var rol))
            {
                return BadRequest(new { message = "Rol inválido. Valores permitidos: ENCARGADO, OPERARIO, VETERINARIO" });
            }
            colaborador.Rol = rol;
        }

        colaborador.TelefonoContacto = dto.TelefonoContacto;
        colaborador.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new ColaboradorDto
        {
            Id = colaborador.Id,
            UPPId = colaborador.UPPId,
            NombreUPP = colaborador.UPP.NombrePredio,
            NombreAlias = colaborador.NombreAlias,
            TelefonoContacto = colaborador.TelefonoContacto,
            Rol = colaborador.Rol.ToString(),
            Estatus = colaborador.Estatus.ToString(),
            CreatedAt = colaborador.CreatedAt,
            UpdatedAt = colaborador.UpdatedAt
        });
    }

    [HttpPatch("{id}/estatus")]
    public async Task<IActionResult> UpdateEstatus(Guid id, [FromBody] string estatus)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var colaborador = await _context.Colaboradores
            .Include(c => c.UPP)
            .FirstOrDefaultAsync(c => c.Id == id && c.UPP.OwnerId == userId);

        if (colaborador == null)
        {
            return NotFound(new { message = "Colaborador no encontrado" });
        }

        if (!Enum.TryParse<EstatusColaborador>(estatus, true, out var nuevoEstatus))
        {
            return BadRequest(new { message = "Estatus inválido. Valores permitidos: ACTIVO, SUSPENDIDO" });
        }

        colaborador.Estatus = nuevoEstatus;
        colaborador.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new ColaboradorDto
        {
            Id = colaborador.Id,
            UPPId = colaborador.UPPId,
            NombreUPP = colaborador.UPP.NombrePredio,
            NombreAlias = colaborador.NombreAlias,
            TelefonoContacto = colaborador.TelefonoContacto,
            Rol = colaborador.Rol.ToString(),
            Estatus = colaborador.Estatus.ToString(),
            CreatedAt = colaborador.CreatedAt,
            UpdatedAt = colaborador.UpdatedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColaborador(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var colaborador = await _context.Colaboradores
            .Include(c => c.UPP)
            .FirstOrDefaultAsync(c => c.Id == id && c.UPP.OwnerId == userId);

        if (colaborador == null)
        {
            return NotFound(new { message = "Colaborador no encontrado" });
        }

        _context.Colaboradores.Remove(colaborador);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
