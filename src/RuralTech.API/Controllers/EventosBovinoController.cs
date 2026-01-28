using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuralTech.Core.DTOs;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using System.Security.Claims;
using System.Text.Json;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventosBovinoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JsonSerializerOptions _jsonOptions;

    public EventosBovinoController(ApplicationDbContext context)
    {
        _context = context;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    [HttpGet("bovino/{bovinoId}")]
    public async Task<IActionResult> GetEventosByBovino(Guid bovinoId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que el bovino pertenezca a una UPP del usuario
        var bovino = await _context.Bovinos
            .Include(b => b.UPP)
            .FirstOrDefaultAsync(b => b.Id == bovinoId && b.UPP.OwnerId == userId);

        if (bovino == null)
        {
            return NotFound(new { message = "Bovino no encontrado o no tienes acceso a él" });
        }

        var eventos = await _context.EventosBovino
            .Where(e => e.BovinoId == bovinoId)
            .Include(e => e.RegistradoPorUser)
            .Include(e => e.RegistradoPorColaborador)
            .OrderByDescending(e => e.FechaEvento)
            .ThenByDescending(e => e.CreatedAt)
            .Select(e => new EventoBovinoDto
            {
                Id = e.Id,
                BovinoId = e.BovinoId,
                BovinoNombre = e.Bovino.Nombre,
                BovinoAreteTrabajo = e.Bovino.AreteTrabajo,
                TipoEvento = e.TipoEvento,
                TipoEventoNombre = e.TipoEvento.ToString(),
                FechaEvento = e.FechaEvento,
                DetallesJson = JsonSerializer.Deserialize<Dictionary<string, object>>(e.DetallesJson) ?? new(),
                CostoAsociado = e.CostoAsociado,
                RegistradoPorUserId = e.RegistradoPorUserId,
                RegistradoPorUserName = e.RegistradoPorUser != null ? e.RegistradoPorUser.FullName : null,
                RegistradoPorColaboradorId = e.RegistradoPorColaboradorId,
                RegistradoPorColaboradorNombre = e.RegistradoPorColaborador != null ? e.RegistradoPorColaborador.NombreAlias : null,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();

        return Ok(eventos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventoBovino(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var evento = await _context.EventosBovino
            .Include(e => e.Bovino)
                .ThenInclude(b => b.UPP)
            .Include(e => e.RegistradoPorUser)
            .Include(e => e.RegistradoPorColaborador)
            .Where(e => e.Id == id && e.Bovino.UPP.OwnerId == userId)
            .Select(e => new EventoBovinoDto
            {
                Id = e.Id,
                BovinoId = e.BovinoId,
                BovinoNombre = e.Bovino.Nombre,
                BovinoAreteTrabajo = e.Bovino.AreteTrabajo,
                TipoEvento = e.TipoEvento,
                TipoEventoNombre = e.TipoEvento.ToString(),
                FechaEvento = e.FechaEvento,
                DetallesJson = JsonSerializer.Deserialize<Dictionary<string, object>>(e.DetallesJson) ?? new(),
                CostoAsociado = e.CostoAsociado,
                RegistradoPorUserId = e.RegistradoPorUserId,
                RegistradoPorUserName = e.RegistradoPorUser != null ? e.RegistradoPorUser.FullName : null,
                RegistradoPorColaboradorId = e.RegistradoPorColaboradorId,
                RegistradoPorColaboradorNombre = e.RegistradoPorColaborador != null ? e.RegistradoPorColaborador.NombreAlias : null,
                CreatedAt = e.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (evento == null)
        {
            return NotFound(new { message = "Evento no encontrado" });
        }

        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEventoBovino([FromBody] CreateEventoBovinoDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Validar que el bovino existe y pertenece a una UPP del usuario
        var bovino = await _context.Bovinos
            .Include(b => b.UPP)
            .FirstOrDefaultAsync(b => b.Id == dto.BovinoId && b.UPP.OwnerId == userId);

        if (bovino == null)
        {
            return NotFound(new { message = "Bovino no encontrado o no tienes acceso a él" });
        }

        // Validar que fecha del evento no sea futura
        if (dto.FechaEvento > DateTime.UtcNow)
        {
            return BadRequest(new { message = "La fecha del evento no puede ser futura" });
        }

        // Validar que solo uno de los dos (RegistradoPorUserId o RegistradoPorColaboradorId) esté presente
        if (dto.RegistradoPorUserId.HasValue && dto.RegistradoPorColaboradorId.HasValue)
        {
            return BadRequest(new { message = "Solo se puede especificar un usuario o un colaborador, no ambos" });
        }

        // Validar que si se especifica RegistradoPorUserId, pertenezca al mismo propietario
        if (dto.RegistradoPorUserId.HasValue)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.RegistradoPorUserId.Value);

            if (user == null || user.Id != userId)
            {
                return BadRequest(new { message = "El usuario especificado no existe o no tienes acceso" });
            }
        }

        // Validar que si se especifica RegistradoPorColaboradorId, pertenezca a la misma UPP
        if (dto.RegistradoPorColaboradorId.HasValue)
        {
            var colaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.Id == dto.RegistradoPorColaboradorId.Value && c.UPPId == bovino.UPPId);

            if (colaborador == null)
            {
                return BadRequest(new { message = "El colaborador especificado no existe o no pertenece a esta UPP" });
            }
        }

        // Si no se especifica quién registró, usar el usuario actual
        if (!dto.RegistradoPorUserId.HasValue && !dto.RegistradoPorColaboradorId.HasValue)
        {
            dto.RegistradoPorUserId = userId;
        }

        // Serializar detalles a JSON
        string detallesJsonString;
        try
        {
            detallesJsonString = JsonSerializer.Serialize(dto.DetallesJson, _jsonOptions);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error al serializar los detalles JSON: {ex.Message}" });
        }

        var evento = new EventoBovino
        {
            Id = Guid.NewGuid(),
            BovinoId = dto.BovinoId,
            TipoEvento = dto.TipoEvento,
            FechaEvento = dto.FechaEvento,
            DetallesJson = detallesJsonString,
            CostoAsociado = dto.CostoAsociado,
            RegistradoPorUserId = dto.RegistradoPorUserId,
            RegistradoPorColaboradorId = dto.RegistradoPorColaboradorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.EventosBovino.Add(evento);
        await _context.SaveChangesAsync();

        // Cargar relaciones para respuesta
        await _context.Entry(evento)
            .Reference(e => e.Bovino).LoadAsync();
        await _context.Entry(evento)
            .Reference(e => e.RegistradoPorUser).LoadAsync();
        await _context.Entry(evento)
            .Reference(e => e.RegistradoPorColaborador).LoadAsync();

        return Ok(new EventoBovinoDto
        {
            Id = evento.Id,
            BovinoId = evento.BovinoId,
            BovinoNombre = evento.Bovino.Nombre,
            BovinoAreteTrabajo = evento.Bovino.AreteTrabajo,
            TipoEvento = evento.TipoEvento,
            TipoEventoNombre = evento.TipoEvento.ToString(),
            FechaEvento = evento.FechaEvento,
            DetallesJson = JsonSerializer.Deserialize<Dictionary<string, object>>(evento.DetallesJson) ?? new(),
            CostoAsociado = evento.CostoAsociado,
            RegistradoPorUserId = evento.RegistradoPorUserId,
            RegistradoPorUserName = evento.RegistradoPorUser != null ? evento.RegistradoPorUser.FullName : null,
            RegistradoPorColaboradorId = evento.RegistradoPorColaboradorId,
            RegistradoPorColaboradorNombre = evento.RegistradoPorColaborador != null ? evento.RegistradoPorColaborador.NombreAlias : null,
            CreatedAt = evento.CreatedAt
        });
    }

    // NOTA: Los eventos son INMUTABLES, por lo que no hay endpoints PUT o DELETE
    // Solo se pueden crear y leer
}
