﻿using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        Boolean dosya_durumu = false;
        long dosyaUzunlugu = 0;
        long say = 0;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private void EncryptFile()
        {
            try
            {
                string inputFile = dosya_yolu.Text;
                string outputFile = dosya_yolu.Text + "SIFRELENDI";
                string password = @sifre.Text;

                UnicodeEncoding UE = new UnicodeEncoding();

                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;

                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                say = 0;
                int data;

                while ((data = fsIn.ReadByte()) != -1)
                {
                    cs.WriteByte((byte)data);
                    say++;
                    double yuzde = ((double)say / (double)dosyaUzunlugu) * (double)100;
                    int yuzde_org = Convert.ToInt32(yuzde);
                    progressBar1.Value = yuzde_org;
                }

                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;

                MessageBox.Show("Şifreleme İşlemi Başarılı Bir Şekilde Gerçekleştirildi!", "ok");
            }
            catch
            {
                MessageBox.Show("Şifreleme İşlemi Başarısız Oldu!", "Error");
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }
        private void DecryptFile()
        {
            try
            {
                string inputFile = dosya_yolu.Text;
                // string outputFile= dosya_yolu.Text + "SIFRELENDI"; -> SIFRELENDI -> Karakteri kadar çıkartılma yapılması gereklidir.
                string outputFile = dosya_yolu.Text.Substring(0, dosya_yolu.Text.Length - 10);
                string password = @sifre.Text;

                UnicodeEncoding UE = new UnicodeEncoding();

                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                say = 0;
                int data;

                while ((data = cs.ReadByte()) != -1)
                {
                    fsOut.WriteByte((byte)data);
                    say++;
                    double yuzde = ((double)say / (double)dosyaUzunlugu) * (double)100;
                    int yuzde_org = Convert.ToInt32(yuzde);
                    progressBar1.Value = yuzde_org;
                }

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

                groupBox1.Enabled = true;
                groupBox2.Enabled = true;

                MessageBox.Show("Şifre Çözme İşlemi Başarılı Bir Şekilde Gerçekleştirildi!", "ok");
            }
            catch
            {
                MessageBox.Show("Şifre Çözme İşleminde Bir Hata Oluştu!", "Error");
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dosya_durumu == true)
            {
                if (sifre.Text == string.Empty)
                {
                    MessageBox.Show("Lütfen Geçerli Bir Şifre Girin!");
                }
                else
                {
                    if (sifre.TextLength == 8)
                    {
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        sifre_coz_fonk.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Şifre 8 Karakter Olmalıdır!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen Dosya Seçiniz!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dosya_durumu == true)
            {
                if (sifre.Text == string.Empty)
                {
                    MessageBox.Show("Lütfen Geçerli Bir Şifre Girin!");
                }
                else
                {
                    if (sifre.TextLength == 8)
                    {
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        sifrele_fonk.RunWorkerAsync();

                    }
                    else
                    {
                        MessageBox.Show("Şifre 8 Karakter Olmalıdır!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen dosya seçiniz.!");
            }

        }

        private void dosya_sec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Tüm Dosyalar |*.*";
            dosya.Title = "İşlem yapılacak dosyayı seçin!";
            if (dosya.ShowDialog() == DialogResult.OK)
            {
                dosya_durumu = true;
                dosya_yolu.Text = dosya.FileName;
                dosya_adi.Text = dosya.SafeFileName;

                FileInfo info = new FileInfo(dosya_yolu.Text);

                dosyaUzunlugu = info.Length;
                dosya_boyut.Text = BoyutHesapla(dosyaUzunlugu);
            }
            else
            {
                dosya_durumu = false;
            }

        }
        public static string BoyutHesapla(long gelenboyut)
        {
            string sonucboyut = "";

            if (gelenboyut >= 1073741824)
            {
                sonucboyut += (gelenboyut / 1073741824) + " GB ";
                gelenboyut = gelenboyut % 1073741824;
            }

            if (gelenboyut >= 1048576)
            {
                sonucboyut += (gelenboyut / 1048576) + " MB ";
                gelenboyut = gelenboyut % 1048576;
            }

            if (gelenboyut >= 1024)
            {
                sonucboyut += (gelenboyut / 1024) + " KB ";
                gelenboyut = gelenboyut % 1024;
            }
            sonucboyut += gelenboyut + " B ";

            return sonucboyut;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sifre.Text = "";
            button3.Visible = false;
        }

        private void sifre_TextChanged(object sender, EventArgs e)
        {
            button3.Visible = true;
        }

        private void sifrele_fonk_DoWork(object sender, DoWorkEventArgs e)
        {
            EncryptFile();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = "%" + progressBar1.Value.ToString();
            label4.Text = "İşlenen Veri:" + BoyutHesapla(say);
            label5.Text = "Kalan Veri:" + BoyutHesapla(dosyaUzunlugu - say);
        }

        private void sifre_coz_fonk_DoWork(object sender, DoWorkEventArgs e)
        {
            DecryptFile();
        }
    }
}
