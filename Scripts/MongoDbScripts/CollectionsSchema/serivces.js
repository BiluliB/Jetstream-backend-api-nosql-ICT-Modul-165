db = db.getSiblingDB("JetStreamApiDb");

db.createCollection("services", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["name", "price"],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        name: {
          bsonType: "string",
          maxLength: 50,
          enum: [
            "Grosser Service",
            "Kleiner Service",
            "Rennskiservice",
            "Bindung montieren und einstellen",
            "Fell zuschneiden",
            "Heisswachsen",
          ],
        },
        price: {
          bsonType: "double",
          minimum: 0,
          maximum: 100,
        },
      },
    },
  },
  validationAction: "warn",
});
