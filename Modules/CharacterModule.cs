using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Drawing;

using Discord;
using Discord.Commands;

using Aspose.Zip;

using SynovianEmpireDiscordBot.Libraries;
using SynovianEmpireDiscordBot.CharacterMakerClasses;
using SynovianEmpireDiscordBot.BotData.Helpers;

namespace SynovianEmpireDiscordBot.Modules
{
    class CharacterModule : ModuleBase<SocketCommandContext>
    {
        IServiceProvider services;

        public CharacterModule(IServiceProvider service) => services = service;

        [Command("ListCharacters")]
        [Summary("Shows a list of characters within the library")]
        [Alias("lgc")]
        public async Task ListCharacters(int page = 0)
        {
            const int sheetsPerPage = 5;

            CharacterLibrary characterLibraryRef = Program.characterLibrary;

            int numPages = characterLibraryRef.NumberOfCharacters() / sheetsPerPage;

            if(page > numPages)
            {
                await ReplyAsync("The page number provided is more than the number of pages, defaulted to 0");
                page = 0;
            }

            EmbedBuilder embedBuilder = new EmbedBuilder();

            foreach(CharacterSheet sheet in characterLibraryRef.GetList())
            {
                embedBuilder.AddField(sheet.Name, $"Rank : {sheet.GetRankAsString()}\nAlignment : {sheet.GetAlignmentAsString()}");
            }
            await ReplyAsync("Results : ", false, embedBuilder.Build());
        }

        [Command("CharacterDetails")]
        [Summary("Provides the detailed info of the character")]
        [Alias("cd")]
        public async Task CharacterDetails(string name)
        {
            if(!Program.characterLibrary.GetNameList().Contains(name))
            {
                await ReplyAsync($"Couldn't find a character of the name {name}");
            }
            else
            {
                CharacterSheet sheet = Program.characterLibrary.GetSheet(name);

                EmbedBuilder embedBuilder = new EmbedBuilder();
                embedBuilder.AddField("Name", sheet.Name);
                embedBuilder.AddField("Rank", sheet.GetRankAsString());
                embedBuilder.AddField("Alignment", sheet.GetAlignmentAsString());
                embedBuilder.AddField("Details", sheet.characterDescription);

                bool allowZips = false;

                if (sheet.characterImage != null && allowZips)
                {
                    try
                    {
                        var stream = new MemoryStream();
                        sheet.characterImage.Save(stream, sheet.characterImage.RawFormat);
                        await Context.User.SendFileAsync(stream, sheet.imageName, embed: new EmbedBuilder { ImageUrl = $"attachment://{sheet.imageName}"}.Build())
                              .ContinueWith(x => Context.User.SendMessageAsync("", false, embedBuilder.Build()));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        await Context.Channel.SendMessageAsync("An error occured. Whether it's you blocking dms, or on the bot-side.");
                        return;
                    }
                }
                else
                {
                    try
                    {
                        await Context.User.SendMessageAsync($"Character details of {sheet.Name}", false, embedBuilder.Build());
                    }
                    catch (Exception e)
                    {
                        await ReplyAsync("Failed to pm the details to you. You blocking me, bruh?");
                        Console.WriteLine(e.Message);
                        return;

                    }

                }
                await ReplyAsync($"Sent details for the character \"{name}\"");
            }
        }

        [Command("UploadCharacter")]
        [Summary("Upload a character to the library")]
        [Alias("uc")]
        public async Task UploadCharacter(bool overriteExisting = true)
        {
            var attachments = Context.Message.Attachments;

            if(attachments.Count > 1)
            {
                await ReplyAsync("The bot only supports the upload of one attachment at a time. Aborting . . .");
                return;
            }

            WebClient webClient = new WebClient();

            foreach(var attachment in attachments)
            {
                string url = attachment.Url;

                byte[] buffer = webClient.DownloadData(url);

                string extension = StringHelpers.GetFileNameExtension(attachment.Filename);

                if(extension == "txt")
                {
                    string download = Encoding.UTF8.GetString(buffer);
                    
                    using(JsonDocument document = JsonDocument.Parse(download))
                    {
                        JsonElement root = document.RootElement;

                        string charName = "";
                        string charDesc = "";
                        CharacterRank charRank = CharacterRank.NoRank;
                        CharacterAlignment charAlignment = CharacterAlignment.None;
                        List<string> abilities = new List<string>();

                        if (root.TryGetProperty("Name", out JsonElement nameElement))
                        {
                            charName = nameElement.GetString();
                        }
                        if (root.TryGetProperty("rank", out JsonElement rankElement))
                        {
                            charRank = (CharacterRank)rankElement.GetInt32();
                        }
                        if (root.TryGetProperty("alignment", out JsonElement alignElement))
                        {
                            charAlignment = (CharacterAlignment)alignElement.GetInt32() + 1;
                        }
                        if (root.TryGetProperty("characterDescription", out JsonElement descElement))
                        {
                            charDesc = descElement.GetString();
                        }
                        if (root.TryGetProperty("Abilities", out JsonElement jsonElement3))
                        {
                            foreach (var abilityElem in jsonElement3.EnumerateArray())
                            {
                                abilities.Add(abilityElem.GetString());
                            }
                        }

                        CharacterSheet characterSheet = new CharacterSheet(charName, charRank, charAlignment, abilities, Context.User.ToString());
                        characterSheet.characterDescription = charDesc;

                        if(characterSheet.IsValid())
                        {
                            Program.characterLibrary.AddCharacter(characterSheet, overriteExisting,true);
                            await ReplyAsync($"The character {characterSheet.Name} was added to library");
                        }
                        else
                        {
                            string errorReason = "";

                            if(characterSheet.Name == "")
                            {
                                errorReason += "name is invalid";
                            }
                            if(characterSheet.rank == CharacterRank.NoRank || characterSheet.rank >= CharacterRank.MaxValue)
                            {
                                if (errorReason != "") errorReason += ", ";
                                errorReason += "rank is invalid";
                            }
                            if (characterSheet.alignment == CharacterAlignment.None || characterSheet.alignment >= CharacterAlignment.AlignAlchemy)
                            {
                                if (errorReason != "") errorReason += ", ";
                                errorReason += "alignment is invalid";
                            }

                            await ReplyAsync($"The provided sheet was invalid because : {errorReason}");
                            return;
                        }
                    }
                }
                else if (extension == "zip")
                {
                    await ReplyAsync("No i dont do zips yet :(");
                    return;

                    File.WriteAllBytes($"{Directory.GetCurrentDirectory()}\\CharacterSheets\\{StringHelpers.GetFileNameWithoutExtension(attachment.Filename)}.zip", buffer);

                    FileStream fileStream = File.Open($"{Directory.GetCurrentDirectory()}\\CharacterSheets\\{StringHelpers.GetFileNameWithoutExtension(attachment.Filename)}.zip", FileMode.Open);
                    Archive archive = new Archive(fileStream);

                    if(!Directory.Exists($"{Directory.GetCurrentDirectory()}\\temp"))
                    {
                        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\temp");
                    }

                    string jsonString = "";
                    string imageName = "";
                    System.Drawing.Image characterImage = null;

                    foreach (var entry in archive.Entries)
                    {
                        string entry_extension = StringHelpers.GetFileNameExtension(entry.Name);

                        if (entry_extension == "txt" && jsonString == "")
                        {

                            entry.Extract($"{Directory.GetCurrentDirectory()}\\temp\\{entry.Name}");
                            jsonString = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}\\temp\\{entry.Name}");
                        }
                        else if ((entry_extension == "jpg" ||
                                  entry_extension == "png") &&
                                  characterImage == null)
                        {
                            characterImage = System.Drawing.Image.FromStream(entry.Open());
                            imageName = entry.Name;
                        }
                    }

                    CharacterSheet characterSheet = new CharacterSheet(jsonString,characterImage);
                    characterSheet.imageName = imageName;
                    Program.characterLibrary.AddCharacter(characterSheet, overriteExisting);
                }
                else
                {
                    await ReplyAsync($"Supplied extension of \"{extension}\" is not supported. Aborting . . .");
                    return;
                }
            }
        }
    }
}
