db = db.getSiblingDB("JetStreamApiDb");

// OrderSubmission Indizes
db.order_submissions.createIndex(
  { pickup_date: 1, status_id: 1 },
  { name: "PickupDate_StatusId_index" }
);
db.order_submissions.createIndex(
  { priority_id: 1 },
  { name: "PriorityId_index" }
);
db.order_submissions.createIndex(
  { service_id: 1 },
  { name: "ServiceId_index" }
);
db.order_submissions.createIndex({ firstname: 1 }, { name: "Firstname_index" });
db.order_submissions.createIndex({ lastname: 1 }, { name: "Lastname_index" });
db.order_submissions.createIndex({ email: 1 }, { name: "Email_index" });
db.order_submissions.createIndex({ phone: 1 }, { name: "Phone_index" });

// Priority Indizes
db.priorities.createIndex({ name: 1 }, { name: "Priority_name_index" });

// Service Indizes
db.services.createIndex({ name: 1 }, { name: "Service_name_index" });

// Status Indizes
db.statuses.createIndex({ name: 1 }, { name: "Status_name_index" });

// User Indizes
db.users.createIndex({ name: 1 }, { name: "User_name_index" });
