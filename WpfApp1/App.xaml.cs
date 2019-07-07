using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        private string txt;
        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {

            TextBox box = sender as TextBox;
            txt = box.Text;
            box.Text = String.Empty;
        }

        private void No_Focus(object sender, RoutedEventArgs e)
        {

            TextBox box = sender as TextBox;
            if (box.Text == "")
            {
                box.Text = txt;
            }

        }

    }
}
