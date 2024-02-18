namespace SES_GUI
{
    partial class Student_Add
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
            ClassComboBox = new ComboBox();
            AltClassComboBox = new ComboBox();
            LoadButton = new Button();
            SaveButton = new Button();
            ClassAgeLabel = new Label();
            AltClassLabel = new Label();
            StudentNameTextBox = new TextBox();
            StudentSchoolNumberTextBox = new TextBox();
            StudentSurNameTextBox = new TextBox();
            StudentNumberLabel = new Label();
            StudentNameLabel = new Label();
            StudentSurNameLabel = new Label();
            DefaultPathLocationButton = new Button();
            SuspendLayout();
            // 
            // ClassComboBox
            // 
            ClassComboBox.FormattingEnabled = true;
            ClassComboBox.Location = new Point(54, 51);
            ClassComboBox.Name = "ClassComboBox";
            ClassComboBox.Size = new Size(45, 23);
            ClassComboBox.TabIndex = 2;
            // 
            // AltClassComboBox
            // 
            AltClassComboBox.FormattingEnabled = true;
            AltClassComboBox.Location = new Point(72, 88);
            AltClassComboBox.Name = "AltClassComboBox";
            AltClassComboBox.Size = new Size(45, 23);
            AltClassComboBox.TabIndex = 3;
            // 
            // LoadButton
            // 
            LoadButton.Location = new Point(12, 12);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(75, 23);
            LoadButton.TabIndex = 1;
            LoadButton.Text = "Yükle";
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += LoadButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Enabled = false;
            SaveButton.Location = new Point(117, 240);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 7;
            SaveButton.Text = "Kaydet";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // ClassAgeLabel
            // 
            ClassAgeLabel.AutoSize = true;
            ClassAgeLabel.Location = new Point(12, 54);
            ClassAgeLabel.Name = "ClassAgeLabel";
            ClassAgeLabel.Size = new Size(36, 15);
            ClassAgeLabel.TabIndex = 4;
            ClassAgeLabel.Text = "Sınıf :";
            // 
            // AltClassLabel
            // 
            AltClassLabel.AutoSize = true;
            AltClassLabel.Location = new Point(12, 91);
            AltClassLabel.Name = "AltClassLabel";
            AltClassLabel.Size = new Size(54, 15);
            AltClassLabel.TabIndex = 5;
            AltClassLabel.Text = "Alt Sınıf :";
            // 
            // StudentNameTextBox
            // 
            StudentNameTextBox.Location = new Point(55, 162);
            StudentNameTextBox.Name = "StudentNameTextBox";
            StudentNameTextBox.Size = new Size(100, 23);
            StudentNameTextBox.TabIndex = 5;
            // 
            // StudentSchoolNumberTextBox
            // 
            StudentSchoolNumberTextBox.Location = new Point(110, 125);
            StudentSchoolNumberTextBox.Name = "StudentSchoolNumberTextBox";
            StudentSchoolNumberTextBox.Size = new Size(45, 23);
            StudentSchoolNumberTextBox.TabIndex = 4;
            // 
            // StudentSurNameTextBox
            // 
            StudentSurNameTextBox.Location = new Point(66, 199);
            StudentSurNameTextBox.Name = "StudentSurNameTextBox";
            StudentSurNameTextBox.Size = new Size(100, 23);
            StudentSurNameTextBox.TabIndex = 6;
            // 
            // StudentNumberLabel
            // 
            StudentNumberLabel.AutoSize = true;
            StudentNumberLabel.Location = new Point(12, 128);
            StudentNumberLabel.Name = "StudentNumberLabel";
            StudentNumberLabel.Size = new Size(92, 15);
            StudentNumberLabel.TabIndex = 9;
            StudentNumberLabel.Text = "Okul Numarası :";
            // 
            // StudentNameLabel
            // 
            StudentNameLabel.AutoSize = true;
            StudentNameLabel.Location = new Point(12, 165);
            StudentNameLabel.Name = "StudentNameLabel";
            StudentNameLabel.Size = new Size(31, 15);
            StudentNameLabel.TabIndex = 10;
            StudentNameLabel.Text = "Adı :";
            // 
            // StudentSurNameLabel
            // 
            StudentSurNameLabel.AutoSize = true;
            StudentSurNameLabel.Location = new Point(12, 202);
            StudentSurNameLabel.Name = "StudentSurNameLabel";
            StudentSurNameLabel.Size = new Size(48, 15);
            StudentSurNameLabel.TabIndex = 11;
            StudentSurNameLabel.Text = "Soyadı :";
            // 
            // DefaultPathLocationButton
            // 
            DefaultPathLocationButton.Location = new Point(12, 240);
            DefaultPathLocationButton.Name = "DefaultPathLocationButton";
            DefaultPathLocationButton.Size = new Size(20, 23);
            DefaultPathLocationButton.TabIndex = 8;
            DefaultPathLocationButton.Text = "V";
            DefaultPathLocationButton.UseVisualStyleBackColor = true;
            DefaultPathLocationButton.Click += DefaultPathLocationButton_Click;
            // 
            // Student_Add
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(204, 275);
            Controls.Add(DefaultPathLocationButton);
            Controls.Add(StudentSurNameLabel);
            Controls.Add(StudentNameLabel);
            Controls.Add(StudentNumberLabel);
            Controls.Add(StudentSurNameTextBox);
            Controls.Add(StudentSchoolNumberTextBox);
            Controls.Add(StudentNameTextBox);
            Controls.Add(AltClassLabel);
            Controls.Add(ClassAgeLabel);
            Controls.Add(SaveButton);
            Controls.Add(LoadButton);
            Controls.Add(AltClassComboBox);
            Controls.Add(ClassComboBox);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "Student_Add";
            Text = "Öğrenci Ekle";
            Load += Student_Add_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox ClassComboBox;
        private ComboBox AltClassComboBox;
        private Button LoadButton;
        private Button SaveButton;
        private Label ClassAgeLabel;
        private Label AltClassLabel;
        private TextBox StudentNameTextBox;
        private TextBox StudentSchoolNumberTextBox;
        private TextBox StudentSurNameTextBox;
        private Label StudentNumberLabel;
        private Label StudentNameLabel;
        private Label StudentSurNameLabel;
        private Button DefaultPathLocationButton;
    }
}