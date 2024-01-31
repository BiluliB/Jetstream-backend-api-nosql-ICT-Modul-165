// Verbindung zur Admin-Datenbank herstellen
adminDatabase = db.getSiblingDB("admin");
jetStreamApiDb = db.getSiblingDB("JetStreamApiDb");

// Überprüfen und Löschen des Benutzers "sAdmin", falls vorhanden
if (adminDatabase.getUser("superadmin")) {
  adminDatabase.dropUser("superadmin");
  print("Benutzer superadmin wurde gelöscht.");
} else {
  print("Benutzer superadmin existiert nicht.");
}

// Überprüfen und Löschen des Benutzers "johny", falls vorhanden
if (jetStreamApiDb.getUser("johny")) {
  jetStreamApiDb.dropUser("johny");
  print("Benutzer johny wurde gelöscht.");
} else {
  print("Benutzer johny existiert nicht.");
}

// Wechsel zur JetStreamApiDb-Datenbank
jetStreamApiDb = db.getSiblingDB("JetStreamApiDb");

// Überprüfen, ob die Datenbank "JetStreamApiDb" existiert und sie löschen
existingDbs = db
  .adminCommand("listDatabases")
  .databases.map((dbInfo) => dbInfo.name);
if (existingDbs.includes("JetStreamApiDb")) {
  jetStreamApiDb.dropDatabase();
  print("Datenbank JetStreamApiDb wurde gelöscht.");
} else {
  print("Datenbank JetStreamApiDb existiert nicht.");
}
