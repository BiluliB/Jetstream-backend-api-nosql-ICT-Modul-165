db = db.getSiblingDB("JetStreamApiDb");

db.createCollection("users", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: [
        "name",
        "password_hash",
        "password_salt",
        "password_input_attempt",
        "is_locked",
        "role",
      ],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        name: {
          bsonType: "string",
          minLength: 1,
          maxLength: 100,
        },
        password_hash: {
          bsonType: "binData",
          minLength: 1,
        },
        password_salt: {
          bsonType: "binData",
          minLength: 1,
        },
        password_input_attempt: {
          bsonType: "int",
          minimum: 0,
          maximum: 5,
        },
        is_locked: {
          bsonType: "bool",
        },
        role: {
          bsonType: "string",
          enum: ["ADMIN", "USER"],
        },
      },
      additionalProperties: false,
    },
  },
  validationAction: "warn",
});
