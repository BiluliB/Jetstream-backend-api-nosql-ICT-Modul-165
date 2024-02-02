## JetStreamApiMongoDb Backend-API

## Haftungsausschluss

Bitte beachten Sie, dass das Frontend für die JetStreamApiMongoDb Backend-API derzeit in Entwicklung ist und möglicherweise nur teilweise funktionsfähig ist. Es wird daran gearbeitet, die volle Funktionalität so schnell wie möglich bereitzustellen. Danke für Ihr Verständniss

### Überblick

### Alle Dokumente des Projekts (Projektdokumentation, Präsentataion, NoSQL/MongoDB-Skripts, Connectin String) befinden sich im Projektordner -> Files

Die Jetstream Backend API ist eine ASP.NET Core Web API, die speziell für das ICT-Modul 165 entwickelt wurde. Um den wachsenden Anforderungen gerecht zu werden und das Backend aus dem Module 295, wird das Backend-System auf ein NoSQL-Datenbanksystem migriert. Dieses Projekt umfasst die Datenbankmigration, Anwendungsentwicklung und Integrationstests, um eine effiziente Auftragsverwaltung sicherzustellen und die Expansion zu unterstützen.

### Hauptfunktionen

- **Datenbank-Setup**: Automatisierte Skripte für das Einrichten der MongoDB-Datenbank, einschliesslich, cleanup, erstellen der Benutzer, Rollen, Collections, Schemas, Indexes und Daten.
- **Benutzerverwaltung**: Funktionen für die Benutzererstellung und Rollenzuweisung in MongoDB.

### Technologie-Stack

- ASP.NET Core für die Backend-API.
- MongoDB NoSQL für das Datenbankmanagement.
- PowerShell für Datenbank-Setup und Wartungsskripte.

# Einrichtung und Installation der JetStreamApiMongoDb Backend-API

## Voraussetzungen:

- Installieren Sie .Net 8.0 SDK von der offiziellen [.NET-Website](https://dotnet.microsoft.com/download).
- Installieren Sie MongoDB auf Ihrem System.
- Setzen Sie die Umgebungsvariablen für MongoDB Tools, damit die Skripte ordnungsgemäss funktionieren wie mongosh, mongodump und mongorestore [MongoDB -Webseite](https://www.mongodb.com/docs/database-tools/installation/installation-windows/).
- PowerShell einrichten um Skripte Ausführen zu können von Datenbank-Setup-Skripten.
- Ein geeigneter Code-Editor, wie Visual Studio oder Visual Studio Code.

## Klonen oder Herunterladen des Projekts:

- Laden Sie das Projekt herunter oder klonen Sie es aus dem [GitHub-Repository](https://github.com/BiluliB/JetStreamApiMongoDb).

## Abhängigkeiten installieren:

- Öffnen Sie die Kommandozeile oder das Terminal im Projektverzeichnis.
- Führen Sie die PowerShell-Skripte im `Scripts`-Verzeichnis aus, um die MongoDB-Datenbank einzurichten.

### Datenbankkonfiguration

Befolgen Sie diese Schritte, um die MongoDB-Datenbank korrekt einzurichten und zu konfigurieren:

1. **Ausführen des PowerShell-Skripts (`create.ps1`)**:

   - Dieses Skript Migriert automatisiert die SQL Datenbank zur MongoDB NoSQL Datenbank, sie wird bereinigt, die Sammlungen und Schemas, das Seeding von Daten sowie die Benutzer- und Rollenerstellung werden in MongoDB erstellt.
   - Führen Sie dieses Skript aus, um die MongoDB-Datenbank mit den notwendigen Sammlungen und Benutzerrollen zu initialisieren.

2. **MongoDB-Sammlungsschemas**:
   - Die Migration der Sammlungen wie `order_submissions`, `priorities`, `services`, `statuses` und `users` werden mit spezifischen Schemas und Validierungsregeln erstellt.
   - Diese Sammlungen sind für die Funktionalität der API von wesentlicher Bedeutung und speichern Daten zu Dienstanfragen und Benutzerinformationen.

### MongoDB-Benutzerkonfiguration

Falls Sie auf Ihrem lokalen MongoDB Server die Authentifizirung in Ihrer `mongod.cfg` aktiviert haben:

```
security:
  authorization: enabled
```

ist es wichtig das sie einen "superadmin" erstellen damit die kontroller der Datenbank vorhanden ist.
Das wird mit dem nachfolgenden Skript durchgeführt was beim ausführen des `create.ps1` gemacht wird.

Das Skript `UsersAndRoles.js` wird vom `create.ps1` ausgeführt nachfolgende Operationen werden durchegeführt:

- Erstellt einen Super-Admin-Benutzer in der MongoDB-Admin-Datenbank mit der Rolle root.
- Definiert eine benutzerdefinierte Rolle mit folgenden Privilegien "find", "insert", "update", "remove", in der `JetStreamApiDb`-Datenbank.
- Weist diese Rolle dem erstellten Benutzer namens "johny" zu.

### Backup und Restore der Datenbank

## Starten der Anwendung:

- Starten Sie die Anwendung über Ihre Entwicklungsumgebung.
- Stellen Sie sicher, dass MongoDB läuft und die Datenbank gemäss den Skripten eingerichtet ist.

## Testen der JetStreamApiMongoDb Backend-API

### Testen mit Postman

- **Postman Collection**: Das Repository enthält eine Postman-Collection zum Testen der API-Endpunkte.
- **Importieren der Collection**: Importieren Sie die bereitgestellte JSON-Datei aus dem `Postman`-Ordner in Postman für effizientes Testen.

### Testen mit Swagger

- **Swagger-Integration**: Die API umfasst Swagger zum Testen von Endpunkten über eine benutzerfreundliche Oberfläche.
- **Zugriff auf Swagger**: Starten Sie die API und navigieren Sie zur Swagger-UI-URL, um die API zu testen und die Dokumentation anzusehen.

### Unit-Tests

- **Login-Test**: Es wurde ein Unit-Test für die Überprüfung des Login-Vorgangs implementiert.
- **Eingeschränkte Testabdeckung**: Aufgrund von zeitlichen Einschränkungen wurde nur ein begrenzter Satz von Unit-Tests erstellt. Diese dienen zur Demonstration und sind ein Ausgangspunkt für eine umfassendere Testabdeckung in Zukunft.
- **Testausführung**: Führen Sie die Unit-Tests über Ihren Entwicklungsumgebung oder über die Kommandozeile mit `dotnet test` aus.

### Hinweis

- Es ist empfehlenswert, die Testabdeckung zu erweitern, um die Zuverlässigkeit und Stabilität der API zu gewährleisten. In zukünftigen Iterationen des Projekts sollte das Hinzufügen weiterer Unit-Tests und Integrationstests in Betracht gezogen werden.

### API-Endpunkte

Die API bietet Endpunkte für die Verwaltung von Dienstanfragen und Benutzerinteraktionen. Diese können mit Postman oder Swagger getestet werden, wie im Repository bereitgestellt.

#### OrderSubmissions

- `POST /api/OrderSubmission`: Erstellen einer neuen Serviceanfrage.
- `GET /api/OrderSubmission`: Abrufen aller Serviceanfragen.
- `GET /api/OrderSubmission/{id}`: Abrufen einer spezifischen Serviceanfrage.
- `PUT /api/OrderSubmission/{id}`: Aktualisieren einer Serviceanfragen.
- `DELETE /api/OrderSubmission/{id}`: Löschen einer Serviceanfrage.
- `PUT /api/OrderSubmission/{id}/cancel`: Stornieren einer Serviceanfrage.
- `POST /api/OrderSubmission/{orderSubmissionId}/assign/{userId}`: User kann sich eine Serviceanfrage zuweisen.
- `PUT /api/OrderSubmission/{orderSubmissionId}/user/{userId}`: Nur Admins können zugewiesene Serviceanfragen jemand anderem zuweisen oder entfernen.

  #### User

  - `POST /api/user/authenticate`: Login Authentifizierung für User und Admins.
  - `POST /api/user/register`: Admins können neue User oder Admins anlegen.
  - `POST /api/user/unlock`: Users und Admins können andere User oder Admins entsperren.
  - `GET /api/user`: Admins können alle User abrufen.
  - `GET /api/user/{id}`: Admins können einen speziefischen User abrufen.
  - `DELETE /api/user/{id}`: Admins können eine User löschen.

### Datenmodelle

- `order_submission`: Modell für Serviceanfragen.
- `user`: Benutzermodell.

### Services

- `OrderSubmissionService`: Verwaltung von Serviceanfragen.
- `TokenService`: Verwaltung von JWT Token.
- `UserService`: Verwaltung von Benutzerdaten.
-

### Sicherheit

Die Sicherheitsmaßnahmen in der Jetstream Backend API umfassen verschiedene Aspekte, die sich auf Authentifizierung, Autorisierung und allgemeine Sicherheitspraktiken beziehen.

#### Authentifizierung und Autorisierung

- **UsersController**: Verwaltet Benutzerregistrierung und Login.
- **TokenService**: Implementiert Token-basierte Authentifizierung mit JWT.
- **User-Modell**: Enthält Rollen- und Berechtigungsinformationen für Autorisierungszwecke.

#### Sicherheitsbezogene Middleware

- **ExceptionHandlingMiddleware**: Hilft bei der sicheren Handhabung von Ausnahmen, um interne Details zu schützen.

#### API-Sicherheitspraktiken

- Sicherheitsüberprüfungen in den Controllern, um den Zugriff auf Ressourcen und Aktionen zu steuern.

#### Hinweise

- Korrekte Implementierung von Authentifizierungs- und Autorisierungsmechanismen ist essentiell.
- Verschlüsselung von sensiblen Daten und sichere Speicherung von Passwörtern sind wichtige Sicherheitsaspekte.

### Mitwirkende

Die Entwicklung der JetStreamApiMongoDb Backend-API wurde von:

- **BiluliB** - Entwickler und Projektleiter

erstellt.

### Framework- und NuGet-Abhängigkeiten

#### Abhängigkeiten des Projekts

- **Target Framework**: .NET 8.0
- **NuGet-Pakete**:
  - `AutoMapper.Extensions.Microsoft.DependencyInjection` (Version 12.0.1)
  - `Microsoft.AspNetCore.Authentication.JwtBearer` (Version 8.0.1)
  - `Microsoft.AspNetCore.OpenApi` (Version 8.0.1)
  - `Microsoft.IdentityModel.Tokens` (Version 7.2.0)
  - `MongoDB.Driver` (Version 2.23.1)
  - `Newtonsoft.Json` (Version 13.0.3)
  - `Serilog.AspNetCore` (Version 8.0.0)
  - `Serilog.Settings.Configuration` (Version 8.0.0)
  - `Serilog.Sinks.File` (Version 5.0.0)
  - `Swashbuckle.AspNetCore` (Version 6.5.0)
  - `System.IdentityModel.Tokens.Jwt` (Version 7.2.0)

Neben den Abhängigkeiten des Hauptprojekts beinhaltet das Unittest-Projekt folgende spezifische Abhängigkeiten:

#### Unittest-Projekt Abhängigkeiten

- **Target Framework**: .NET 8.0
- **NuGet-Pakete**:
  - `Microsoft.NET.Test.Sdk` (Version 17.6.0)
  - `Moq` (Version 4.20.70)
  - `xunit` (Version 2.4.2)
  - `xunit.runner.visualstudio` (Version 2.4.5)
  - `coverlet.collector` (Version 6.0.0)
- **Projektreferenzen**:
  - Verweis auf das Hauptprojekt (`JetstreamApi.csproj`), was für Integrationstests oder den Zugriff auf die Haupt-API-Komponenten notwendig ist.

### Hinweise

- Stellen Sie sicher, dass alle NuGet-Pakete auf die neueste kompatible Version aktualisiert sind.
- Überprüfen Sie die Kompatibilität der NuGet-Pakete mit dem .NET 7.0 Framework.

---

Dieses README bietet einen Überblick über die JetStreamApiMongoDb Backend-API, einschliesslich Einrichtung, Funktionen, Testen und weiteren wesentlichen Aspekten.
