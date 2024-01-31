db = db.getSiblingDB("JetStreamApiDb");

db.createCollection("priorities", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["name"],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        name: {
          bsonType: "string",
        },
        price: {
          bsonType: "double",
          minimum: 0,
          maximum: 1000,
        },
      },
    },
  },
  validationAction: "warn",
});
