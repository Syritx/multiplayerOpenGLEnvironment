using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenGL.Environment.Server
{
    class Server
    {
        // run the server first using the terminal with the command: dotnet run Server.cs
        static Socket server;
        static List<Socket> clients;
        static ClientCommands clientCommands = new ClientCommands();
        static int clientId = -1;

        static int playerCount = 10;
        static bool[] places;

        static IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
        static int port = 5050;

        public static void Main(string[] args) {
            Console.Title = "[SERVER PANEL]";
            Console.WriteLine("[SERVER PANEL]");

            places = new bool[playerCount];
            for (int i = 0; i < playerCount; i++){
                places[i] = false;
            }

            clients = new List<Socket>();
            server = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream,
                                ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(ip, port);
            server.Bind(endPoint);
            server.Listen(5);
            Console.WriteLine("Created server");

            ReceiveClients();
        }

        static void RemoveClientFromServer(Socket client, int id) {
            client.Close();
            clients.Remove(client);
            places[id-1] = false;

            Console.WriteLine("Client {0} has disconnected from the server",id);
        }

        static void ReceiveAndSendMessages(Socket client, string idString, int id) {

            IPEndPoint clientEndPoint = client.RemoteEndPoint as IPEndPoint;
            byte[] bytes = new byte[2048];

            while (client.Connected) {
                bytes = new byte[2048];
                int i = client.Receive(bytes);

                string command = Encoding.UTF8.GetString(bytes);
                Console.WriteLine("[{0}, {1}] SENT: {2}", clientEndPoint.Address,
                                                    clientEndPoint.Port,
                                                    command);
                string newCommand = idString + "-CMD-" + command;

                if (client.Connected) {
                    // SENDING COMMANDS
                    foreach(Socket clientSocket in clients) {
                        if (clientSocket != client) {
                            try {
                                byte[] idBytes = Encoding.UTF8.GetBytes(newCommand);
                                clientSocket.Send(idBytes);
                            }
                            catch (Exception e) {}
                        }
                    }
                }
            }
        }

        static void DetectClientStatus(Socket client, int clientID) {
            while (client != null) {
                if (!client.Connected) RemoveClientFromServer(client, clientID);
                Thread.Sleep(10);
            }
        }

        static void clientThread(Socket client) {
            int currentClientId = 0;

            for (int i = 0; i < places.Length; i++) {
                if (!places[i]) {
                    currentClientId = (i + 1);
                    places[i] = true;
                    break;
                }
            }
            if (currentClientId == 0) RemoveClientFromServer(client, 0);

            string message = "ID: " + currentClientId.ToString()+ " ";
            string idString = "ID: " + currentClientId.ToString();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] bytes = new byte[2048];

            client.Send(messageBytes);
            ClientConnected(client, idString);

            Task.Run(() => ReceiveAndSendMessages(client, idString, currentClientId));
            Task.Run(() => DetectClientStatus(client, currentClientId));
        }

        static void ReceiveClients() {
            while (true) {

                Socket client = server.Accept();
                clientId++;
                clients.Add(client);
                Task.Run(() => clientThread(client));
            }
        }

        // CLIENT METHODS
        static void ClientConnected(Socket client, string idString) {

            // sending to all clients a notification
            string newClientMessage = "[CLIENT_CREATED]: " + idString;
            foreach (Socket socketClient in clients)
            {
                byte[] newClient = Encoding.UTF8.GetBytes(newClientMessage);
                if (socketClient != client) socketClient.Send(newClient);
            }
            return;
        }

        static void ClientDisconnected() {

        }
    }
}
