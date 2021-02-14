using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynovianEmpireDiscordBot.BotData;
using System.Text.Json;
using System.Runtime.Remoting.Proxies;


using SynovianEmpireDiscordBot.Windows;
using SynovianEmpireDiscordBot.Libraries;
using SynovianEmpireDiscordBot.CharacterMakerClasses;

using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

namespace SynovianEmpireDiscordBot.Firebase
{
    public class FirebaseManager
    {
        private string firebase_url = "";
        private string api_key = "";
        private string userID = "";
        private bool IsValid = false;
        private bool LogginedIn = false;
        private string username = "", password = "";

        private FirebaseClient firebaseClient;
        private FirebaseAuthProvider firebaseAuthProvider;
        private FirebaseAuthLink firebaseAuthLink;

        public FirebaseManager()
        {
            if (InfoImportClass.GetFirebaseJson(out string jsonString))
            {
                using (JsonDocument jsonDocument = JsonDocument.Parse(jsonString))
                {
                    JsonElement jsonRoot = jsonDocument.RootElement;
                    if(jsonRoot.TryGetProperty("project_info",out JsonElement json_project_info))
                    {
                        if(json_project_info.TryGetProperty("firebase_url",out JsonElement firebaseurl))
                        {
                            firebase_url = firebaseurl.ToString();
                            
                        }
                    }
                    if (jsonRoot.TryGetProperty("client",out JsonElement json_client))
                    {
                        // TODO : find a new method to get api key from json. This hurts . . . 
                        // Because a firebase project can have multiple apps, a client section of the json 
                        // is an array that must be parsed to make sure we can get the correct authorization
                        // key to connect to the firebase project and database. This is important as apps 
                        // save unique variations of access/auth permissions in terms of connection/login, reading,
                        // and writing to any specific server-side containers, such as the database.
                        // Rather than doing this, perhaps making a class that reads all the service data
                        // into memory would be easier?
                        foreach(JsonElement jsonElement in json_client.EnumerateArray())
                        {                            
                            if(jsonElement.TryGetProperty("client_info",out JsonElement json_client_info))
                            {
                                if(json_client_info.TryGetProperty("android_client_info", out JsonElement json_android_client_info))
                                {
                                    if(json_android_client_info.TryGetProperty("package_name",out JsonElement json_package_name))
                                    {
                                        if(json_package_name.ToString() == "Syno.DiscordBot")
                                        {
                                            if(jsonElement.TryGetProperty("api_key", out JsonElement json_api_key))
                                            {
                                                foreach(JsonElement keyElement in json_api_key.EnumerateArray())
                                                {
                                                    if (keyElement.TryGetProperty("current_key", out JsonElement json_current_key))
                                                    {
                                                        api_key = json_current_key.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }                            
                        }
                    }
                }
            }

            IsValid = firebase_url != "" && api_key != "";

            if (IsValid)
            {
                FirebaseSignIn firebaseSignIn = new FirebaseSignIn();
                firebaseSignIn.ShowDialog();                

                if(firebaseSignIn.username != "" && firebaseSignIn.password != "")
                {
                    username = firebaseSignIn.username;
                    password = firebaseSignIn.password;

                    firebaseClient = new FirebaseClient(
                    firebase_url,
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => LoginAsync()
                    });

                    firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(api_key));

                    try
                    {
                        firebaseAuthProvider.SignInWithEmailAndPasswordAsync(username, password).ContinueWith(x => firebaseAuthLink = x.Result).Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not log in with syno account to firebase authentication, logging in as anon.");
                        Console.WriteLine("Exception : \n " + e.Message);
                        firebaseAuthProvider.SignInAnonymouslyAsync().ContinueWith(x => firebaseAuthLink = x.Result).Wait();
                    }   

                    string jsonData = JsonSerializer.Serialize(Program.abilityLibrary.GetAbility("testname1"));

                    firebaseClient.Child("Abilities").PostAsync<Dictionary<string, Ability>>(Program.abilityLibrary.GetAbilitiesAsDictionary());
                    //firebaseClient.Child("Abilities").PostAsync<Ability>(Program.abilityLibrary.GetAbility("testname2"));
                    //firebaseClient.Child("Abilities").PostAsync<Ability>(Program.abilityLibrary.GetAbility("testname3"));
                    //firebaseClient.Child("Abilities").PostAsync<Ability>(Program.abilityLibrary.GetAbility("testname4"));

                    
                    //string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";
                    //
                    //if (!Directory.Exists(curDir))
                    //{
                    //    Directory.CreateDirectory(curDir);
                    //    Console.WriteLine("Error! List Folder could not be found! Made directory at " + curDir);
                    //}
                    //
                    //string JsonString = System.IO.File.ReadAllText(curDir + "AbilityLibrary.txt");
                    //
                    //firebaseClient.Child("dinosours").Child(userID).PostAsync(JsonString);
                    //Task.Delay(10000);
                    //TestData();
                }
            }
        }

        ~FirebaseManager()
        {

        }

        public async Task<string> LoginAsync()
        {
            return firebaseAuthLink.FirebaseToken;
        }

        public async void TestData()
        {
            //string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";
            //
            //if (!Directory.Exists(curDir))
            //{
            //    Directory.CreateDirectory(curDir);
            //    Console.WriteLine("Error! List Folder could not be found! Made directory at " + curDir);
            //}
            //
            //string JsonString = System.IO.File.ReadAllText(curDir + "AbilityLibrary.txt");
            //
            //firebaseClient.Child("dinosours").Child(userID).PostAsync(JsonString);

            try
            {
                int numAbilities = await firebaseClient.Child("AbilityListSize").OnceSingleAsync<int>();
                Console.WriteLine($"There are {numAbilities} abilities in the database");

                //for(int i = 0; i < numAbilities; i++)
                //{
                //    string abilityName = await firebaseClient.Child("Abilities").Child($"{i}").Child("Name").OnceSingleAsync<string>();
                //    Console.WriteLine(abilityName);
                //}
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public bool IsFirebaseValid() { return IsValid; }
    }
}
