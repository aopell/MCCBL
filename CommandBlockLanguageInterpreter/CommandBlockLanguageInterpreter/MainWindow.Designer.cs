namespace CommandBlockLanguageInterpreter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.CommandInput = new System.Windows.Forms.TextBox();
            this.SendCommandButton = new System.Windows.Forms.Button();
            this.ChooseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ConsoleWindow = new System.Windows.Forms.RichTextBox();
            this.MainWindowToolStrip = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operatorsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whitelistMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCommandBlockLanguageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minRamHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gBGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mBMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.for1GBType1GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.for512MBType512MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minRAM = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.maxRamHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gBGToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mBMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.for1GBType1GToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.for512MBType512MToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.maxRAM = new System.Windows.Forms.ToolStripTextBox();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainWindowToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // CommandInput
            // 
            this.CommandInput.BackColor = System.Drawing.Color.Black;
            this.CommandInput.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommandInput.ForeColor = System.Drawing.Color.White;
            this.CommandInput.Location = new System.Drawing.Point(12, 396);
            this.CommandInput.Name = "CommandInput";
            this.CommandInput.Size = new System.Drawing.Size(614, 22);
            this.CommandInput.TabIndex = 3;
            this.CommandInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CommandInput_KeyUp);
            // 
            // SendCommandButton
            // 
            this.SendCommandButton.BackColor = System.Drawing.Color.Black;
            this.SendCommandButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendCommandButton.ForeColor = System.Drawing.Color.White;
            this.SendCommandButton.Location = new System.Drawing.Point(625, 395);
            this.SendCommandButton.Name = "SendCommandButton";
            this.SendCommandButton.Size = new System.Drawing.Size(36, 21);
            this.SendCommandButton.TabIndex = 4;
            this.SendCommandButton.Text = "OK";
            this.SendCommandButton.UseVisualStyleBackColor = false;
            this.SendCommandButton.Click += new System.EventHandler(this.SendCommandButton_Click);
            // 
            // ChooseFileDialog
            // 
            this.ChooseFileDialog.Filter = "Jar Files (*.jar)|*.jar";
            this.ChooseFileDialog.Tag = "Jar Files (*.jar)|*.jar";
            // 
            // ConsoleWindow
            // 
            this.ConsoleWindow.BackColor = System.Drawing.Color.Black;
            this.ConsoleWindow.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsoleWindow.ForeColor = System.Drawing.Color.White;
            this.ConsoleWindow.Location = new System.Drawing.Point(12, 27);
            this.ConsoleWindow.Name = "ConsoleWindow";
            this.ConsoleWindow.ReadOnly = true;
            this.ConsoleWindow.Size = new System.Drawing.Size(649, 363);
            this.ConsoleWindow.TabIndex = 5;
            this.ConsoleWindow.Text = "";
            // 
            // MainWindowToolStrip
            // 
            this.MainWindowToolStrip.BackColor = System.Drawing.Color.Gray;
            this.MainWindowToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainWindowToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainWindowToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainWindowToolStrip.Name = "MainWindowToolStrip";
            this.MainWindowToolStrip.Size = new System.Drawing.Size(673, 24);
            this.MainWindowToolStrip.TabIndex = 6;
            this.MainWindowToolStrip.Text = "menuStrip1";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.switchToolStripMenuItem});
            this.serverToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.startToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.stopToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopToolStripMenuItem.ForeColor = System.Drawing.Color.LightCoral;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stopToolStripMenuItem.Tag = "Disabled";
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.restartToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restartToolStripMenuItem.ForeColor = System.Drawing.Color.LightCoral;
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restartToolStripMenuItem.Tag = "Disabled";
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // switchToolStripMenuItem
            // 
            this.switchToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.switchToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.switchToolStripMenuItem.ForeColor = System.Drawing.Color.LightCoral;
            this.switchToolStripMenuItem.Name = "switchToolStripMenuItem";
            this.switchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.switchToolStripMenuItem.Tag = "Disabled";
            this.switchToolStripMenuItem.Text = "Switch";
            this.switchToolStripMenuItem.Click += new System.EventHandler(this.switchToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operatorsMenuItem,
            this.propertiesMenuItem,
            this.whitelistMenuItem,
            this.importCommandBlockLanguageFileToolStripMenuItem});
            this.toolsToolStripMenuItem.Enabled = false;
            this.toolsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Tag = "";
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // operatorsMenuItem
            // 
            this.operatorsMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.operatorsMenuItem.ForeColor = System.Drawing.Color.White;
            this.operatorsMenuItem.Name = "operatorsMenuItem";
            this.operatorsMenuItem.Size = new System.Drawing.Size(275, 22);
            this.operatorsMenuItem.Text = "Show Operators";
            this.operatorsMenuItem.Click += new System.EventHandler(this.operatorsMenuItem_Click);
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.propertiesMenuItem.ForeColor = System.Drawing.Color.White;
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Size = new System.Drawing.Size(275, 22);
            this.propertiesMenuItem.Text = "Open Server Properties";
            this.propertiesMenuItem.Click += new System.EventHandler(this.propertiesMenuItem_Click);
            // 
            // whitelistMenuItem
            // 
            this.whitelistMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.whitelistMenuItem.ForeColor = System.Drawing.Color.White;
            this.whitelistMenuItem.Name = "whitelistMenuItem";
            this.whitelistMenuItem.Size = new System.Drawing.Size(275, 22);
            this.whitelistMenuItem.Text = "Show Whitelist";
            this.whitelistMenuItem.Click += new System.EventHandler(this.whitelistMenuItem_Click);
            // 
            // importCommandBlockLanguageFileToolStripMenuItem
            // 
            this.importCommandBlockLanguageFileToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.importCommandBlockLanguageFileToolStripMenuItem.ForeColor = System.Drawing.Color.LightCoral;
            this.importCommandBlockLanguageFileToolStripMenuItem.Name = "importCommandBlockLanguageFileToolStripMenuItem";
            this.importCommandBlockLanguageFileToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.importCommandBlockLanguageFileToolStripMenuItem.Tag = "Disabled";
            this.importCommandBlockLanguageFileToolStripMenuItem.Text = "Import CommandBlock Language File";
            this.importCommandBlockLanguageFileToolStripMenuItem.Click += new System.EventHandler(this.importCommandBlockLanguageFileToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverOptionsToolStripMenuItem,
            this.minRamHelpMenuItem,
            this.minRAM,
            this.toolStripSeparator1,
            this.maxRamHelpMenuItem,
            this.maxRAM});
            this.optionsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // serverOptionsToolStripMenuItem
            // 
            this.serverOptionsToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.serverOptionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.serverOptionsToolStripMenuItem.Name = "serverOptionsToolStripMenuItem";
            this.serverOptionsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.serverOptionsToolStripMenuItem.Text = "Server Options";
            this.serverOptionsToolStripMenuItem.Click += new System.EventHandler(this.serverOptionsToolStripMenuItem_Click);
            // 
            // minRamHelpMenuItem
            // 
            this.minRamHelpMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.minRamHelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gBGToolStripMenuItem,
            this.mBMToolStripMenuItem,
            this.for1GBType1GToolStripMenuItem,
            this.for512MBType512MToolStripMenuItem});
            this.minRamHelpMenuItem.ForeColor = System.Drawing.Color.White;
            this.minRamHelpMenuItem.Name = "minRamHelpMenuItem";
            this.minRamHelpMenuItem.Size = new System.Drawing.Size(173, 22);
            this.minRamHelpMenuItem.Text = "Minimum RAM [?]";
            this.minRamHelpMenuItem.ToolTipText = "GB = G, MB = M\r\nFor 1GB type 1G\r\nFor 512MB type 512M";
            // 
            // gBGToolStripMenuItem
            // 
            this.gBGToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.gBGToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.gBGToolStripMenuItem.Name = "gBGToolStripMenuItem";
            this.gBGToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.gBGToolStripMenuItem.Text = "GB = G";
            // 
            // mBMToolStripMenuItem
            // 
            this.mBMToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.mBMToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.mBMToolStripMenuItem.Name = "mBMToolStripMenuItem";
            this.mBMToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.mBMToolStripMenuItem.Text = "MB = M";
            // 
            // for1GBType1GToolStripMenuItem
            // 
            this.for1GBType1GToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.for1GBType1GToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.for1GBType1GToolStripMenuItem.Name = "for1GBType1GToolStripMenuItem";
            this.for1GBType1GToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.for1GBType1GToolStripMenuItem.Text = "For 1GB, type 1G";
            // 
            // for512MBType512MToolStripMenuItem
            // 
            this.for512MBType512MToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.for512MBType512MToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.for512MBType512MToolStripMenuItem.Name = "for512MBType512MToolStripMenuItem";
            this.for512MBType512MToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.for512MBType512MToolStripMenuItem.Text = "For 512MB, type 512M";
            // 
            // minRAM
            // 
            this.minRAM.BackColor = System.Drawing.Color.DimGray;
            this.minRAM.ForeColor = System.Drawing.Color.White;
            this.minRAM.Name = "minRAM";
            this.minRAM.Size = new System.Drawing.Size(100, 23);
            this.minRAM.Text = "1G";
            this.minRAM.ToolTipText = "Type value for minimum RAM here";
            this.minRAM.TextChanged += new System.EventHandler(this.minRAM_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // maxRamHelpMenuItem
            // 
            this.maxRamHelpMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.maxRamHelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gBGToolStripMenuItem1,
            this.mBMToolStripMenuItem1,
            this.for1GBType1GToolStripMenuItem1,
            this.for512MBType512MToolStripMenuItem1});
            this.maxRamHelpMenuItem.ForeColor = System.Drawing.Color.White;
            this.maxRamHelpMenuItem.Name = "maxRamHelpMenuItem";
            this.maxRamHelpMenuItem.Size = new System.Drawing.Size(173, 22);
            this.maxRamHelpMenuItem.Text = "Maximum RAM [?]";
            this.maxRamHelpMenuItem.ToolTipText = "GB = G, MB = M\r\nFor 1GB type 1G\r\nFor 512MB type 512M";
            // 
            // gBGToolStripMenuItem1
            // 
            this.gBGToolStripMenuItem1.BackColor = System.Drawing.Color.Black;
            this.gBGToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.gBGToolStripMenuItem1.Name = "gBGToolStripMenuItem1";
            this.gBGToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.gBGToolStripMenuItem1.Text = "GB = G";
            // 
            // mBMToolStripMenuItem1
            // 
            this.mBMToolStripMenuItem1.BackColor = System.Drawing.Color.Black;
            this.mBMToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.mBMToolStripMenuItem1.Name = "mBMToolStripMenuItem1";
            this.mBMToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.mBMToolStripMenuItem1.Text = "MB = M";
            // 
            // for1GBType1GToolStripMenuItem1
            // 
            this.for1GBType1GToolStripMenuItem1.BackColor = System.Drawing.Color.Black;
            this.for1GBType1GToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.for1GBType1GToolStripMenuItem1.Name = "for1GBType1GToolStripMenuItem1";
            this.for1GBType1GToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.for1GBType1GToolStripMenuItem1.Text = "For 1GB, type 1G";
            // 
            // for512MBType512MToolStripMenuItem1
            // 
            this.for512MBType512MToolStripMenuItem1.BackColor = System.Drawing.Color.Black;
            this.for512MBType512MToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.for512MBType512MToolStripMenuItem1.Name = "for512MBType512MToolStripMenuItem1";
            this.for512MBType512MToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.for512MBType512MToolStripMenuItem1.Text = "For 512MB, type 512M";
            // 
            // maxRAM
            // 
            this.maxRAM.BackColor = System.Drawing.Color.DimGray;
            this.maxRAM.ForeColor = System.Drawing.Color.White;
            this.maxRAM.Name = "maxRAM";
            this.maxRAM.Size = new System.Drawing.Size(100, 23);
            this.maxRAM.Text = "1G";
            this.maxRAM.ToolTipText = "Type value for maximum RAM here";
            this.maxRAM.TextChanged += new System.EventHandler(this.maxRAM_TextChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.BackColor = System.Drawing.Color.DimGray;
            this.helpToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // MainWindow
            // 
            this.AcceptButton = this.SendCommandButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(673, 422);
            this.Controls.Add(this.ConsoleWindow);
            this.Controls.Add(this.SendCommandButton);
            this.Controls.Add(this.CommandInput);
            this.Controls.Add(this.MainWindowToolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainWindowToolStrip;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Minecraft Server Wrapper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MainWindowToolStrip.ResumeLayout(false);
            this.MainWindowToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox CommandInput;
        private System.Windows.Forms.Button SendCommandButton;
        private System.Windows.Forms.OpenFileDialog ChooseFileDialog;
        private System.Windows.Forms.RichTextBox ConsoleWindow;
        private System.Windows.Forms.MenuStrip MainWindowToolStrip;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operatorsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whitelistMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox minRAM;
        private System.Windows.Forms.ToolStripTextBox maxRAM;
        private System.Windows.Forms.ToolStripMenuItem minRamHelpMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem maxRamHelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCommandBlockLanguageFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gBGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mBMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem for1GBType1GToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gBGToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mBMToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem for1GBType1GToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem for512MBType512MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem for512MBType512MToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem serverOptionsToolStripMenuItem;
    }
}

