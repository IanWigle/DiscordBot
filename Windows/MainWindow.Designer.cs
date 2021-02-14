namespace SynovianEmpireDiscordBot.Windows
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GuildSelector = new System.Windows.Forms.ComboBox();
            this.RefreshServers = new System.Windows.Forms.Button();
            this.TextChannelList = new System.Windows.Forms.ComboBox();
            this.CurrentTextChat = new System.Windows.Forms.ListView();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.ShutdownButton = new System.Windows.Forms.Button();
            this.RefreshChannelButton = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // GuildSelector
            // 
            this.GuildSelector.FormattingEnabled = true;
            this.GuildSelector.Location = new System.Drawing.Point(946, 21);
            this.GuildSelector.Name = "GuildSelector";
            this.GuildSelector.Size = new System.Drawing.Size(121, 21);
            this.GuildSelector.TabIndex = 0;
            this.GuildSelector.SelectedIndexChanged += new System.EventHandler(this.GuildSelector_SelectedIndexChanged);
            // 
            // RefreshServers
            // 
            this.RefreshServers.Location = new System.Drawing.Point(859, 21);
            this.RefreshServers.Name = "RefreshServers";
            this.RefreshServers.Size = new System.Drawing.Size(75, 23);
            this.RefreshServers.TabIndex = 1;
            this.RefreshServers.Text = "Refresh";
            this.RefreshServers.UseVisualStyleBackColor = true;
            this.RefreshServers.Click += new System.EventHandler(this.RefreshServerButton_Click);
            // 
            // TextChannelList
            // 
            this.TextChannelList.FormattingEnabled = true;
            this.TextChannelList.Location = new System.Drawing.Point(946, 65);
            this.TextChannelList.Name = "TextChannelList";
            this.TextChannelList.Size = new System.Drawing.Size(121, 21);
            this.TextChannelList.TabIndex = 2;
            this.TextChannelList.SelectedIndexChanged += new System.EventHandler(this.TextChannelCombo_SelectedIndexChanged);
            // 
            // CurrentTextChat
            // 
            this.CurrentTextChat.AutoArrange = false;
            this.CurrentTextChat.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeColumn,
            this.textHeader});
            this.CurrentTextChat.HideSelection = false;
            this.CurrentTextChat.Location = new System.Drawing.Point(13, 13);
            this.CurrentTextChat.Name = "CurrentTextChat";
            this.CurrentTextChat.ShowGroups = false;
            this.CurrentTextChat.Size = new System.Drawing.Size(840, 410);
            this.CurrentTextChat.TabIndex = 3;
            this.CurrentTextChat.UseCompatibleStateImageBehavior = false;
            this.CurrentTextChat.View = System.Windows.Forms.View.Details;
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(13, 430);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(840, 20);
            this.InputBox.TabIndex = 4;
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyDown);
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(860, 430);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.SendMessageButton.TabIndex = 5;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // ShutdownButton
            // 
            this.ShutdownButton.Location = new System.Drawing.Point(1009, 479);
            this.ShutdownButton.Name = "ShutdownButton";
            this.ShutdownButton.Size = new System.Drawing.Size(75, 23);
            this.ShutdownButton.TabIndex = 6;
            this.ShutdownButton.Text = "Shutdown";
            this.ShutdownButton.UseVisualStyleBackColor = true;
            this.ShutdownButton.Click += new System.EventHandler(this.ShutdownButton_Click);
            // 
            // RefreshChannelButton
            // 
            this.RefreshChannelButton.Location = new System.Drawing.Point(946, 430);
            this.RefreshChannelButton.Name = "RefreshChannelButton";
            this.RefreshChannelButton.Size = new System.Drawing.Size(94, 23);
            this.RefreshChannelButton.TabIndex = 7;
            this.RefreshChannelButton.Text = "RefreshChannel";
            this.RefreshChannelButton.UseVisualStyleBackColor = true;
            this.RefreshChannelButton.Click += new System.EventHandler(this.RefreshChannelButton_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(860, 347);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(860, 460);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(742, 459);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timeColumn
            // 
            this.timeColumn.Text = "Timestamp";
            this.timeColumn.Width = 200;
            // 
            // textHeader
            // 
            this.textHeader.Text = "Message";
            this.textHeader.Width = 500;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 514);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.RefreshChannelButton);
            this.Controls.Add(this.ShutdownButton);
            this.Controls.Add(this.SendMessageButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.CurrentTextChat);
            this.Controls.Add(this.TextChannelList);
            this.Controls.Add(this.RefreshServers);
            this.Controls.Add(this.GuildSelector);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TextChannelList_TextChanged(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.ComboBox GuildSelector;
        private System.Windows.Forms.Button RefreshServers;
        private System.Windows.Forms.ComboBox TextChannelList;
        private System.Windows.Forms.ListView CurrentTextChat;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.Button ShutdownButton;
        private System.Windows.Forms.Button RefreshChannelButton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;

        public void RefreshFromOutside()
        {
            RefreshServerButton_Click(null,new System.EventArgs());
        }

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ColumnHeader timeColumn;
        private System.Windows.Forms.ColumnHeader textHeader;
    }
}