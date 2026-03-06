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
using System.Windows.Forms.DataVisualization.Charting;

namespace OdaOtomasyon
{
    public partial class Form4 : Form
    {
        string connectionString = "Data Source=oda_database.db;Version=3;";

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            GrafikGuncelle();
        }

        private void GrafikGuncelle()
        {
            int dolu = 0;
            int bos = 0;

            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(connectionString))
                {
                    baglanti.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Odalar WHERE Durum = 'dolu'", baglanti))
                        dolu = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Odalar WHERE Durum = 'boş'", baglanti))
                        bos = Convert.ToInt32(cmd.ExecuteScalar());
                }

                chart1.Series.Clear();
          

                chart1.Titles.Add("Otel Doluluk Analizi");

                Series seri = new Series("Durumlar");
                seri.ChartType = SeriesChartType.Pie;
       

                seri.Points.AddXY("Dolu (" + dolu + ")", dolu);
                seri.Points.AddXY("Boş (" + bos + ")", bos);

                seri.Points[0].Color = Color.Firebrick;
                seri.Points[1].Color = Color.ForestGreen;

                chart1.Series.Add(seri);
                chart1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grafik çizilirken bir hata oluştu: " + ex.Message);
            }
        }

        private void chart1_Click(object sender, EventArgs e) { }
    }
}