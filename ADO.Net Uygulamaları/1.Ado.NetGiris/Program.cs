using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ado.NetGiris
{
    internal class Program
    {   
        static SqlConnection con = new SqlConnection("Data Source=DESKTOP-CMDAG63\\SQLEXPRESS;Initial Catalog=adonet;User ID=sa;password=1");

        static void Main(string[] args)
        {
            kayitSil(4); //id'si 4 olan kaydı sil.   
        }

        //insert, update, delete ile executeNoneQuery(); metodunu kullanırız.
        //Kayıtları getireceksek select ile ExecuteReader(); metodunu kullanırız.
        public static void kayitSil(int id)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("delete from loginTable where id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            int donenDeger = cmd.ExecuteNonQuery();

            if(donenDeger == 1)
            
                Console.WriteLine("Kayıt başarıyla silinmiştir.");
            else
                Console.WriteLine("Silme işlemi başarısız.");

            con.Close();
            Console.Read();
        }
        public static void kayitGuncelle(int id, string kullaniciAdi)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update logintable set kullaniciAdi=@kulad where id=@id", con);
            //con bağlantısındaki loginTable'a git ve id'si ... olanın kullaniciAdi'nı güncelle.
            cmd.Parameters.AddWithValue("@kulad", kullaniciAdi); //metoda gelen parametreler
            cmd.Parameters.AddWithValue("@id", id);
            int donenDeger = cmd.ExecuteNonQuery();

            if(donenDeger == 1)
                Console.WriteLine("Kayıt güncellendi.");
            else
                Console.WriteLine("Güncellenemedi.");

            con.Close();
            Console.ReadLine();
        }
        public static void kayitEkle(string kullaniciAdi, string sifre, string yetki)
        {
            con.Open(); //bağlantımızı açtık.

            SqlCommand cmd = new SqlCommand("insert into loginTable(kullaniciAdi,sifre,yetki) values(@kulad,@sifre,@yetki)", con);
                                                                    //tablodaki kolonlarımıza           //buradaki değerleri ekle @parametre ile.
                                                                    //kullaniciAdi, @kulad'a karşılık gelir. Diğerleri de sırayla.
            cmd.Parameters.AddWithValue("@kulad", kullaniciAdi);//@kulad'a kayitEkle'den gelen kullaniciAdi parametresinin değeri gelsin.
                                                                //o da aldığı değeri loginTable'daki kullaniciAdi'na gönderecek.
            cmd.Parameters.AddWithValue("@sifre", sifre);
            cmd.Parameters.AddWithValue("@yetki", yetki);
            //buraya kadar sorguyu hazırladık.

            int donenDeger = cmd.ExecuteNonQuery(); //ile sorgumuzu çalıştırırız. Eğer ki ekleme işlemi başarılı ise donenDeger=1 değisle 0 olur.
            if(donenDeger == 1)
            {
                Console.WriteLine("Kayıt eklenmiştir.");
            }
            else
                Console.WriteLine("Bir sorun oluştu.");

            Console.ReadLine();
            con.Close();
        }
        public static void kayıtlariGetir()
        {
            List<Musteri> musteriList = new List<Musteri>();

            SqlConnection con = new SqlConnection("Data Source=DESKTOP-CMDAG63\\SQLEXPRESS;Initial Catalog=adonet;User ID=sa;password=1");
            con.Open(); 

            SqlCommand cmd = new SqlCommand("select * from loginTable", con); 

            SqlDataReader dr = cmd.ExecuteReader(); 

            while (dr.Read()) 
            {
                Musteri musteri = new Musteri();
                musteri.id = int.Parse(dr["id"].ToString());
                musteri.kullaniciAdi = dr["kullaniciAdi"].ToString();
                musteri.sifre = dr["sifre"].ToString();
                musteri.yetki = dr["yetki"].ToString();

                musteriList.Add(musteri);
            }
            con.Close();
            foreach (Musteri musteri in musteriList)
            {
                Console.WriteLine("id: " + musteri.id + " Kullanıcı Adı: " + musteri.kullaniciAdi + " Şifre:" + musteri.sifre + " Yetki: " + musteri.yetki);
            }
            Console.Read();
        }
    }
}
