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
    public partial class Form3 : Form
    {
        string connectionString = "Data Source=oda_database.db;Version=3;";

        public Form3()
        {
            InitializeComponent();
            this.FormClosing += Form3_FormClosing;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            OdaDurumlariniYukle();
            ButtonOlaylariniBagla();
        }

        private void ButtonOlaylariniBagla()
        {
            for (int i = 1; i <= 8; i++)
            {
                Control[] bulunanlar = this.Controls.Find("button" + i, true);
                if (bulunanlar.Length > 0 && bulunanlar[0] is Button btn)
                {
                    btn.Click += (s, ev) =>
                    {
                        string[] satirlar = btn.Text.Split('\n');
                        label1.Text = satirlar[0].ToLower();
                    };
                }
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        public void OdaDurumlariniYukle()
        {
            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
                {
                    baglanti.Open();
                    string sql = "SELECT OdaNo, Durum FROM Odalar LIMIT 8";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        using (SQLiteDataReader oku = komut.ExecuteReader())
                        {
                            int butonIndeks = 1;
                            while (oku.Read())
                            {
                                string odaNo = oku["OdaNo"].ToString();
                                string durum = oku["Durum"].ToString();
                                Control[] bulunanlar = this.Controls.Find("button" + butonIndeks, true);

                                if (bulunanlar.Length > 0 && bulunanlar[0] is Button btn)
                                {
                                    btn.Text = odaNo.ToUpper() + "\n" + durum.ToUpper();                                  
                                    if (durum.Trim().ToLower() == "dolu")
                                        btn.BackColor = Color.Red;
                                    else
                                        btn.BackColor = Color.Green;

                                    btn.Enabled = true;
                                }
                                butonIndeks++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (label1.Text == "@OdaNo" || string.IsNullOrEmpty(label1.Text))
            {
                MessageBox.Show("Lütfen bir oda seçin!");
                return;
            }

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.Text.ToLower().Contains(label1.Text.ToLower()))
                {
                    if (btn.BackColor == Color.Red)
                    {
                        MessageBox.Show("Bu oda zaten dolu! Başka bir oda seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            Form5 f5 = new Form5();
            f5.SecilenOda = label1.Text;
            f5.AnaForm = this;
            f5.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (label1.Text == "@OdaNo" || string.IsNullOrEmpty(label1.Text))
            {
                MessageBox.Show("Lütfen bir oda seçin!");
                return;
            }

            bool odaDoluMu = false;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.Text.ToLower().Contains(label1.Text.ToLower()))
                {
                    if (btn.BackColor == Color.Red) odaDoluMu = true;
                    break;
                }
            }

            if (!odaDoluMu)
            {
                MessageBox.Show("Bu oda zaten boş!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult onay = MessageBox.Show(label1.Text.ToUpper() + " nolu odayı boşaltmak istiyor musunuz?", "Onay", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
                    {
                        baglanti.Open();
                        string sql = "UPDATE Odalar SET Durum = 'boş' WHERE OdaNo = @oda";
                        using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                        {
                            komut.Parameters.AddWithValue("@oda", label1.Text);
                            komut.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Oda boşaltıldı.");
                    OdaDurumlariniYukle();
                    label1.Text = "@OdaNo";
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            }
        }

        private void label1_Click(object sender, EventArgs e) { label1.Text = "@OdaNo"; }
        private void button1_Click(object sender, EventArgs e) { }
    }
}