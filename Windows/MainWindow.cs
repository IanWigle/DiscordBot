using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Discord;
using Discord.WebSocket;

namespace SynovianEmpireDiscordBot.Windows
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            button2.Visible = Program.IsFirebaseEnabled();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            foreach(SocketGuild guild in Program.GetAllConnectedGuilds())
            {
                GuildSelector.Items.Add(guild.ToString());
            }

            CurrentServer = "";
            CurrentChannel = "";
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Do something
            CurrentServer = "";
            CurrentChannel = "";
        }

        private void GuildSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextChannelList.Items.Clear();
            
            foreach(SocketTextChannel channel in Program.GetAllGuildChannels(GuildSelector.SelectedItem.ToString()))
            {

                TextChannelList.Items.Add(channel.ToString());
                CurrentServer = GuildSelector.SelectedItem.ToString();
            }
        }

        private void RefreshServerButton_Click(object sender, EventArgs e)
        {
            GuildSelector.Items.Clear();
            TextChannelList.Items.Clear();
            CurrentTextChat.Items.Clear();

            foreach (SocketGuild guild in Program.GetAllConnectedGuilds())
            {
                GuildSelector.Items.Add(guild.ToString());
            }
        }

        private async void TextChannelCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentChannel = TextChannelList.SelectedItem.ToString();
            CurrentTextChat.Items.Clear();

            IEnumerable<IMessage> messages = await Program.GetTextChannel(Program.GetGuild(CurrentServer), CurrentChannel).GetMessagesAsync(NumberOfMessagesToGet+1).FlattenAsync();
            
            foreach (var message in messages)
            {
                string tableMessage = message.Author.ToString() + " : " + message.ToString();
                string[] messageData = { message.Timestamp.ToString(), tableMessage };
                ListViewItem listViewItem = new ListViewItem(messageData);
                CurrentTextChat.Items.Add(listViewItem);
            }
        }

        private void ShutdownButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void RefreshChannelButton_Click(object sender, EventArgs e)
        {
            CurrentTextChat.Items.Clear();

            IEnumerable<IMessage> messages = await Program.GetTextChannel(Program.GetGuild(CurrentServer), CurrentChannel).GetMessagesAsync(NumberOfMessagesToGet+1).FlattenAsync();

            foreach (var message in messages)
            {
                string tableMessage = message.Author.ToString() + " : " + message.ToString();
                string[] messageData = { message.Timestamp.ToString(), tableMessage };
                ListViewItem listViewItem = new ListViewItem(messageData);
                CurrentTextChat.Items.Add(listViewItem);
                //CurrentTextChat.Items.Add(tableMessage);
            }
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            if (CurrentChannel != "" && CurrentServer != "")
            {
                Program.SendMessageToChannel(CurrentChannel, CurrentServer, InputBox.Text);
                InputBox.Clear();
            }
        }

        private string CurrentServer, CurrentChannel;
        private int NumberOfMessagesToGet = 30;

        private async void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (CurrentChannel != "" && CurrentServer != "")
                {
                    Program.SendMessageToChannel(CurrentChannel, CurrentServer, InputBox.Text);
                    InputBox.Clear();

                    CurrentTextChat.Items.Clear();

                    IEnumerable<IMessage> messages = await Program.GetTextChannel(Program.GetGuild(CurrentServer), CurrentChannel).GetMessagesAsync(NumberOfMessagesToGet + 1).FlattenAsync();

                    foreach (var message in messages)
                    {
                        string tableMessage = message.Author.ToString() + " : " + message.ToString();
                        //CurrentTextChat.Items.Add(tableMessage);
                        string[] messageData = { message.Timestamp.ToString(), tableMessage };
                        ListViewItem listViewItem = new ListViewItem(messageData);
                        CurrentTextChat.Items.Add(listViewItem);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "png files (*.png)|*.PNG|jpg files (*.jpg)|*.JPG|gif files (*.gif)|*.GIF|mp4 files (*.mp4)|*.MP4|mov files (*.mov)|*.MOV|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (CurrentChannel != "" && CurrentServer != "")
                {
                    Program.UploadFileToChannel(CurrentChannel, CurrentServer, openFileDialog.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Program.IsFirebaseEnabled())
            {
                Program.firebaseManager.TestData();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumberOfMessagesToGet = (int)numericUpDown1.Value;
        }
    }
}
