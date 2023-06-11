using System.Net.Mail;
using Business.Abstractions;
using Business.Mappers;
using Business.Models;
using Database;
using Domain.Entities;

namespace Business.Behaviours;

public class PrijavaService : IPrijavaService
{
    private readonly ApplicationDbContext _dbContext;

    public PrijavaService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public AutorskoPravoModel DodajPrijavu(AutorskoPravoModel model)
    {
        AutorskoPravo domain = AutorskoPravoMapper.MapToDomain(model);

        domain.Sifra = RandomString(6);
        domain.CreatedBy = model.User;

        _dbContext.AutorskaPrava.Add(domain);
        _dbContext.SaveChanges();

        model.Id = domain.Id;

        return model;
    }

    private static Random random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public List<AutorskoPravoModel> PretraziPoMetapodacima(AutorskoPravoModel model)
    {
        List<AutorskoPravo> autorskaPrava = new List<AutorskoPravo>();

        switch (model.SearchType)
        {
            case SearchType.And:
                {
                    autorskaPrava = _dbContext.AutorskaPrava
                        .Where(x => (string.IsNullOrEmpty(model.PodnosilacDetalji) || x.PodnosilacDetalji == model.PodnosilacDetalji)
                                 && (string.IsNullOrEmpty(model.Telefon) || x.Telefon == model.Telefon)
                                 && (string.IsNullOrEmpty(model.Email) || x.Email == model.Email)
                                 && (string.IsNullOrEmpty(model.Pseudonim) || x.Pseudonim == model.Pseudonim)
                                 && (string.IsNullOrEmpty(model.Punomocnik) || x.Punomocnik == model.Punomocnik)
                                 && (string.IsNullOrEmpty(model.Naslov) || x.Naslov == model.Naslov)
                                 && (string.IsNullOrEmpty(model.PodaciONaslovu) || x.PodaciONaslovu == model.PodaciONaslovu)
                                 && (string.IsNullOrEmpty(model.PodaciOVrsti) || x.PodaciOVrsti == model.PodaciOVrsti)
                                 && (string.IsNullOrEmpty(model.PodaciOFormiZapisa) || x.PodaciOFormiZapisa == model.PodaciOFormiZapisa)
                                 && (string.IsNullOrEmpty(model.PodaciOAutoru) || x.PodaciOAutoru == model.PodaciOAutoru)
                                 && (string.IsNullOrEmpty(model.DaLiJeURadnomODnosu) || x.DaLiJeURadnomODnosu == model.DaLiJeURadnomODnosu)
                                 && (string.IsNullOrEmpty(model.NacinKoriscenja) || x.NacinKoriscenja == model.NacinKoriscenja)
                                 && (string.IsNullOrEmpty(model.PodnosilacPrijavePotpis) || x.PodnosilacPrijavePotpis == model.PodnosilacPrijavePotpis)
                                 && (model.DatumPodnosenja == DateTime.MinValue || x.DatumPodnosenja == model.DatumPodnosenja)
                                 && (x.Zaveden == model.Zaveden))
                        .ToList();
                    break;
                }
            case SearchType.Or:
                {
                    autorskaPrava = _dbContext.AutorskaPrava
                        .Where(x => (string.IsNullOrEmpty(model.PodnosilacDetalji) || x.PodnosilacDetalji == model.PodnosilacDetalji)
                                 || (string.IsNullOrEmpty(model.Telefon) || x.Telefon == model.Telefon)
                                 || (string.IsNullOrEmpty(model.Email) || x.Email == model.Email)
                                 || (string.IsNullOrEmpty(model.Pseudonim) || x.Pseudonim == model.Pseudonim)
                                 || (string.IsNullOrEmpty(model.Punomocnik) || x.Punomocnik == model.Punomocnik)
                                 || (string.IsNullOrEmpty(model.Naslov) || x.Naslov == model.Naslov)
                                 || (string.IsNullOrEmpty(model.PodaciONaslovu) || x.PodaciONaslovu == model.PodaciONaslovu)
                                 || (string.IsNullOrEmpty(model.PodaciOVrsti) || x.PodaciOVrsti == model.PodaciOVrsti)
                                 || (string.IsNullOrEmpty(model.PodaciOFormiZapisa) || x.PodaciOFormiZapisa == model.PodaciOFormiZapisa)
                                 || (string.IsNullOrEmpty(model.PodaciOAutoru) || x.PodaciOAutoru == model.PodaciOAutoru)
                                 || (string.IsNullOrEmpty(model.DaLiJeURadnomODnosu) || x.DaLiJeURadnomODnosu == model.DaLiJeURadnomODnosu)
                                 || (string.IsNullOrEmpty(model.NacinKoriscenja) || x.NacinKoriscenja == model.NacinKoriscenja)
                                 || (string.IsNullOrEmpty(model.PodnosilacPrijavePotpis) || x.PodnosilacPrijavePotpis == model.PodnosilacPrijavePotpis)
                                 || (x.DatumPodnosenja == model.DatumPodnosenja)
                                 || (x.Zaveden == model.Zaveden))
                        .ToList();
                    break;
                }
            case SearchType.Not:
                {
                    autorskaPrava = _dbContext.AutorskaPrava
                        .Where(x => !((string.IsNullOrEmpty(model.PodnosilacDetalji) || x.PodnosilacDetalji == model.PodnosilacDetalji)
                                 && (string.IsNullOrEmpty(model.Telefon) || x.Telefon == model.Telefon)
                                 && (string.IsNullOrEmpty(model.Email) || x.Email == model.Email)
                                 && (string.IsNullOrEmpty(model.Pseudonim) || x.Pseudonim == model.Pseudonim)
                                 && (string.IsNullOrEmpty(model.Punomocnik) || x.Punomocnik == model.Punomocnik)
                                 && (string.IsNullOrEmpty(model.Naslov) || x.Naslov == model.Naslov)
                                 && (string.IsNullOrEmpty(model.PodaciONaslovu) || x.PodaciONaslovu == model.PodaciONaslovu)
                                 && (string.IsNullOrEmpty(model.PodaciOVrsti) || x.PodaciOVrsti == model.PodaciOVrsti)
                                 && (string.IsNullOrEmpty(model.PodaciOFormiZapisa) || x.PodaciOFormiZapisa == model.PodaciOFormiZapisa)
                                 && (string.IsNullOrEmpty(model.PodaciOAutoru) || x.PodaciOAutoru == model.PodaciOAutoru)
                                 && (string.IsNullOrEmpty(model.DaLiJeURadnomODnosu) || x.DaLiJeURadnomODnosu == model.DaLiJeURadnomODnosu)
                                 && (string.IsNullOrEmpty(model.NacinKoriscenja) || x.NacinKoriscenja == model.NacinKoriscenja)
                                 && (string.IsNullOrEmpty(model.PodnosilacPrijavePotpis) || x.PodnosilacPrijavePotpis == model.PodnosilacPrijavePotpis)
                                 && (x.DatumPodnosenja == model.DatumPodnosenja)
                                 && (x.Zaveden == model.Zaveden)))
                        .ToList();
                    break;
                }
        }

        if (!model.IsAdmin)
            autorskaPrava = autorskaPrava.Where(x => x.Status != Status.Podnesen || x.CreatedBy == model.User).ToList();

        return AutorskoPravoMapper.MapToModelCollection(autorskaPrava);
    }


    public List<AutorskoPravoModel> PretraziPrijave(string pojam, bool isUserAdmin, string userName)
    {
        string formattedPojam = pojam.ToLower();

        var autorskaPrava = _dbContext.AutorskaPrava
                                      .Where(x => x.PodnosilacDetalji.Contains(formattedPojam) ||
                                                  x.Telefon.Contains(formattedPojam) ||
                                                  x.Email.Contains(formattedPojam) ||
                                                  x.Pseudonim.Contains(formattedPojam) ||
                                                  x.Punomocnik.Contains(formattedPojam) ||
                                                  x.Naslov.Contains(formattedPojam) ||
                                                  x.PodaciONaslovu.Contains(formattedPojam) ||
                                                  x.PodaciOVrsti.Contains(formattedPojam) ||
                                                  x.PodaciOFormiZapisa.Contains(formattedPojam) ||
                                                  x.PodaciOAutoru.Contains(formattedPojam) ||
                                                  x.DaLiJeURadnomODnosu.Contains(formattedPojam) ||
                                                  x.NacinKoriscenja.Contains(formattedPojam) ||
                                                  x.PodnosilacPrijavePotpis.Contains(formattedPojam))
                                      .ToList();

        //Ako je obican korisnik, moze da vidi samo vec zavedene zahteve (admin ih je zaveo - odbio/odobrio)
        if (!isUserAdmin)
            autorskaPrava = autorskaPrava.Where(x => x.Status != Status.Podnesen || x.CreatedBy == userName).ToList();

        return AutorskoPravoMapper.MapToModelCollection(autorskaPrava);
    }

    public AutorskoPravoModel VratiPrijavu(int id)
    {
        var autorskoPravo = _dbContext.AutorskaPrava.FirstOrDefault(x => x.Id == id);

        if (autorskoPravo is null) return null;

        return AutorskoPravoMapper.MapToModel(autorskoPravo);
    }

    public List<AutorskoPravoModel> VratiSvePrijave(bool isUserAdmin, string userName)
    {
        var autorskaPrava = _dbContext.AutorskaPrava.ToList();
        
        if (!isUserAdmin)
            autorskaPrava = autorskaPrava.Where(x => x.Status != Status.Podnesen || x.CreatedBy == userName).ToList();

        return AutorskoPravoMapper.MapToModelCollection(autorskaPrava);
    }

    public List<AutorskoPravoModel> VratiSveZavedenePrijave()
    {
        var autorskaPrava = _dbContext.AutorskaPrava.Where(x => x.Zaveden).ToList();

        return AutorskoPravoMapper.MapToModelCollection(autorskaPrava);
    }

    public bool PodnesiResenje(ResenjeModel model)
    {
        var resenje = ResenjeMapper.MapToDomain(model);

        _dbContext.Resenja.Add(resenje);
        _dbContext.SaveChanges();

        var autorskoPravo = _dbContext.AutorskaPrava.FirstOrDefault(x => x.Id == model.AutorskoPravoId);

        if (autorskoPravo is not null)
        {
            autorskoPravo.Zaveden = true;
            autorskoPravo.Status = model.Odobren ? Status.Odobren : Status.Odbijen;
            autorskoPravo.Sifra = model.Sifra;

            _dbContext.SaveChanges();

            ObavestiKorisnika(resenje, autorskoPravo.Email, model.PdfFile);
        }

        return true;
    }

    private bool ObavestiKorisnika(Resenje resenje, string email, string pdfFile)
    {
        string emailUsername = "edukativnisajt@gmail.com";
        string emailFrom = "edukativnisajt@gmail.com";
        string emailPassword = "cblauxgayngfruht";
        string smtpClient = "smtp.gmail.com";

        MailMessage mail = new MailMessage();
        SmtpClient smtpServer = new SmtpClient(smtpClient);

        mail.From = new MailAddress(emailFrom, "AutorskaPrava Platforma");
        mail.Headers.Add("Message-Id", "<AutorskaPrava Platforma>");

        try
        {
            mail.To.Add(email);
        }
        catch (Exception ex) { }

        mail.Subject = "Vas zahtev je zaveden";
        mail.Body = "<div style='text-align: center'>";
        mail.Body += "<div style='text-align: left'> Zdravo,";
        mail.Body += "<br/><br/>";
        mail.Body += "Vas zahtev pod sifrom " + resenje.Sifra + " je zaveden.";
        mail.Body += "<br/>Rezultat: <b>" + (resenje.Odobren ? "Odobren" : "Odbijen") + "</b>";
        if (!resenje.Odobren)
        {
            mail.Body += "<br/>Obrazlozenje odbijanja: <b>" + resenje.ObrazlozenjeOdbijanja + "</b>";
        }
        mail.Body += "<br/>Datum: " + resenje.DatumObradeZahteva.ToString("dd/MM/yyyy") + "</b>";
        mail.Body += "<br/>Sluzbenik: <b>" + resenje.SluzbenikIme + " " + resenje.SluzbenikPrezime + "</b>";
        mail.Body += "<br/>Referenca: <b>" + resenje.Referenca + "</b>";
        mail.Body += "</div>";

        string html = mail.Body;
        mail.IsBodyHtml = true;

        Attachment attachment = new Attachment(pdfFile);
        mail.Attachments.Add(attachment);

        smtpServer.Port = 587;
        smtpServer.UseDefaultCredentials = false;
        smtpServer.Credentials = new System.Net.NetworkCredential(emailUsername, emailPassword);
        smtpServer.EnableSsl = true;

        smtpServer.Send(mail);

        return true;
    }

    public IzvestajModel VratiIzvestaj(string start, string end)
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;

        bool startSuccess = DateTime.TryParse(start, out startDate);

        if (!startSuccess) return new IzvestajModel();

        bool endSuccess = DateTime.TryParse(end, out endDate);

        if (!endSuccess) return new IzvestajModel();

        var zahtevi = _dbContext.AutorskaPrava.Where(x => x.DatumPodnosenja >= startDate && x.DatumPodnosenja <= endDate);

        return new IzvestajModel
        {
            BrojPodnetihZahteva = zahtevi.Count(),
            BrojPrihvacenihZahteva = zahtevi.Count(x => x.Status == Status.Odobren),
            BrojOdbijenihZahteva = zahtevi.Count(x => x.Status == Status.Odbijen)
        };
    }
}
