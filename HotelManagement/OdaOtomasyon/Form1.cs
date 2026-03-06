using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace OdaOtomasyon
{
    public partial class Form1 : Form
    {
        string connectionString = "Data Source=oda_database.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            VeritabaniniHazirla();
            textBox2.PasswordChar = '*';
        }

        private void VeritabaniniHazirla()
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
            {
                baglanti.Open();
                string sql = "CREATE TABLE IF NOT EXISTS Kullanicilar (KullaniciAdi TEXT, Sifre TEXT)";
                using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                {
                    komut.ExecuteNonQuery();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz!");
                return;
            }

            using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
            {
                baglanti.Open();
                string sql = "INSERT INTO Kullanicilar (KullaniciAdi, Sifre) VALUES (@id, @sifre)";
                using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                {
                    komut.Parameters.AddWithValue("@id", textBox1.Text);
                    komut.Parameters.AddWithValue("@sifre", textBox2.Text);
                    komut.ExecuteNonQuery();
                }
                MessageBox.Show("Kayıt başarıyla oluşturuldu!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
            {
                baglanti.Open();
                string sql = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @id AND Sifre = @sifre";
                using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                {
                    komut.Parameters.AddWithValue("@id", textBox1.Text);
                    komut.Parameters.AddWithValue("@sifre", textBox2.Text);

                    long sonuc = (long)komut.ExecuteScalar();

                    if (sonuc > 0)
                    {
                        Form2 f2 = new Form2();
                        f2.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Hatalı kullanıcı adı veya şifre!");
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}