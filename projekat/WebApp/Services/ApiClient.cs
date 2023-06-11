using System.Net.Http.Headers;
using Business.Models;

namespace WebApp.Services
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;

            _client.BaseAddress = new Uri("https://localhost:7232/api/");
            _client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public async Task<List<AutorskoPravoModel>> VratiSvePrijave(bool isUserAdmin, string userName)
        {
            string path = _client.BaseAddress + "AutorskoPravo/vratiSvePrijave?isUserAdmin=" + isUserAdmin + "&userName=" + userName;
            HttpResponseMessage response = await _client.GetAsync(path);

            List<AutorskoPravoModel> prijave = new List<AutorskoPravoModel>();

            if (response.IsSuccessStatusCode)
            {
                prijave = await response.Content.ReadFromJsonAsync<List<AutorskoPravoModel>>();
            }

            return prijave;
        }

        public async Task<AutorskoPravoModel> NovaPrijava(AutorskoPravoModel prijava)
        {
            string path = _client.BaseAddress + $"AutorskoPravo/dodajPrijavu";

            HttpResponseMessage response = await _client.PostAsJsonAsync(path, prijava);

            AutorskoPravoModel novaPrijava = new AutorskoPravoModel();

            if (response.IsSuccessStatusCode)
            {
                novaPrijava = await response.Content.ReadFromJsonAsync<AutorskoPravoModel>();
            }

            return novaPrijava;
        }

        public async Task<bool> PodnesiResenje(ResenjeModel resenje)
        {
            string path = _client.BaseAddress + $"AutorskoPravo/podnesiResenje";

            HttpResponseMessage response = await _client.PostAsJsonAsync(path, resenje);

            bool resenjeResult = false;

            if (response.IsSuccessStatusCode)
            {
                resenjeResult = await response.Content.ReadFromJsonAsync<bool>();
            }

            return resenjeResult;
        }

        public async Task<AutorskoPravoModel> VratiPrijavu(int id)
        {
            string path = _client.BaseAddress + $"AutorskoPravo/vratiPrijavu?id={id}";
            HttpResponseMessage response = await _client.GetAsync(path);

            AutorskoPravoModel prijava = new AutorskoPravoModel();

            if (response.IsSuccessStatusCode)
            {
                prijava = await response.Content.ReadFromJsonAsync<AutorskoPravoModel>();
            }

            return prijava;
        }

        public async Task<List<AutorskoPravoModel>> PretragaMetapodaci(AutorskoPravoModel metapodaci)
        {
            string path = _client.BaseAddress + $"AutorskoPravo/pretragaMetapodaci";

            metapodaci.FajloviPutanje = new List<string>();

            HttpResponseMessage response = await _client.PostAsJsonAsync(path, metapodaci);

            List<AutorskoPravoModel> rekordi = new List<AutorskoPravoModel>();

            if (response.IsSuccessStatusCode)
            {
                rekordi = await response.Content.ReadFromJsonAsync<List<AutorskoPravoModel>>();
            }

            return rekordi;
        }

        public async Task<List<AutorskoPravoModel>> PretraziPrijave(string searchTerm, bool isUserAdmin, string userName)
        {
            string path = _client.BaseAddress + "AutorskoPravo/pretraziPrijave?pojam=" + searchTerm + "&isUserAdmin=" + isUserAdmin + "&userName=" + userName;
            HttpResponseMessage response = await _client.GetAsync(path);

            List<AutorskoPravoModel> prijave = new List<AutorskoPravoModel>();

            if (response.IsSuccessStatusCode)
            {
                prijave = await response.Content.ReadFromJsonAsync<List<AutorskoPravoModel>>();
            }

            return prijave;
        }

        public async Task<IzvestajModel> VratiIzvestaj(DateTime start, DateTime end)
        {
            string path = _client.BaseAddress + "AutorskoPravo/vratiIzvestaj?start=" + start.ToString("yyyy/MM/dd") + "&end=" + end.ToString("yyyy/MM/dd");
            HttpResponseMessage response = await _client.GetAsync(path);

            IzvestajModel izvestaj = new IzvestajModel();

            if (response.IsSuccessStatusCode)
            {
                izvestaj = await response.Content.ReadFromJsonAsync<IzvestajModel>();
            }

            return izvestaj;
        }
    }
}
