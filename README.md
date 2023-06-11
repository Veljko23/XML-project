# XML-project

Članovi tima:
Veljko Radić SL10/2014

Za pokretanje aplikacije potrebno je:

Visual studio 2022: https://visualstudio.microsoft.com/vs/community/?fbclid=IwAR2dIgBsdgomSQZ1MBDzfR0E3Brn_SFnEXtkxeKAlUpqVRItdEK5DtYGP9A

SQL Server Express: https://www.microsoft.com/en-us/sql-server/sql-server-downloads

SQL Server Management Studio (SSMS): https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16&fbclid=IwAR2eU9yfq4LCgm0Jaz3X49uuFo0oYCCICHmjhxhDOirazEH6rxjIqItybOk

U C folderu računara napraviti folder "autorskaprava" i u njemu dva foldera: "metadata" i "zahtevi".
U C folderu računara napraviti folder "AutorskaPravaIzvestaji".

U SSMS-u kreirati konekciju pod nazivom DESKTOP-3SMU9E8\\SQLEXPRESS. Ako ne može da se kreira pod ovim nazivom, u tom slučaju:
u fajlovima appsettings.json (u API i WebApp) zameniti postojeci naziv konekcije sa nazivom kreirane konekcije: "SERVER=naziv_konekcije;

U VS-u uraditi migraciju baze: View -> Other Windows -> Package Manager Console sa komandom update-database.

Podesiti API i WebApp kao startne projekte: Desni klik na Solution -> Properties -> Multiple startup projects.

Pri pokretanju, otvoriće se dva Chrom-a, jedan za API, drugi za WebApp. Na početnoj strani postoji login dugme, koje će odvesti
na formu za prijavu, gde se može naći dugme za registraciju.

Za registraciju admina koristiti URL: https://localhost:7127/Identity/Account/RegisterAdmin

NPOMENA: Nakon logovanja, registracije ili odjave, kliknuti na "home" dugme kako bi zavrsio operaciju.
