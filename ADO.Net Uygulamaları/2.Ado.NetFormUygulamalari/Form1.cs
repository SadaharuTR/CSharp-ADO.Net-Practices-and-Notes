using _2.Ado.NetFormUygulamalari.Model;
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

namespace _2.Ado.NetFormUygulamalari
{
    public partial class Form1 : Form
    {

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-CMDAG63\\SQLEXPRESS;Initial Catalog=adonet;User ID=sa;password=1");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = kayitlariGetir(); //1)formumuz ilk yüklendiğinde yani start'a basıldığında metodumuza gidiyoruz.
            
            //7)dönen listeyi de burada dataGridView1'in DataSource attribute'una basmış oluyoruz.
        }

        public List<Musteri> kayitlariGetir() //6)bu metot bize Musteri tipinde bir List dönüyor.
        {
            List<Musteri> musteriList = new List<Musteri>(); //5)atım işlemi bittikten sonra kaybolmaması için musteriList koleksiyonumun içerisinde tutuyoruz.

            con.Open();//bağlantımızı açıyoruz.

            SqlCommand cmd = new SqlCommand("select * from musteri", con); //2)açtıktan sonra da kaydımızı hazırlıyoruz.
            SqlDataReader dr = cmd.ExecuteReader(); //3)çalıştırıp SqlDataReader ile yakalıyoruz.

            while(dr.Read())//4)her bir kayıtta dönerek oluşturmuş olduğumuz buradaki musteri tipindeki nesnemize(class'ı da denir) elimizdeki kaydı parça parça atıyoruz.
            {
                Musteri musteri = new Musteri();
                musteri.id = int.Parse(dr["id"].ToString());
                musteri.isim = dr["isim"].ToString();
                musteri.soyisim = dr["soyisim"].ToString();
                musteri.emailAdres = dr["emailAdres"].ToString();
                musteri.telefonNo = dr["telefonNo"].ToString();
                musteriList.Add(musteri);
            }
            con.Close();

            return musteriList;
        }

        public int kayitEkle()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into musteri (isim,soyisim,emailAdres,telefonNo) values(@isim,@soyisim,@emailAdres,@telefonno)", con);
            cmd.Parameters.AddWithValue("@isim", txt_isim.Text);
            cmd.Parameters.AddWithValue("@soyisim", txt_soyisim.Text);
            cmd.Parameters.AddWithValue("@emailAdres",txt_emailAdres.Text);
            cmd.Parameters.AddWithValue("@telefonno",maskedTextBox1.Text);

            int donenDeger = cmd.ExecuteNonQuery();
            con.Close();

            if (donenDeger == 1)
                return 1;
            else
                return 0;
        }
        private void btn_kayitEkle_Click(object sender, EventArgs e)
        {
            int sonuc = kayitEkle();

            if (sonuc == 1) 
            { 
                MessageBox.Show("Kayıt Eklenmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //eklenen kaydın da ekrana yansıması için
                dataGridView1.DataSource = kayitlariGetir(); //metodu nu çağırırsak veritabanına gidio en güncel halini ekrana çağıracaktır.
            }
            else
                MessageBox.Show("Bir sorun oluştu. Kayıt Eklenemedi.", "Ölümcül Hata.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public int kayitGuncelle()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update musteri set emailAdres = @emailAdres where id=@id", con);
            cmd.Parameters.AddWithValue("@emailAdres", txt_emailAdres.Text);
            cmd.Parameters.AddWithValue("@id", int.Parse(txt_id.Text));
            int donenDeger = cmd.ExecuteNonQuery();
            con.Close();

            if (donenDeger == 1)
                return 1; //başarılı
            else
                return 0; //başarısız
        }

        private void btn_kayitGuncelle_Click(object sender, EventArgs e)
        {
            int sonuc = kayitGuncelle(); //butona bastık ve kayitGuncelle metoduna gittik.
            if(sonuc == 1)
            {
                MessageBox.Show("Kayıt Güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = kayitlariGetir();
            }
            else
                MessageBox.Show("Bir sorun oluştu. Kayıt Güncellenemedi.", "Büyük Hata.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public int kayitSil()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from musteri where id=@id", con);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txt_id.Text));
            int donenDeger = cmd.ExecuteNonQuery();
            con.Close();

            if (donenDeger == 1)
            {
                return 1;
            }
            else
                return 0;
        }
        private void btn_kayitSil_Click(object sender, EventArgs e)
        {
            int sonuc = kayitSil();
            if(sonuc == 1)
            {
                MessageBox.Show("Kayıt Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = kayitlariGetir();
            }
            else
                MessageBox.Show("Bir sorun oluştu. Kayıt Silinemedi.", "Dünyayı Sarsacak Bir Hata.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
