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
public class LotesAvesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LotesAvesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("upp/{uppId}")]
    public async Task<IActionResult> GetLotesAvesByUPP(Guid uppId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == uppId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        var lotes = await _context.LotesAves
            .Where(l => l.UPPId == uppId)
            .Include(l => l.Infraestructura)
            .Select(l => new LoteAvesDto
            {
                Id = l.Id,
                UPPId = l.UPPId,
                InfraestructuraId = l.InfraestructuraId,
                InfraestructuraNombre = l.Infraestructura.Nombre,
                CodigoLote = l.CodigoLote,
                NumeroLoteGranja = l.NumeroLoteGranja,
                FechaIngreso = l.FechaIngreso,
                CantidadInicial = l.CantidadInicial,
                CantidadActual = l.CantidadActual,
                MortalidadTotal = l.CantidadInicial - l.CantidadActual,
                MortalidadPorcentaje = l.CantidadInicial > 0 
                    ? (decimal?)((l.CantidadInicial - l.CantidadActual) * 100.0 / l.CantidadInicial) 
                    : 0,
                Raza = l.Raza,
                TipoAve = l.TipoAve,
                EdadDias = l.EdadDias,
                EdadActualDias = (int?)((DateTime.UtcNow - l.FechaIngreso).TotalDays) + l.EdadDias,
                Estatus = l.Estatus,
                EstatusNombre = l.Estatus.ToString(),
                FechaFinalizacion = l.FechaFinalizacion,
                PesoPromedioActual = l.RegistrosPeso
                    .OrderByDescending(r => r.Fecha)
                    .Select(r => (decimal?)r.PesoPromedio)
                    .FirstOrDefault(),
                TotalVacunaciones = l.Vacunaciones.Count,
                TotalTratamientos = l.Tratamientos.Count,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            })
            .ToListAsync();

        return Ok(lotes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoteAves(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var lote = await _context.LotesAves
            .Include(l => l.UPP)
            .Include(l => l.Infraestructura)
            .Where(l => l.Id == id && l.UPP.OwnerId == userId)
            .Select(l => new LoteAvesDto
            {
                Id = l.Id,
                UPPId = l.UPPId,
                InfraestructuraId = l.InfraestructuraId,
                InfraestructuraNombre = l.Infraestructura.Nombre,
                CodigoLote = l.CodigoLote,
                NumeroLoteGranja = l.NumeroLoteGranja,
                FechaIngreso = l.FechaIngreso,
                CantidadInicial = l.CantidadInicial,
                CantidadActual = l.CantidadActual,
                MortalidadTotal = l.CantidadInicial - l.CantidadActual,
                MortalidadPorcentaje = l.CantidadInicial > 0 
                    ? (decimal?)((l.CantidadInicial - l.CantidadActual) * 100.0 / l.CantidadInicial) 
                    : 0,
                Raza = l.Raza,
                TipoAve = l.TipoAve,
                EdadDias = l.EdadDias,
                EdadActualDias = (int?)((DateTime.UtcNow - l.FechaIngreso).TotalDays) + l.EdadDias,
                Estatus = l.Estatus,
                EstatusNombre = l.Estatus.ToString(),
                FechaFinalizacion = l.FechaFinalizacion,
                PesoPromedioActual = l.RegistrosPeso
                    .OrderByDescending(r => r.Fecha)
                    .Select(r => (decimal?)r.PesoPromedio)
                    .FirstOrDefault(),
                TotalVacunaciones = l.Vacunaciones.Count,
                TotalTratamientos = l.Tratamientos.Count,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (lote == null)
        {
            return NotFound(new { message = "Lote de aves no encontrado" });
        }

        return Ok(lote);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoteAves([FromBody] CreateLoteAvesDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Validar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == dto.UPPId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        // Validar que la infraestructura existe, pertenece a la UPP y es NAVE_AVICOLA
        var infraestructura = await _context.Infraestructuras
            .FirstOrDefaultAsync(i => i.Id == dto.InfraestructuraId && i.UPPId == dto.UPPId);

        if (infraestructura == null)
        {
            return BadRequest(new { message = "La infraestructura especificada no existe o no pertenece a esta UPP" });
        }

        if (infraestructura.TipoInstalacion != TipoInstalacion.NAVE_AVICOLA)
        {
            return BadRequest(new { message = "La infraestructura debe ser de tipo NAVE_AVICOLA" });
        }

        // Validar que el código de lote sea único dentro de la UPP
        if (await _context.LotesAves
            .AnyAsync(l => l.UPPId == dto.UPPId && l.CodigoLote.ToLower() == dto.CodigoLote.ToLower()))
        {
            return BadRequest(new { message = "Ya existe un lote con ese código en esta UPP" });
        }

        // Validar fecha de ingreso no sea futura
        if (dto.FechaIngreso > DateTime.UtcNow)
        {
            return BadRequest(new { message = "La fecha de ingreso no puede ser futura" });
        }

        var lote = new LoteAves
        {
            Id = Guid.NewGuid(),
            UPPId = dto.UPPId,
            InfraestructuraId = dto.InfraestructuraId,
            CodigoLote = dto.CodigoLote.Trim().ToUpper(),
            NumeroLoteGranja = dto.NumeroLoteGranja?.Trim(),
            FechaIngreso = dto.FechaIngreso,
            CantidadInicial = dto.CantidadInicial,
            CantidadActual = dto.CantidadInicial, // Inicialmente igual a la cantidad inicial
            Raza = dto.Raza.Trim(),
            TipoAve = dto.TipoAve?.Trim(),
            EdadDias = dto.EdadDias,
            Estatus = dto.Estatus,
            CreatedAt = DateTime.UtcNow
        };

        _context.LotesAves.Add(lote);
        await _context.SaveChangesAsync();

        // Cargar relaciones para respuesta
        await _context.Entry(lote)
            .Reference(l => l.Infraestructura).LoadAsync();

        return Ok(new LoteAvesDto
        {
            Id = lote.Id,
            UPPId = lote.UPPId,
            InfraestructuraId = lote.InfraestructuraId,
            InfraestructuraNombre = lote.Infraestructura.Nombre,
            CodigoLote = lote.CodigoLote,
            NumeroLoteGranja = lote.NumeroLoteGranja,
            FechaIngreso = lote.FechaIngreso,
            CantidadInicial = lote.CantidadInicial,
            CantidadActual = lote.CantidadActual,
            MortalidadTotal = 0,
            MortalidadPorcentaje = 0,
            Raza = lote.Raza,
            TipoAve = lote.TipoAve,
            EdadDias = lote.EdadDias,
            EdadActualDias = lote.EdadDias,
            Estatus = lote.Estatus,
            EstatusNombre = lote.Estatus.ToString(),
            CreatedAt = lote.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLoteAves(Guid id, [FromBody] CreateLoteAvesDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var lote = await _context.LotesAves
            .Include(l => l.UPP)
            .FirstOrDefaultAsync(l => l.Id == id && l.UPP.OwnerId == userId);

        if (lote == null)
        {
            return NotFound(new { message = "Lote de aves no encontrado" });
        }

        // Validar que la infraestructura existe, pertenece a la UPP y es NAVE_AVICOLA
        var infraestructura = await _context.Infraestructuras
            .FirstOrDefaultAsync(i => i.Id == dto.InfraestructuraId && i.UPPId == dto.UPPId);

        if (infraestructura == null)
        {
            return BadRequest(new { message = "La infraestructura especificada no existe o no pertenece a esta UPP" });
        }

        if (infraestructura.TipoInstalacion != TipoInstalacion.NAVE_AVICOLA)
        {
            return BadRequest(new { message = "La infraestructura debe ser de tipo NAVE_AVICOLA" });
        }

        // Validar que el código de lote sea único (excepto el actual)
        if (await _context.LotesAves
            .AnyAsync(l => l.UPPId == dto.UPPId && 
                          l.CodigoLote.ToLower() == dto.CodigoLote.ToLower() && 
                          l.Id != id))
        {
            return BadRequest(new { message = "Ya existe un lote con ese código en esta UPP" });
        }

        // Validar fecha de ingreso no sea futura
        if (dto.FechaIngreso > DateTime.UtcNow)
        {
            return BadRequest(new { message = "La fecha de ingreso no puede ser futura" });
        }

        // Actualizar campos
        lote.UPPId = dto.UPPId;
        lote.InfraestructuraId = dto.InfraestructuraId;
        lote.CodigoLote = dto.CodigoLote.Trim().ToUpper();
        lote.NumeroLoteGranja = dto.NumeroLoteGranja?.Trim();
        lote.FechaIngreso = dto.FechaIngreso;
        lote.CantidadInicial = dto.CantidadInicial;
        // CantidadActual no se actualiza directamente, se actualiza con registros de mortalidad
        lote.Raza = dto.Raza.Trim();
        lote.TipoAve = dto.TipoAve?.Trim();
        lote.EdadDias = dto.EdadDias;
        lote.Estatus = dto.Estatus;
        if (dto.Estatus == EstatusLoteAves.FINALIZADO && !lote.FechaFinalizacion.HasValue)
        {
            lote.FechaFinalizacion = DateTime.UtcNow;
        }
        lote.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Cargar relaciones para respuesta
        await _context.Entry(lote)
            .Reference(l => l.Infraestructura).LoadAsync();

        return Ok(new LoteAvesDto
        {
            Id = lote.Id,
            UPPId = lote.UPPId,
            InfraestructuraId = lote.InfraestructuraId,
            InfraestructuraNombre = lote.Infraestructura.Nombre,
            CodigoLote = lote.CodigoLote,
            NumeroLoteGranja = lote.NumeroLoteGranja,
            FechaIngreso = lote.FechaIngreso,
            CantidadInicial = lote.CantidadInicial,
            CantidadActual = lote.CantidadActual,
            MortalidadTotal = lote.CantidadInicial - lote.CantidadActual,
            MortalidadPorcentaje = lote.CantidadInicial > 0 
                ? (decimal?)((lote.CantidadInicial - lote.CantidadActual) * 100.0 / lote.CantidadInicial) 
                : 0,
            Raza = lote.Raza,
            TipoAve = lote.TipoAve,
            EdadDias = lote.EdadDias,
            EdadActualDias = (int?)((DateTime.UtcNow - lote.FechaIngreso).TotalDays) + lote.EdadDias,
            Estatus = lote.Estatus,
            EstatusNombre = lote.Estatus.ToString(),
            FechaFinalizacion = lote.FechaFinalizacion,
            TotalVacunaciones = lote.Vacunaciones.Count,
            TotalTratamientos = lote.Tratamientos.Count,
            CreatedAt = lote.CreatedAt,
            UpdatedAt = lote.UpdatedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLoteAves(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var lote = await _context.LotesAves
            .Include(l => l.UPP)
            .FirstOrDefaultAsync(l => l.Id == id && l.UPP.OwnerId == userId);

        if (lote == null)
        {
            return NotFound(new { message = "Lote de aves no encontrado" });
        }

        // Verificar que no tenga registros históricos importantes
        // (Los registros se eliminan en cascada, pero podemos validar si es necesario)

        _context.LotesAves.Remove(lote);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
