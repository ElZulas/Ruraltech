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
public class AlertsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AlertsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AlertDto>>> GetAlerts()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var today = DateTime.UtcNow.Date;

        var animals = await _context.Animals
            .Where(a => a.UserId == userId)
            .Include(a => a.Vaccines)
            .ToListAsync();

        var alerts = new List<AlertDto>();

        foreach (var animal in animals)
        {
            foreach (var vaccine in animal.Vaccines)
            {
                var daysLeft = (vaccine.NextDueDate.Date - today).Days;

                if (daysLeft <= 30 && daysLeft >= 0)
                {
                    alerts.Add(new AlertDto
                    {
                        AnimalId = animal.Id,
                        AnimalName = animal.Name,
                        VaccineName = vaccine.Name,
                        DueDate = vaccine.NextDueDate,
                        DaysLeft = daysLeft,
                        IsUrgent = daysLeft <= 7
                    });
                }
            }
        }

        return Ok(alerts.OrderBy(a => a.DaysLeft));
    }
}
