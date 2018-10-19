using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace WebsocketSharpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer ws = new WebSocketServer("ws://127.0.0.1:8080");

            ws.AddWebSocketService<Chat>("/chat");
            ws.Start();

            //ws.WebSocketServices["/chat"].Sessions.Broadcast("Hello from server");

            if (ws.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", ws.Port);
                Console.WriteLine("Service - {0}", ws.WebSocketServices.Paths);
                //ws.Log.Level = LogLevel.Debug;
                //Console.WriteLine("\r\n"+ws.Log.ToString());
            }

            Console.WriteLine("\nPress [Enter] key to stop the server...");
            Console.ReadKey();
            ws.Stop();
        }
    }
}
