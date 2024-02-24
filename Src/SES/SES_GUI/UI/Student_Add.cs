using Entity.Entities.Mains;
using System.Text.Json;

namespace SES_GUI;

public partial class Student_Add : Form
{
    public Student_Add()
    {
        InitializeComponent();
    }

    private bool IsMove;
    private string FilePath { get; set; }
    private IList<Student> Students { get; set; }

    private void ClassComboBoxLoad()
    {
        for (int i = 0; i <= 12; i++)
            ClassComboBox.Items.Add(i);
    }

    private void AltClassComboBoxLoad()
    {
        for (char i = 'A'; i <= 'Z'; i++)
            AltClassComboBox.Items.Add(i);
    }

    private void Student_Add_Load(object sender, EventArgs e)
    {
        IsMove = false;
        ClassComboBoxLoad();
        AltClassComboBoxLoad();
        Students = new List<Student>();
        SaveButton.Enabled = true;
        DefaultPathLocationButton.Enabled = false;
    }

    private void LoadButton_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Json Dosyası|*.Json",
            Title = "Öğrenci Dosya Veritabanını Aç...",
            DefaultExt = "json",
            CheckFileExists = false,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };
        if (openFileDialog.ShowDialog() == DialogResult.OK)
            try
            {
                FilePath = openFileDialog.FileName;
                string stdFile = File.ReadAllText(FilePath);
                IList<Student>? students = JsonSerializer.Deserialize<IList<Student>>(stdFile);
                if (stdFile.Length == 0 || stdFile == null || students == null)
                {
                    MessageBox.Show("Dosya Boş");
                    return;
                }
                else
                    Students = students;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Dosya okuma hatası: " + ex.Message);
                return;
            }
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (!IsControlForm())
            return;

        CollectDataFromFormObjectsAndSaveStudent();

        UpdateFile();
    }

    private void ClearForm()
    {
        StudentNameTextBox.Clear();
        StudentSurNameTextBox.Clear();
        StudentSchoolNumberTextBox.Clear();
    }

    private void CollectDataFromFormObjectsAndSaveStudent()
    {
        int lastId = Students.Select(x => x.Id).Last() + 1;

        int classYear = int.TryParse(ClassComboBox.SelectedItem.ToString(), out int result) ? result : -1;
        char classBranch = AltClassComboBox.SelectedItem.ToString()[0];

        string sNum = StudentSchoolNumberTextBox.Text.ToUpperInvariant().ToString();
        string name = StudentNameTextBox.Text.ToUpperInvariant().ToString();
        string surname = StudentSurNameTextBox.Text.ToUpperInvariant().ToString();

        Student student = new() { ClassBranch = classBranch, ClassAge = classYear, Name = name, SurName = surname, SchoolNumber = sNum, Id = lastId };

        Students.Add(student);

        ClearForm();
    }

    private void UpdateFile()
    {
        var _ = Students.Distinct().ToList();
        string updatedData = JsonSerializer.Serialize(_);

        File.WriteAllText(FilePath, updatedData);
    }

    private bool IsControlForm()
    {
        if (ClassComboBox.SelectedItem == null)
        {
            MessageBox.Show("Öğrenci sınıfını seçiniz");
            return false;
        }

        if (AltClassComboBox.SelectedItem == null)
        {
            MessageBox.Show("Öğrenci şubesini seçiniz");
            return false;
        }

        if (string.IsNullOrWhiteSpace(StudentNameTextBox.Text))
        {
            MessageBox.Show("Öğrenci adı boş olamaz");
            return false;
        }

        if (string.IsNullOrWhiteSpace(StudentSurNameTextBox.Text))
        {
            MessageBox.Show("Öğrenci soyadı boş olamaz");
            return false;
        }


        if (string.IsNullOrWhiteSpace(StudentSchoolNumberTextBox.Text))
        {
            MessageBox.Show("Öğrenci numarası boş olamaz");
            return false;
        }

        return true;
    }

    private void DefaultPathLocationButton_Click(object sender, EventArgs e)
    {

        FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SES_S");
        string pathF = Path.Combine(FilePath, "S_List.json");


        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);
        if (!File.Exists(pathF))
        {
            string updatedData = JsonSerializer.Serialize(Students);
            File.WriteAllText(pathF, updatedData);
        }

        else
        {
            string exAData = File.ReadAllText(pathF);

            IList<Student>? exStudents = JsonSerializer.Deserialize<IList<Student>>(exAData);
            if (exStudents != null)
                foreach (Student student in exStudents)
                    Students.Add(student);

            List<Student> newStudents = Students.Distinct().ToList();

            string newS = JsonSerializer.Serialize(newStudents);
            File.WriteAllText(pathF, newS);
        }


        if (!IsMove && Students != null)
        {
            LoadButton.Enabled = false;
            IsMove = true;
        }
        else
        {
            FilePath = Path.Combine(FilePath, pathF);
            UpdateFile();
            DefaultPathLocationButton.Enabled = false;
        }

    }
}
