using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

        static IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
        static int port = 5050;

        public static void Main(string[] args) {
            Console.Title = "[SERVER PANEL]";
            Console.WriteLine("[SERVER PANEL]");

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

        static void clientThread(Socket client) {
            int currentClientId = clientId;

            string message = "ID: " + currentClientId.ToString()+ " ";
            string idString = "ID: " + currentClientId.ToString();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] bytes = new byte[2048];

            client.Send(messageBytes);
            Console.WriteLine("sent message to client");
            IPEndPoint clientEndPoint = client.RemoteEndPoint as IPEndPoint;

            string newClientMessage = "[CLIENT_CREATED]: " + idString;
            foreach (Socket socketClient in clients)
            {
                byte[] newClient = Encoding.UTF8.GetBytes(newClientMessage);
                if (socketClient != client) socketClient.Send(newClient);
            }

            while (client.Connected) {
                bytes = new byte[2048];
                int i = client.Receive(bytes);

                string command = Encoding.UTF8.GetString(bytes);
                Console.WriteLine("[{0}, {1}] SENT: {2}",clientEndPoint.Address,
                                                    clientEndPoint.Port,
                                                    command);
                string newCommand = idString + "-CMD-" + command;

                // SENDING ID
                foreach(Socket clientSocket in clients) {
                    if (clientSocket != client) {
                        try {
                            byte[] idBytes = Encoding.UTF8.GetBytes(newCommand);
                            clientSocket.Send(idBytes);
                        }
                        catch (Exception e) { clients.Remove(clientSocket); }
                    }
                }
            }
            clients.Remove(client);
        }

        static void ReceiveClients() {
            while (true) {

                Socket client = server.Accept();
                clientId++;
                clients.Add(client);
                Task.Run(() => clientThread(client));
            }
        }
    }
}
