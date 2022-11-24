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
    /// Interaction logic for EllipseDraw.xaml
    /// </summary>   
    public partial class EllipseDraw : Window
    {     
        public double poluprecnik1;
        public double poluprecnik2;
        public double konturnaLinija;
        public Color bojaUnutrasnjosti;
        public Color bojaKonture;

        public Color bojaTextaOpciono;

        public double xosa;
        public double yosa;

        public EllipseDraw()
        {
            InitializeComponent();
            comboBoxBojaKonturneLinije.ItemsSource = typeof(Colors).GetProperties();
            comboBoxBojaUnutrasnjosti.ItemsSource = typeof(Colors).GetProperties();
            comboBoxBojaTextaOpciono.ItemsSource = typeof(Colors).GetProperties();
        }
        
        private void ButtonIscrtaj_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                Ellipse elipsa = new Ellipse();
                if (TextBoxTextOpciono.Text == "" && comboBoxBojaTextaOpciono.SelectedItem == null)
                {                    
                    //sirina i visiina
                    elipsa.Width = poluprecnik2 * 2;
                    elipsa.Height = poluprecnik1 * 2;
                    //sirina linije
                    elipsa.StrokeThickness = konturnaLinija;

                    //boja
                    elipsa.Stroke = new SolidColorBrush(bojaKonture);
                    elipsa.Fill = new SolidColorBrush(bojaUnutrasnjosti);

                    Thickness margina = elipsa.Margin;
                    margina.Left = xosa;
                    margina.Top = yosa;
                    elipsa.Margin = margina;
                }
                else if (TextBoxTextOpciono.Text != "" && comboBoxBojaTextaOpciono.SelectedItem != null)
                {
                    //sirina i visiina
                    elipsa.Width = poluprecnik2 * 2;
                    elipsa.Height = poluprecnik1 * 2;
                    Thickness margina = elipsa.Margin;
                    margina.Left = xosa;
                    margina.Top = yosa;
                    elipsa.Margin = margina;

                    elipsa.StrokeThickness = konturnaLinija;
                    elipsa.Stroke = new SolidColorBrush(bojaKonture);


                    VisualBrush visualBrush = new VisualBrush();
                    StackPanel stackPanel = new StackPanel();
                 

                    TextBlock textBlock = new TextBlock();
                    if (TextBoxTextOpciono.Text != "" && comboBoxBojaTextaOpciono.SelectedItem != null)
                    {
                        if (validateText())
                        {
                            textBlock.Text = TextBoxTextOpciono.Text;
                            textBlock.Foreground = new SolidColorBrush(bojaTextaOpciono);
                            textBlock.FontSize = (double)10;
                            textBlock.Margin = new Thickness(10);
                        }
                    }
                    stackPanel.Background = new SolidColorBrush(bojaUnutrasnjosti);  //MORAM JOS TEKST DA STAVIM U GRANICE OBLIKA JER IZLAZI IZ GRANICA
                    stackPanel.Children.Add(textBlock);
                    visualBrush.Visual = stackPanel;
                    elipsa.Fill = visualBrush;
                }

                //AKO OVAKO URADIM ONDA MI UNDO NECE RADITI JER JE DODAO NA CANVAS 2 DETETA
                ((MainWindow)Application.Current.MainWindow).Canvas.Children.Add(elipsa);
                ((MainWindow)Application.Current.MainWindow).listaDodatihObjekata.Add(elipsa);
                ((MainWindow)Application.Current.MainWindow).undoBool = true; //mozda ne mora ovde 
                                    

                this.Close();
            }
            else
            {
                MessageBox.Show("Podaci nisu dobro popunjeni.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxBojaKonturneLinije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (PropertyInfo)comboBoxBojaKonturneLinije.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaKonture = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());

        }
        private void ComboBoxBojaUnutrasnjosti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (PropertyInfo)comboBoxBojaUnutrasnjosti.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaUnutrasnjosti = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());

        }
        private void ComboBoxBojaTextaOpciono_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedItem = (PropertyInfo)comboBoxBojaTextaOpciono.SelectedItem;
            var color = (Color)selectedItem.GetValue(null, null);
            bojaTextaOpciono = (Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString());

        }

        //=======================================================================
        private bool validate()
        {
            bool result = true;

            //poluprecnik1
            bool isDouble = Double.TryParse(TextBoxPoluprecnik1.Text, out poluprecnik1);
            if (isDouble)
            {
                labelGreskaPoluprecnik1.Content = "";
                labelGreskaPoluprecnik1.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaPoluprecnik1.Content = "Unesite double vrednost!";
                labelGreskaPoluprecnik1.BorderBrush = Brushes.Red;
                result = false;
            }

            //poluprecnik2
            bool isDouble2 = Double.TryParse(TextBoxPoluprecnik2.Text, out poluprecnik2);

            if (isDouble)
            {
                labelGreskaPoluprecnik2.Content = "";
                labelGreskaPoluprecnik2.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaPoluprecnik2.Content = "Unesite double vrednost!";
                labelGreskaPoluprecnik2.BorderBrush = Brushes.Red;
                result = false;
            }

            //debljina konturne linije
            bool isDoubleKon = Double.TryParse(TextBoxKonturnaLinija.Text, out konturnaLinija);
            if (isDoubleKon)
            {
                labelGreskaBorderThickness.Content = "";
                labelGreskaBorderThickness.BorderBrush = Brushes.Green;
            }
            else
            {
                labelGreskaBorderThickness.Content = "Unesite double vrednost!";
                labelGreskaBorderThickness.BorderBrush = Brushes.Red;
                result = false;
            }
            //boja konturne linije
            if (comboBoxBojaKonturneLinije.SelectedItem == null)
            {
                result = false;
                comboBoxBojaKonturneLinije.BorderBrush = Brushes.Red;
                labelGreskaBorderColor.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaKonturneLinije.BorderBrush = Brushes.Green;
                labelGreskaBorderColor.Content = string.Empty;
            }
            //boja unutrasnjosti
            if (comboBoxBojaUnutrasnjosti.SelectedItem == null)
            {
                result = false;
                comboBoxBojaUnutrasnjosti.BorderBrush = Brushes.Red;
                labelGreskaFillColor.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaUnutrasnjosti.BorderBrush = Brushes.Green;
                labelGreskaFillColor.Content = string.Empty;
            }
            return result;
        }
        private bool validateText()
        {
            bool result = true;

            //Text
            if (TextBoxTextOpciono.Text.Trim().Equals(""))
            {
                labelGreskaBojaTextaOpciono.Content = "Unesite text!";
                labelBojaTextaOpciono.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                labelGreskaBojaTextaOpciono.Content = string.Empty;
                labelGreskaBojaTextaOpciono.BorderBrush = Brushes.Green;
            }
            //boja Texta
            if (comboBoxBojaTextaOpciono.SelectedItem == null)
            {
                result = false;
                comboBoxBojaTextaOpciono.BorderBrush = Brushes.Red;
                labelGreskaBojaTextaOpciono.Content = "Odaberite boju!";
            }
            else
            {
                comboBoxBojaTextaOpciono.BorderBrush = Brushes.Green;
                labelGreskaBojaTextaOpciono.Content = string.Empty;
            }
            return result;
        }
    }
}
