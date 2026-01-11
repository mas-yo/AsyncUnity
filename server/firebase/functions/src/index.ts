/**
 * Import function triggers from their respective submodules:
 *
 * import {onCall} from "firebase-functions/v2/https";
 * import {onDocumentWritten} from "firebase-functions/v2/firestore";
 *
 * See a full list of supported triggers at https://firebase.google.com/docs/functions
 */

// import {setGlobalOptions} from "firebase-functions";
// import {onRequest} from "firebase-functions/https";
// import * as logger from "firebase-functions/logger";
import * as functions from "firebase-functions";
import * as admin from "firebase-admin";

// Start writing functions
// https://firebase.google.com/docs/functions/typescript

// For cost control, you can set the maximum number of containers that can be
// running at the same time. This helps mitigate the impact of unexpected
// traffic spikes by instead downgrading performance. This limit is a
// per-function limit. You can override the limit for each function using the
// `maxInstances` option in the function's options, e.g.
// `onRequest({ maxInstances: 5 }, (req, res) => { ... })`.
// NOTE: setGlobalOptions does not apply to functions using the v1 API. V1
// functions should each use functions.runWith({ maxInstances: 10 }) instead.
// In the v1 API, each function can only serve one request per container, so
// this will be the maximum concurrent request count.
functions.setGlobalOptions({ maxInstances: 10 });


admin.initializeApp();

const db = admin.firestore();

if (process.env.FUNCTIONS_EMULATOR) {
  db.settings({
    host: "localhost:8080",
    ssl: false,
  });
}


export const helloWorld = functions.https.onRequest((request, response) => {
  functions.logger.info("Hello logs!", {structuredData: true});
  response.send("Hello from Firebase!");
});

export const getAllUsers = functions.https.onRequest(
  async (req, res) => {
    const snapshot = await db.collection("users").get();

    const users = snapshot.docs.map(d => ({
      id: d.id,
      ...d.data(),
    }));

    res.json(users);
  }
);


export const getUser = functions.https.onRequest(
  async (req, res) => {
    try {
      const userId = req.query.userId as string;

      functions.logger.info("Fetching user:", userId);

      const doc = await db.collection("users").doc(userId).get();

      functions.logger.info("User data:", doc.data());

      if (!doc.exists) {
        res.status(404).send("Not found");
        return;
      }

      res.json(doc.data());
    } catch (e) {
      res.status(500).send(e);
    }
  }
);

export const createUser = functions.https.onRequest(
  async (req, res) => {
    try {
    //   const { userId, name, age } = req.body;
      // const userId = "testuser1";
      const name = "Alice";
      const age = 30;

      await db.collection("users").add({
        name: name,
        age:age,
        createdAt: admin.firestore.FieldValue.serverTimestamp(),
      });

      // res.send("ok");
      const snapshot = await db.collection("users").get();

      const users = snapshot.docs.map(d => ({
        id: d.id,
        ...d.data(),
      }));

      res.json(users);


    } catch (e) {
      res.status(500).send(e);
    }
  }
);

export const debugUsers = functions.https.onRequest(
  async (req, res) => {
    const ref = await db.collection("users").add({
      name: "debug",
      age: 1,
      // createdAt: admin.firestore.FieldValue.serverTimestamp(),
    });

    const snap = await db.collection("users").get();

    res.json({
      addedId: ref.id,
      count: snap.size,
      users: snap.docs.map(d => ({
        id: d.id,
        ...d.data(),
      })),
    });
  }
);

