using Business.Models;
using Domain.Entities;

namespace Business.Mappers;
public static class ResenjeMapper
{
    public static Resenje MapToDomain(ResenjeModel model)
    {
        return new Resenje
        {
            AutorskoPravoId = model.AutorskoPravoId,
            DatumObradeZahteva = model.DatumObradeZahteva,
            Odobren = model.Odobren,
            UserId = model.UserId,
            Referenca = model.Referenca,
            SluzbenikIme = model.SluzbenikIme,
            SluzbenikPrezime = model.SluzbenikPrezime,
            Sifra = model.Sifra,
             ObrazlozenjeOdbijanja = model.ObrazlozenjeOdbijanja
        };
    }

    public static ResenjeModel MapToModel(Resenje model)
    {
        return new ResenjeModel
        {
            AutorskoPravoId = model.AutorskoPravoId,
            DatumObradeZahteva = model.DatumObradeZahteva,
            Odobren = model.Odobren,
            UserId = model.UserId,
            Referenca = model.Referenca,
            SluzbenikIme = model.SluzbenikIme,
            SluzbenikPrezime = model.SluzbenikPrezime,
            Sifra = model.Sifra,
            Id = model.Id,
            ObrazlozenjeOdbijanja = model.ObrazlozenjeOdbijanja
        };
    }

    public static List<Resenje> MapToDomainCollection(List<ResenjeModel> sluzbenikAkcije)
    {
        return sluzbenikAkcije.Select(model => new Resenje
        {
            AutorskoPravoId = model.AutorskoPravoId,
            DatumObradeZahteva = model.DatumObradeZahteva,
            Odobren = model.Odobren,
            UserId = model.UserId,
            Referenca = model.Referenca,
            SluzbenikIme = model.SluzbenikIme,
            SluzbenikPrezime = model.SluzbenikPrezime,
            Sifra = model.Sifra,
            ObrazlozenjeOdbijanja = model.ObrazlozenjeOdbijanja
        }).ToList();
    }

    public static List<ResenjeModel> MapToModelCollection(List<Resenje> sluzbenikAkcije)
    {
        return sluzbenikAkcije.Select(model => new ResenjeModel
        {
            AutorskoPravoId = model.AutorskoPravoId,
            DatumObradeZahteva = model.DatumObradeZahteva,
            Odobren = model.Odobren,
            UserId = model.UserId,
            Referenca = model.Referenca,
            SluzbenikIme = model.SluzbenikIme,
            SluzbenikPrezime = model.SluzbenikPrezime,
            Sifra = model.Sifra,
            Id = model.Id,
            ObrazlozenjeOdbijanja = model.ObrazlozenjeOdbijanja
        }).ToList();
    }
}