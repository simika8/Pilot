
------

unit teszt
jwt authentication

rekurzív navigation properties
dátum kezelés

--------------------
Apache jmeter 

okos odata controller készítése
- megszakítható
- async futás
- adat hozzáférhetőség (létrehozás dátuma/ készlet)

keresési sorrend: szókezdet találat, belső szókezdet találat (hogy oldották meg Juhász Tamás és Konrád Gábor?)

index létrehozás


postman


--------------------
performance issues
- megszakítható legyen
- async/sync működés
- gzip
- apache jmeter
- memória profil
- Normál lekérdezési módok indexelése

--------------------

frontend illesztés
-------------------------------
Juhász Tamás, Konrád Gábor:
Sziasztok! Backend oktatást fogok tartani, szüségem lenne segítségre.
Elvileg ti (/ valamelyikőtök) hoztátok össze a tegnapelőtt bemutatott cikkkereső frontendet. Egy hasonlót kellene összehozni majd a backend bemutató számára. (jövő hét közepe környékén kellene ezt elkészíteni)
A másik az, hogy érdekelne, a technikai megvalósítása annak a problémának, hogy ha beírják az általános keresőbe, hogy "Kal" akkor előbb hozza a "Kal" kezdetű cikkeket, majd azokat, amiknek a cikknév közepében van "Kal" kezdetű szó. Ez utóbbi infóra a jövő hét elején lenne szükségem!
Előre is köszönöm a segítséget!

Simi  4:44 PM
Meg még van ez is:
- szeretném elérni, hogy a frontendben a devexpress táblázat általános keresője ne különböző adatmezőkre írjon odata szűrést, hanem adja át a az urlben search vagy $search paraméterben (ahogy azt a syncfusion komponens is csinálja.) Ha máshogy nem, a táblázaton le kell tiltani az általános keresőt, és fölé kell rakni egy saját készítésű bekérő mezőt, és az ott beírt adatot továbbadni...
- A frontendnek képesnek kell lennie arra, hogy pl a cikkeresőben a frontend által nyilvántartott kiválasztott raktár (pl comboboxban látszik az ablak tetején) Idjét átadja raktarid paraméterben.
- A frontend jó lenne, ha képes lenne kezelni azt, hogy ha lekérdez egy index nélküli táblázatot, ami másodpercekig számoltatja a backendet (és az adatbázist), és mielőtt választ kapnánk változik a lekérdezés, akkor a korábban elküldött odata lekérdezéseket szakítsa meg (hogy adatbázis ne számolja tovább, amire már nincs szükség).

----------
követelmények:
visual stidio 2022, asp.net 6, pgadmin, local postgresql, postman, apachge jmeter

