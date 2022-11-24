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
    /// Interaction logic for IzmeniObjekat.xaml
    /// </summary>
    public partial class IzmeniObjekat : Window
    {
        public double konturnaLinija;
        public Color bojaUnutrasnjosti;
        public Color bojaKonture;

        public IzmeniObjekat()
        {
            InitializeComponent();
            comboBoxBojaKonturneLinijeIzmena.ItemsSource = typeof(Colors).GetProperties();
            comboBoxBojaUnutrasnjostiIzmena.ItemsSource = typeof(Colors).GetProperties();
        }
        private void ButtonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ComboBoxBojaKonturneLinije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (PropertyInfo)comboBoxBojaKonturneLinijeIzmena.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaKonture = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());

        }
        private void ComboBoxBojaUnutrasnjosti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (PropertyInfo)comboBoxBojaUnutrasnjostiIzmena.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaUnutrasnjosti = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());

        }
        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                if (((MainWindow)Application.Current.MainWindow).trenutniOblik == "ELIPSA")
                {
                    ((Ellipse)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).StrokeThickness = konturnaLinija;
                    ((Ellipse)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).Stroke = new SolidColorBrush(bojaKonture);
                    ((Ellipse)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).Fill = new SolidColorBrush(bojaUnutrasnjosti); // ovde imam problem kod izmene ako ima tekst
                    ((MainWindow)Application.Current.MainWindow).undoBool = true; //mozda ne mora ovde 
                }
                else if (((MainWindow)Application.Current.MainWindow).trenutniOblik == "POLYGON")
                {
                    ((Polygon)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).StrokeThickness = konturnaLinija;
                    ((Polygon)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).Stroke = new SolidColorBrush(bojaKonture);
                    ((Polygon)(((MainWindow)Application.Current.MainWindow).Canvas.Children[((MainWindow)Application.Current.MainWindow).tretnutni])).Fill = new SolidColorBrush(bojaUnutrasnjosti);
                    ((MainWindow)Application.Current.MainWindow).undoBool = true; //mozda ne mora ovde 
                }
                this.Close();
            }
            
        }

        //=======================================================================
        private bool validate()
        {
            bool result = true;


            //debljina konturne linije
            bool isDoubleKon = Double.TryParse(TextBoxKonturnaLinijaIzmena.Text, out konturnaLinija);
            if (isDoubleKon)
            {
                labelGreskaBorderThicknessIzmena.Content = "";
                labelGreskaBorderThicknessIzmena.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaBorderThicknessIzmena.Content = "Unesite double vrednost!";
                labelGreskaBorderThicknessIzmena.BorderBrush = Brushes.Red;
                result = false;
            }
            //boja konturne linije
            if (comboBoxBojaKonturneLinijeIzmena.SelectedItem == null)
            {
                result = false;
                comboBoxBojaKonturneLinijeIzmena.BorderBrush = Brushes.Red;
                labelGreskaBorderColorIzmena.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaKonturneLinijeIzmena.BorderBrush = Brushes.Green;
                labelGreskaBorderColorIzmena.Content = string.Empty;
            }
            //boja unutrasnjosti
            if (comboBoxBojaUnutrasnjostiIzmena.SelectedItem == null)
            {
                result = false;
                comboBoxBojaUnutrasnjostiIzmena.BorderBrush = Brushes.Red;
                labelGreskaFillColorIzmena.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaUnutrasnjostiIzmena.BorderBrush = Brushes.Green;
                labelGreskaFillColorIzmena.Content = string.Empty;
            }
            return result;
        }
    }
}
