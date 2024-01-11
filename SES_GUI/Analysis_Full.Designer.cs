namespace SES_GUI
{
    partial class Analysis_Full
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
            Btn_StudentLoad = new Button();
            ClassAgeComboBox = new ComboBox();
            AltClassComboBox = new ComboBox();
            ClassLabel = new Label();
            AltClassLabel = new Label();
            StudentsNotesDataGridView = new DataGridView();
            HowMuchQuestion = new TextBox();
            QuestionScoreLoad = new Button();
            StudentCount = new Label();
            ScoreReferenceDGW = new DataGridView();
            InfoLabel = new Label();
            GenAnalisys = new Button();
            StudentScoreDGWLabel = new Label();
            ExamSemesterYear = new Label();
            SemesterYearComboBox = new ComboBox();
            LabelOfLessonName = new Label();
            LessonNameTextBox = new TextBox();
            SemesterSesionLabel = new Label();
            SemesterComboBox = new ComboBox();
            SemesterSessionComboBox = new ComboBox();
            SemesterSessionLabel = new Label();
            ExamCodeLabel = new Label();
            ExamCodeTextbox = new TextBox();
            FooterTextBox = new TextBox();
            FooterLabel = new Label();
            SchoolTextBox = new TextBox();
            SchoolLabel = new Label();
            TeacherLabel = new Label();
            TeacherTextbox = new TextBox();
            PrincipalTextBox = new TextBox();
            PrincipalLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)StudentsNotesDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScoreReferenceDGW).BeginInit();
            SuspendLayout();
            // 
            // Btn_StudentLoad
            // 
            Btn_StudentLoad.Location = new Point(12, 16);
            Btn_StudentLoad.Name = "Btn_StudentLoad";
            Btn_StudentLoad.Size = new Size(98, 23);
            Btn_StudentLoad.TabIndex = 0;
            Btn_StudentLoad.Text = "Öğrenci Yükle";
            Btn_StudentLoad.UseVisualStyleBackColor = true;
            Btn_StudentLoad.Click += Btn_StudentLoad_Click;
            // 
            // ClassAgeComboBox
            // 
            ClassAgeComboBox.FormattingEnabled = true;
            ClassAgeComboBox.Location = new Point(158, 16);
            ClassAgeComboBox.Name = "ClassAgeComboBox";
            ClassAgeComboBox.Size = new Size(50, 23);
            ClassAgeComboBox.TabIndex = 2;
            // 
            // AltClassComboBox
            // 
            AltClassComboBox.FormattingEnabled = true;
            AltClassComboBox.Location = new Point(277, 16);
            AltClassComboBox.Name = "AltClassComboBox";
            AltClassComboBox.Size = new Size(50, 23);
            AltClassComboBox.TabIndex = 3;
            // 
            // ClassLabel
            // 
            ClassLabel.AutoSize = true;
            ClassLabel.Location = new Point(116, 20);
            ClassLabel.Name = "ClassLabel";
            ClassLabel.Size = new Size(36, 15);
            ClassLabel.TabIndex = 5;
            ClassLabel.Text = "Sınıfı:";
            // 
            // AltClassLabel
            // 
            AltClassLabel.AutoSize = true;
            AltClassLabel.Location = new Point(230, 20);
            AltClassLabel.Name = "AltClassLabel";
            AltClassLabel.Size = new Size(41, 15);
            AltClassLabel.TabIndex = 6;
            AltClassLabel.Text = "Şubesi";
            // 
            // StudentsNotesDataGridView
            // 
            StudentsNotesDataGridView.AllowUserToOrderColumns = true;
            StudentsNotesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            StudentsNotesDataGridView.Location = new Point(277, 80);
            StudentsNotesDataGridView.Name = "StudentsNotesDataGridView";
            StudentsNotesDataGridView.Size = new Size(516, 445);
            StudentsNotesDataGridView.TabIndex = 7;
            StudentsNotesDataGridView.CellFormatting += StudentsNotesDataGridView_CellFormatting;
            StudentsNotesDataGridView.EditingControlShowing += EditingControlShowing;
            // 
            // HowMuchQuestion
            // 
            HowMuchQuestion.Location = new Point(352, 16);
            HowMuchQuestion.Name = "HowMuchQuestion";
            HowMuchQuestion.PlaceholderText = "kaç adet soru var?";
            HowMuchQuestion.Size = new Size(100, 23);
            HowMuchQuestion.TabIndex = 8;
            HowMuchQuestion.KeyPress += HowMuchQuestion_KeyPress;
            // 
            // QuestionScoreLoad
            // 
            QuestionScoreLoad.Location = new Point(485, 16);
            QuestionScoreLoad.Name = "QuestionScoreLoad";
            QuestionScoreLoad.Size = new Size(75, 23);
            QuestionScoreLoad.TabIndex = 9;
            QuestionScoreLoad.Text = "Uygula";
            QuestionScoreLoad.UseVisualStyleBackColor = true;
            QuestionScoreLoad.Click += QuestionAcept_Click;
            // 
            // StudentCount
            // 
            StudentCount.AutoSize = true;
            StudentCount.Location = new Point(615, 19);
            StudentCount.Name = "StudentCount";
            StudentCount.Size = new Size(85, 15);
            StudentCount.TabIndex = 10;
            StudentCount.Text = "Öğrenci seçildi";
            // 
            // ScoreReferenceDGW
            // 
            ScoreReferenceDGW.AllowUserToOrderColumns = true;
            ScoreReferenceDGW.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ScoreReferenceDGW.Location = new Point(12, 81);
            ScoreReferenceDGW.Name = "ScoreReferenceDGW";
            ScoreReferenceDGW.Size = new Size(259, 444);
            ScoreReferenceDGW.TabIndex = 11;
            ScoreReferenceDGW.EditingControlShowing += EditingControlShowing;
            // 
            // InfoLabel
            // 
            InfoLabel.AutoSize = true;
            InfoLabel.Location = new Point(12, 62);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new Size(129, 15);
            InfoLabel.TabIndex = 12;
            InfoLabel.Text = "Referans Puan Dağılımı";
            // 
            // GenAnalisys
            // 
            GenAnalisys.Location = new Point(681, 665);
            GenAnalisys.Name = "GenAnalisys";
            GenAnalisys.Size = new Size(111, 23);
            GenAnalisys.TabIndex = 13;
            GenAnalisys.Text = "Oluştur / Kaydet";
            GenAnalisys.UseVisualStyleBackColor = true;
            GenAnalisys.Click += GenAnalisys_Click;
            // 
            // StudentScoreDGWLabel
            // 
            StudentScoreDGWLabel.AutoSize = true;
            StudentScoreDGWLabel.Location = new Point(277, 62);
            StudentScoreDGWLabel.Name = "StudentScoreDGWLabel";
            StudentScoreDGWLabel.Size = new Size(126, 15);
            StudentScoreDGWLabel.TabIndex = 14;
            StudentScoreDGWLabel.Text = "Öğrenci Puan Dağılımı";
            // 
            // ExamSemesterYear
            // 
            ExamSemesterYear.AutoSize = true;
            ExamSemesterYear.Location = new Point(12, 557);
            ExamSemesterYear.Name = "ExamSemesterYear";
            ExamSemesterYear.Size = new Size(77, 15);
            ExamSemesterYear.TabIndex = 15;
            ExamSemesterYear.Text = "Okul Dönemi";
            // 
            // SemesterYearComboBox
            // 
            SemesterYearComboBox.FormattingEnabled = true;
            SemesterYearComboBox.Location = new Point(95, 553);
            SemesterYearComboBox.Name = "SemesterYearComboBox";
            SemesterYearComboBox.Size = new Size(121, 23);
            SemesterYearComboBox.TabIndex = 16;
            // 
            // LabelOfLessonName
            // 
            LabelOfLessonName.AutoSize = true;
            LabelOfLessonName.Location = new Point(12, 612);
            LabelOfLessonName.Name = "LabelOfLessonName";
            LabelOfLessonName.Size = new Size(51, 15);
            LabelOfLessonName.TabIndex = 17;
            LabelOfLessonName.Text = "Ders Adı";
            // 
            // LessonNameTextBox
            // 
            LessonNameTextBox.Location = new Point(69, 609);
            LessonNameTextBox.Name = "LessonNameTextBox";
            LessonNameTextBox.PlaceholderText = "Ders adını girin. Örn ... Dersi";
            LessonNameTextBox.Size = new Size(160, 23);
            LessonNameTextBox.TabIndex = 18;
            // 
            // SemesterSesionLabel
            // 
            SemesterSesionLabel.AutoSize = true;
            SemesterSesionLabel.Location = new Point(245, 557);
            SemesterSesionLabel.Name = "SemesterSesionLabel";
            SemesterSesionLabel.Size = new Size(46, 15);
            SemesterSesionLabel.TabIndex = 19;
            SemesterSesionLabel.Text = "Dönem";
            // 
            // SemesterComboBox
            // 
            SemesterComboBox.FormattingEnabled = true;
            SemesterComboBox.Location = new Point(297, 553);
            SemesterComboBox.Name = "SemesterComboBox";
            SemesterComboBox.Size = new Size(121, 23);
            SemesterComboBox.TabIndex = 20;
            // 
            // SemesterSessionComboBox
            // 
            SemesterSessionComboBox.FormattingEnabled = true;
            SemesterSessionComboBox.Location = new Point(477, 554);
            SemesterSessionComboBox.Name = "SemesterSessionComboBox";
            SemesterSessionComboBox.Size = new Size(121, 23);
            SemesterSessionComboBox.TabIndex = 21;
            // 
            // SemesterSessionLabel
            // 
            SemesterSessionLabel.AutoSize = true;
            SemesterSessionLabel.Location = new Point(436, 557);
            SemesterSessionLabel.Name = "SemesterSessionLabel";
            SemesterSessionLabel.Size = new Size(35, 15);
            SemesterSessionLabel.TabIndex = 22;
            SemesterSessionLabel.Text = "Sınav";
            // 
            // ExamCodeLabel
            // 
            ExamCodeLabel.AutoSize = true;
            ExamCodeLabel.Location = new Point(245, 612);
            ExamCodeLabel.Name = "ExamCodeLabel";
            ExamCodeLabel.Size = new Size(66, 15);
            ExamCodeLabel.TabIndex = 23;
            ExamCodeLabel.Text = "Sınav Kodu";
            // 
            // ExamCodeTextbox
            // 
            ExamCodeTextbox.Enabled = false;
            ExamCodeTextbox.Location = new Point(317, 609);
            ExamCodeTextbox.Name = "ExamCodeTextbox";
            ExamCodeTextbox.Size = new Size(100, 23);
            ExamCodeTextbox.TabIndex = 24;
            ExamCodeTextbox.Text = "000";
            // 
            // FooterTextBox
            // 
            FooterTextBox.Enabled = false;
            FooterTextBox.Location = new Point(513, 609);
            FooterTextBox.Multiline = true;
            FooterTextBox.Name = "FooterTextBox";
            FooterTextBox.Size = new Size(230, 30);
            FooterTextBox.TabIndex = 25;
            FooterTextBox.Text = "Designed and Coded by Furkan Bozkurt (furkanhuman)    furkanhuman.app";
            // 
            // FooterLabel
            // 
            FooterLabel.AutoSize = true;
            FooterLabel.Location = new Point(433, 612);
            FooterLabel.Name = "FooterLabel";
            FooterLabel.Size = new Size(74, 15);
            FooterLabel.TabIndex = 26;
            FooterLabel.Text = "Alt Açıklama";
            // 
            // SchoolTextBox
            // 
            SchoolTextBox.Location = new Point(71, 665);
            SchoolTextBox.Name = "SchoolTextBox";
            SchoolTextBox.Size = new Size(167, 23);
            SchoolTextBox.TabIndex = 27;
            SchoolTextBox.Text = "HACIİLBEY MENSUCAT SANTRAL ORTAOKULU";
            // 
            // SchoolLabel
            // 
            SchoolLabel.AutoSize = true;
            SchoolLabel.Location = new Point(12, 669);
            SchoolLabel.Name = "SchoolLabel";
            SchoolLabel.Size = new Size(53, 15);
            SchoolLabel.TabIndex = 28;
            SchoolLabel.Text = "Okul Adı";
            // 
            // TeacherLabel
            // 
            TeacherLabel.AutoSize = true;
            TeacherLabel.Location = new Point(266, 668);
            TeacherLabel.Name = "TeacherLabel";
            TeacherLabel.Size = new Size(61, 15);
            TeacherLabel.TabIndex = 29;
            TeacherLabel.Text = "Öğretmen";
            // 
            // TeacherTextbox
            // 
            TeacherTextbox.Location = new Point(333, 665);
            TeacherTextbox.Name = "TeacherTextbox";
            TeacherTextbox.Size = new Size(93, 23);
            TeacherTextbox.TabIndex = 30;
            TeacherTextbox.Text = "Emre Dumancı";
            // 
            // PrincipalTextBox
            // 
            PrincipalTextBox.Location = new Point(513, 666);
            PrincipalTextBox.Name = "PrincipalTextBox";
            PrincipalTextBox.Size = new Size(93, 23);
            PrincipalTextBox.TabIndex = 32;
            PrincipalTextBox.Text = "Hüseyin Baran";
            // 
            // PrincipalLabel
            // 
            PrincipalLabel.AutoSize = true;
            PrincipalLabel.Location = new Point(464, 668);
            PrincipalLabel.Name = "PrincipalLabel";
            PrincipalLabel.Size = new Size(43, 15);
            PrincipalLabel.TabIndex = 31;
            PrincipalLabel.Text = "Müdür";
            // 
            // Analysis_Full
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 700);
            Controls.Add(PrincipalTextBox);
            Controls.Add(PrincipalLabel);
            Controls.Add(TeacherTextbox);
            Controls.Add(TeacherLabel);
            Controls.Add(SchoolLabel);
            Controls.Add(SchoolTextBox);
            Controls.Add(FooterLabel);
            Controls.Add(FooterTextBox);
            Controls.Add(ExamCodeTextbox);
            Controls.Add(ExamCodeLabel);
            Controls.Add(SemesterSessionLabel);
            Controls.Add(SemesterSessionComboBox);
            Controls.Add(SemesterComboBox);
            Controls.Add(SemesterSesionLabel);
            Controls.Add(LessonNameTextBox);
            Controls.Add(LabelOfLessonName);
            Controls.Add(SemesterYearComboBox);
            Controls.Add(ExamSemesterYear);
            Controls.Add(StudentScoreDGWLabel);
            Controls.Add(GenAnalisys);
            Controls.Add(InfoLabel);
            Controls.Add(ScoreReferenceDGW);
            Controls.Add(StudentCount);
            Controls.Add(QuestionScoreLoad);
            Controls.Add(HowMuchQuestion);
            Controls.Add(StudentsNotesDataGridView);
            Controls.Add(AltClassLabel);
            Controls.Add(ClassLabel);
            Controls.Add(AltClassComboBox);
            Controls.Add(ClassAgeComboBox);
            Controls.Add(Btn_StudentLoad);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Analysis_Full";
            Text = "Analizör";
            Load += Analysis_Full_Load;
            ((System.ComponentModel.ISupportInitialize)StudentsNotesDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScoreReferenceDGW).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ClassLabel;
        private Label AltClassLabel;
        private Label StudentCount;
        private Label InfoLabel;
        private Label StudentScoreDGWLabel;
        private Label ExamSemesterYear;
        private Label LabelOfLessonName;
        private Label SemesterSesionLabel;
        private Label SemesterSessionLabel;
        private Label ExamCodeLabel;
        private Label FooterLabel;
        private Label SchoolLabel;
        private Label TeacherLabel;
        private Label PrincipalLabel;
        private Button Btn_StudentLoad;
        private Button QuestionScoreLoad;
        private Button GenAnalisys;
        private TextBox HowMuchQuestion;
        private TextBox LessonNameTextBox;
        private TextBox ExamCodeTextbox;
        private TextBox FooterTextBox;
        private TextBox SchoolTextBox;
        private TextBox TeacherTextbox;
        private TextBox PrincipalTextBox;
        private ComboBox ClassAgeComboBox;
        private ComboBox AltClassComboBox;
        private ComboBox SemesterYearComboBox;
        private ComboBox SemesterComboBox;
        private ComboBox SemesterSessionComboBox;
        private DataGridView StudentsNotesDataGridView;
        private DataGridView ScoreReferenceDGW;
    }
}