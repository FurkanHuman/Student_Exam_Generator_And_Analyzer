using Entity.Entities.Mains;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SES_GUI
{
    public partial class Student_Add : Form
    {
        public Student_Add()
        {
            InitializeComponent();
        }

        private string FilePath { get; set; }
        private IList<Student> Students { get; set; }

        private void Student_Add_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Şu An Çalışmıyor");
            Close();
            Students = new List<Student>();
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

                FilePath = openFileDialog.FileName;
        }
    }
}
