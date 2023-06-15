## Hoe het programma runnen:
```bash
dotnet run <myfile>
```

## Opmerkingen:
Om de code zo leesbaar (en simpel) mogelijk te houden, heb ik een paar beslissingen genomen die ik voor een groot project niet zou nemen.
Hieronder een woordje uitleg daarover:
1. Ik heb met opzet de verschillende klassen in één file gestoken, omdat het mij voor deze kleine opdracht gewoon handiger leek.
    Moest dit een groter project zijn, of ik zou hier nog verder op moeten bouwen, dan had ik het opgesplitst in verschillende files.
2. De parameter van de maximale woordlengte is hardgecodeerd in de code. Ik zou die ook laten meegeven als parameter aan de app.
3. Wanneer ik een file lijn per lijn inlees, wordt er niet geverifieerd dat de streamreader en de lines die eruit komen non-null zijn.
    Maar aangezien dit maar een test is, leek het mij beter om die boiler-plate error-handling achterwege te laten. Zo blijft de code leesbaarder.
