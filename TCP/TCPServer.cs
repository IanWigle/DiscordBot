using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Text.Json;
using System.IO;

//using SynovianEmpireDiscordBot.CharacterMakerClasses;

namespace SynovianEmpireDiscordBot.TCP
{
    class TCPServer
    {
        private int portNum;

        private const int MAX_BYTE_READ = 5000;

        ThreadStart childref;
        Thread childThread;
        TcpListener listener;

        public TCPServer(int port = 13)
        {
            portNum = port;
            try
            {
                Console.WriteLine("Starting TCP Server");
                listener = new TcpListener(IPAddress.Any, portNum);
                listener.Start();
                childref = new ThreadStart(RunServer);
                childThread = new Thread(childref);
                //childThread.IsBackground = true;
                childThread.Name = "TCP Server";
                childThread.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ShutdownTCP()
        {
            if (childThread.IsAlive)
            {
                childThread.Abort();
                listener.Stop();
            }
        }

        ~TCPServer()
        {
            ShutdownTCP();
        }

        private void RunServer()
        {
            while(true)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();

                    Console.WriteLine("TCP: Incoming Character . . .");
                    byte[] bytes = new byte[MAX_BYTE_READ];
                    int bytesRead = ns.Read(bytes, 0, bytes.Length);
                    Console.WriteLine("TCP: Translating bytes . . .");
                    string jsonString = Encoding.ASCII.GetString(bytes, 0, bytesRead);
#if DEBUG
                    Console.WriteLine(jsonString);
#endif

                    using (JsonDocument document = JsonDocument.Parse(jsonString))
                    {
                        JsonElement root = document.RootElement;

                        bool overrideExisting = false;
                        string Author = "";
                        string Date = "";

                        if(root.TryGetProperty("submissionDetails",out JsonElement datapackElement))
                        {
                            if(datapackElement.TryGetProperty("Author",out JsonElement authorElement))
                            {
                                Author = authorElement.GetString();
                            }
                            if(datapackElement.TryGetProperty("Date", out JsonElement dateElement))
                            {
                                Date = dateElement.GetString();
                            }
                            if(datapackElement.TryGetProperty("OverrideSubmission", out JsonElement overrideElement))
                            {
                                overrideExisting = overrideElement.GetBoolean();
                            }
                        }

                        string CharacterName = "";
                        string characterDescription = "";                        
                        //CharacterRank characterRank = CharacterRank.NoRank;
                       // CharacterAlignment characterAlignment = CharacterAlignment.None;
                        List<string> abilities = new List<string>();

                        if(root.TryGetProperty("characterSheet", out JsonElement characterElement))
                        { 
                            if(characterElement.TryGetProperty("Name", out JsonElement nameElement))
                            {
                                CharacterName = nameElement.GetString();
                            }
                            if(characterElement.TryGetProperty("rank", out JsonElement rankElement))
                            {
                                //characterRank = (CharacterRank)rankElement.GetInt32();
                            }
                            if(characterElement.TryGetProperty("alignment", out JsonElement alignElement))
                            {
                                //characterAlignment = (CharacterAlignment)alignElement.GetInt32()+1;
                            }
                            if(characterElement.TryGetProperty("characterDescription", out JsonElement descElement))
                            {
                                characterDescription = descElement.GetString();
                            }
                            if (root.TryGetProperty("Abilities", out JsonElement jsonElement3))
                            {
                                foreach (var abilityElem in jsonElement3.EnumerateArray())
                                {
                                    abilities.Add(abilityElem.GetString());
                                }
                            }
                        }

                        //CharacterSheet characterSheet = new CharacterSheet(CharacterName, characterRank, characterAlignment, abilities, Date, Author);
                        //characterSheet.characterDescription = characterDescription;

                        //Program.characterLibrary.AddCharacter(characterSheet, overrideExisting,true);
                        Console.WriteLine($"New character sheet added to the bots library.");
                    }

                    ns.Close();
                    client.Close();
                }
            }
        }
    }
}
