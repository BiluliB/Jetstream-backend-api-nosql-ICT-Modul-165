db = db.getSiblingDB("JetStreamApiDb");

db.services.insertMany([
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea2"),
    name: "Kleiner Service",
    price: 49,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea3"),
    name: "Grosser Service",
    price: 69,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea4"),
    name: "Rennskiservice",
    price: 99,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea5"),
    name: "Bindung montieren und einstellen",
    price: 39,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea6"),
    name: "Fell zuschneiden",
    price: 25,
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0ea7"),
    name: "Heisswachsen",
    price: 18,
  },
]);
