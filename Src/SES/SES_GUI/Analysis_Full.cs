using App.PdfPageProduct.AnalysisPageFeature;
using Entity.Entities.Mains;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Data;
using System.Text.Json;

namespace SES_GUI;

public partial class Analysis_Full : Form
{
    private readonly IAnalsysPage _analsysPage;

    public IList<Student> Students { get; set; }
    public IList<Student> SelectedStudents { get; set; }
    public IList<StudentQuizAnswer> StudentQuizAnswers { get; set; }
    private int[] RefScores { get; set; }
    public string[] RefBenefitNumbers { get; set; }
    public string[] RefBenefitComments { get; private set; }
    public bool IsOneShootRefTable { get; set; }

    private readonly string SnapshotFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SES_Snapshots");

    public Analysis_Full([FromKeyedServices(1)] IAnalsysPage analsysPage)
    {
        InitializeComponent();
        _analsysPage = analsysPage;
    }
    // Todo : referans tablosu korunup diğer sınıfa geçilebilecek 
    // TODO : pasta grafiğini düzelt +
    // kopyala yapıştır deteğini gelişştir.
    // Todo : exel like yapı
    // todo : çık tıkla otamatik ref puanını getir.
    // özel durumlar için otomatik doldurma
    // g için otomatik doldurma
    // todo : ayrı referans kayıdı
    // double sayı  irişine izin ver. g ler sınıf başarı tablosunda yer almayacak
    // dosya kayıt ve geri çağırma 
    // ünvanlar olacak adların altında olacak.
    // top info ya dikkat
    // sınııf listesini çekme. 
    // todo : öğrenci numarasına göre sırala +
    private void Analysis_Full_Load(object sender, EventArgs e)
    {
        LoadSemesterYears();
        LoadSemester();
        LoadSemesterSession();
        StudentQuizAnswers = new List<StudentQuizAnswer>();
        IsOneShootRefTable = false;

        if (!Directory.Exists(SnapshotFolderPath))
        {
            try
            {
                Directory.CreateDirectory(SnapshotFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Klasör oluşturma hatası: " + ex.Message);
            }
        }
    }

    private void LoadSemesterSession()
    {
        List<string> SemestersSession = [];
        for (int i = 0; i < 5; i++)
        {
            SemestersSession.Add($"{i + 1}. Sınav");
        }
        SemesterSessionComboBox.DataSource = SemestersSession;
    }

    private void LoadSemester()
    {
        List<string> Semesters = [];
        for (int i = 0; i < 2; i++)
        {
            Semesters.Add($"{i + 1}. Dönem");
        }
        SemesterComboBox.DataSource = Semesters;
    }

    private void LoadSemesterYears()
    {
        List<string> semesterYears = [];
        for (int i = 2000; i < DateTime.UtcNow.Year + 1; i++)
        {
            semesterYears.Add($"{i} - {i + 1}");
        }
        SemesterYearComboBox.DataSource = semesterYears;
    }

    private void Btn_StudentLoad_Click(object sender, EventArgs e)
    {
        OpenFileDialog studentFile = new()
        {
            Filter = "Json dosyası (*.json)|*.json",
            FilterIndex = 0
        };
        if (studentFile.ShowDialog() == DialogResult.OK)
            try
            {
                string filePath = studentFile.FileName;

                string stdFile = File.ReadAllText(filePath);
                IList<Student>? loadedStudents = JsonSerializer.Deserialize<IList<Student>>(stdFile);
                if (loadedStudents == null || loadedStudents.Count == 0)
                {
                    MessageBox.Show("Dosya Boş");
                    return;
                }
                Students = loadedStudents;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Dosya okuma hatası: " + ex.Message);
            }

        else
            return;

        ClassAgeComboBox.DataSource = Students.Select(_ => _.ClassAge).Distinct().ToList();
        AltClassComboBox.DataSource = Students.Select(_ => _.ClassBranch).Distinct().ToList();

    }

    private void RefScoreDGWLoad(int count)
    {
        ScoreReferenceDGW.Columns.Add("#ri", "#");
        ScoreReferenceDGW.Columns.Add("#rn", "Referans Puan");
        ScoreReferenceDGW.Columns.Add("RefName", "Kazanım Numarası");
        ScoreReferenceDGW.Columns.Add("RefComment", "Kazanım Açıklaması");

        ScoreReferenceDGW.Columns["#ri"].ValueType = typeof(int);
        ScoreReferenceDGW.Columns["#rn"].ValueType = typeof(int);
        ScoreReferenceDGW.Columns["RefName"].ValueType = typeof(string);
        ScoreReferenceDGW.Columns["RefComment"].ValueType = typeof(string);


        for (int i = 0; i < count; i++)
        {
            DataGridViewRow row = new();
            row.CreateCells(ScoreReferenceDGW);
            ScoreReferenceDGW.Rows.Add(row);
            ScoreReferenceDGW.Rows[i].Cells["#ri"].Value = i + 1;
        }
        ScoreReferenceDGW.AutoResizeColumns();
    }

    private void StudentScoreDGWLoad(int count)
    {
        StudentsNotesDataGridView.Columns.Add("#s", "#Durum");
        StudentsNotesDataGridView.Columns.Add("no", "#No");
        StudentsNotesDataGridView.Columns.Add("name", "Ad");
        StudentsNotesDataGridView.Columns.Add("surname", "Soyad");

        for (int i = 0; i < count; i++)
        {
            StudentsNotesDataGridView.Columns.Add($"#n{i}", $"#{i + 1}");
            StudentsNotesDataGridView.Columns[$"#n{i}"].ValueType = typeof(string);
        }
        foreach (Student student in SelectedStudents)
        {
            DataGridViewRow row = new();

            row.CreateCells(StudentsNotesDataGridView, "", int.Parse(student.SchoolNumber), student.Name, student.SurName);
            StudentsNotesDataGridView.Rows.Add(row);
        }

        StudentsNotesDataGridView.Columns["#s"].ValueType = typeof(string);
        StudentsNotesDataGridView.Columns.Add("sumary", "Toplam");
        StudentsNotesDataGridView.AutoResizeColumns();

    }

    private void SelectStudentForComboBox()
    {

        int sClass = (int)ClassAgeComboBox.SelectedItem;
        char sAClass = (char)AltClassComboBox.SelectedItem;

        SelectedStudents = Students.Where(_ => _.ClassAge == sClass & _.ClassBranch == sAClass).ToList();
        StudentCount.Text = $"{SelectedStudents.Count} {StudentCount.Text}";
    }

    private void MakeReadOnlyDGWs()
    {
        List<string> readOnlyColumns = ["no", "name", "surname"];

        foreach (DataGridViewColumn column in StudentsNotesDataGridView.Columns)
        {
            if (readOnlyColumns.Contains(column.Name))
                column.ReadOnly = true;
        }
        ScoreReferenceDGW.Columns["#ri"].ReadOnly = true;

    }

    private void QuestionAcept_Click(object sender, EventArgs e)
    {
        ScoreReferenceDGW.Rows.Clear();
        StudentsNotesDataGridView.Rows.Clear();
        int count = int.Parse(HowMuchQuestion.Text ?? "1");

        SelectStudentForComboBox();
        RefScoreDGWLoad(count);
        StudentScoreDGWLoad(count);
        MakeReadOnlyDGWs();
        QuestionScoreLoad.Enabled = true;
        IsOneShootRefTable = true;
        ScoreReferenceDGW.AutoResizeColumns();
        AutomaticSavingTenM.Start();
    }

    private void SnapshotSaveButton_Click(object sender, EventArgs e)
    {
        SaveDataGridViews(Path.Combine(SnapshotFolderPath, $"SES_DGW_SS_{DateTime.Now}.DGWSS"));
    }


    private void LoadSnapshotFromFileButton_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Anlık Kayıt Dosyası|*.DGWSS",
            Title = "Anlık Dosya Seç",
            DefaultExt = "DGWSS",
            InitialDirectory = SnapshotFolderPath,
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            LoadDataGridViews(openFileDialog.FileName);
        }
    }


    private async void GenAnalisys_Click(object sender, EventArgs e)
    {
        CollectDataFromDGWs();
        DialogResult msgBox = MessageBox.Show("Şimdi kaydeceğin yeri seçeceksin!", "Dikkat Et!!!", MessageBoxButtons.OKCancel);

        if (msgBox != DialogResult.OK)
            return;
        (string name, string surname) teacherNames = NameSplit(TeacherTextbox);
        (string name, string surname) principleNames = NameSplit(PrincipalTextBox);

        AnalysisHeader analysisHeader = new()
        {
            ClassAge = (int)ClassAgeComboBox.SelectedItem,
            AltClass = (char)AltClassComboBox.SelectedItem,
            ExamSemesterYear = (string)SemesterYearComboBox.SelectedItem,
            LessonName = LessonNameTextBox.Text.ToUpper(),
            Semester = (string)SemesterComboBox.SelectedItem,
            SemesterSession = (string)SemesterComboBox.SelectedItem,
            ExamCode = ExamCodeTextbox.Text,
            FooterNote = FooterTextBox.Text,
            School = new School() { Name = SchoolTextBox.Text, Id = -1 },
            SchoolId = -1,
            StudentQuizAnswers = StudentQuizAnswers,
            RefScorePerQuestions = RefScores,
            SqaId = -1,
            Teacher = new() { Id = -1, Name = teacherNames.name, SurName = teacherNames.surname },
            Principal = new() { Id = -1, Name = principleNames.name, SurName = principleNames.surname },
            PrincipalId = -1,
            TeacherId = -1,
            Id = -1
        };
        IDocument analysispage = _analsysPage.PageGenerate(analysisHeader: analysisHeader);
        analysispage.ShowInPreviewerAsync();
        SaveFile(analysisHeader, analysispage);
        // todo : sıfırlanacaklar ve üste binmeleri engelle

        StudentQuizAnswers = new List<StudentQuizAnswer>();
        SelectedStudents = new List<Student>();
    }

    private void AutomaticSavingTenM_Tick(object sender, EventArgs e)
    {
        SaveDataGridViews(Path.Combine(SnapshotFolderPath, $"SES_DGW_SS_{DateTime.Now}.DGWSS"));
    }

    private static (string name, string surname) NameSplit(TextBox textBox)
    {
        string[] names = textBox.Text.Split(" ");

        return (string.Join(" ", names.Take(names.Length - 1)), names.Last());
    }

    private static void SaveFile(AnalysisHeader analysisHeader, IDocument analysispage)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "PDF Dosyası|*.pdf",
            Title = "Analiz dosyasını kaydet",
            DefaultExt = "pdf",
            FileName = $"{analysisHeader.ExamSemesterYear} {analysisHeader.Semester} {analysisHeader.SemesterSession} {analysisHeader.LessonName} Analizi",
            CheckFileExists = false,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        byte[] pdfFile = analysispage.GeneratePdf();

        saveFileDialog.ShowDialog();

        Stream stream = saveFileDialog.OpenFile();
        stream.Write(pdfFile);
        stream.Close();
    }


    private void CollectDataFromDGWs()
    {
        ScoreReferenceDGWLoadData();

        int count = ScoreReferenceDGW.RowCount;

        for (int i = 0; i < StudentsNotesDataGridView.RowCount; i++)
        {
            StudentQuizAnswer studentQuizAnswer = new();
            List<QuestionScore> questionScores = [];

            studentQuizAnswer.ExamId = -1;

            object getSNumber = StudentsNotesDataGridView.Rows[i].Cells["no"].Value;
            studentQuizAnswer.Student = SelectedStudents.First(s => s.SchoolNumber == getSNumber.ToString());

            for (int j = 0; j < count; j++)
            {
                List<char> getSSC = new();
                object getScore = StudentsNotesDataGridView.Rows[i].Cells[$"#n{j}"].Value;
                string? getSS = StudentsNotesDataGridView.Rows[i].Cells["#s"].Value.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(getScore.ToString()))
                {
                    if (int.TryParse(getScore.ToString(), out int cellScore))
                        getScore = cellScore;
                    else
                        StudentsNotesDataGridView.Rows[i].Cells[$"#n{j}"].Value = 0;
                    getScore = 0;
                }

                if (!string.IsNullOrWhiteSpace(getSS))
                    getSSC.AddRange(getSS);

                studentQuizAnswer.SpecialStatuses = getSSC.ToArray();
                studentQuizAnswer.TotalScore = (int)StudentsNotesDataGridView.Rows[i].Cells["sumary"].Value;

                questionScores.Add(new QuestionScore { Id = i, Score = int.Parse(getScore.ToString()), MaxScore = RefScores[j] });
            }

            studentQuizAnswer.ScorePerQuestions = [.. questionScores];
            StudentQuizAnswers.Add(studentQuizAnswer);
        }
    }

    private void ScoreReferenceDGWLoadData()
    {

        List<int> rSList = [];
        List<string> refBenefits = [];
        List<string> refBenefitsComments = [];

        for (int i = 0; i < ScoreReferenceDGW.RowCount; i++)
        {
            string? refBnenefit = ScoreReferenceDGW.Rows[i].Cells["RefName"].Value?.ToString() ?? "Bilinmiyor";
            string? refBnenefitComment = ScoreReferenceDGW.Rows[i].Cells["RefComment"].Value?.ToString() ?? "Bilinmiyor";
            int refScore = int.Parse(ScoreReferenceDGW.Rows[i].Cells["#rn"].Value.ToString() ?? "0");

            rSList.Add(refScore);
            refBenefits.Add(refBnenefit);
            refBenefitsComments.Add(refBnenefitComment);
        }

        RefScores = [.. rSList];
        RefBenefitNumbers = [.. refBenefits];
        RefBenefitComments = [.. refBenefitsComments];
    }

    private void HowMuchQuestion_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
        {
            e.Handled = true;
        }
    }

    private void StudentsNotesDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        int count = int.Parse(HowMuchQuestion.Text);
        List<int> preSums = [];
        DataGridViewRow studentsNotesDGVRow = StudentsNotesDataGridView.Rows[e.RowIndex];

        //int scoreReferenceDGWIndex = ScoreReferenceDGW.Columns["#rn"].Index;
        //int benefitsNoDGWIndex = ScoreReferenceDGW.Columns["RefName"].Index;


        for (int i = 0; i < count; i++)
        {
            bool scn = int.TryParse(studentsNotesDGVRow.Cells[$"#n{i}"].Value?.ToString() ?? "0", out int cellNote);
            bool rscn = int.TryParse(ScoreReferenceDGW.Rows[i].Cells["#rn"].Value?.ToString() ?? "0", out int refCellNote);

            if (scn == false || rscn == false)
            {
                MessageBox.Show("Lütfen Sayı Girin!!!", "Dikkat!!!");
                studentsNotesDGVRow.Cells[$"#n{i}"].Value = 0;
                ScoreReferenceDGW.Rows[i].Cells["#rn"].Value = 0;
                return;
            }

            if (cellNote > refCellNote)
            {
                MessageBox.Show("Girdiğiniz değer referans değerden büyük olamaz!!!", "Dikkat!!!");
                studentsNotesDGVRow.Cells[$"#n{i}"].Value = refCellNote;
                return;
            }

            preSums.Add(cellNote);
        }

        int total = preSums.Sum(x => x);
        studentsNotesDGVRow.Cells["sumary"].Value = total;
        ScoreReferenceDGW.AllowUserToAddRows = false;
        StudentsNotesDataGridView.AllowUserToAddRows = false;

    }

    private void EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        //if (e.Control is TextBox textBox)
        //{
        //    textBox.KeyPress -= TextBox_KeyPress;
        //    textBox.KeyPress += TextBox_KeyPress;
        //}
    }




    //private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    //{
    //    char[] allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,.()/ ".ToCharArray();
    //    char[] allowedCharsOfStatus = "gkrm,".ToCharArray();


    //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
    //        e.Handled = true;

    //    //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !allowedChars.Contains(e.KeyChar) && !allowedCharsOfStatus.Contains(e.KeyChar) && (ScoreReferenceDGW.Columns["#rn"].Index == ScoreReferenceDGW.CurrentCell.ColumnIndex))
    //    //    e.Handled = false;
    //}


    private void SaveDataGridViews(string fileName)
    {

        //using StreamWriter sw = new(fileName);
        //// İlk DataGridView verisini dosyaya kaydet
        //SaveDataGridView(ScoreReferenceDGW, sw);

        //// İkinci DataGridView verisini dosyaya kaydet
        //SaveDataGridView(StudentsNotesDataGridView, sw);
    }

    private static void SaveDataGridView(DataGridView dataGridView, StreamWriter sw)
    {
        if (IsDataGridViewEmpty(dataGridView))
            return;

        sw.WriteLine($"[{dataGridView.Name}]");

        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (!row.IsNewRow)
            {
                string rowValues = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value));
                sw.WriteLine(rowValues);
            }
        }

        sw.WriteLine(); // Bölümleri birbirinden ayırmak için bir satır boşluk bırak
    }


    private void LoadDataGridViews(string fileName)
    {
        using StreamReader sr = new(fileName);
        LoadDataGridView(ScoreReferenceDGW, sr);
        LoadDataGridView(StudentsNotesDataGridView, sr);
    }

    private static void LoadDataGridView(DataGridView dataGridView, StreamReader sr)
    {
        dataGridView.Rows.Clear();

        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();

            if (line == $"[{dataGridView.Name}]")
            {
                // İlgili bölüme ulaşıldı, verileri yükle
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        // Bölüm bitti
                        break;

                    string[] rowValues = line.Split(',');
                    dataGridView.Rows.Add(rowValues);
                }
            }
        }
    }

    private static bool IsDataGridViewEmpty(DataGridView dataGridView)
    {
        return (dataGridView == null || dataGridView.Rows.Count == 0);
    }

}