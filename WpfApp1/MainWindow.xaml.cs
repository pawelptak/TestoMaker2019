using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Properties;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        List<string> praw_odpowiedzi = new List<string>(); //przechowuje informacje o o kazdej z odpowiedzi w postaci 1-prawda, 0-falsz
        List<string> odpowiedzi = new List<string>(); //przechowuje tresci odpowiedzi
        private string[] alfabet = { "A","B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" }; //warianty
        public static int nrPytania = 1;
     
        public MainWindow()
        {
            InitializeComponent();
            NrPytania.Text = Settings.Default["nrPytania"].ToString();

        }

        
        private void ZapiszOdp(object sender, RoutedEventArgs e)
        {
            nrPytania = Int32.Parse( NrPytania.Text); //wczytanie nr pytania z textboxu
            odpowiedzi.Clear();
            praw_odpowiedzi.Clear();
                     
            praw_odpowiedzi.Add("X");
            foreach (CheckBox cbox in PrawdaFalsz.Children)
            {
                if (cbox.IsChecked==true)
                {
                    praw_odpowiedzi.Add("1");
                }
                else praw_odpowiedzi.Add("0");
                cbox.IsChecked = false;

            }
            if (!praw_odpowiedzi.Contains("1"))
            {
                MessageBox.Show("Ktoraś z odpowiedzi musi być prawdziwa.", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            int j = 0;
            foreach (TextBox box in Odpowiedzi.Children)
            {
                odpowiedzi.Add(box.Text);
                box.Text = "Odpowiedź " + alfabet.ElementAt(j);
                j++;
            }

            String sciezkaPliku = System.IO.Directory.GetCurrentDirectory();
            sciezkaPliku += "\\baza"; //baza tworzona jest w folderze 'baza' 

           

            if (!Directory.Exists(sciezkaPliku))
            {
                Directory.CreateDirectory(sciezkaPliku);
            }

            if (nrPytania.ToString().Length == 1)
            {
                if (nazwaPliku != "")
                {
                    System.IO.File.Copy(nazwaPliku, sciezkaPliku + "\\00" + nrPytania + ".png");
                    nazwaPliku = "";
                }
                sciezkaPliku += "\\00" + nrPytania + ".txt";
            }
            else if (nrPytania.ToString().Length == 2)
            {
                if (nazwaPliku != "")
                {
                    System.IO.File.Copy(nazwaPliku, sciezkaPliku + "\\0" + nrPytania + ".png");
                    nazwaPliku = "";
                }
                sciezkaPliku += "\\0" + nrPytania + ".txt";
            }
            else
            {
                if (nazwaPliku != "")
                {
                    System.IO.File.Copy(nazwaPliku, sciezkaPliku + "\\" + nrPytania + ".png");
                    nazwaPliku = "";
                }
                sciezkaPliku += "\\" + nrPytania + ".txt";
            }


            if (File.Exists(sciezkaPliku)) 
            {
                File.Delete(sciezkaPliku);
            }

            using (StreamWriter sw = File.CreateText(sciezkaPliku))
            {
               

                foreach (string odp in praw_odpowiedzi)
                {
                    sw.Write(odp);
                }
                sw.WriteLine();

                sw.WriteLine(Wprowadzanie.Text);
                
                Wprowadzanie.Text = "Wprowadź pytanie";

                foreach (string odp in odpowiedzi)
                {
                    sw.WriteLine(odp);
                }      
            }          
            nrPytania++;
            ZapiszNrPytania(nrPytania);
            NrPytania.Text = nrPytania.ToString();      
        }

        private void ZapiszNrPytania(int nr)
        {
            Settings.Default["nrPytania"] = nr;
            Settings.Default.Save();
        }

        private string txt;   
        private void TextBox_Focus(object sender, RoutedEventArgs e) //usuwa tekst z textboxu jesli zostal klikniety
        {
           
                TextBox box = sender as TextBox;
                txt = box.Text;
                box.Text = String.Empty;    
        }

        private void No_Focus(object sender, RoutedEventArgs e) //przywraca poprzedni tekst w textboxie jesli zostal odklikniety
        {

                TextBox box = sender as TextBox;
                if (box.Text == "")
            {
                box.Text = txt;           
            }   else
            {
                box.Text = box.Text.Replace("\n", "").Replace("\r", ""); //usuwa znaki nowej linii z tekstu
            }
                        
        }

   
        private void Wyjdz(object sender, RoutedEventArgs e) //wyjscie za pomoca przycisku X
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Minimalizuj(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Minimized) WindowState = WindowState.Minimized;

        }

 
        private void Maksymalizuj(object sender, RoutedEventArgs e) //umozliwia maksymalizacje okna
        {
            if (WindowState != WindowState.Maximized) 
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
           
        }

        private void Przenoszenie(object sender, MouseButtonEventArgs e) //umozliwia przenoszenie okna

        {

            this.DragMove();

        }


        private int i = 1;
        private int liczbaElementow = 0;
        public void btnAddMore_Click(object sender, RoutedEventArgs e) //dodanie wariantu odpowiedzi

        {
            if (i > 12) return;
            List<string> litery = new List<string>(alfabet);
            System.Windows.Controls.TextBox nowy = new TextBox();     
            nowy.Text = "Odpowiedź "+litery.ElementAt(i);
            Style styl1 = this.FindResource("Odpowiedz") as Style;
            nowy.Style = styl1;            
            System.Windows.Controls.CheckBox nowyChecker = new CheckBox();
            Style styl2 = this.FindResource("PrawdaFalsz") as Style;
           nowyChecker.Style = styl2;

       
            Odpowiedzi.Children.Add(nowy);                            
            nowy.Uid = i.ToString();
            PrawdaFalsz.Children.Add(nowyChecker);
            nowyChecker.Uid = i.ToString();
            nowy.KeyDown += ZaznaczPrawde;          
            i++;
            liczbaElementow++;
            nowy.Focus();
        }

        public void btnRemove_Click(object sender, RoutedEventArgs e) //usunieie wariantu odpowiedzi

        {          
            if (liczbaElementow > 0)
            {
                Odpowiedzi.Children.RemoveAt(Odpowiedzi.Children.Count - 1);
                PrawdaFalsz.Children.RemoveAt(PrawdaFalsz.Children.Count - 1);
                liczbaElementow--;
                i--;
            }
        }

        private string nazwaPliku="";
        public void DodajObraz(object sender, RoutedEventArgs e) //dodawanie pytania w postaci pliku graficznego

        {
            String sciezkaPliku = System.IO.Directory.GetCurrentDirectory();
            sciezkaPliku += "\\baza";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Obrazy (*.png)|*.png|Wszystkie pliki (*.*)|*.*";
            openFileDialog.ShowDialog();
            nazwaPliku = openFileDialog.FileName;

            if (nazwaPliku != "")
            {
                if (NrPytania.Text.ToString().Length == 1)
                {
                    Wprowadzanie.Text = "[img]00" + NrPytania.Text + ".png[/img]";
                }
                else if (NrPytania.Text.ToString().Length == 2)
                {
                    Wprowadzanie.Text = "[img]0" + NrPytania.Text + ".png[/img]";
                }
                else
                {
                    Wprowadzanie.Text = "[img]" + NrPytania.Text + ".png[/img]";
                }
            }
                   
                 
        }

        private void ZaznaczPrawde(object sender, KeyEventArgs e) //zaznaczenie checkboxu kombinacja klawiszy CTRL + P
        {
            TextBox box = sender as TextBox;
            if (e.Key== Key.P && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {

                foreach (CheckBox cbox in PrawdaFalsz.Children)
                {
                    if (cbox.Uid == box.Uid)
                    {
                        if (cbox.IsChecked == false)
                        {
                            cbox.IsChecked = true;
                        }
                        else cbox.IsChecked = false;                      
                    }
                    
                }
            }
        }

        private void WcisnietoKlawisz(object sender, KeyEventArgs e) //dodawanie i usuwanie wariantow odpowiedzi za pomoca ENTER i ESC
        {         
            switch (e.Key)
            {
                case Key.Enter:           
                    btnAddMore_Click(sender, e);                  
                    break;
                case Key.Escape:
                    btnRemove_Click(sender, e);              
                    break;
            }
        }

    }
}
