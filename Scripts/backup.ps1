# Setze das Basisverzeichnis für das Backup
$backupBaseDir = "..\Backups"

# Erzeuge einen Zeitstempel
$timestamp = Get-Date -Format "yyyyMMddHHmmss"

# Erzeuge den Backup-Ordnerpfad mit dem Zeitstempel
$backupDir = Join-Path -Path $backupBaseDir -ChildPath ("MongoBackup_" + $timestamp)

# Erstelle den Backup-Ordner
New-Item -ItemType Directory -Path $backupDir

# Führe mongodump aus
mongodump --host localhost --port 27017 --username superadmin --password password --authenticationDatabase admin --dumpDbUsersAndRoles --db JetStreamApiDb --out $backupDir

# Optional: Feedback in der Konsole
Write-Host "Backup erstellt in: $backupDir"
Read-Host