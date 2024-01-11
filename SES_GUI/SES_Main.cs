using App.PdfPageProduct.AnalysisPageFeature;
using Entity.Entities.Mains;
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
            IAnalsysPage analsysPage = _serviceProvider.GetRequiredService<IAnalsysPage>();
            Analysis_Full f = new(analsysPage);
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
            Student_Add student_Add = new();
            student_Add.Show();
        }
    }
}
