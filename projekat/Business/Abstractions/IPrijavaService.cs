using Business.Models;

namespace Business.Abstractions;

public interface IPrijavaService
{
    AutorskoPravoModel DodajPrijavu(AutorskoPravoModel model);
    AutorskoPravoModel VratiPrijavu(int id);
    List<AutorskoPravoModel> VratiSvePrijave(bool isUserAdmin, string userName);
    List<AutorskoPravoModel> VratiSveZavedenePrijave();
    List<AutorskoPravoModel> PretraziPrijave(string pojam, bool isUserAdmin, string userName);
    List<AutorskoPravoModel> PretraziPoMetapodacima(AutorskoPravoModel model);
    bool PodnesiResenje(ResenjeModel model);
    IzvestajModel VratiIzvestaj(string start, string end);
}