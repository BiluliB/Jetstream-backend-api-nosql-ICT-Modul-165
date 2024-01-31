db = db.getSiblingDB("JetStreamApiDb");

db.statuses.insertMany([
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea8"),
    name: "Offen",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea9"),
    name: "In Arbeit",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0eaa"),
    name: "Abgeschlossen",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0eab"),
    name: "Storniert",
  },
]);
