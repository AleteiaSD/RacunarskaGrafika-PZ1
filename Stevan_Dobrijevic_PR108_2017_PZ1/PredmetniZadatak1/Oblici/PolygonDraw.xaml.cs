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
    /// Interaction logic for PolygonDraw.xaml
    /// </summary>
    public partial class PolygonDraw : Window
    {
        public List<Point> koordinatePoligona = new List<Point>();
        public double konturnaLinijPolygone;
        public Color bojaUnutrasnjostiPolygon;
        public Color bojaKonturePolygon;
        public Color bojaTextaPolygonOpciono;
        
        public PolygonDraw()
        {
            InitializeComponent();
            comboBoxBojaKonturneLinijePolygon.ItemsSource = typeof(Colors).GetProperties();
            comboBoxBojaUnutrasnjostiPolygon.ItemsSource = typeof(Colors).GetProperties();
            comboBoxBojaTextaPolygonOpciono.ItemsSource = typeof(Colors).GetProperties();
        }
        
        private void ComboBoxBojaKonturneLinijePolygon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (PropertyInfo)comboBoxBojaKonturneLinijePolygon.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaKonturePolygon = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());
        }
        private void ComboBoxBojaUnutrasnjostiPolygon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (PropertyInfo)comboBoxBojaUnutrasnjostiPolygon.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaUnutrasnjostiPolygon = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());
        }
        private void ComboBoxBojaTextaPolygonOpciono_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (PropertyInfo)comboBoxBojaTextaPolygonOpciono.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaTextaPolygonOpciono = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());

        }
        private void ButtonIzadjiPolygon_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonIscrtajPolygon_Click(object sender, RoutedEventArgs e)
        {
            if (validatePolygon())
            {
                Polygon polygon = new Polygon();
                
                if (TextBoxPolygonTextOpciono.Text == "" && comboBoxBojaTextaPolygonOpciono.SelectedItem == null)
                {
                    foreach (Point p in koordinatePoligona)
                    {
                        polygon.Points.Add(p);
                    }

                    //sirina linije
                    polygon.StrokeThickness = konturnaLinijPolygone;

                    //boja
                    polygon.Stroke = new SolidColorBrush(bojaKonturePolygon);
                    polygon.Fill = new SolidColorBrush(bojaUnutrasnjostiPolygon);
                }
                else if (TextBoxPolygonTextOpciono.Text != "" && comboBoxBojaTextaPolygonOpciono.SelectedItem != null) //opciono
                {
                    foreach (Point p in koordinatePoligona)
                    {
                        polygon.Points.Add(p);
                    }

                    //sirina linije
                    polygon.StrokeThickness = konturnaLinijPolygone;

                    //boja
                    polygon.Stroke = new SolidColorBrush(bojaKonturePolygon);


                    VisualBrush visualBrush = new VisualBrush();
                    StackPanel stackPanel = new StackPanel();


                    TextBlock textBlock = new TextBlock();
                    if (TextBoxPolygonTextOpciono.Text != "" && comboBoxBojaTextaPolygonOpciono.SelectedItem != null)
                    {
                        if (validateText())
                        {
                            textBlock.Text = TextBoxPolygonTextOpciono.Text;
                            textBlock.Foreground = new SolidColorBrush(bojaTextaPolygonOpciono);
                            textBlock.FontSize = 10;
                            textBlock.Margin = new Thickness(10);
                        }
                    }
                    stackPanel.Background = new SolidColorBrush(bojaUnutrasnjostiPolygon);
                    
                    stackPanel.Children.Add(textBlock);
                    visualBrush.Visual = stackPanel;

                    polygon.Fill = visualBrush;
                }
                  

                ((MainWindow)Application.Current.MainWindow).Canvas.Children.Add(polygon);
                koordinatePoligona.Clear();
                ((MainWindow)Application.Current.MainWindow).listaDodatihObjekata.Add(polygon);
                ((MainWindow)Application.Current.MainWindow).undoBool = true; //mozda ne mora ovde 

                this.Close();
            }
            else
            {
                MessageBox.Show("Podaci nisu dobro popunjeni.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //===================VALIDATE======================================
        private bool validatePolygon()
        {
            bool result = true;

            //debljina konturne linije
            bool isDoubleKon = Double.TryParse(TextBoxKonturnaLinijaPolygon.Text, out konturnaLinijPolygone);
            if (isDoubleKon)
            {
                labelGreskaBorderThicknessPolygon.Content = "";
                labelGreskaBorderThicknessPolygon.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaBorderThicknessPolygon.Content = "Unesite double vrednost!";
                labelGreskaBorderThicknessPolygon.BorderBrush = Brushes.Red;
                result = false;
            }
            //boja konturne linije
            if (comboBoxBojaKonturneLinijePolygon.SelectedItem == null)
            {
                result = false;
                comboBoxBojaKonturneLinijePolygon.BorderBrush = Brushes.Red;
                labelGreskaBorderColorPolygon.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaKonturneLinijePolygon.BorderBrush = Brushes.Green;
                labelGreskaBorderColorPolygon.Content = string.Empty;
            }
            //boja unutrasnjosti
            if (comboBoxBojaUnutrasnjostiPolygon.SelectedItem == null)
            {
                result = false;
                comboBoxBojaUnutrasnjostiPolygon.BorderBrush = Brushes.Red;
                labelGreskaFillColorPolygon.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaUnutrasnjostiPolygon.BorderBrush = Brushes.Green;
                labelGreskaFillColorPolygon.Content = string.Empty;
            }
            return result;
        }
        private bool validateText()
        {
            bool result = true;

            //Text
            if (TextBoxPolygonTextOpciono.Text.Trim().Equals(""))
            {
                labelGreskaBojaTextaPolygonOpciono.Content = "Unesite text!";
                labelBojaTextaPolygonOpciono.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                labelGreskaBojaTextaPolygonOpciono.Content = string.Empty;
                labelGreskaBojaTextaPolygonOpciono.BorderBrush = Brushes.Green;
            }
            //boja Texta
            if (comboBoxBojaTextaPolygonOpciono.SelectedItem == null)
            {
                result = false;
                comboBoxBojaTextaPolygonOpciono.BorderBrush = Brushes.Red;
                labelGreskaBojaTextaPolygonOpciono.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaTextaPolygonOpciono.BorderBrush = Brushes.Green;
                labelGreskaBojaTextaPolygonOpciono.Content = string.Empty;
            }
            return result;
        }
    }
}
