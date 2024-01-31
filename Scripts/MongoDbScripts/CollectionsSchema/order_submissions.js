db = db.getSiblingDB("JetStreamApiDb");

db.createCollection("order_submissions", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: [
        "firstname",
        "lastname",
        "email",
        "phone",
        "priority_id",
        "service_id",
        "status_id",
      ],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        firstname: {
          bsonType: "string",
          minLength: 1,
          maxLength: 100,
        },
        lastname: {
          bsonType: "string",
          minLength: 1,
          maxLength: 100,
        },
        email: {
          bsonType: "string",
          pattern: "^.+@.+$",
        },
        phone: {
          bsonType: "string",
          pattern: "^(\\+\\d{1,3}[- ]?)?[0-9 ]{13}$",
        },
        priority_id: {
          bsonType: "objectId",
        },
        create_date: {
          bsonType: "date",
        },
        pickup_date: {
          bsonType: "date",
        },
        service_id: {
          bsonType: "objectId",
        },
        total_price_chf: {
          bsonType: "double",
          minimum: 0,
        },
        status_id: {
          bsonType: "objectId",
        },
        comment: {
          bsonType: "string",
          maxLength: 500,
        },
        user_id: {
          bsonType: "objectId",
        },
      },
      additionalProperties: false,
    },
  },
  validationAction: "warn",
});
