## Cum merge asta:

* Jucatorul prinde obiectul (XRGrabInteractable)
* Jucatorul da drumul obiectului
* ThrowableObject marcheaza obiectul ca fiind in zbor
* Obiectul loveste prima suprafata de scor (layer ScoringSurface)
* ThrowableObject trimite punctul de contact si multiplicatorul catre ScoreManager
* ScoreManager calculeaza distanta fata de linia de start si scorul, apoi publica un eveniment
* ScoreView primeste evenimentul si afiseaza scorul pe ecran


### Scripturi bagate: 

**ThrowableObject**	Detecteaza aruncarea si aterizarea, raporteaza catre ScoreManager
**HardwareData**    ScriptableObject cu datele unui tip de hardware (nume, masa, multiplicator)
**ScoreManager**    Singleton, tine scorul total, publica ThrowScored si ScoreReset
**ThrowTrack**      Pus pe StartLine, calculeaza distanta inainte fata de linia de start
**ScoreCalculator** Formula de scor
**ScoreView**       Afiseaza scorul pe un Canvas World Space


## Guid de adaugare proiectile atomice noi
1, Creaza Obiectul nou
2, Salveaza-l in Assets/_Project/Data/Hardware/ cu numele SO_Hardware_<Nume>
3, Completeaza:
    Display Name
    Score Multiplier: 
    Mass Kg: masa fizica
3, Pe Obiect trebuie:
    ThrowableObject
        Hardware Data cu asset-ul
        Scoring Surfaces cu layerul ScoringSurface (aici imi exploda mie)


## La ScoreModifier: 
putem adauga o diferinta de puncte
putem adauga o diferenta de Multiplier
putem cere ca obiectul sa treaca dintr-o directieanumita
putem face ca un obiect(serverul, tinta(targget-poate in viitor niste stele) sa dispara)
Adaugam asta pe un obiect solid(se loveste) sau tregger(trece prin)