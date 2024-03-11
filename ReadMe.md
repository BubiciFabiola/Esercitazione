## Comando per la creazione su docker del database (ItaliaTreni)

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pinco@pallino1" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest


## Info utili per Console
Per quanto riguarda la console per il caricamento dei dati dal file csv, la mia idea è stata quella di creare la repository in "Esercitazione\ItaliaTreni-console\ItaliaTreni.Console\file" dove sono presenti due sottocartelle:
- in: qui bisogna mettere i file csv da caricare. Il vincolo è che il nome del file deve essere "Import dd-MM-yyyy.csv", ho scelto di inserire questo vincolo per far si che i file indicano la data in cui sono stati caricati e per poter rendere più facile la consultazione anche in futuro
- archive: qui se tutta la lavorazione è andata bene verrà spostato il file importato, in modo da avere una cartella di storico che potrebbe tornare utile in futuro per verifiche