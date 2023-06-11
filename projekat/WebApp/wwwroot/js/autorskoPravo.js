var AutorskoPravo = {

}

AutorskoPravo.pretraziPrijave = function() {

    var searchTerm = $("#pretraziPrijaveInput").val();

    window.location = "/AutorskaPrava/Index?pojam=" + searchTerm;
}

AutorskoPravo.generisiIvestaj = function() {

    var start = $("#startDate").val();
    var end = $("#endDate").val();

    window.location = "/AutorskaPrava/DownloadIzvestaj?start=" + start + "&end=" + end;
}

AutorskoPravo.tipResenja = function() {

    var value = $("#rezultatZahteva").val();

    //Odbijen zahtev
    if (value == "0") {
        $("#odobrenZahtev").css("display", "none");
        $("#odbijenZahtev").css("display", "block");
    }
    else {
        $("#odbijenZahtev").css("display", "none");
        $("#odobrenZahtev").css("display", "block");
    }
}

AutorskoPravo.podnesiResenje = function() {

    var value = $("#rezultatZahteva").val();

    var autorskoPravoId = parseInt($("#autorskoPravoId").val());

    var model = {
        AutorskoPravoId: autorskoPravoId,
        Sifra: "",
        DatumObradeZahteva: null,
        Odobren: false,
        Referenca: "/AutorskaPrava/Prijava?id=" + autorskoPravoId,
        SluzbenikIme: "",
        SluzbenikPrezime: "",
        ObrazlozenjeOdbijanja: ""
    };

    if (value == "1") //Odobren
    {
        model.Sifra = $("#sifraOdobrenjaZahteva").val();
        model.DatumObradeZahteva = $("#datumOdobravanja").val();
        model.Odobren = true;
        model.SluzbenikPrezime = $("#sluzbenikPrezimeOdobrenje").val();
        model.SluzbenikIme = $("#sluzbenikImeOdobrenje").val();
        model.ObrazlozenjeOdbijanja = "";
    }
    else
    {
        model.Sifra = $("#sifraOdbijanjaZahteva").val();
        model.DatumObradeZahteva = $("#datumOdbijanja").val();
        model.ObrazlozenjeOdbijanja = $("#razlogOdbijanja").val();
        model.SluzbenikPrezime = $("#sluzbenikPrezimeOdbijanje").val();
        model.SluzbenikIme = $("#sluzbenikImeOdbijanje").val();
    }

    $.ajax({
        url: '/AutorskaPrava/PodnesiResenje',
        data: model,
        type: 'POST',
        success: function(result) {
            window.location.reload();
        },
        error: function(err) {
            console.log(err);
        }
    });
}

AutorskoPravo.pretraziPrijaveMetapodaci = function() {
    var queryParams = $('#searchForm').serialize();

    window.location.href = '/AutorskaPrava/Index?metadataSearch=true&' + queryParams;
}


$('form').on('submit', function(e) {
    e.preventDefault();

    var formData = new FormData(this);

    $.ajax({
        url: $(this).attr('action'),
        type: 'POST',
        data: formData,
        contentType: false, // Necessity for file upload
        processData: false, // Necessity for file upload
        success: function(response) {
            if (response == "") {
                $("#uploadSuccess").css("display", "block");
            }
            else {
                $("#uploadSuccess").css("display", "none");
            }
        }
    });
});


AutorskoPravo.dodajPrijavu = function() {

    var podnosilac = $("#podnosilacValue").val();
    var telefon = $("#podnosilacTelefon").val();
    var email = $("#podnosilacEmail").val();
    var pseudonim = $("#pseudonimValue").val();
    var punomocnikDetalji = $("#punomocnikDetalji").val();
    var naslovAutorskogDela = $("#naslovAutorskogDela").val();
    var podaciONaslovuAutorskogDela = $("#podaciONaslovuAutorskogDela").val();
    var vrstaAutorskogDela = $("#vrstaAutorskogDela").val();
    var podaciOAutoruDetalji = $("#podaciOAutoruDetalji").val();
    var formaAutorskogDela = $("#formaAutorskogDela").val();
    var daLiJeRadniOdnos = $("#daLiJeRadniOdnos").val();
    var nacinKoriscenjaAutorskogDela = $("#nacinKoriscenjaAutorskogDela").val();
    var potpis = $("#potpis").val();

    var prijava = {
        PodnosilacDetalji: podnosilac,
        Telefon: telefon,
        Email: email,
        Pseudonim: pseudonim,
        Punomocnik: punomocnikDetalji,
        Naslov: naslovAutorskogDela,
        PodaciONaslovu: podaciONaslovuAutorskogDela,
        PodaciOVrsti: vrstaAutorskogDela,
        PodaciOFormiZapisa: formaAutorskogDela,
        PodaciOAutoru: podaciOAutoruDetalji,
        DaLiJeURadnomODnosu: daLiJeRadniOdnos,
        NacinKoriscenja: nacinKoriscenjaAutorskogDela,
        PodnosilacPrijavePotpis: potpis
    };

    $.ajax({
        url: '/AutorskaPrava/NovaPrijava',
        data: prijava,
        type: 'POST',
        success: function(result) {
            $("#novaPrijavaSuccess").css("display", "block");
        },
        error: function(err) {
            $("#novaPrijavaSuccess").css("display", "block");
            $("#novaPrijavaSuccess").text("Desila se greška, pokušajte ponovo ili kontaktirajte admina.");
            console.log(err);
        }
    });
}