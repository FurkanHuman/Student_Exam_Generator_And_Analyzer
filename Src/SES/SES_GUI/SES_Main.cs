using App.PdfPageProduct.AnalysisPageFeature;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace SES_GUI
{
    public partial class SES_Main : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public SES_Main(IServiceProvider serviceProvider)
        {

            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private void Analsys_Click(object sender, EventArgs e)
        {
            Analysis_Full f = _serviceProvider.GetRequiredService<Analysis_Full>();
            f.Show();
        }

        private void SES_Main_Load(object sender, EventArgs e)
        {
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;
            QuestPDF.Settings.License = LicenseType.Community;
            MessageBox.Show("Programda Lütfen dikkatli olun. pre alpha sürümüdür.", "DÝKKAT", MessageBoxButtons.OK);
        }

        private void StudentAddButton_Click(object sender, EventArgs e)
        {
            Student_Add student_Add = _serviceProvider.GetRequiredService<Student_Add>();
            student_Add.Show();
        }
    }
}
