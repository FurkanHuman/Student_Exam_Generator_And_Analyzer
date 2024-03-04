using System.Drawing;
using System.Windows.Forms;

namespace SES_GUI.UI
{
    partial class DbConnectionBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbConnectionBuilder));
            Connect = new Button();
            ServerTextBox = new TextBox();
            DatabaseTextbox = new TextBox();
            UsernameTextBox = new TextBox();
            PasswordTextBox = new TextBox();
            HostLabel = new Label();
            PortLabel = new Label();
            DatabaseLabel = new Label();
            UserNameLabel = new Label();
            PasswordLabel = new Label();
            PasswordRepeatTextBox = new TextBox();
            PasswordRepeatLabel = new Label();
            MessageLabel = new Label();
            PortNumericUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // Connect
            // 
            resources.ApplyResources(Connect, "Connect");
            Connect.Name = "Connect";
            Connect.UseVisualStyleBackColor = true;
            Connect.Click += Connect_Click;
            // 
            // ServerTextBox
            // 
            resources.ApplyResources(ServerTextBox, "ServerTextBox");
            ServerTextBox.Name = "ServerTextBox";
            // 
            // DatabaseTextbox
            // 
            resources.ApplyResources(DatabaseTextbox, "DatabaseTextbox");
            DatabaseTextbox.Name = "DatabaseTextbox";
            // 
            // UsernameTextBox
            // 
            resources.ApplyResources(UsernameTextBox, "UsernameTextBox");
            UsernameTextBox.Name = "UsernameTextBox";
            // 
            // PasswordTextBox
            // 
            resources.ApplyResources(PasswordTextBox, "PasswordTextBox");
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.TextChanged += TextBox_TextChanged;
            // 
            // HostLabel
            // 
            resources.ApplyResources(HostLabel, "HostLabel");
            HostLabel.Name = "HostLabel";
            // 
            // PortLabel
            // 
            resources.ApplyResources(PortLabel, "PortLabel");
            PortLabel.Name = "PortLabel";
            // 
            // DatabaseLabel
            // 
            resources.ApplyResources(DatabaseLabel, "DatabaseLabel");
            DatabaseLabel.Name = "DatabaseLabel";
            // 
            // UserNameLabel
            // 
            resources.ApplyResources(UserNameLabel, "UserNameLabel");
            UserNameLabel.Name = "UserNameLabel";
            // 
            // PasswordLabel
            // 
            resources.ApplyResources(PasswordLabel, "PasswordLabel");
            PasswordLabel.Name = "PasswordLabel";
            // 
            // PasswordRepeatTextBox
            // 
            resources.ApplyResources(PasswordRepeatTextBox, "PasswordRepeatTextBox");
            PasswordRepeatTextBox.Name = "PasswordRepeatTextBox";
            PasswordRepeatTextBox.TextChanged += TextBox_TextChanged;
            // 
            // PasswordRepeatLabel
            // 
            resources.ApplyResources(PasswordRepeatLabel, "PasswordRepeatLabel");
            PasswordRepeatLabel.Name = "PasswordRepeatLabel";
            // 
            // MessageLabel
            // 
            resources.ApplyResources(MessageLabel, "MessageLabel");
            MessageLabel.ForeColor = Color.Crimson;
            MessageLabel.Name = "MessageLabel";
            // 
            // PortNumericUpDown
            // 
            resources.ApplyResources(PortNumericUpDown, "PortNumericUpDown");
            PortNumericUpDown.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            PortNumericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            PortNumericUpDown.Name = "PortNumericUpDown";
            PortNumericUpDown.Value = new decimal(new int[] { 5432, 0, 0, 0 });
            // 
            // DbConnectionBuilder
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(PortNumericUpDown);
            Controls.Add(MessageLabel);
            Controls.Add(PasswordRepeatLabel);
            Controls.Add(PasswordRepeatTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(UserNameLabel);
            Controls.Add(DatabaseLabel);
            Controls.Add(PortLabel);
            Controls.Add(HostLabel);
            Controls.Add(PasswordTextBox);
            Controls.Add(UsernameTextBox);
            Controls.Add(DatabaseTextbox);
            Controls.Add(ServerTextBox);
            Controls.Add(Connect);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DbConnectionBuilder";
            ShowIcon = false;
            FormClosing += DbConnectionBuilder_FormClosing;
            Load += DbConnectionBuilder_Load;
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox ServerTextBox;
        private TextBox DatabaseTextbox;
        private TextBox UsernameTextBox;
        private TextBox PasswordTextBox;
        private TextBox PasswordRepeatTextBox;
        private NumericUpDown PortNumericUpDown;
        private Label HostLabel;
        private Label PortLabel;
        private Label DatabaseLabel;
        private Label UserNameLabel;
        private Label PasswordLabel;
        private Label PasswordRepeatLabel;
        private Label MessageLabel;
        private Button Connect;
    }
}