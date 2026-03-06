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
    public partial class Form2 : Form
    {
        string connectionString = "Data Source=oda_database.db;Version=3;";

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            VeritabaniniOlusturVeDoldur();
        }

        private void VeritabaniniOlusturVeDoldur()
        {
            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
                {
                    baglanti.Open();

                    string tabloSorgusu = "CREATE TABLE IF NOT EXISTS Odalar (OdaNo TEXT, Durum TEXT)";
                    using (SQLiteCommand komut = new SQLiteCommand(tabloSorgusu, baglanti))
                    {
                        komut.ExecuteNonQuery();
                    }

                    string kontrolSorgusu = "SELECT COUNT(*) FROM Odalar";
                    using (SQLiteCommand kontrolKomut = new SQLiteCommand(kontrolSorgusu, baglanti))
                    {
                        long kayitSayisi = (long)kontrolKomut.ExecuteScalar();

                        if (kayitSayisi == 0)
                        {
                            string veriEklemeSorgusu = @"
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda101', 'dolu');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda102', 'boş');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda103', 'boş');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda104', 'boş');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda105', 'boş');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda106', 'boş');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda107', 'boş');
                                INSERT INTO Odalar (OdaNo, Durum) VALUES ('oda108', 'boş');";

                            using (SQLiteCommand ekleKomut = new SQLiteCommand(veriEklemeSorgusu, baglanti))
                            {
                                ekleKomut.ExecuteNonQuery();
                            }
                        }
                    }
                    baglanti.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.Show();
        }
    }
}