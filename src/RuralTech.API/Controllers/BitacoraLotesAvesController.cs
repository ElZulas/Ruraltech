using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuralTech.Core.DTOs;
using RuralTech.Infrastructure.Data;
using System.Security.Claims;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BitacoraLotesAvesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BitacoraLotesAvesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("lote/{loteAvesId}")]
    public async Task<IActionResult> GetBitacorasByLote(Guid loteAvesId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que el lote pertenezca a una UPP del usuario
        var lote = await _context.LotesAves
            .Include(l => l.UPP)
            .FirstOrDefaultAsync(l => l.Id == loteAvesId && l.UPP.OwnerId == userId);

        if (lote == null)
        {
            return NotFound(new { message = "Lote no encontrado o no tienes acceso a él" });
        }

        var bitacoras = await _context.BitacorasLoteAve
            .Where(b => b.LoteAvesId == loteAvesId)
            .OrderByDescending(b => b.FechaRegistro)
            .ThenByDescending(b => b.CreatedAt)
            .Select(b => new BitacoraLoteAveDto
            {
                Id = b.Id,
                LoteAvesId = b.LoteAvesId,
                LoteCodigo = b.LoteAves.CodigoLote,
                FechaRegistro = b.FechaRegistro,
                DiaCiclo = b.DiaCiclo,
                Mortalidad = b.Mortalidad,
                Descarte = b.Descarte,
                ConsumoKg = b.ConsumoKg,
                PesoPromedio = b.PesoPromedio,
                Observaciones = b.Observaciones,
                CreatedAt = b.CreatedAt,
                ConversionAlimenticia = b.PesoPromedio.HasValue && b.LoteAves.CantidadActual > 0
                    ? b.ConsumoKg / (b.LoteAves.CantidadActual * b.PesoPromedio.Value)
                    : null
            })
            .ToListAsync();

        return Ok(bitacoras);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBitacoraLoteAve(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var bitacora = await _context.BitacorasLoteAve
            .Include(b => b.LoteAves)
                .ThenInclude(l => l.UPP)
            .Where(b => b.Id == id && b.LoteAves.UPP.OwnerId == userId)
            .Select(b => new BitacoraLoteAveDto
            {
                Id = b.Id,
                LoteAvesId = b.LoteAvesId,
                LoteCodigo = b.LoteAves.CodigoLote,
                FechaRegistro = b.FechaRegistro,
                DiaCiclo = b.DiaCiclo,
                Mortalidad = b.Mortalidad,
                Descarte = b.Descarte,
                ConsumoKg = b.ConsumoKg,
                PesoPromedio = b.PesoPromedio,
                Observaciones = b.Observaciones,
                CreatedAt = b.CreatedAt,
                ConversionAlimenticia = b.PesoPromedio.HasValue && b.LoteAves.CantidadActual > 0
                    ? b.ConsumoKg / (b.LoteAves.CantidadActual * b.PesoPromedio.Value)
                    : null
            })
            .FirstOrDefaultAsync();

        if (bitacora == null)
        {
            return NotFound(new { message = "Bitácora no encontrada" });
        }

        return Ok(bitacora);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBitacoraLoteAve([FromBody] CreateBitacoraLoteAveDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Validar que el lote existe y pertenece a una UPP del usuario
        var lote = await _context.LotesAves
            .Include(l => l.UPP)
            .FirstOrDefaultAsync(l => l.Id == dto.LoteAvesId && l.UPP.OwnerId == userId);

        if (lote == null)
        {
            return NotFound(new { message = "Lote no encontrado o no tienes acceso a él" });
        }

        // Validar que el lote esté activo
        if (lote.Estatus != Core.Entities.EstatusLoteAves.ACTIVO)
        {
            return BadRequest(new { message = "Solo se pueden registrar bitácoras para lotes activos" });
        }

        // Validar que fecha del registro no sea futura
        if (dto.FechaRegistro.Date > DateTime.UtcNow.Date)
        {
            return BadRequest(new { message = "La fecha del registro no puede ser futura" });
        }

        // Validar que no exista ya un registro para este lote en esta fecha (unique constraint)
        var existeRegistro = await _context.BitacorasLoteAve
            .AnyAsync(b => b.LoteAvesId == dto.LoteAvesId && b.FechaRegistro.Date == dto.FechaRegistro.Date);

        if (existeRegistro)
        {
            return BadRequest(new { message = "Ya existe un registro de bitácora para este lote en esta fecha" });
        }

        // Calcular día del ciclo: fecha_registro - fecha_ingreso_lote
        var diaCiclo = (dto.FechaRegistro.Date - lote.FechaIngreso.Date).Days;
        if (diaCiclo < 0)
        {
            return BadRequest(new { message = "La fecha del registro no puede ser anterior a la fecha de ingreso del lote" });
        }

        // Validar que la cantidad de mortalidad + descarte no exceda la cantidad actual
        var totalPerdidas = dto.Mortalidad + dto.Descarte;
        if (totalPerdidas > lote.CantidadActual)
        {
            return BadRequest(new { 
                message = $"La suma de mortalidad ({dto.Mortalidad}) y descarte ({dto.Descarte}) no puede exceder la cantidad actual del lote ({lote.CantidadActual})" 
            });
        }

        // Crear el registro de bitácora
        var bitacora = new Core.Entities.BitacoraLoteAve
        {
            Id = Guid.NewGuid(),
            LoteAvesId = dto.LoteAvesId,
            FechaRegistro = dto.FechaRegistro.Date,
            DiaCiclo = diaCiclo,
            Mortalidad = dto.Mortalidad,
            Descarte = dto.Descarte,
            ConsumoKg = dto.ConsumoKg,
            PesoPromedio = dto.PesoPromedio,
            Observaciones = dto.Observaciones?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        // Actualizar CantidadActual del lote: restar mortalidad y descarte
        lote.CantidadActual -= totalPerdidas;
        lote.UpdatedAt = DateTime.UtcNow;

        // Validar que la cantidad actual no sea negativa (double check)
        if (lote.CantidadActual < 0)
        {
            return BadRequest(new { message = "La cantidad actual del lote no puede ser negativa" });
        }

        _context.BitacorasLoteAve.Add(bitacora);
        await _context.SaveChangesAsync();

        // Cargar relación para respuesta
        await _context.Entry(bitacora)
            .Reference(b => b.LoteAves).LoadAsync();

        return Ok(new BitacoraLoteAveDto
        {
            Id = bitacora.Id,
            LoteAvesId = bitacora.LoteAvesId,
            LoteCodigo = bitacora.LoteAves.CodigoLote,
            FechaRegistro = bitacora.FechaRegistro,
            DiaCiclo = bitacora.DiaCiclo,
            Mortalidad = bitacora.Mortalidad,
            Descarte = bitacora.Descarte,
            ConsumoKg = bitacora.ConsumoKg,
            PesoPromedio = bitacora.PesoPromedio,
            Observaciones = bitacora.Observaciones,
            CreatedAt = bitacora.CreatedAt,
            ConversionAlimenticia = bitacora.PesoPromedio.HasValue && lote.CantidadActual > 0
                ? bitacora.ConsumoKg / (lote.CantidadActual * bitacora.PesoPromedio.Value)
                : null
        });
    }

    // NOTA: Los registros de bitácora son inmutables una vez creados
    // No se permiten actualizaciones ni eliminaciones para mantener la integridad histórica
}
