namespace RuralTech.Core.DTOs;

public class CreateUPPDto
{
    public string ClavePGN { get; set; } = string.Empty;
    public string NombrePredio { get; set; } = string.Empty;
    public string PropietarioLegal { get; set; } = string.Empty;
    public string EstadoMX { get; set; } = string.Empty; // Clave INEGI (2 caracteres)
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
