namespace SES_GUI.UI
{
    partial class SchoolAdd
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
            Save = new Button();
            Label = new Label();
            SchoolNameTextBox = new TextBox();
            SuspendLayout();
            // 
            // Save
            // 
            Save.Location = new Point(324, 18);
            Save.Name = "Save";
            Save.Size = new Size(110, 23);
            Save.TabIndex = 3;
            Save.Text = "Kaydet";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // Label
            // 
            Label.AutoSize = true;
            Label.Location = new Point(3, 24);
            Label.Name = "Label";
            Label.Size = new Size(59, 15);
            Label.TabIndex = 1;
            Label.Text = "Okul Adı :";
            // 
            // SchoolNameTextBox
            // 
            SchoolNameTextBox.Location = new Point(68, 18);
            SchoolNameTextBox.Name = "SchoolNameTextBox";
            SchoolNameTextBox.Size = new Size(250, 23);
            SchoolNameTextBox.TabIndex = 0;
            // 
            // SchoolAdd
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 64);
            ControlBox = false;
            Controls.Add(SchoolNameTextBox);
            Controls.Add(Label);
            Controls.Add(Save);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "SchoolAdd";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Okul Ekle";
            Load += SchoolAdd_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Save;
        private Label Label;
        private TextBox SchoolNameTextBox;
    }
}