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
public class AnimalsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnimalsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnimalDto>>> GetAnimals()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animals = await _context.Animals
            .Where(a => a.UserId == userId)
            .Include(a => a.WeightHistory.OrderByDescending(w => w.Date))
            .Include(a => a.Vaccines)
            .Include(a => a.Treatments)
            .ToListAsync();

        return Ok(animals.Select(a => new AnimalDto
        {
            Id = a.Id,
            Name = a.Name,
            Breed = a.Breed,
            BirthDate = a.BirthDate,
            Sex = a.Sex,
            CurrentWeight = a.CurrentWeight,
            LastVaccineDate = a.LastVaccineDate,
            WeightHistory = a.WeightHistory.Select(w => new WeightRecordDto
            {
                Id = w.Id,
                Weight = w.Weight,
                Date = w.Date,
                Notes = w.Notes
            }).ToList(),
            Vaccines = a.Vaccines.Select(v => new VaccineDto
            {
                Id = v.Id,
                Name = v.Name,
                DateApplied = v.DateApplied,
                NextDueDate = v.NextDueDate,
                Notes = v.Notes
            }).ToList(),
            Treatments = a.Treatments.Select(t => new TreatmentDto
            {
                Id = t.Id,
                Condition = t.Condition,
                TreatmentDescription = t.TreatmentDescription,
                Date = t.Date,
                Notes = t.Notes
            }).ToList()
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalDto>> GetAnimal(int id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = await _context.Animals
            .Include(a => a.WeightHistory.OrderByDescending(w => w.Date))
            .Include(a => a.Vaccines)
            .Include(a => a.Treatments)
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        return Ok(new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Breed = animal.Breed,
            BirthDate = animal.BirthDate,
            Sex = animal.Sex,
            CurrentWeight = animal.CurrentWeight,
            LastVaccineDate = animal.LastVaccineDate,
            WeightHistory = animal.WeightHistory.Select(w => new WeightRecordDto
            {
                Id = w.Id,
                Weight = w.Weight,
                Date = w.Date,
                Notes = w.Notes
            }).ToList(),
            Vaccines = animal.Vaccines.Select(v => new VaccineDto
            {
                Id = v.Id,
                Name = v.Name,
                DateApplied = v.DateApplied,
                NextDueDate = v.NextDueDate,
                Notes = v.Notes
            }).ToList(),
            Treatments = animal.Treatments.Select(t => new TreatmentDto
            {
                Id = t.Id,
                Condition = t.Condition,
                TreatmentDescription = t.TreatmentDescription,
                Date = t.Date,
                Notes = t.Notes
            }).ToList()
        });
    }

    [HttpPost]
    public async Task<ActionResult<AnimalDto>> CreateAnimal([FromBody] CreateAnimalDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = new Animal
        {
            Name = dto.Name,
            Breed = dto.Breed,
            BirthDate = dto.BirthDate,
            Sex = dto.Sex,
            CurrentWeight = dto.CurrentWeight,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        // Add initial weight record
        animal.WeightHistory.Add(new WeightRecord
        {
            Weight = dto.CurrentWeight,
            Date = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        });

        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Breed = animal.Breed,
            BirthDate = animal.BirthDate,
            Sex = animal.Sex,
            CurrentWeight = animal.CurrentWeight
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnimal(int id, [FromBody] CreateAnimalDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        animal.Name = dto.Name;
        animal.Breed = dto.Breed;
        animal.BirthDate = dto.BirthDate;
        animal.Sex = dto.Sex;
        animal.CurrentWeight = dto.CurrentWeight;
        animal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/weight")]
    public async Task<ActionResult<WeightRecordDto>> AddWeightRecord(int id, [FromBody] AddWeightRecordDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        var weightRecord = new WeightRecord
        {
            AnimalId = id,
            Weight = dto.Weight,
            Date = dto.Date,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        animal.CurrentWeight = dto.Weight;
        animal.UpdatedAt = DateTime.UtcNow;

        _context.WeightRecords.Add(weightRecord);
        await _context.SaveChangesAsync();

        return Ok(new WeightRecordDto
        {
            Id = weightRecord.Id,
            Weight = weightRecord.Weight,
            Date = weightRecord.Date,
            Notes = weightRecord.Notes
        });
    }

    [HttpPost("{id}/vaccines")]
    public async Task<ActionResult<VaccineDto>> AddVaccine(int id, [FromBody] AddVaccineDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        var vaccine = new Vaccine
        {
            AnimalId = id,
            Name = dto.Name,
            DateApplied = dto.DateApplied,
            NextDueDate = dto.NextDueDate,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        animal.LastVaccineDate = dto.DateApplied;
        animal.UpdatedAt = DateTime.UtcNow;

        _context.Vaccines.Add(vaccine);
        await _context.SaveChangesAsync();

        return Ok(new VaccineDto
        {
            Id = vaccine.Id,
            Name = vaccine.Name,
            DateApplied = vaccine.DateApplied,
            NextDueDate = vaccine.NextDueDate,
            Notes = vaccine.Notes
        });
    }

    [HttpPost("{id}/treatments")]
    public async Task<ActionResult<TreatmentDto>> AddTreatment(int id, [FromBody] AddTreatmentDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        var treatment = new Treatment
        {
            AnimalId = id,
            Condition = dto.Condition,
            TreatmentDescription = dto.TreatmentDescription,
            Date = dto.Date,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        animal.UpdatedAt = DateTime.UtcNow;

        _context.Treatments.Add(treatment);
        await _context.SaveChangesAsync();

        return Ok(new TreatmentDto
        {
            Id = treatment.Id,
            Condition = treatment.Condition,
            TreatmentDescription = treatment.TreatmentDescription,
            Date = treatment.Date,
            Notes = treatment.Notes
        });
    }
}
