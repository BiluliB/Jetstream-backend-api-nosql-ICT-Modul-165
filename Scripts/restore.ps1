# Setze das Basisverzeichnis, in dem die Backups gespeichert sind
$backupBaseDir = "..\Backups"

# Liste alle Backup-Ordner auf und speichere die Namen in einer Variablen
$backupDirs = Get-ChildItem -Path $backupBaseDir -Directory | Sort-Object Name
$backupList = $backupDirs | Select-Object Name, FullName

# Zeige ein Popup-Fenster mit den verfügbaren Backup-Ordnern zur Auswahl
$selectedBackup = $backupList | Out-GridView -Title "Wähle ein Backup zum Wiederherstellen" -OutputMode Single

# Überprüfe, ob eine Auswahl getroffen wurde
if ($null -ne $selectedBackup) {
    $restoreDir = $selectedBackup.FullName

    # Führe mongorestore aus
    mongorestore --host localhost --port 27017 --username superadmin --password "password" --authenticationDatabase admin --db JetStreamApiDb --drop --dir $restoreDir\JetStreamApiDb --restoreDbUsersAndRoles

    if (!$?) {
        mongorestore --host localhost --port 27017 --db JetStreamApiDb --drop --dir $restoreDir\JetStreamApiDb  --restoreDbUsersAndRoles
    }

    # Optional: Feedback in der Konsole
    Write-Host "Datenbank wurde wiederhergestellt aus: $restoreDir"
}
else {
    Write-Host "Keine Auswahl getroffen."
}

# Halte das Skript an, bis der Benutzer eingreift, um das Ergebnis zu sehen
Read-Host "Drücken Sie Enter, um fortzufahren..."
