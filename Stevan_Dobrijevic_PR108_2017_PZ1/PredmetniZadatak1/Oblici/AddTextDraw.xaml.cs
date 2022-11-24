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
    /// Interaction logic for AddTextDraw.xaml
    /// </summary>
    public partial class AddTextDraw : Window
    {
        public double font;
        public Color bojaTexta;

        public double xosa;
        public double yosa;

        public AddTextDraw()
        {
            InitializeComponent();
            comboBoxBojaTexta.ItemsSource = typeof(Colors).GetProperties();            
        }
        private void ButtonIscrtajText_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {                       
                TextBlock tb = new TextBlock();
                tb.Text = samText.Text;
                tb.FontSize = font;
                tb.Foreground = new SolidColorBrush(bojaTexta);
                Thickness margina = tb.Margin;
                margina.Left = xosa;
                margina.Top = yosa;
                tb.Margin = margina;

                ((MainWindow)Application.Current.MainWindow).Canvas.Children.Add(tb);
                ((MainWindow)Application.Current.MainWindow).listaDodatihObjekata.Add(tb);
                ((MainWindow)Application.Current.MainWindow).undoBool = true; //mozda ne mora ovde 

                this.Close(); 
            }
            else
            {
                MessageBox.Show("Podaci nisu dobro popunjeni.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonIzadjiText_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxBojaText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (PropertyInfo)comboBoxBojaTexta.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaTexta = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());
        }
        //=======================================================================
        private bool validate()
        {
            bool result = true;

            //Text
            if (samText.Text.Trim().Equals(""))
            {
                labelGreskaText.Content = "Unesite text!";
                labelGreskaText.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                labelGreskaText.Content = string.Empty;
                labelGreskaText.BorderBrush = Brushes.Green;
            }
            //boja Texta
            if (comboBoxBojaTexta.SelectedItem == null)
            {
                result = false;
                comboBoxBojaTexta.BorderBrush = Brushes.Red;
                labelGreskaBoja.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaTexta.BorderBrush = Brushes.Green;
                labelGreskaBoja.Content = string.Empty;
            }
            //Font
            bool isDouble = Double.TryParse(FontTexta.Text, out font);

            if (isDouble)
            {
                labelGreskaFont.Content = "";
                labelGreskaFont.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaFont.Content = "Unesite double vrednost!";
                labelGreskaFont.BorderBrush = Brushes.Red;
                result = false;
            }   

            return result;
        }
    }
}
