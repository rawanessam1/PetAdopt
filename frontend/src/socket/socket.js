import * as signalR from "@microsoft/signalr";

let connection = null;

export async function getOrCreateConnection(userId) {
  if (!connection) {
    connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5073/hubs/notifications", {
        withCredentials: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect([0, 1000, 2000, 5000])
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  if (connection.state === signalR.HubConnectionState.Disconnected) {
    await connection.start();
    console.log("✅ SignalR connected! State:", connection.state);
  }

  if (connection.state === signalR.HubConnectionState.Connected && userId) {
    await connection.invoke("JoinGroup", `owner_${userId}`);
    await connection.invoke("JoinGroup", `user_${userId}`);
    console.log(`✅ Joined groups: owner_${userId}, user_${userId}`);
  }

  return connection;
}

export function getConnection() {
  return connection;
}
