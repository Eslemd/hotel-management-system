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
using System.IO;

namespace OdaOtomasyon
{
    public partial class Form5 : Form
    {
        string connectionString = "Data Source=oda_database.db;Version=3;";
        public string SecilenOda { get; set; }
        public Form3 AnaForm { get; set; }

        public Form5()
        {
            InitializeComponent();
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            label1.Text = SecilenOda;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { "Kart", "Nakit", "Kupon" });
            comboBox1.SelectedIndex = 0;

            MusteriTablosunuHazirla();
            FiyatHesapla();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FiyatHesapla();
        }

        private void FiyatHesapla()
        {
            int gece = (int)numericUpDown1.Value;
            int toplamFiyat = gece * 200;
            label8.Text = toplamFiyat.ToString() + " TL";
        }

        private void MusteriTablosunuHazirla()
        {
            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
                {
                    baglanti.Open();
                    string sql = @"CREATE TABLE IF NOT EXISTS Musteriler (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Ad TEXT, Soyad TEXT, Telefon TEXT, 
                                    TC TEXT, GeceSayisi INTEGER, 
                                    OdemeYontemi TEXT, OdaNo TEXT, Fiyat INTEGER)";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablo hatası: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Lütfen Ad ve T.C. alanlarını doldurunuz!");
                return;
            }

            try
            {
                int hesaplananFiyat = (int)numericUpDown1.Value * 200;

                using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
                {
                    baglanti.Open();

                    string sqlMusteri = "INSERT INTO Musteriler (Ad, Soyad, Telefon, TC, GeceSayisi, OdemeYontemi, OdaNo, Fiyat) " +
                                       "VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";

                    using (SQLiteCommand komut = new SQLiteCommand(sqlMusteri, baglanti))
                    {
                        komut.Parameters.AddWithValue("@p1", textBox1.Text);
                        komut.Parameters.AddWithValue("@p2", textBox2.Text);
                        komut.Parameters.AddWithValue("@p3", maskedTextBox1.Text);
                        komut.Parameters.AddWithValue("@p4", textBox3.Text);
                        komut.Parameters.AddWithValue("@p5", (int)numericUpDown1.Value);
                        komut.Parameters.AddWithValue("@p6", comboBox1.Text);
                        komut.Parameters.AddWithValue("@p7", label1.Text);
                        komut.Parameters.AddWithValue("@p8", hesaplananFiyat);
                        komut.ExecuteNonQuery();
                    }

                    string sqlOda = "UPDATE Odalar SET Durum = 'dolu' WHERE OdaNo = @oda";
                    using (SQLiteCommand komutOda = new SQLiteCommand(sqlOda, baglanti))
                    {
                        komutOda.Parameters.AddWithValue("@oda", label1.Text);
                        komutOda.ExecuteNonQuery();
                    }
                }

                string logMesaji = $"{DateTime.Now}: {label1.Text} nolu oda, {textBox1.Text} adına {label8.Text} rezerve edildi.\n";
                File.AppendAllText("musteri.txt", logMesaji);

                MessageBox.Show("İşlem Başarılı!");

                if (AnaForm != null)
                {
                    AnaForm.OdaDurumlariniYukle();
                    AnaForm.Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AnaForm != null) AnaForm.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e) { }
    }
}