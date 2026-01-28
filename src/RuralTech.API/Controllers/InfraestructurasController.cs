using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuralTech.Core.DTOs;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using System.Security.Claims;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InfraestructurasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InfraestructurasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("upp/{uppId}")]
    public async Task<IActionResult> GetInfraestructurasByUPP(Guid uppId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == uppId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        var infraestructuras = await _context.Infraestructuras
            .Where(i => i.UPPId == uppId)
            .Select(i => new InfraestructuraDto
            {
                Id = i.Id,
                UPPId = i.UPPId,
                Nombre = i.Nombre,
                TipoInstalacion = i.TipoInstalacion,
                TipoInstalacionNombre = i.TipoInstalacion.ToString(),
                CapacidadMaxima = i.CapacidadMaxima,
                SuperficieHectareas = i.SuperficieHectareas,
                Estatus = i.Estatus,
                EstatusNombre = i.Estatus.ToString(),
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
                AnimalesActuales = i.Animals.Count
            })
            .ToListAsync();

        return Ok(infraestructuras);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfraestructura(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var infraestructura = await _context.Infraestructuras
            .Include(i => i.UPP)
            .Where(i => i.Id == id && i.UPP.OwnerId == userId)
            .Select(i => new InfraestructuraDto
            {
                Id = i.Id,
                UPPId = i.UPPId,
                Nombre = i.Nombre,
                TipoInstalacion = i.TipoInstalacion,
                TipoInstalacionNombre = i.TipoInstalacion.ToString(),
                CapacidadMaxima = i.CapacidadMaxima,
                SuperficieHectareas = i.SuperficieHectareas,
                Estatus = i.Estatus,
                EstatusNombre = i.Estatus.ToString(),
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
                AnimalesActuales = i.Animals.Count
            })
            .FirstOrDefaultAsync();

        if (infraestructura == null)
        {
            return NotFound(new { message = "Infraestructura no encontrada" });
        }

        return Ok(infraestructura);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInfraestructura([FromBody] CreateInfraestructuraDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Validar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == dto.UPPId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        // Validar que el nombre no esté duplicado dentro de la misma UPP
        if (await _context.Infraestructuras
            .AnyAsync(i => i.UPPId == dto.UPPId && i.Nombre.ToLower() == dto.Nombre.ToLower()))
        {
            return BadRequest(new { message = "Ya existe una infraestructura con ese nombre en esta UPP" });
        }

        // Validar que si es POTRERO, tenga superficie
        if (dto.TipoInstalacion == TipoInstalacion.POTRERO && !dto.SuperficieHectareas.HasValue)
        {
            return BadRequest(new { message = "Los potreros deben tener superficie en hectáreas especificada" });
        }

        // Validar que si no es POTRERO, no tenga superficie
        if (dto.TipoInstalacion != TipoInstalacion.POTRERO && dto.SuperficieHectareas.HasValue)
        {
            return BadRequest(new { message = "Solo los potreros pueden tener superficie en hectáreas" });
        }

        var infraestructura = new Infraestructura
        {
            Id = Guid.NewGuid(),
            UPPId = dto.UPPId,
            Nombre = dto.Nombre.Trim(),
            TipoInstalacion = dto.TipoInstalacion,
            CapacidadMaxima = dto.CapacidadMaxima,
            SuperficieHectareas = dto.SuperficieHectareas,
            Estatus = dto.Estatus,
            CreatedAt = DateTime.UtcNow
        };

        _context.Infraestructuras.Add(infraestructura);
        await _context.SaveChangesAsync();

        return Ok(new InfraestructuraDto
        {
            Id = infraestructura.Id,
            UPPId = infraestructura.UPPId,
            Nombre = infraestructura.Nombre,
            TipoInstalacion = infraestructura.TipoInstalacion,
            TipoInstalacionNombre = infraestructura.TipoInstalacion.ToString(),
            CapacidadMaxima = infraestructura.CapacidadMaxima,
            SuperficieHectareas = infraestructura.SuperficieHectareas,
            Estatus = infraestructura.Estatus,
            EstatusNombre = infraestructura.Estatus.ToString(),
            CreatedAt = infraestructura.CreatedAt,
            AnimalesActuales = 0
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInfraestructura(Guid id, [FromBody] CreateInfraestructuraDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var infraestructura = await _context.Infraestructuras
            .Include(i => i.UPP)
            .FirstOrDefaultAsync(i => i.Id == id && i.UPP.OwnerId == userId);

        if (infraestructura == null)
        {
            return NotFound(new { message = "Infraestructura no encontrada" });
        }

        // Validar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == dto.UPPId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        // Validar que el nombre no esté duplicado (excepto la actual)
        if (await _context.Infraestructuras
            .AnyAsync(i => i.UPPId == dto.UPPId && 
                          i.Nombre.ToLower() == dto.Nombre.ToLower() && 
                          i.Id != id))
        {
            return BadRequest(new { message = "Ya existe una infraestructura con ese nombre en esta UPP" });
        }

        // Validar superficie según tipo
        if (dto.TipoInstalacion == TipoInstalacion.POTRERO && !dto.SuperficieHectareas.HasValue)
        {
            return BadRequest(new { message = "Los potreros deben tener superficie en hectáreas especificada" });
        }

        if (dto.TipoInstalacion != TipoInstalacion.POTRERO && dto.SuperficieHectareas.HasValue)
        {
            return BadRequest(new { message = "Solo los potreros pueden tener superficie en hectáreas" });
        }

        infraestructura.UPPId = dto.UPPId;
        infraestructura.Nombre = dto.Nombre.Trim();
        infraestructura.TipoInstalacion = dto.TipoInstalacion;
        infraestructura.CapacidadMaxima = dto.CapacidadMaxima;
        infraestructura.SuperficieHectareas = dto.SuperficieHectareas;
        infraestructura.Estatus = dto.Estatus;
        infraestructura.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new InfraestructuraDto
        {
            Id = infraestructura.Id,
            UPPId = infraestructura.UPPId,
            Nombre = infraestructura.Nombre,
            TipoInstalacion = infraestructura.TipoInstalacion,
            TipoInstalacionNombre = infraestructura.TipoInstalacion.ToString(),
            CapacidadMaxima = infraestructura.CapacidadMaxima,
            SuperficieHectareas = infraestructura.SuperficieHectareas,
            Estatus = infraestructura.Estatus,
            EstatusNombre = infraestructura.Estatus.ToString(),
            CreatedAt = infraestructura.CreatedAt,
            UpdatedAt = infraestructura.UpdatedAt,
            AnimalesActuales = infraestructura.Animals.Count
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInfraestructura(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var infraestructura = await _context.Infraestructuras
            .Include(i => i.UPP)
            .FirstOrDefaultAsync(i => i.Id == id && i.UPP.OwnerId == userId);

        if (infraestructura == null)
        {
            return NotFound(new { message = "Infraestructura no encontrada" });
        }

        // Verificar que no tenga animales asociados
        if (infraestructura.Animals.Any())
        {
            return BadRequest(new { message = "No se puede eliminar una infraestructura que tiene animales asociados. Primero mueve los animales a otra ubicación." });
        }

        _context.Infraestructuras.Remove(infraestructura);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
