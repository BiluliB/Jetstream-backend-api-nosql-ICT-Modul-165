db = db.getSiblingDB("JetStreamApiDb");

db.priorities.insertMany([
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e9f"),
    name: "Tief",
    price: 0.0,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea0"),
    name: "Standard",
    price: 5.0,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea1"),
    name: "Hoch",
    price: 10.0,
  },
]);
