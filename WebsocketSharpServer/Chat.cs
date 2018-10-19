using System;
using System.Collections;
using System.IO;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebsocketSharpServer
{
    class Chat : WebSocketBehavior
    {
        private readonly string _suffix;
        private string _name;
        private DateTime date;
        private ArrayList history = new ArrayList();
        private ArrayList usersRegisted = new ArrayList();
        private readonly string path_string = @"C:\Users\andre_figueira\source\repos\WebsocketSharpServer\WebsocketSharpServer\logChat.txt";
        private string[] historyFileReaded;
        private int index;

        public Chat() : this(null)
        {
        }

        public Chat(string suffix)
        {
            _suffix = suffix ?? string.Empty;
            // OR _suffix = suffix ?? string.Empty;

        }

        private string GetName()
        {
            var name = Context.QueryString["name"];
            return !name.IsNullOrEmpty() ? name : _suffix;
        }

        private void SearchLastUserMessages()
        {
            historyFileReaded = File.ReadAllLines(path_string);

            for (var i = historyFileReaded.Length - 1; i > 1; i--)
            {
                if (historyFileReaded[i].Contains(_name))
                {
                    index = i + 1;

                    if (index < historyFileReaded.Length)
                    {
                        Console.WriteLine("Enviando mensagens de historico!");

                        for (var p = index; p < historyFileReaded.Length - 1; p++)
                        {
                            if (!historyFileReaded[p].Contains("connected!"))
                            {
                                if (!historyFileReaded[p].Contains("disconnected!"))
                                {
                                    Send(historyFileReaded[p]);
                                    Console.WriteLine(historyFileReaded[p]);
                                }
                            }
                        }
                        return;
                    }
                }
            }
        }

        protected override void OnOpen()
        {
            _name = GetName();

            SearchLastUserMessages();

            date = DateTime.Now;

            var texOpen = $"{date} : {_name} :> Has connected!" + Environment.NewLine;
            File.AppendAllText(path_string, texOpen);


            //Descomentar??????????????? para dizer no chat que fez connect?
            //Sessions.Broadcast($"{date} : {_name} :> Has connected!");
            //Send($"{date} : {_name} :> Has connected!");

            Console.WriteLine(string.Format($"{date} : {_name} :> Has connected!"));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            date = DateTime.Now;

            var texMessage = $"{date} : {Context.QueryString["name"]} :> {e.Data}" + Environment.NewLine;

            File.AppendAllText(path_string, texMessage);

            Sessions.Broadcast($"{date} : {Context.QueryString["name"]} :> {e.Data}");

            Console.WriteLine(string.Format($"{date} : {_name} :> {e.Data}"));
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            date = DateTime.Now;

            var texError = $"{date} : {Context.QueryString["name"]} » ERROR :> {e.Message}" + Environment.NewLine;

            File.AppendAllText(path_string, texError);

            Sessions.Broadcast($"{date} : {Context.QueryString["name"]} » ERROR :> {e}");

            Console.WriteLine(string.Format($"       ERROR           {e.Message}"));
        }

        protected override void OnClose(CloseEventArgs e)
        {
            date = DateTime.Now;

            var texClose = $"{date} : {Context.QueryString["name"]} :> Has disconnected!" + Environment.NewLine;

            File.AppendAllText(path_string, texClose);

            //Sessions.Broadcast($"{date} : {Context.QueryString["name"]} :> Has disconnected!");

            Console.WriteLine(string.Format($"{date} : {Context.QueryString["name"]} :> Has disconnected!"));
        }
    }
}
