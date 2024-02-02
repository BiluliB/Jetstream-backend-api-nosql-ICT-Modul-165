db = db.getSiblingDB("JetStreamApiDb");

db.createCollection("statuses", {
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
          maxLength: 50,
          enum: ["Offen", "In Arbeit", "Erledigt", "Storniert"],
        },
      },
    },
  },
  validationAction: "warn",
});
