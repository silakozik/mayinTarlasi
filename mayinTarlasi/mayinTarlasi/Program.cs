using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayinTarlasi
{
    using System;

    class MayinTarlasi
    {
        const int BOYUT = 20; // Mayın tarlası boyutu
        const int MAYIN_SAYISI = 40; // Mayın sayısı
        static char[,] alan = new char[BOYUT, BOYUT]; // Görünen oyun alanı
        static bool[,] mayinlar = new bool[BOYUT, BOYUT]; // Mayınların bulunduğu yerler
        static bool[,] acilanHucres = new bool[BOYUT, BOYUT]; // Açılmış hücreler
        static int guvenliHucres; // Güvenli hücre sayısı

        static void Main()
        {
            OyunHazirla();
            OyunBaslat();
        }

        static void OyunHazirla()
        {
            for (int i = 0; i < BOYUT; i++)
            {
                for (int j = 0; j < BOYUT; j++)
                {
                    alan[i, j] = '#';
                    mayinlar[i, j] = false;
                    acilanHucres[i, j] = false;
                }
            }

            Random rnd = new Random();
            int yerlestirilenMayin = 0;
            while (yerlestirilenMayin < MAYIN_SAYISI)
            {
                int x = rnd.Next(BOYUT);
                int y = rnd.Next(BOYUT);
                if (!mayinlar[x, y])
                {
                    mayinlar[x, y] = true;
                    yerlestirilenMayin++;
                }
            }

            guvenliHucres = BOYUT * BOYUT - MAYIN_SAYISI;
        }

        static void OyunBaslat()
        {
            bool oyunDevam = true;

            while (oyunDevam)
            {
                KonsoluGoster();
                Console.WriteLine("Hücre açmak için satır ve sütun girin (örnek: 3 4):");
                string giris = Console.ReadLine();
                string[] girdi = giris.Split();

                // Giriş doğrulama
                if (girdi.Length != 2 || !int.TryParse(girdi[0], out int satir) || !int.TryParse(girdi[1], out int sutun))
                {
                    Console.WriteLine("Hatalı giriş. Lütfen 0 ile 19 arasında bir satır ve sütun değeri girin.");
                    continue;
                }

                // Sınır kontrolü
                if (satir < 0 || satir >= BOYUT || sutun < 0 || sutun >= BOYUT)
                {
                    Console.WriteLine("Geçersiz hücre! Lütfen 0 ile 19 arasında bir değer girin.");
                    continue;
                }

                if (acilanHucres[satir, sutun])
                {
                    Console.WriteLine("Bu hücre zaten açılmış!");
                    continue;
                }

                if (mayinlar[satir, sutun])
                {
                    Console.WriteLine("Mayına bastınız! Oyun bitti.");
                    OyunSonu(false);
                    oyunDevam = false;
                }
                else
                {
                    int etrafMayinSayisi = EtrafindakiMayinlariSay(satir, sutun);
                    alan[satir, sutun] = etrafMayinSayisi > 0 ? etrafMayinSayisi.ToString()[0] : ' ';
                    acilanHucres[satir, sutun] = true;
                    guvenliHucres--;

                    if (guvenliHucres == 0)
                    {
                        Console.WriteLine("Tebrikler! Tüm güvenli hücreleri açtınız.");
                        OyunSonu(true);
                        oyunDevam = false;
                    }
                }
            }
        }

        static void KonsoluGoster()
        {
            Console.Clear();
            Console.Write("   ");
            for (int i = 0; i < BOYUT; i++) Console.Write(i.ToString("D2") + " ");
            Console.WriteLine();

            for (int i = 0; i < BOYUT; i++)
            {
                Console.Write(i.ToString("D2") + " ");
                for (int j = 0; j < BOYUT; j++)
                {
                    Console.Write(alan[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

        static int EtrafindakiMayinlariSay(int x, int y)
        {
            int sayi = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int yeniX = x + i;
                    int yeniY = y + j;

                    if (yeniX >= 0 && yeniX < BOYUT && yeniY >= 0 && yeniY < BOYUT && mayinlar[yeniX, yeniY])
                    {
                        sayi++;
                    }
                }
            }

            return sayi;
        }

        static void OyunSonu(bool kazandi)
        {
            KonsoluGoster();
            if (kazandi)
            {
                Console.WriteLine("Kazandınız!");
            }
            else
            {
                Console.WriteLine("Kaybettiniz! İşte mayınların yerleri:");

                for (int i = 0; i < BOYUT; i++)
                {
                    for (int j = 0; j < BOYUT; j++)
                    {
                        if (mayinlar[i, j]) alan[i, j] = '*';
                    }
                }

                KonsoluGoster();
                Console.ReadLine();
            }
        }
    }

}
