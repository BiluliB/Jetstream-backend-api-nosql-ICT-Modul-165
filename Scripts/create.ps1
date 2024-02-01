Try {
    Write-Host "Starte Bereinigung der Datenbank, Benutzer und Rollen...`n" -ForegroundColor Cyan
    mongosh --quiet --file ".\MongoDbScripts\purge.js"
    Write-Host "`nBereinigung erfolgreich abgeschlossen." -ForegroundColor Green
}
Catch {
    Write-Host "Fehler bei der Bereinigung: $_" -ForegroundColor Red
}

Try {
    $collectionNames = Get-ChildItem -Path .\MongoDbScripts\CollectionsSchema\*.js | ForEach-Object {
        $_.BaseName
    }

    # Alle Collections auf einmal auflisten
    Write-Host "`nErstelle Collections und Schema: $($collectionNames -join ', ')..." -ForegroundColor Cyan
    
    # Durch jede JS-Datei iterieren und das Skript ausführen
    foreach ($collectionName in $collectionNames) {
        $filePath = ".\MongoDbScripts\CollectionsSchema\$collectionName.js"
        mongosh --quiet --file $filePath
    }

    # Bestätigungsmeldung nach erfolgreicher Ausführung aller Skripte
    Write-Host "Alle Collections erfolgreich erstellt: $($collectionNames -join ', ')." -ForegroundColor Green
}
Catch {
    Write-Host "`nFehler beim Erstellen der Collections und Schemas: $_" -ForegroundColor Red
}


Try {
    Write-Host "`nErstelle indexes..." -ForegroundColor Cyan
    mongosh --quiet --file ".\MongoDbScripts\index.js"
    Write-Host "Index wurde erfolgreich erstellt." -ForegroundColor Green
}
Catch {
    Write-Host "`nFehler bei der Ausführung von index.js: $_" -ForegroundColor Red
}

Try {
    $seedNames = Get-ChildItem -Path .\MongoDbScripts\Migration\*.js | ForEach-Object {
        $_.BaseName
    }

    Write-Host "`nErstelle Daten in der Datenbank $($seedNames -join ', ')..." -ForegroundColor Cyan

    foreach ($seedName in $seedNames) {
        $filePath = ".\MongoDbScripts\Migration\$seedName.js"
        mongosh --quiet --file $filePath
    }
    Write-Host "Daten erfolgreich in die Datenbank geseedet." -ForegroundColor Green
}
Catch {
    Write-Host "`nFehler bei der Ausführung von seedDatabase.js: $_" -ForegroundColor Red
}

Try {
    Write-Host "`nErstelle Benutzer und Rollen..." -ForegroundColor Cyan
    mongosh --quiet --file ".\MongoDbScripts\UsersAndRoles.js"
    Write-Host "Benutzer und Rollen erfolgreich erstellt." -ForegroundColor Green
}
Catch {
    Write-Host "`nFehler beim Erstellen von Benutzern und Rollen: $_" -ForegroundColor Red
}

pause