# Prüft, ob das Skript als Administrator ausgeführt wird, und fordert andernfalls erhöhte Berechtigungen an
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    # Erhöht die Berechtigungen des Skripts durch Neustart mit Administratorrechten
    Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
    # Beendet die aktuelle Instanz des Skripts
    exit
}

# Festlegen des Zeitpunkts, zu dem die Backup-Aufgabe ausgeführt werden soll
$ScheduledTime = New-ScheduledTaskTrigger -At 4am -Daily

# Bestimmt den Speicherort des Backup-Skripts
$BackupScriptPath = Join-Path $PSScriptRoot "backup.ps1"

# Spezifiziert die auszuführende Aktion, in diesem Fall das Ausführen des Backup-Skripts
$ExecuteAction = New-ScheduledTaskAction -Execute "Powershell.exe" -Argument " -File `"$BackupScriptPath`""

# Registrieren der neuen geplanten Aufgabe im Task Scheduler
Register-ScheduledTask -TaskName "MongoDBDailyBackup" -Description "Tägliche Sicherung der MongoDB-Datenbank" -Trigger $ScheduledTime -Action $ExecuteAction -RunLevel Highest -User "SYSTEM"

# Gibt eine Meldung aus und wartet auf Benutzereingabe, nützlich für Testzwecke
Write-Host "Geplante Backup-Aufgabe 'MongoDBDailyBackup' wurde erfolgreich erstellt."
Read-Host -Prompt "Drücken Sie eine beliebige Taste, um fortzufahren..."
