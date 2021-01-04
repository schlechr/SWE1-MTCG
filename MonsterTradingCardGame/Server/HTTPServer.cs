using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MonsterTradingCardGame.Server
{
    public class HTTPServer
    {
        public const string NAME = "SWE1 HTTP Server v0.1";
        public static List<string> MESSAGES = new List<string>();
        public static List<string> USERS = new List<string>();
        private TcpListener listener;


        public HTTPServer(int port)
        {
            //listener = new TcpListener(IPAddress.Any, port);
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            listener.Start();

            while(true)
            {
                Console.WriteLine("Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient();

                if (client == null)
                    break;

                Console.WriteLine("Client connected!");

                HandleClient(client);

                client.Close();

            }
            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            string msg = "";

            while (reader.Peek() != -1)
            {
                msg += (char)reader.Read();
            }

            //Console.WriteLine("Request: \n" + msg);

            Request req = Request.GetRequest(msg);
            
            /*
            if (req != null)
            {
                foreach (var keyVal in req.HeaderValues)
                {
                    Console.WriteLine(keyVal.Key + ": " + keyVal.Value);
                }
            }*/

            Response resp = Response.From(req);
            resp.Post(client.GetStream());
        }
    }
}
