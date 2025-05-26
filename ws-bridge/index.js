const express = require('express');
const bodyParser = require('body-parser');
const WebSocket = require('ws');

const app = express();
const port = 8080;

let wsClient = null;

const wss = new WebSocket.Server({ noServer: true });

app.use(bodyParser.json());

app.post('/send', (req, res) => {
  if (wsClient && wsClient.readyState === WebSocket.OPEN) {
    wsClient.send(JSON.stringify(req.body));
    res.send({ status: 'Sent to Revit' });
  } else {
    res.status(400).send({ error: 'No Revit client connected' });
  }
});

const server = app.listen(port, () => {
  console.log(`Bridge server running on port ${port}`);
});

server.on('upgrade', (req, socket, head) => {
  wss.handleUpgrade(req, socket, head, (ws) => {
    wsClient = ws;
    console.log('Revit client connected via WebSocket');

    ws.on('message', (message) => {
      console.log('Received from Revit:', message);
      // Optionally forward or store the message
    });

    ws.on('close', () => {
      console.log('Revit client disconnected');
      wsClient = null;
    });
  });
});
