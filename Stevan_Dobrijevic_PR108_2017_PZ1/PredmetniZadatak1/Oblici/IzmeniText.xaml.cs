using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PredmetniZadatak1.Oblici
{
    /// <summary>
    /// Interaction logic for IzmeniText.xaml
    /// </summary>
    public partial class IzmeniText : Window
    {
        public double velicinaTexta;
        public Color bojaTexta;
        public IzmeniText()
        {
            InitializeComponent();
            comboBoxBojaTextaIzmena.ItemsSource = typeof(Colors).GetProperties();
        }
        private void ComboBoxBojaTextaIzmena_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (PropertyInfo)comboBoxBojaTextaIzmena.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaTexta = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());
        }
        private void ButtonIzadjiTextIzmena_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ButtonIzmeniTextIzmena_Click(object sender, RoutedEventArgs e)
        {
            if(validate())
            {
                if (((MainWindow)Application.Current.MainWindow).trenutniOblik == "TEXT")
                {
                    ((TextBlock)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).FontSize = velicinaTexta;
                    ((TextBlock)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).Foreground = new SolidColorBrush(bojaTexta);
                    ((MainWindow)Application.Current.MainWindow).undoBool = true; //mozda ne mora ovde 
                }
                this.Close();
            }
           
        }
        //==========================================================================
        private bool validate()
        {
            bool result = true;


            //boja Texta
            if (comboBoxBojaTextaIzmena.SelectedItem == null)
            {
                result = false;
                comboBoxBojaTextaIzmena.BorderBrush = Brushes.Red;
                labelGreskaBojaTextaIzmena.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaTextaIzmena.BorderBrush = Brushes.Green;
                labelGreskaBojaTextaIzmena.Content = string.Empty;
            }
            //Font
            bool isDouble = Double.TryParse(TextBoxVelicinaTextaIzmena.Text, out velicinaTexta);

            if (isDouble)
            {
                labelGreskaVelicinaTextaIzmena.Content = "";
                labelGreskaVelicinaTextaIzmena.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaVelicinaTextaIzmena.Content = "Unesite double vrednost!";
                labelGreskaVelicinaTextaIzmena.BorderBrush = Brushes.Red;
                result = false;
            }
            return result;
        }
    }
}
