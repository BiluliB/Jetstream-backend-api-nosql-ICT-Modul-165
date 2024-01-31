adminDatabase = db.getSiblingDB("admin");
jetStreamApiDb = db.getSiblingDB("JetStreamApiDb");

if (!adminDatabase.getUser("superadmin")) {
  adminDatabase.createUser({
    user: "superadmin",
    pwd: "password",
    roles: ["root"],
  });
}

if (!jetStreamApiDb.getRole("customRole")) {
  jetStreamApiDb.createRole({
    role: "customRole",
    privileges: [
      {
        resource: { db: "JetStreamApiDb", collection: "" },
        actions: ["find", "insert", "update", "remove"],
      },
    ],
    roles: [],
  });
}

if (!jetStreamApiDb.getUser("johny")) {
  jetStreamApiDb.createUser({
    user: "johny",
    pwd: "password",
    roles: ["customRole"],
  });
}
