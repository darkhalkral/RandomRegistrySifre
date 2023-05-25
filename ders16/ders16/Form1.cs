using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.OleDb;

namespace ders16
{
    public partial class Form1 : Form
    {
        OleDbConnection con;
        DataTable dt = new DataTable();
        int deneme = 0;
        string sayistr;
        bool sifredurum = false;
        public Form1()
        {
            InitializeComponent();
        }
        void baglanti()
        {
            dt.Clear();
            listView1.Items.Clear();
            con = new OleDbConnection("Provider=Microsoft.JET.Oledb.4.0; Data Source=C:\\Users\\halid\\source\\repos\\ogrnot.mdb");
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from ogr", con);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listView1.Items.Add(dt.Rows[i]["ogrno"].ToString());
                listView1.Items[i].SubItems.Add(dt.Rows[i]["adsoyad"].ToString());
                listView1.Items[i].SubItems.Add(dt.Rows[i]["vize1"].ToString());
                listView1.Items[i].SubItems.Add(dt.Rows[i]["vize2"].ToString());
                listView1.Items[i].SubItems.Add(dt.Rows[i]["final"].ToString());
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("adsoyad");
            listView1.Columns.Add("vize1");
            listView1.Columns.Add("vize2");
            listView1.Columns.Add("final");
            baglanti();
            Random rastgele = new Random();
            int ilksayi = rastgele.Next(100000,999999);
            int sonsayi = rastgele.Next(10000, 99999);
            sayistr = ilksayi.ToString() + sonsayi.ToString();
            Registry.CurrentUser.CreateSubKey("RndSifre").SetValue("sifre", sayistr);
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "insert into sifre(sifre) values('" + sayistr + "')";
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show(sayistr);
        }

        void sifredeneme()
        {
            for (int i = 3; i >= 0; i--)
            {
                if (deneme == 3)
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM ogr";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Veriler Silindi");
                    baglanti();
                    deneme = 0;
                }
                else
                {
                    if (Microsoft.VisualBasic.Interaction.InputBox("Şifre Giriniz") == sayistr)
                    {
                        sifredurum = true;
                        deneme = 0;
                        MessageBox.Show("Şifre Doğru");
                        break;
                    }
                    else
                    {
                        deneme++;
                        if (i != 1)
                        {
                            MessageBox.Show((i - 1) + " Deneme Hakkınız Kaldı");
                        }
                       
                    }
                }
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
           sifredeneme();
            if (sifredurum == true)
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.CommandText = "Insert into ogr values(" + textBox1.Text + ",'" + textBox2.Text + "'," + textBox3.Text + "," + textBox4.Text + "," + textBox5.Text + ")";
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            baglanti();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
    }
}
