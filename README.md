# Grafika
Projekat iz Grafike, zadatak 9.2

Modelovanje statičke 3D scene (prva faza): 

- Uključiti testiranje dubine i sakrivanje nevidljivih površina. Definisati projekciju u perspektivi (fov=50, near=0.5, a vrednost far po potrebi) i viewport-om preko celog prozora unutar Resize() metode.
Koristeći AssimpNet bibloteku i klasu AssimpScene, učitati model fudbalske lopte. Ukoliko je model podeljen u nekoliko fajlova, potrebno ih je sve učitati i iscrtati. Skalirati model, ukoliko je neophodno, tako da bude vidljiv u celosti. Model lopte postaviti na podlogu ispred gola.

- Modelovati sledeće objekte: 
podlogu koristeći GL_QUADS primitivu, i
okvir gola bez mreže koristeći Cylinder i Disk klase (nalazi se na podlozi).

- Ispisati vektorski tekst crnom bojom u gornjem desnom uglu prozora (redefinisati projekciju korišćenjem gluOrtho2D() metode). Font je Arial, 10pt, bold. Tekst treba da bude oblika: 
Predmet: Racunarska grafika 
Sk.god: 2017/18
Ime: <ime_studenta> 
Prezime: <prezime_studenta> 
Sifra zad: <sifra_zadatka> 


Definisanje materijala, osvetljenja, tekstura, interakcije i kamere u 3D sceni  (druga faza):

- Uključiti color tracking mehanizam i podesiti da se pozivom metode glColor() definiše ambijentalna i difuzna komponenta materijala.

- Definisati tačkasti svetlosni izvor bele boje i pozicionirati ga desno od gola (na pozitivnom delu x-ose scene). Svetlosni izvor treba da bude stacionaran (tj. transformacije nad modelom ne utiču na njega). Definisati normale za podlogu. Za Quadric objekte podesiti automatsko generisanje normala.

- Za teksture podesiti wrapping da bude GL_REPEAT po obema osama. Podesiti filtere za teksture tako da se koristi najbliži sused filtriranje. Način stapanja teksture sa materijalom postaviti da bude GL_MODULATE. 

- Golu pridružiti teksturu bele plastike. Definisati koordinate tekstura. 

- Podlozi pridružiti teksturu trave (slika koja se koristi je jedan segment trave). Pritom obavezno skalirati teksturu (shodno potrebi). Skalirati teksturu korišćenjem Texture matrice.

- Pozicionirati kameru iza lopte i usmeriti je ka golu. Koristiti gluLookAt() metodu.

- Pomoću ugrađenih WPF kontrola, omogućiti sledeće:
izbor boje ambijentalne komponente reflektorskog svetlosnog izvora,
izbor faktora (uniformnog) skaliranja lopte, i
izbor brzine automatske rotacije lopte oko svoje y-ose.
Omogućiti interakciju sa korisnikom preko tastature: sa F4 se izlazi iz aplikacije, tasterima 
E/D vrši se rotacija za 5 stepeni oko horizontalne ose, tasterima S/F vrši se rotacija za 5 stepeni oko vertikalne ose, a tasterima +/- približavanje i udaljavanje centru scene. Ograničiti rotaciju tako da se nikada ne vidi donja strana podloge. Dodatno ograničiti rotaciju oko horizontalne ose tako da scena nikada ne bude prikazana naopako.

- Definisati reflektorski svetlosni izvor (cut-off=35º) plave boje iznad lopte, usmeren ka lopti. 

- Način stapanja teksture sa materijalom gola postaviti na GL_ADD. 

- Kreirati animaciju automatskog odskakanja lopte u vertikalnom pravcu  i rotacije lopte oko svoje y-ose. 

- Kreirati animaciju koja uključuje kretanje lopte ka golu i prolazak kroz levi gornji ugao gola. U toku animacije, onemogućiti interakciju sa korisnikom (pomoću kontrola korisničkog interfejsa i tastera). Animacija se može izvršiti proizvoljan broj puta i pokreće se pritiskom na taster V. 



