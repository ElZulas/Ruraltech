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
public class BovinosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BovinosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("upp/{uppId}")]
    public async Task<IActionResult> GetBovinosByUPP(Guid uppId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Verificar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == uppId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        var bovinos = await _context.Bovinos
            .Where(b => b.UPPId == uppId)
            .Include(b => b.Infraestructura)
            .Include(b => b.Madre)
            .Include(b => b.Padre)
            .Select(b => new BovinoDto
            {
                Id = b.Id,
                UPPId = b.UPPId,
                InfraestructuraId = b.InfraestructuraId.HasValue ? b.InfraestructuraId.ToString() : null,
                InfraestructuraNombre = b.Infraestructura != null ? b.Infraestructura.Nombre : null,
                AreteSiniiga = b.AreteSiniiga,
                AreteTrabajo = b.AreteTrabajo,
                Nombre = b.Nombre,
                FechaNacimiento = b.FechaNacimiento,
                EdadMeses = (int?)((DateTime.UtcNow - b.FechaNacimiento).TotalDays / 30),
                Sexo = b.Sexo,
                SexoNombre = b.Sexo == 'M' ? "Macho" : "Hembra",
                RazaPredominante = b.RazaPredominante,
                MadreId = b.MadreId,
                MadreNombre = b.Madre != null ? b.Madre.Nombre : null,
                PadreId = b.PadreId,
                PadreNombre = b.Padre != null ? b.Padre.Nombre : null,
                EstadoProductivo = b.EstadoProductivo,
                EstadoProductivoNombre = b.EstadoProductivo.ToString(),
                EstatusSistema = b.EstatusSistema,
                EstatusSistemaNombre = b.EstatusSistema.ToString(),
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync();

        return Ok(bovinos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBovino(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var bovino = await _context.Bovinos
            .Include(b => b.UPP)
            .Include(b => b.Infraestructura)
            .Include(b => b.Madre)
            .Include(b => b.Padre)
            .Where(b => b.Id == id && b.UPP.OwnerId == userId)
            .Select(b => new BovinoDto
            {
                Id = b.Id,
                UPPId = b.UPPId,
                InfraestructuraId = b.InfraestructuraId.HasValue ? b.InfraestructuraId.ToString() : null,
                InfraestructuraNombre = b.Infraestructura != null ? b.Infraestructura.Nombre : null,
                AreteSiniiga = b.AreteSiniiga,
                AreteTrabajo = b.AreteTrabajo,
                Nombre = b.Nombre,
                FechaNacimiento = b.FechaNacimiento,
                EdadMeses = (int?)((DateTime.UtcNow - b.FechaNacimiento).TotalDays / 30),
                Sexo = b.Sexo,
                SexoNombre = b.Sexo == 'M' ? "Macho" : "Hembra",
                RazaPredominante = b.RazaPredominante,
                MadreId = b.MadreId,
                MadreNombre = b.Madre != null ? b.Madre.Nombre : null,
                PadreId = b.PadreId,
                PadreNombre = b.Padre != null ? b.Padre.Nombre : null,
                EstadoProductivo = b.EstadoProductivo,
                EstadoProductivoNombre = b.EstadoProductivo.ToString(),
                EstatusSistema = b.EstatusSistema,
                EstatusSistemaNombre = b.EstatusSistema.ToString(),
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (bovino == null)
        {
            return NotFound(new { message = "Bovino no encontrado" });
        }

        return Ok(bovino);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBovino([FromBody] CreateBovinoDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Validar que la UPP pertenezca al usuario
        var upp = await _context.UPPs
            .FirstOrDefaultAsync(u => u.Id == dto.UPPId && u.OwnerId == userId);

        if (upp == null)
        {
            return NotFound(new { message = "UPP no encontrada o no tienes acceso a ella" });
        }

        // Validar fecha de nacimiento no sea futura
        if (dto.FechaNacimiento > DateTime.UtcNow)
        {
            return BadRequest(new { message = "La fecha de nacimiento no puede ser futura" });
        }

        // Validar que la madre existe y pertenece a la misma UPP (si se especifica)
        if (dto.MadreId.HasValue)
        {
            var madre = await _context.Bovinos
                .FirstOrDefaultAsync(b => b.Id == dto.MadreId.Value && b.UPPId == dto.UPPId);

            if (madre == null)
            {
                return BadRequest(new { message = "La madre especificada no existe o no pertenece a esta UPP" });
            }
        }

        // Validar que el padre existe y pertenece a la misma UPP (si se especifica)
        if (dto.PadreId.HasValue)
        {
            var padre = await _context.Bovinos
                .FirstOrDefaultAsync(b => b.Id == dto.PadreId.Value && b.UPPId == dto.UPPId);

            if (padre == null)
            {
                return BadRequest(new { message = "El padre especificado no existe o no pertenece a esta UPP" });
            }

            // Validar que el padre y la madre no sean el mismo animal
            if (dto.MadreId.HasValue && dto.PadreId == dto.MadreId)
            {
                return BadRequest(new { message = "El padre y la madre no pueden ser el mismo animal" });
            }
        }

        // Validar arete SINIIGA único (si se proporciona)
        if (!string.IsNullOrWhiteSpace(dto.AreteSiniiga))
        {
            if (await _context.Bovinos.AnyAsync(b => b.AreteSiniiga == dto.AreteSiniiga))
            {
                return BadRequest(new { message = "El arete SINIIGA ya está registrado" });
            }
        }

        // Validar Infraestructura (si se proporciona)
        if (dto.InfraestructuraId.HasValue)
        {
            var infraestructura = await _context.Infraestructuras
                .FirstOrDefaultAsync(i => i.Id == dto.InfraestructuraId.Value && i.UPPId == dto.UPPId);

            if (infraestructura == null)
            {
                return BadRequest(new { message = "La infraestructura especificada no existe o no pertenece a esta UPP" });
            }
        }

        var bovino = new Bovino
        {
            Id = Guid.NewGuid(),
            UPPId = dto.UPPId,
            InfraestructuraId = dto.InfraestructuraId,
            AreteSiniiga = dto.AreteSiniiga?.Trim().ToUpper(),
            AreteTrabajo = dto.AreteTrabajo.Trim(),
            Nombre = dto.Nombre?.Trim(),
            FechaNacimiento = dto.FechaNacimiento,
            Sexo = dto.Sexo,
            RazaPredominante = dto.RazaPredominante.Trim(),
            MadreId = dto.MadreId,
            PadreId = dto.PadreId,
            EstadoProductivo = dto.EstadoProductivo,
            EstatusSistema = dto.EstatusSistema,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bovinos.Add(bovino);
        await _context.SaveChangesAsync();

        // Cargar relaciones para respuesta
        await _context.Entry(bovino)
            .Reference(b => b.Madre).LoadAsync();
        await _context.Entry(bovino)
            .Reference(b => b.Padre).LoadAsync();
        await _context.Entry(bovino)
            .Reference(b => b.Infraestructura).LoadAsync();

        return Ok(new BovinoDto
        {
            Id = bovino.Id,
            UPPId = bovino.UPPId,
            InfraestructuraId = bovino.InfraestructuraId.HasValue ? bovino.InfraestructuraId.ToString() : null,
            InfraestructuraNombre = bovino.Infraestructura != null ? bovino.Infraestructura.Nombre : null,
            AreteSiniiga = bovino.AreteSiniiga,
            AreteTrabajo = bovino.AreteTrabajo,
            Nombre = bovino.Nombre,
            FechaNacimiento = bovino.FechaNacimiento,
            EdadMeses = (int?)((DateTime.UtcNow - bovino.FechaNacimiento).TotalDays / 30),
            Sexo = bovino.Sexo,
            SexoNombre = bovino.Sexo == 'M' ? "Macho" : "Hembra",
            RazaPredominante = bovino.RazaPredominante,
            MadreId = bovino.MadreId,
            MadreNombre = bovino.Madre != null ? bovino.Madre.Nombre : null,
            PadreId = bovino.PadreId,
            PadreNombre = bovino.Padre != null ? bovino.Padre.Nombre : null,
            EstadoProductivo = bovino.EstadoProductivo,
            EstadoProductivoNombre = bovino.EstadoProductivo.ToString(),
            EstatusSistema = bovino.EstatusSistema,
            EstatusSistemaNombre = bovino.EstatusSistema.ToString(),
            CreatedAt = bovino.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBovino(Guid id, [FromBody] CreateBovinoDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var bovino = await _context.Bovinos
            .Include(b => b.UPP)
            .FirstOrDefaultAsync(b => b.Id == id && b.UPP.OwnerId == userId);

        if (bovino == null)
        {
            return NotFound(new { message = "Bovino no encontrado" });
        }

        // Validar fecha de nacimiento no sea futura
        if (dto.FechaNacimiento > DateTime.UtcNow)
        {
            return BadRequest(new { message = "La fecha de nacimiento no puede ser futura" });
        }

        // Validar que el sexo no cambie (inmutable)
        if (bovino.Sexo != dto.Sexo)
        {
            return BadRequest(new { message = "El sexo no puede modificarse una vez establecido" });
        }

        // Validar que la madre existe y pertenece a la misma UPP (si se especifica)
        if (dto.MadreId.HasValue)
        {
            var madre = await _context.Bovinos
                .FirstOrDefaultAsync(b => b.Id == dto.MadreId.Value && b.UPPId == dto.UPPId);

            if (madre == null)
            {
                return BadRequest(new { message = "La madre especificada no existe o no pertenece a esta UPP" });
            }

            // Validar que la madre no sea el mismo animal
            if (dto.MadreId == id)
            {
                return BadRequest(new { message = "Un animal no puede ser su propia madre" });
            }
        }

        // Validar que el padre existe y pertenece a la misma UPP (si se especifica)
        if (dto.PadreId.HasValue)
        {
            var padre = await _context.Bovinos
                .FirstOrDefaultAsync(b => b.Id == dto.PadreId.Value && b.UPPId == dto.UPPId);

            if (padre == null)
            {
                return BadRequest(new { message = "El padre especificado no existe o no pertenece a esta UPP" });
            }

            // Validar que el padre no sea el mismo animal
            if (dto.PadreId == id)
            {
                return BadRequest(new { message = "Un animal no puede ser su propio padre" });
            }

            // Validar que el padre y la madre no sean el mismo animal
            if (dto.MadreId.HasValue && dto.PadreId == dto.MadreId)
            {
                return BadRequest(new { message = "El padre y la madre no pueden ser el mismo animal" });
            }
        }

        // Validar arete SINIIGA único (si cambió)
        if (!string.IsNullOrWhiteSpace(dto.AreteSiniiga) && 
            dto.AreteSiniiga != bovino.AreteSiniiga)
        {
            if (await _context.Bovinos.AnyAsync(b => b.AreteSiniiga == dto.AreteSiniiga && b.Id != id))
            {
                return BadRequest(new { message = "El arete SINIIGA ya está registrado" });
            }
        }

        // Validar Infraestructura (si se proporciona)
        if (dto.InfraestructuraId.HasValue)
        {
            var infraestructura = await _context.Infraestructuras
                .FirstOrDefaultAsync(i => i.Id == dto.InfraestructuraId.Value && i.UPPId == dto.UPPId);

            if (infraestructura == null)
            {
                return BadRequest(new { message = "La infraestructura especificada no existe o no pertenece a esta UPP" });
            }
        }

        // Actualizar campos
        bovino.UPPId = dto.UPPId;
        bovino.InfraestructuraId = dto.InfraestructuraId;
        bovino.AreteSiniiga = dto.AreteSiniiga?.Trim().ToUpper();
        bovino.AreteTrabajo = dto.AreteTrabajo.Trim();
        bovino.Nombre = dto.Nombre?.Trim();
        bovino.FechaNacimiento = dto.FechaNacimiento;
        // Sexo es inmutable, no se actualiza
        bovino.RazaPredominante = dto.RazaPredominante.Trim();
        bovino.MadreId = dto.MadreId;
        bovino.PadreId = dto.PadreId;
        bovino.EstadoProductivo = dto.EstadoProductivo;
        bovino.EstatusSistema = dto.EstatusSistema;
        bovino.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Cargar relaciones para respuesta
        await _context.Entry(bovino)
            .Reference(b => b.Madre).LoadAsync();
        await _context.Entry(bovino)
            .Reference(b => b.Padre).LoadAsync();
        await _context.Entry(bovino)
            .Reference(b => b.Infraestructura).LoadAsync();

        return Ok(new BovinoDto
        {
            Id = bovino.Id,
            UPPId = bovino.UPPId,
            InfraestructuraId = bovino.InfraestructuraId.HasValue ? bovino.InfraestructuraId.ToString() : null,
            InfraestructuraNombre = bovino.Infraestructura != null ? bovino.Infraestructura.Nombre : null,
            AreteSiniiga = bovino.AreteSiniiga,
            AreteTrabajo = bovino.AreteTrabajo,
            Nombre = bovino.Nombre,
            FechaNacimiento = bovino.FechaNacimiento,
            EdadMeses = (int?)((DateTime.UtcNow - bovino.FechaNacimiento).TotalDays / 30),
            Sexo = bovino.Sexo,
            SexoNombre = bovino.Sexo == 'M' ? "Macho" : "Hembra",
            RazaPredominante = bovino.RazaPredominante,
            MadreId = bovino.MadreId,
            MadreNombre = bovino.Madre != null ? bovino.Madre.Nombre : null,
            PadreId = bovino.PadreId,
            PadreNombre = bovino.Padre != null ? bovino.Padre.Nombre : null,
            EstadoProductivo = bovino.EstadoProductivo,
            EstadoProductivoNombre = bovino.EstadoProductivo.ToString(),
            EstatusSistema = bovino.EstatusSistema,
            EstatusSistemaNombre = bovino.EstatusSistema.ToString(),
            CreatedAt = bovino.CreatedAt,
            UpdatedAt = bovino.UpdatedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBovino(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var bovino = await _context.Bovinos
            .Include(b => b.UPP)
            .FirstOrDefaultAsync(b => b.Id == id && b.UPP.OwnerId == userId);

        if (bovino == null)
        {
            return NotFound(new { message = "Bovino no encontrado" });
        }

        // Verificar si tiene hijos (no se puede eliminar si es padre/madre de otros bovinos)
        var tieneHijos = await _context.Bovinos
            .AnyAsync(b => b.MadreId == id || b.PadreId == id);

        if (tieneHijos)
        {
            return BadRequest(new { message = "No se puede eliminar un bovino que tiene descendencia registrada" });
        }

        _context.Bovinos.Remove(bovino);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
