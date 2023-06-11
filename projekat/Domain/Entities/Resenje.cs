using Domain.Common;

namespace Domain.Entities;

public class Resenje : AuditableEntity
{
    public int Id { get; set; }
    public AutorskoPravo AutorskoPravo { get; set; }
    public string? UserId { get; set; }
    public int AutorskoPravoId { get; set; }
    public string? Sifra { get;set; }
    public DateTime DatumObradeZahteva { get; set; }
    public bool Odobren { get; set; }
    public string? Referenca { get; set; }
    public string? SluzbenikIme { get; set; }
    public string? SluzbenikPrezime { get; set; }
    public string? ObrazlozenjeOdbijanja { get; set; }
}