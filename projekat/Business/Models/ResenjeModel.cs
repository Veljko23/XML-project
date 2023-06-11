namespace Business.Models;
public class ResenjeModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int AutorskoPravoId { get; set; }
    public string Sifra { get; set; }
    public DateTime DatumObradeZahteva { get; set; }
    public bool Odobren { get; set; }
    public string Referenca { get; set; }
    public string SluzbenikIme { get; set; }
    public string SluzbenikPrezime { get; set; }
    public string PdfFile { get; set; }
    public string ObrazlozenjeOdbijanja { get; set; } = string.Empty;
}
