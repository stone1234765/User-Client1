﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace User_Client
{
    class ConnectorToChat
    {
        public ConnectorToChat(Communication communication, WriterGroups writerGroups)
        {
            this.communication = communication;
            this.writerGroups = writerGroups;
        }
        private WriterGroups writerGroups;
        Communication communication;
        public void SelectChat()
        {
            AnswerAndWriteServer();
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length != 0)
                {
                    SendMessage(line);
                    AnswerAndWriteServer();
                    ModeSelection(line);
                    //var first4 = $"{line[0]}{line[1]}{line[2]}{line[3]}";
                    //for (int i = 0; i < line.Length; i++)
                    //{

                    //}
                    //if (first4 == "?/cc")
                    //{
                    //    ReciveChats(6);
                    //}
                    //else if (first4 == "?/gg")
                    //{
                    //    ReciveChats(4);
                    //}
                    //else if(first4 == "?/ng")
                    //{
                    //    var openChat = CreateNewGroup();
                    //    if (openChat)
                    //    {
                    //        OpenChat();
                    //    }
                    //}
                    //else
                    //{
                    //    ReciveChats(1);
                    //}
                    //if (communication.data.ToString() == "You connect to chat")
                    //{
                    //    return;
                    //}
                }
            }
        }
        private void ModeSelection(string message)
        {
            var serverMessage = communication.data.ToString();
            if (serverMessage[6] == 't')
            {
                return;
            }
            else if (serverMessage == "Enter name of chat")
            {
                FindGroup();
                OpenChat();
            }
            else if (serverMessage == "If you want to join a group write: join\n\r" +
                "if you want to look at the invitation, write: look")
            {
                if (AcceptTheInvitation())
                {
                    OpenChat();
                }
            }
            else
            {
                var first4 = $"{message[0]}{message[1]}{message[2]}{message[3]}";
                if (first4 == "?/cc")
                {
                    writerGroups.Run(6);
                }
                else if (first4 == "?/gg")
                {
                    writerGroups.Run(4);
                }
                else if (first4 == "?/ng")
                {
                    var openChat = CreateNewGroup();
                    if (openChat)
                    {
                        OpenChat();
                    }
                }
                else
                {
                    writerGroups.Run(1);
                }
                if (communication.data.ToString() == "You connect to chat")
                {
                    return;
                }
            }
        }
        private bool AcceptTheInvitation()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length > 0)
                {
                    SendMessage(line);
                    if (line == "look")
                    {
                        writerGroups.Run(1);
                    }
                    else if (line == "join")
                    {
                        AnswerAndWriteServer();
                        while (true)
                        {
                            var line2 = Console.ReadLine();
                            if (line2.Length > 0)
                            {
                                SendMessage(line2);
                                AnswerAndWriteServer();
                                if (communication.data.ToString() == "You join the group\n\r" +
                            "If you want open chats, write: open")
                                {
                                    var line3 = Console.ReadLine();
                                    if (line3.Length > 0)
                                    {
                                        SendMessage(line3);
                                        AnswerAndWriteServer();
                                        if (communication.data.ToString() == "You enter to the group")
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        AnswerAndWriteServer();
                    }
                }
            }
        }
        private void FindGroup()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length > 0)
                {
                    SendMessage(line);
                    AnswerAndWriteServer();
                    if (communication.data.ToString() == "You connect to chat")
                    {
                        return;
                    }
                }
            }
        }
        private void OpenChat()
        {
            Chat chat = new Chat(communication);
            chat.Run();
            Console.WriteLine("ok, good");
            Console.ReadKey();
        }
        private bool CreateNewGroup()
        {
            while (true)
            {
                //AnswerAndWriteServer();
                WriteToServer("Enter a group name");
                WriteToServer("Who do you want to invite to your group?\n\r" +
                            "If you want to check people, write ?/yes\n\r" +
                            "If you don`t want to add people, write ?/no\n\r");
                WriteToServer("You create group, thanks.\n\r" +
                    "If you want to open it, write ok, else - press else");
                while (true)
                {
                    var lineTwo = Console.ReadLine();
                    if (lineTwo.Length > 0)
                    {
                        SendMessage(lineTwo);
                        if (lineTwo == "ok")
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }
        private void WriteToServer(string finalMesage)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length > 0)
                {
                    SendMessage(line);
                    AnswerAndWriteServer();
                    if (communication.data.ToString() == finalMesage)
                    {
                        break;
                    }
                    if (communication.data.ToString() == "People:")
                    {
                        SendMessage("Ok");
                        writerGroups.WriteGroup(Console.CursorLeft, Console.CursorTop);
                    }
                }
            }
        }
        private void AnswerAndWriteServer()
        {
            AnswerServer();
            Console.WriteLine(communication.data);
        }
        private void AnswerServer()
        {
            communication.AnswerServer();
        }
        private void SendMessage(string message)
        {
            communication.SendMessage(message);
        }
    }
}