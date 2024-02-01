db = db.getSiblingDB("JetStreamApiDb");

db.users.insertMany([
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e92"),
    name: "admin",
    password_hash: Binary(
      Buffer.from(
        "mkJPWEROZ1WJvq4dOJIzS0BMCez2pZT4ts4WYzfk4BU1OOFCtw8FKnl8UctDzgDe2KHNh5J/5Avm87lb2sZ8CQ==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "Uh2/kTmWTxKzjxFGEFQdDJyHstm3g0niRaO6ftKKy7RgOeF2AZu+fzgzUvByDL5j08LRdTDyRBNnjQ68ighgyV6fRFZqaKweQdOMetFBl5n2CMardNJQGEwTHr0rx6rQPmyW9ZptUEVWNP7lK1ENb9SJWLJmr1HYG3oBL75EDVk=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "ADMIN",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e93"),
    name: "admin1",
    password_hash: Binary(
      Buffer.from(
        "Ultm2L54QDVGefdhyjAC/nidqzPK1MGX3eKZV0ratSe/RnlTZ5UoK9kJPd+sovPNJ6Jk5rKjf7FLinqYwbXkgw==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "pBbW/PnW2P0vGSFoQUvgWcfoKaYQDCH0IfTsWwJWGqZXzz7Fw2TROTBgNomoaDEkpO/4aWhJQWzo28hgsJYEopX9WiWUSd1bKUt6N/8ExXEEl4eq96bsDbttz6mMGAdT6RhrrmYCc5S1a6iNk2b9k7f8va+oOHzsYLj57K7nGlE=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "ADMIN",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e94"),
    name: "user",
    password_hash: Binary(
      Buffer.from(
        "HjITHPSn0qsjDfZBsg1OcjQPQ6/grrHP/epsCePkKiyjmNpRFdvn+N2xHkhGqHKwyGcwtEcNmkbFs/XYw6jgxA==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "bYbzFBwYqU22f8eR2HZ7n+5xNrmrh49GvUz1VPrK1zz7//561sWeAQzTBG/KQiJD8amfMyWIBWzB7AaeUvBcZhAlken8s5vzakclWYUaIV23DKoDX7ptds8auf329k3kAJra48QEIaBfT7DD+S5nVSOS+FarjuFptUqUi2V3stE=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e95"),
    name: "user1",
    password_hash: Binary(
      Buffer.from(
        "uqaHwluNYMpRRcgexGhnk3EjJmsWHzDkc0DaFmAl/GuOBMpixHBpRKu1+fYz9ZJ8pL9BM1H4hENELGOXt6ytxg==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "UtPimk2WOkP9Zv38NXoELJJq0nfJAJIHJzG5W+itIAJrsKfbrEC5cEDbC/cdaJGeNQkSQOwzLrU9SNY+btSAWhEoUKxUtqOa6KmHjkmaHLYTb/8qJIhHT6NmwtGREU8zDcrVgpU5+q5eHvgrzkVPkpvgmjN+M2q8+Jh8ObAfM/0=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e96"),
    name: "user2",
    password_hash: Binary(
      Buffer.from(
        "7zPE5j9hTxFDXTrk0byMUxXANebhX6y+jJyUwHd/60HO4ffRc7dXdrvRg0gw6zFs53jD/4yzkhGN4d3BdyUiUA==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "xHPHEVpxIliM/+9gC9eJgo1xlWdF3uCj85HSnYxBNNDGYr8ajdLedy7vBclOr2dRyZBIZIeo8LwejhRV+KYtWPy4PIA48EOomVCdBHUo80JAU/ggBRyb2QPp5D0Bg/M8nWfnF4VlGT8aNOImtOFzD5mSCZZ59J4mF+HjDts3VoY=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e97"),
    name: "user3",
    password_hash: Binary(
      Buffer.from(
        "VFfpvF4CnlZvv4cR3UhLv67JuW/WyCsN8hxyQ7srw6DOUis6kM5DIKPY9Hmh/Inzs78FME1ScFa53xQr8HFrMA==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "97MUpvlOQT2aByGB15ipE0hy33tm6X9X6Vzwgd+0AQbfK3Oiv08m7zm6qDeZ7+uYnh5Y4yOg1GkKI51HdCqgXDoS8UK2ip+wyyC3lwC7Y1Jiyc7hmSV1acnb0neRZipnazX8CeKeVurZRdgqJuDfh3Q6laAV5WaMbAw5WeOQEv0=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e98"),
    name: "user4",
    password_hash: Binary(
      Buffer.from(
        "s24LUCKGNJfbmBWMWmjE5dvDNbmC8g5Ox5bIHpbUt8EUnPW4rGMhR9qn4gLE8fVoTZkPoBaMbnJ1uv0Hxd0M7A==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "NfDVgils4W+zxlthnD3h2TvgjKEVHn8QYCSU3MLwW09oROve4hvIuPvmNsUyHKfaZ/f+ypDU04AeNTK6V51lfHNKtUdNbftcsNbUqgtYK7MfvbS/tEHyIru1of2Vxtbd4Ogq0qi9CrhWySRPM+QmgGbwQt5mcgvTflAJIHQojvo=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e99"),
    name: "user5",
    password_hash: Binary(
      Buffer.from(
        "oFgPUoAE0umJ6HJhqWSo1/eEY/gtUyaJY7F7FpyBVsBVJEOWGNxppfENXFgvnEkyJ6tn1Mt/TNZJt7JaQpMTQA==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "mBEqLUAXkaQt+uHeuZ6ghGPavdZ2qFjnBWHdWaFzqJrbvcOzz0dVp5T5hBKSG4lJhqXMcECMuGk35k0QIHUOQIs2dNgn98kWcwY5PtK4JJATsVoe/31l0lPTadX8VSPCEtkh1fe2r6cNONabkS7S4UKfU/l0dI8gJksAKUoMVXw=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e9a"),
    name: "user6",
    password_hash: Binary(
      Buffer.from(
        "sSsFbAEW4S4Ld9ayW3tt+5pKXxOUItMVfU0rPbPAMILv9dh+Q2sihBZ2QJmYTTRCohRxSxIcuNPhaU0eMloaaA==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "TIdl245cyOeTu5eLQjWMRiKoCbbKD3zonI7LloAzUyjVxVAQV7y0CTP27nTmq3hCURLmes55+0ZGSM03zjli5ypvbTCcN1IxF5q1vV7E+P9B63xRlZKLU8lyY/cO1r/Dxfbn8Jmrd3MoWHhrOXFBDuUziVx8k/nbiS+stzLwceE=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e9b"),
    name: "user7",
    password_hash: Binary(
      Buffer.from(
        "8iJ8Oljivb8QcSOXhHrnnLeGG4599KezL+GV3hJZv8s5QzjO/RsJ7JjujdB1Fy94WMQqlmXIbXRlA7Ga4YmR2w==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "8UCWQUariF8mfYZPW/vmXeP8xLsxK546gSclZzYOUS6l5vWeaOXDpOrvAlgz+joXGfbtP82gT7tgbaBqGDthBdEUH59vwvEAK82Nb/v6JuU/p1VRH61Ru9a4rwht53QVDz5MvCAP7kRgLsdojDmwUmXt5hnAMSUIgwyBjX5lwKc=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e9c"),
    name: "user8",
    password_hash: Binary(
      Buffer.from(
        "hbG4jK9gtmcV83EyhEp17OHGzLy7YHREBgJmbzFFG1PyMWXGRelhltgqHVWfIRzg0QjY860WSMe1c79KaQEuHA==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "felJhzw5ERMRVNx0NYNQUeYFb7dtcq4PG6XFyuThqnRHVY2NH2mVI22zn2KKbgRnov7POws+s/2wfI1eNMvWlKjYyI2ZIUUjkEwR6PW5l6ClwbcENI9ELuf222WVygj3zUWbX/ecEtuMu5tFtTi8DXDiXhj/7BMTSWEGoMtmHq0=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 3,
    is_locked: true,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e9d"),
    name: "user9",
    password_hash: Binary(
      Buffer.from(
        "EeqcdsvWRMbR4TfPrTt3/jDpmuC7ukX0o/WC8/E7jGT/K3gyXWOo0VCMSAjC/+SZddMKqVV+yy4U3xyeET5I0Q==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "z4RzsZWZUC6fEpUvCx8gZSHXeGN3+WK7WgKKxJUPRMQnAJtZDODRDhbA6fGoEQJsQ5P5hNYLNGgR3OddAwkkkeP5Fqctc+iqNNsxB+iUSQtXfOWYJi786OTk4vFfuhiBRTwTHfakcGUKz0BXMQBhtz7UP2mINp55SHhildfSVkI=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
  {
    _id: ObjectId("65ba5b084b0c591ed39d0e9e"),
    name: "user10",
    password_hash: Binary(
      Buffer.from(
        "wriPnJQootNe82icSPhAKSreAjn039W9IP5WWMmXSEX0YdrRC+eWDizYmMbGCt6rBpn+JNvGjCmAnz3Xr+sgXg==",
        "base64"
      ),
      0
    ),
    password_salt: Binary(
      Buffer.from(
        "nUkza+T+vhhTCsNO3qwJSdJLlYoNGAJ6CjJ+Skj+tOjqoIGxQcRdmiuS4gHpkV+bfTcBJQXLZZFoFcOaoGQz3LMoxw12iOmSbjGstMgc6ieMt9mQjF4wrQeF8rL28vK4Jx8QXQFB6fcXysFmV7UExB3V739iRkNsNs5g9d+yAPI=",
        "base64"
      ),
      0
    ),
    password_input_attempt: 0,
    is_locked: false,
    role: "USER",
  },
]);
