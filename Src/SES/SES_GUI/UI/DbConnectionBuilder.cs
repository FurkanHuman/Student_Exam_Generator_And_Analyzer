using Npgsql;

namespace SES_GUI.UI;

public partial class DbConnectionBuilder : Form
{
    public DbConnectionBuilder(SES_Main sES_Main)
    {
        InitializeComponent();
    }

    private void DbConnectionBuilder_Load(object sender, EventArgs e)
    {

    }

    private void Connect_Click(object sender, EventArgs e)
    {
        if (AreAllTextBoxesEmpty(this))
        {
            MessageBox.Show("Lütfen tüm kutucukları kontrol edin!!!");
            return;
        }

        if (PasswordTextBox.Text != PasswordRepeatTextBox.Text)
            return;

        string connetionString = $"Host={ServerTextBox.Text};Port={PortNumericUpDown.Value};Username={UsernameTextBox.Text};Password={PasswordRepeatTextBox.Text};Database={DatabaseTextbox.Text}";

        using NpgsqlConnection connection = new(connetionString);
        connection.Open();

        if (connection.State == System.Data.ConnectionState.Open)
        {
            File.WriteAllTextAsync(Paths.GetConfFile(), connetionString);
            connection.Close();
            Close();
        }

        MessageBox.Show("Hata Veritabanına bağlantı başarısız");
        return;
    }

    private bool AreAllTextBoxesEmpty(Control container)
    {
        // Container içindeki tüm TextBox kontrolerini kontrol et
        foreach (Control control in container.Controls)
        {
            if (control is TextBox textBox && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                return false; // En az bir TextBox doluysa false döndür
            }

            // Eğer kontrol bir konteyner ise, içindeki TextBox'ları kontrol et
            if (control.HasChildren)
            {
                if (!AreAllTextBoxesEmpty(control))
                {
                    return false; // En az bir TextBox doluysa false döndür
                }
            }
        }

        return true; // Tüm TextBox'lar boşsa true döndür
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(PasswordRepeatTextBox.Text) | string.IsNullOrEmpty(PasswordTextBox.Text))
        {
            PasswordTextBox.BackColor = Color.Red;
            PasswordRepeatTextBox.BackColor = Color.Red;
        }
        else
        {
            PasswordTextBox.BackColor = Color.White;
            PasswordRepeatTextBox.BackColor = Color.White;
        }

        if (PasswordTextBox.Text == PasswordRepeatTextBox.Text)
            PasswordRepeatTextBox.BackColor = Color.White;


        else
            PasswordRepeatTextBox.BackColor = Color.Red;

    }

    private void DbConnectionBuilder_FormClosing(object sender, FormClosingEventArgs e)
    {
        Application.Exit();
    }
}
