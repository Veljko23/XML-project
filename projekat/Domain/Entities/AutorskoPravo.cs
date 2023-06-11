using Domain.Common;

namespace Domain.Entities;

public class AutorskoPravo : AuditableEntity
{
    public int Id { get; set; }
    public string Sifra { get; set; }
    public string PodnosilacDetalji { get; set; }
    public string Telefon { get; set; }
    public string Email { get; set; }
    public string Pseudonim { get; set; }
    public string Punomocnik { get; set; }
    public string Naslov { get; set; }
    public string PodaciONaslovu { get; set; }
    public string PodaciOVrsti { get; set; }
    public string PodaciOFormiZapisa { get; set; }
    public string PodaciOAutoru { get; set; }
    public string DaLiJeURadnomODnosu { get; set; }
    public string NacinKoriscenja { get; set; }
    public string PodnosilacPrijavePotpis { get; set; }
    public DateTime DatumPodnosenja { get; set; }
    public bool Zaveden { get; set; }
    public Status Status { get; set; }
    public string FajloviPutanje { get; set; }
}

public enum Status
{
    Podnesen = 0,
    Odobren = 1,
    Odbijen = 2
}