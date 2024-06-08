using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Application.Repositories;
using Entity.Entities.Mains;

namespace SES_GUI.UI;

public partial class SchoolAdd : Form
{
    private readonly ISchoolRepository _SchoolRepository;
    private bool IsUpdate = false;
    private School? _school;

    public SchoolAdd(ISchoolRepository schoolRepository)
    {

        InitializeComponent();
        _SchoolRepository = schoolRepository;
    }

    private void SchoolAdd_Load(object sender, EventArgs e)
    {
        _school = _SchoolRepository.Get(s=>s.Id==1);

        switch (_school!=null)
        {
            case true:
                IsUpdate = true;
                Name = "Okul Güncelle";
                SchoolNameTextBox.Text = _school.Name;
                break;            
            default:
                MessageBox.Show("Lütfen Dikkatli Olun...");
                break;
        }  
    }

    private void Save_Click(object sender, EventArgs e)
    {
        switch (IsUpdate)
        {
            case true:
                _SchoolRepository.Update(new() { Name = SchoolNameTextBox.Text.ToString(), Id = 1 });
                MessageBox.Show("Okul adı güncellendi");
                break;
            default:
                _SchoolRepository.Add(new() { Name = SchoolNameTextBox.Text.ToString() });
                MessageBox.Show("Okul Eklendi");
                break;
        }
        System.Windows.Forms.Application.Exit();
    }
}
