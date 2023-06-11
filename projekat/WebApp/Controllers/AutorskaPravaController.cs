using System.Drawing;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Business.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharp;
using VDS.RDF;
using VDS.RDF.Writing;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]
    public class AutorskaPravaController : Controller
    {
        private readonly ApiClient _apiClient;
        private static AutorskoPravoModel AutorskoPravo { get; set; }

        public AutorskaPravaController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Index(string? pojam, bool metadataSearch, int searchType, string PodnosilacDetalji,
                                    string operatorPodnosilacDetalji,
                                    string Telefon, string operatorTelefon, string Email, string operatorEmail,
                                    string Pseudonim, string operatorPseudonim, string Punomocnik, string operatorPunomocnik,
                                    string Naslov, string operatorNaslov, string PodaciONaslovu, string operatorPodaciONaslovu,
                                    string PodaciOVrsti, string operatorPodaciOVrsti, string PodaciOFormiZapisa,
                                    string operatorPodaciOFormiZapisa, string PodaciOAutoru, string operatorPodaciOAutoru,
                                    string DaLiJeURadnomODnosu, string operatorDaLiJeURadnomODnosu,
                                    string NacinKoriscenja, string operatorNacinKoriscenja, string PodnosilacPrijavePotpis, string operatorPodnosilacPrijavePotpis, DateTime? DatumPodnosenja, string operatorDatumPodnosenja, bool? Zaveden, string operatorZaveden, string Status,
                                    string operatorStatus, string FajloviPutanje, string operatorFajloviPutanje)
        {

            var isUserAdmin = User.IsInRole("Admin");

            var userName = User.Identity.Name;

            List<AutorskoPravoModel> prijave = new List<AutorskoPravoModel>();
            if (metadataSearch)
            {
                var autorskoPravoModel = new AutorskoPravoModel
                {
                    PodnosilacDetalji = PodnosilacDetalji ?? "",
                    Telefon = Telefon ?? "",
                    Email = Email ?? "",
                    Pseudonim = Pseudonim ?? "",
                    Punomocnik = Punomocnik ?? "",
                    Naslov = Naslov ?? "",
                    PodaciONaslovu = PodaciONaslovu ?? "",
                    PodaciOVrsti = PodaciOVrsti ?? "",
                    PodaciOFormiZapisa = PodaciOFormiZapisa ?? "",
                    PodaciOAutoru = PodaciOAutoru ?? "",
                    DaLiJeURadnomODnosu = DaLiJeURadnomODnosu ?? "",
                    NacinKoriscenja = NacinKoriscenja ?? "",
                    PodnosilacPrijavePotpis = PodnosilacPrijavePotpis ?? "",
                    DatumPodnosenja = DatumPodnosenja.HasValue ? DatumPodnosenja.Value : default,
                    Zaveden = Zaveden.HasValue ? Zaveden.Value : false,
                    SearchType = (SearchType)searchType,
                    IsAdmin = isUserAdmin,
                    User = userName
                };

                prijave = _apiClient.PretragaMetapodaci(autorskoPravoModel).Result;
            }
            else
            {
                prijave = !string.IsNullOrEmpty(pojam) ?
                _apiClient.PretraziPrijave(pojam, isUserAdmin, userName).Result : _apiClient.VratiSvePrijave(isUserAdmin, userName).Result;

                ViewBag.Pojam = pojam;
            }

            return View(prijave);
        }


        public IActionResult MetadataSearch()
        {
            var isUserAdmin = User.IsInRole("Admin");

            List<AutorskoPravoModel> prijave = !string.IsNullOrEmpty("") ?
            _apiClient.PretraziPrijave("", isUserAdmin, User.Identity.Name).Result : _apiClient.VratiSvePrijave(isUserAdmin, User.Identity.Name).Result;

            ViewBag.Pojam = "";

            return View(prijave);
        }

        public IActionResult Prijava(int id)
        {
            AutorskoPravoModel prijava = _apiClient.VratiPrijavu(id).Result;

            AutorskoPravo = prijava;

            return View(prijava);
        }

        private static List<string> CurrentFilePaths = new List<string>();

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var fileNameTemp = GetRandomString(5);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = $@"C:\autorskaprava\{fileNameTemp}_{formFile.FileName}";

                    filePaths.Add(filePath);
                    CurrentFilePaths.Add(filePath);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok();
        }

        private static string GetRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public IActionResult NovaPrijava()
        {
            return View();
        }

        [HttpPost]
        public AutorskoPravoModel NovaPrijava(AutorskoPravoModel prijava)
        {
            prijava.FajloviPutanje = CurrentFilePaths;
            prijava.Pdf = "";
            prijava.Sifra = "";

            prijava.User = User.Identity.Name;

            AutorskoPravoModel kreiranaPrijava = _apiClient.NovaPrijava(prijava).Result;

            CurrentFilePaths = new List<string>();
            return kreiranaPrijava;
        }

        [HttpPost]
        public bool PodnesiResenje(ResenjeModel resenje)
        {
            if (string.IsNullOrEmpty(resenje.ObrazlozenjeOdbijanja))
                resenje.ObrazlozenjeOdbijanja = string.Empty;

            var nameIdentifier = User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier"));

            resenje.UserId = nameIdentifier?.Value;

            var filePath = Path.Combine(@"C:\autorskaprava\zahtevi", $"{AutorskoPravo.Naslov}.pdf");

            KreirajPdf(filePath);

            resenje.PdfFile = filePath ?? string.Empty;

            string domainPath = Request.Scheme + "://" + Request.Host;

            resenje.Referenca = domainPath + resenje.Referenca;

            bool resenjeResult = _apiClient.PodnesiResenje(resenje).Result;

            return resenjeResult;
        }


        public IActionResult DownloadMetadataRdf()
        {
            var graph = new Graph();

            var rdfNamespace = graph.NamespaceMap;
            rdfNamespace.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            rdfNamespace.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            rdfNamespace.AddNamespace("ex", new Uri("http://example.org/"));

            var subjectNode = graph.CreateUriNode("ex:subject");
            graph.Assert(subjectNode, graph.CreateUriNode("rdf:type"), graph.CreateUriNode("ex:AutorskoPravo"));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:Sifra"), graph.CreateLiteralNode(AutorskoPravo.Sifra));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:PodnosilacDetalji"), graph.CreateLiteralNode(AutorskoPravo.PodnosilacDetalji));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:Telefon"), graph.CreateLiteralNode(AutorskoPravo.Telefon));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:Email"), graph.CreateLiteralNode(AutorskoPravo.Email));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:Pseudonim"), graph.CreateLiteralNode(AutorskoPravo.Pseudonim));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:Punomocnik"), graph.CreateLiteralNode(AutorskoPravo.Punomocnik));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:Naslov"), graph.CreateLiteralNode(AutorskoPravo.Naslov));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:PodaciONaslovu"), graph.CreateLiteralNode(AutorskoPravo.PodaciONaslovu));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:PodaciOVrsti"), graph.CreateLiteralNode(AutorskoPravo.PodaciOVrsti));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:PodaciOFormiZapisa"), graph.CreateLiteralNode(AutorskoPravo.PodaciOFormiZapisa));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:PodaciOAutoru"), graph.CreateLiteralNode(AutorskoPravo.PodaciOAutoru));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:DaLiJeURadnomODnosu"), graph.CreateLiteralNode(AutorskoPravo.DaLiJeURadnomODnosu));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:NacinKoriscenja"), graph.CreateLiteralNode(AutorskoPravo.NacinKoriscenja));
            graph.Assert(subjectNode, graph.CreateUriNode("ex:PodnosilacPrijavePotpis"), graph.CreateLiteralNode(AutorskoPravo.PodnosilacPrijavePotpis));

            var rdfWriter = new CompressingTurtleWriter();
            var rdfData = new System.IO.StringWriter();
            rdfWriter.Save(graph, rdfData);

            var filePath = System.IO.Path.Combine(@"C:\autorskaprava\metadata", $"{AutorskoPravo.Naslov}.rdf");
            System.IO.File.WriteAllText(filePath, rdfData.ToString());

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            string fileN = System.IO.Path.GetFileName(filePath);
            string contentType = "application/rdf+xml";

            return File(fileBytes, contentType, fileN);
        }

        public IActionResult DownloadMetadataJson()
        {
            var metadataJson = JsonConvert.SerializeObject(AutorskoPravo, Newtonsoft.Json.Formatting.Indented);

            var filePath = Path.Combine(@"C:\autorskaprava\metadata", $"{AutorskoPravo.Naslov}.json");

            System.IO.File.WriteAllText(filePath, metadataJson);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            string fileN = Path.GetFileName(filePath);
            string contentType = "application/json";

            return File(fileBytes, contentType, fileN);
        }

        [HttpGet]
        public IActionResult DownloadXHTMLZahtev()
        {
            var filePath = Path.Combine(@"C:\autorskaprava\zahtevi", $"{AutorskoPravo.Naslov}.xhtml");

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                Encoding = Encoding.UTF8
            };

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");

                writer.WriteStartElement("head");
                writer.WriteStartElement("title");
                writer.WriteString(AutorskoPravo.Naslov);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("body");

                writer.WriteStartElement("h1");
                writer.WriteString("Zahtev za unošenje u evidenciju i deponovanje autorskih prava");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString("1) Podnosilac - ime, prezime, adresa i državljanstvo autora ili drugog nosioca autorskog prava ako je podnosilac fizičko lice, odnosno poslovno ime i sedište nosioca autorskog prava ako je podnosilac pravno lice:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.PodnosilacDetalji);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("Telefon:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.Telefon);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("Email:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.Email);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("2) Pseudonim ili znak autora, (ako ga ima):");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.Pseudonim);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("3) Ime, prezime i adresa punomoćnika, ako se prijava podnosi preko punomoćnika:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.Punomocnik);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("4) Naslov autorskog dela, odnosno alternativni naslov, ako ga ima, po kome autorsko delo može da se identifikuje*:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.Naslov);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("5) Podaci o naslovu autorskog dela na kome se zasniva delo prerade, ako je u pitanju autorsko delo prerade, kao i podataka o autoru izvornog dela:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.PodaciONaslovu);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("6) Podaci o vrsti autorskog dela (književno delo, muzičko delo, likovno delo, računarski program i dr.)*:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.PodaciOVrsti);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("7) Podaci o formi zapisa autorskog dela (štampani tekst, optički disk i slično)*:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.PodaciOFormiZapisa);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("8) Podaci o autoru ako podnosilac prijave iz tačke 1. ovog zahteva nije autori i to: prezime, ime, adresa, i državljanstvo autora (grupe autora ili koautora), a ako su u pitanju jedan ili više autora koji nisu živi, imena autora i godine smrti autora, a ako je u pitanju autorsko delo anonimnog autora navod da je autorsko delo delo anonimnog autora:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.PodaciOAutoru);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("9) Podaci da li je u pitanju autorsko delo stvoreno u radnom odnosu:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.DaLiJeURadnomODnosu);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("10) Način korišćenja autorskog dela ili nameravani način korišćenja autorskog dela:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.NacinKoriscenja);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("11) Podnosilac prijave, nosilac prava:");
                writer.WriteEndElement();

                writer.WriteStartElement("p");
                writer.WriteString(AutorskoPravo.PodnosilacPrijavePotpis);
                writer.WriteEndElement();

                //writer.WriteStartElement("hr");

                writer.WriteStartElement("p");
                writer.WriteString("12) Prilozi koji se podnose uz zahtev:");
                writer.WriteEndElement();

                foreach (var putanja in AutorskoPravo.FajloviPutanje)
                {
                    writer.WriteStartElement("p");
                    writer.WriteString(putanja.Replace("C:\\autorskaprava\\", ""));
                    writer.WriteEndElement();
                }

                //writer.WriteStartElement("hr");

                writer.WriteEndElement(); // body
                writer.WriteEndElement(); // html
                writer.WriteEndDocument();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            string fileN = Path.GetFileName(filePath);
            string contentType = "application/xhtml+xml";

            return File(fileBytes, contentType, fileN);
        }

        private byte[] KreirajPdf(string filePath)
        {
            Document document = new Document(iTextSharp.text.PageSize.A4);

            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write));
            document.Open();

            Paragraph paragraph = new Paragraph("Zahtev za unošenje u evidenciju i deponovanje autorskih prava");
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.Font.SetStyle((int)FontStyle.Underline);
            paragraph.Font.Size = 18;
            paragraph.SpacingAfter = 30;
            document.Add(paragraph);

            Paragraph podnosilacLabel = new Paragraph("1) Podnosilac - ime, prezime, adresa i državljanstvo autora ili drugog nosioca autorskog prava ako je podnosilac fizičko lice, odnosno poslovno ime i sedište nosioca autorskog prava ako je podnosilac pravno lice:\n");
            podnosilacLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(podnosilacLabel);

            Paragraph podnosilacValue = new Paragraph(AutorskoPravo.PodnosilacDetalji);
            podnosilacValue.Alignment = Element.ALIGN_LEFT;
            podnosilacValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(podnosilacValue);

            Paragraph empty = new Paragraph("________________________________________________________________________");
            empty.Alignment = Element.ALIGN_LEFT;
            document.Add(empty);

            Paragraph telefonLabel = new Paragraph("Telefon:\n");
            podnosilacValue.Alignment = Element.ALIGN_LEFT;
            document.Add(telefonLabel);

            Paragraph telefonValue = new Paragraph(AutorskoPravo.Telefon);
            telefonValue.Alignment = Element.ALIGN_LEFT;
            telefonValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(telefonValue);

            document.Add(empty);

            Paragraph emailLabel = new Paragraph("Email:\n");
            emailLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(emailLabel);

            Paragraph emailValue = new Paragraph(AutorskoPravo.Email);
            emailValue.Alignment = Element.ALIGN_LEFT;
            emailValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(emailValue);

            document.Add(empty);

            Paragraph pseudonimLabel = new Paragraph("2) Pseudonim ili znak autora, (ako ga ima):\n");
            pseudonimLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(pseudonimLabel);

            Paragraph pseudonimValue = new Paragraph(AutorskoPravo.Pseudonim);
            pseudonimValue.Alignment = Element.ALIGN_LEFT;
            pseudonimValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(pseudonimValue);

            document.Add(empty);

            Paragraph punomocnikLabel = new Paragraph("3) Ime, prezime i adresa punomoćnika, ako se prijava podnosi preko punomoćnika:\n");
            punomocnikLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(punomocnikLabel);

            Paragraph punomocnikValue = new Paragraph(AutorskoPravo.Punomocnik);
            punomocnikValue.Alignment = Element.ALIGN_LEFT;
            punomocnikValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(punomocnikValue);

            document.Add(empty);

            Paragraph naslovLabel = new Paragraph("4) Naslov autorskog dela, odnosno alternativni naslov, ako ga ima, po kome autorsko delo može da se identifikuje*:\n");
            naslovLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(naslovLabel);

            Paragraph naslovValue = new Paragraph(AutorskoPravo.Naslov);
            naslovValue.Alignment = Element.ALIGN_LEFT;
            naslovValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(naslovValue);

            document.Add(empty);

            Paragraph podaciNaslovLabel = new Paragraph("5) Podaci o naslovu autorskog dela na kome se zasniva delo prerade, ako je u pitanju autorsko delo prerade, kao i podataka o autoru izvornog dela:\n");
            podaciNaslovLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(podaciNaslovLabel);

            Paragraph podaciNaslovValue = new Paragraph(AutorskoPravo.PodaciONaslovu);
            podaciNaslovValue.Alignment = Element.ALIGN_LEFT;
            podaciNaslovValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(podaciNaslovValue);

            document.Add(empty);

            Paragraph vrstaLabel = new Paragraph("6) Podaci o vrsti autorskog dela (književno delo, muzičko delo, likovno delo, računarski program i dr.)*:\n");
            vrstaLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(vrstaLabel);

            Paragraph vrstaValue = new Paragraph(AutorskoPravo.PodaciOVrsti);
            vrstaValue.Alignment = Element.ALIGN_LEFT;
            vrstaValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(vrstaValue);

            document.Add(empty);

            Paragraph formaLabel = new Paragraph("7) Podaci o formi zapisa autorskog dela (štampani tekst, optički disk i slično)*:\n");
            formaLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(formaLabel);

            Paragraph formaValue = new Paragraph(AutorskoPravo.PodaciOFormiZapisa);
            formaValue.Alignment = Element.ALIGN_LEFT;
            formaValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(formaValue);

            document.Add(empty);

            Paragraph autorPodaciLabel = new Paragraph("8) Podaci o autoru ako podnosilac prijave iz tačke 1. ovog zahteva nije autori i to: prezime, ime, adresa, i državljanstvo autora (grupe autora ili koautora), a ako su u pitanju jedan ili više autora koji nisu živi, imena autora i godine smrti autora, a ako je u pitanju autorsko delo anonimnog autora navod da je autorsko delo delo anonimnog autora:\n");
            autorPodaciLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(autorPodaciLabel);

            Paragraph autorPodaciValue = new Paragraph(AutorskoPravo.PodaciOAutoru);
            autorPodaciValue.Alignment = Element.ALIGN_LEFT;
            autorPodaciValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(autorPodaciValue);

            document.Add(empty);

            Paragraph uRadnomOdnosuLabel = new Paragraph("9) Podaci da li je u pitanju autorsko delo stvoreno u radnom odnosu:\n");
            uRadnomOdnosuLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(uRadnomOdnosuLabel);

            Paragraph uRadnomOdnosuValue = new Paragraph(AutorskoPravo.DaLiJeURadnomODnosu);
            uRadnomOdnosuValue.Alignment = Element.ALIGN_LEFT;
            uRadnomOdnosuValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(uRadnomOdnosuValue);

            document.Add(empty);

            Paragraph nacinKoriscenjaLabel = new Paragraph("10) Način korišćenja autorskog dela ili nameravani način korišćenja autorskog dela:\n");
            nacinKoriscenjaLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(nacinKoriscenjaLabel);

            Paragraph nacinKoriscenjaValue = new Paragraph(AutorskoPravo.NacinKoriscenja);
            nacinKoriscenjaValue.Alignment = Element.ALIGN_LEFT;
            nacinKoriscenjaValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(nacinKoriscenjaValue);

            document.Add(empty);

            Paragraph nosilacPravaLabel = new Paragraph("10) Podnosilac prijave, nosilac prava:\n");
            nosilacPravaLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(nosilacPravaLabel);

            Paragraph nosilacPravaValue = new Paragraph(AutorskoPravo.PodnosilacPrijavePotpis);
            nosilacPravaValue.Alignment = Element.ALIGN_LEFT;
            nosilacPravaValue.Font.SetStyle((int)FontStyle.Bold);
            document.Add(nosilacPravaValue);

            document.Add(empty);

            Paragraph priloziLabel = new Paragraph("12) Prilozi koji se podnose uz zahtev:\n");
            priloziLabel.Alignment = Element.ALIGN_LEFT;
            document.Add(priloziLabel);

            foreach (var putanja in AutorskoPravo.FajloviPutanje)
            {
                Paragraph priloziValue = new Paragraph(putanja.Replace("C:\\autorskaprava\\", ""));
                priloziValue.Alignment = Element.ALIGN_LEFT;
                priloziValue.Font.SetStyle((int)FontStyle.Bold);
                document.Add(priloziValue);
            }

            document.Add(empty);

            document.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return fileBytes;
        }

        [HttpGet]
        public IActionResult DownloadZahtev()
        {
            var filePath = Path.Combine(@"C:\autorskaprava\zahtevi", $"{AutorskoPravo.Naslov}.pdf");

            AutorskoPravo.Pdf = filePath;

            string contentType = "application/octet-stream";
            string fileN = Path.GetFileName(filePath);

            var fileBytes = KreirajPdf(filePath);

            return File(fileBytes, contentType, fileN);
        }

        [HttpGet]
        public IActionResult DownloadIzvestaj(DateTime start, DateTime end)
        {
            string filename = "";

            var filePath = Path.Combine("C:\\autorskaprava", filename);

            IzvestajModel izvestaj = _apiClient.VratiIzvestaj(start, end).Result;

            Document document = new Document(iTextSharp.text.PageSize.A4);
            string fileName = "Izvestaj_" + start.ToString("yyyy-MM-dd") + "_" + end.ToString("yyyy-MM-dd");

            var path = @"C:\AutorskaPravaIzvestaji\" + fileName + ".pdf";

            PdfWriter.GetInstance(document, new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write));
            document.Open();

            Paragraph paragraph = new Paragraph("Izvestaj zahteva");
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.Font.SetStyle((int)FontStyle.Underline);
            paragraph.Font.Size = 18;
            paragraph.SpacingAfter = 30;
            document.Add(paragraph);

            Paragraph odDo = new Paragraph(start.ToString("yyyy-MM-dd") + " - " + end.ToString("yyyy-MM-dd"));
            odDo.Alignment = Element.ALIGN_CENTER;
            odDo.Font.SetStyle((int)FontStyle.Regular);
            odDo.Font.Size = 15;
            odDo.SpacingAfter = 30;
            document.Add(odDo);

            Paragraph podneto = new Paragraph("Broj podnetih zahteva: " + izvestaj.BrojPodnetihZahteva);
            podneto.Alignment = Element.ALIGN_LEFT;
            document.Add(podneto);

            Paragraph prihvaceno = new Paragraph("Broj prihvacenih zahteva: " + izvestaj.BrojPrihvacenihZahteva);
            prihvaceno.Alignment = Element.ALIGN_LEFT;
            document.Add(prihvaceno);

            Paragraph odbijeno = new Paragraph("Broj odbijenih zahteva: " + izvestaj.BrojOdbijenihZahteva);
            odbijeno.Alignment = Element.ALIGN_LEFT;
            document.Add(odbijeno);

            document.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            string fileN = Path.GetFileName(path);
            string contentType = "application/octet-stream";

           

            return File(fileBytes, contentType, fileN);
        }

        [HttpGet]
        public List<AutorskoPravoModel> PretraziPrijave(string searchTerm)
        {
            var isUserAdmin = User.IsInRole("Admin");

            List<AutorskoPravoModel> prijave = _apiClient.PretraziPrijave(searchTerm, isUserAdmin, User.Identity.Name).Result;

            return prijave;
        }
    }
}