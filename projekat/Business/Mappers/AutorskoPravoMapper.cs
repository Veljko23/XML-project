using Business.Models;
using Domain.Entities;

namespace Business.Mappers
{
    public static class AutorskoPravoMapper
    {
        public static AutorskoPravo MapToDomain(AutorskoPravoModel model)
        {
            return new AutorskoPravo
            {
                DaLiJeURadnomODnosu = model.DaLiJeURadnomODnosu,
                DatumPodnosenja = DateTime.Now,
                Email = model.Email,
                NacinKoriscenja = model.NacinKoriscenja,
                Naslov = model.Naslov,
                PodaciOAutoru = model.PodaciOAutoru,
                PodaciOFormiZapisa = model.PodaciOFormiZapisa,
                PodaciONaslovu = model.PodaciONaslovu,
                PodaciOVrsti = model.PodaciOVrsti,
                PodnosilacDetalji = model.PodnosilacDetalji,
                PodnosilacPrijavePotpis = model.PodnosilacDetalji,
                Pseudonim = model.Pseudonim,
                Punomocnik = model.Punomocnik,
                Telefon = model.Telefon,
                Status = Status.Podnesen,
                Zaveden = false,
                FajloviPutanje = String.Join(',', model.FajloviPutanje),
                Sifra = model.Sifra
            };
        }

        public static List<AutorskoPravo> MapToDomainCollection(List<AutorskoPravoModel> autorskaPrava)
        {
            return autorskaPrava.Select(model => new AutorskoPravo
            {
                DaLiJeURadnomODnosu = model.DaLiJeURadnomODnosu,
                DatumPodnosenja = model.DatumPodnosenja,
                Email = model.Email,
                NacinKoriscenja = model.NacinKoriscenja,
                Naslov = model.Naslov,
                PodaciOAutoru = model.PodaciOAutoru,
                PodaciOFormiZapisa = model.PodaciOFormiZapisa,
                PodaciONaslovu = model.PodaciONaslovu,
                PodaciOVrsti = model.PodaciOVrsti,
                PodnosilacDetalji = model.PodnosilacDetalji,
                PodnosilacPrijavePotpis = model.PodnosilacDetalji,
                Pseudonim = model.Pseudonim,
                Punomocnik = model.Punomocnik,
                Sifra = model.Sifra,
                Telefon = model.Telefon,
                FajloviPutanje = String.Join(',', model.FajloviPutanje)
            }).ToList();
        }


        public static AutorskoPravoModel MapToModel(AutorskoPravo model)
        {
            return new AutorskoPravoModel
            {
                Id = model.Id,
                DaLiJeURadnomODnosu = model.DaLiJeURadnomODnosu,
                DatumPodnosenja = model.DatumPodnosenja,
                Email = model.Email,
                NacinKoriscenja = model.NacinKoriscenja,
                Naslov = model.Naslov,
                PodaciOAutoru = model.PodaciOAutoru,
                PodaciOFormiZapisa = model.PodaciOFormiZapisa,
                PodaciONaslovu = model.PodaciONaslovu,
                PodaciOVrsti = model.PodaciOVrsti,
                PodnosilacDetalji = model.PodnosilacDetalji,
                PodnosilacPrijavePotpis = model.PodnosilacDetalji,
                Pseudonim = model.Pseudonim,
                Punomocnik = model.Punomocnik,
                Telefon = model.Telefon,
                Status = model.Status,
                Zaveden = model.Zaveden,
                Sifra = model.Sifra,
                 FajloviPutanje = model.FajloviPutanje.Split(',').ToList()
            };
        }

        public static List<AutorskoPravoModel> MapToModelCollection(List<AutorskoPravo> autorskaPrava)
        {
            return autorskaPrava.Select(model => new AutorskoPravoModel
            {
                Id = model.Id,
                DaLiJeURadnomODnosu = model.DaLiJeURadnomODnosu,
                DatumPodnosenja = model.DatumPodnosenja,
                Email = model.Email,
                NacinKoriscenja = model.NacinKoriscenja,
                Naslov = model.Naslov,
                PodaciOAutoru = model.PodaciOAutoru,
                PodaciOFormiZapisa = model.PodaciOFormiZapisa,
                PodaciONaslovu = model.PodaciONaslovu,
                PodaciOVrsti = model.PodaciOVrsti,
                PodnosilacDetalji = model.PodnosilacDetalji,
                PodnosilacPrijavePotpis = model.PodnosilacDetalji,
                Pseudonim = model.Pseudonim,
                Punomocnik = model.Punomocnik,
                Telefon = model.Telefon,
                Status = model.Status,
                Zaveden = model.Zaveden,
                Sifra = model.Sifra,
                FajloviPutanje = model.FajloviPutanje.Split(',').ToList()
            }).ToList();
        }
    }
}