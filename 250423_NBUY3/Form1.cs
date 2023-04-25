using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _250423_NBUY3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Server=.;Database=Northwind;User=sa;Pwd=123");
        private void Form1_Load(object sender, EventArgs e)
        {
            UrunListele();
            label2.Visible = true;
            label3.Visible = true;
            label1.Visible = true;
            txtUrunAdi.Visible = true;
            nudFiyat.Visible = true;
            nudStok.Visible = true;
            btnEkle.Visible = true;
            btnGuncelle.Visible = true;
        }
        private void UrunListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select * from Urunler", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgvListe.DataSource = dt;
            dgvListe.Columns["UrunId"].Visible = false;
            dgvListe.Columns["KategoriID"].Visible = false;
            dgvListe.Columns["TedarikciID"].Visible = false;
        }
        private void urunlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UrunListele();
            label2.Visible = true;
            label3.Visible = true;
            label1.Visible = true;
            txtUrunAdi.Visible = true;
            nudFiyat.Visible = true;
            nudStok.Visible = true;
            txtKategoriAdi.Visible = false;
            txtTanimi.Visible = false;
            btnKategoriEkle.Visible = false;
            btnEkle.Visible = true;
        }
        private void kategorilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KategoriListele();
            label2.Visible = false;
            label3.Visible = false;
            label1.Visible = false;
            txtUrunAdi.Visible = false;
            nudFiyat.Visible = false;
            nudStok.Visible = false;
            txtKategoriAdi.Visible = true;
            txtTanimi.Visible = true;
            btnKategoriEkle.Visible = true;
            btnEkle.Visible = false;
        }
        private void KategoriListele()
        {
            dgvListe.RowTemplate.Height = 150;
            SqlDataAdapter adp = new SqlDataAdapter("Select * from Kategoriler", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgvListe.DataSource = dt;
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            string adi = txtUrunAdi.Text;
            decimal fiyat = nudFiyat.Value;
            decimal stok = nudStok.Value;
            if (adi == string.Empty || fiyat == 0 || stok == 0)
            {
                MessageBox.Show("Lütfen alanları doldurun.");
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = string.Format("insert urunler(UrunAdi,BirimFiyati,HedefStokDuzeyi) values ('{0}',{1},{2})", adi, fiyat, stok);
                cmd.Connection = baglanti;
                baglanti.Open();
                int etk = cmd.ExecuteNonQuery();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Eklendi.");
                    UrunListele();
                }
                else
                {
                    MessageBox.Show("Hata.");
                }
                baglanti.Close();
                txtUrunAdi.Text = string.Empty;
                nudFiyat.Value = 0;
                nudStok.Value = 0;
            }
        }

        private void btnKategoriEkle_Click(object sender, EventArgs e)
        {
            string adi = txtKategoriAdi.Text;
            string tanimi = txtTanimi.Text;
            if (adi == string.Empty || tanimi == string.Empty)
            {
                MessageBox.Show("Lütfen alanları doldurun.");
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = string.Format("insert kategoriler(KategoriAdi,Tanimi) values ('{0}','{1}')", adi, tanimi);
                cmd.Connection = baglanti;
                baglanti.Open();
                int etk = cmd.ExecuteNonQuery();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Eklendi.");
                    KategoriListele();
                }
                else
                {
                    MessageBox.Show("Hata.");
                }
                baglanti.Close();
                txtKategoriAdi.Text = string.Empty;
                txtTanimi.Text = string.Empty;
            }
        }

        private void faturalarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select * from Faturalar", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgvListe.DataSource = dt;
            label2.Visible = false;
            label3.Visible = false;
            label1.Visible = false;
            txtUrunAdi.Visible = false;
            nudFiyat.Visible = false;
            nudStok.Visible = false;
            txtKategoriAdi.Visible = false;
            txtTanimi.Visible = false;
            btnKategoriEkle.Visible = false;
        }

        private void dgvListe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUrunAdi.Tag = dgvListe.CurrentRow.Cells["UrunID"].Value;
            txtUrunAdi.Text = dgvListe.CurrentRow.Cells["UrunAdi"].Value.ToString();
            nudFiyat.Value = (decimal)dgvListe.CurrentRow.Cells["BirimFiyati"].Value;
            nudStok.Value = Convert.ToInt32(dgvListe.CurrentRow.Cells["HedefStokDuzeyi"].Value);
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string adi = txtUrunAdi.Text;
            int fiyat = (int)nudFiyat.Value;
            decimal stok = nudStok.Value;
            SqlCommand komut = new SqlCommand();
            komut.CommandText = string.Format("update urunler set UrunAdi='{0}',BirimFiyati={1},HedefStokDuzeyi={2} where UrunID={3}", adi, fiyat, stok, txtUrunAdi.Tag);
            komut.Connection = baglanti;
            baglanti.Open();
            try
            {
                int etk = komut.ExecuteNonQuery();
                if (etk > 0)
                {
                    MessageBox.Show("Güncellendi.");
                    UrunListele();
                }
                else
                {
                    MessageBox.Show("Güncelleme Başarısız.");
                }

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }
    }
}

