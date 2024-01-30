db.createUser({
  user: "Johny",
  pwd: "password",
  roles: [{ role: "readWrite", db: "JetStreamApiDb" }],
});
