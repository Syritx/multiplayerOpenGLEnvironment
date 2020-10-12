using System;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenTK;

namespace OpenGL.Environment.Client.Game
{
    class Client
    {
        // run two of instances of the client using the command: dotnet run Client.cs
        static IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
        static TcpClient client;
        static NetworkStream stream;

        static string command;
        static List<Vector3> clientPositions;
        static GameUI game;

        public static void Main(string[] args)
        {
            clientPositions = new List<Vector3>();

            for (int i = 0; i < 10; i++) {
                clientPositions.Add(new Vector3(0, 0, 0));
            }

            client = new TcpClient(ip.ToString(), 5050);

            string message = "[CO-NNE_CT3D-TO_$ER_VER}"; // encrypted to people can't guess
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            stream = client.GetStream();
            stream.Write(messageBytes, 0, messageBytes.Length);
            stream.Flush();
            StayConnected();
        }

        public static void sendMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            byte[] bytes = Encoding.UTF8.GetBytes(message);
            NetworkStream str = client.GetStream();
            str.Write(bytes, 0, bytes.Length);
            str.Flush();
        }

        static void CreateNewGame() {
            game = new GameUI(1000, 1000, "Multiplayer Game");
        }

        static void ReceiveMessages() {
            while (true) {
                byte[] data = new byte[2048];

                int response = stream.Read(data, 0, data.Length);
                string responseData = Encoding.ASCII.GetString(data, 0, response);
                Console.WriteLine(responseData);

                command = responseData;
                string[] commands = command.Split('-');
                int id = 0;

                if (command.StartsWith("[CLIENT_CREATED]: ID: "))
                {
                    int newClientId = 0;
                    string clientID = command.Split(new string[]
                                                    {"[CLIENT_CREATED]: ID: "},
                                                    StringSplitOptions.None)[1];

                    newClientId = Int32.Parse(clientID);
                    clientPositions[newClientId] = new Vector3(0, 0, 0);
                    GameUI.CreatePlayer(clientPositions[newClientId], newClientId);
                }


                if (commands.Length == 3) {
                    if (commands[0].StartsWith("ID: "))
                    {
                        string idString = commands[0].Split(new string[] {"ID: "}, StringSplitOptions.None)[1];
                        id = Int32.Parse(idString);
                    }

                    string[] coordinates = commands[2].Split(new string[] { ", " },
                                                             StringSplitOptions.None);
                    float[] floatCoordinates = new float[coordinates.Length];

                    for (int i = 0; i < coordinates.Length; i++) {
                        floatCoordinates[i] = float.Parse(coordinates[i]);
                    }
                    clientPositions[id] = new Vector3(floatCoordinates[0],
                                                      floatCoordinates[1],
                                                      floatCoordinates[2]);

                    try {
                        GameUI.CreatePlayer(clientPositions[id], id);
                        Console.WriteLine("created player");
                    }
                    catch(Exception e) {}
                }

                else if (commands.Length == 2) {
                    if (commands[0].StartsWith("ID: ")) {
                        string idString = commands[0].Split(new string[] { "ID: " }, StringSplitOptions.None)[1];
                        id = Int32.Parse(idString);
                    }

                    if (commands[1].StartsWith("[POSITION]: ")) {
                        string positionCoords = commands[1].Split(new string[] { "[POSITION]: " }, StringSplitOptions.None)[1];
                        string[] coordinates = positionCoords.Split(new string[] { ", " },
                                                             StringSplitOptions.None);
                        float[] floatCoordinates = new float[coordinates.Length];

                        for (int i = 0; i < coordinates.Length; i++) {
                            floatCoordinates[i] = float.Parse(coordinates[i]);
                        }
                        clientPositions[id] = new Vector3(floatCoordinates[0],
                                                          floatCoordinates[1],
                                                          floatCoordinates[2]);
                        try {
                            GameUI.CreatePlayer(clientPositions[id], id);
                            Console.WriteLine("created player");
                        }
                        catch(Exception e) {}
                    }
                }
            }
        }

        static void StayConnected()
        {
            Task receiver = Task.Factory.StartNew(ReceiveMessages);
            CreateNewGame();

            Task.WaitAll(receiver);
        }

        ~Client()
        {
            sendMessage("disconnected from the server");

            stream.Close();
            client.Close();
        }
    }
}
