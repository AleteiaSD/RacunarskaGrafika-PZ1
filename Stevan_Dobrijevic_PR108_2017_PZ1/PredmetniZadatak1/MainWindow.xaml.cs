using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using PredmetniZadatak1.Models;
using PredmetniZadatak1.Oblici;

namespace PredmetniZadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region PRVI DEO
        #region GLAVNE LISTE I PROMENLJIVE
        private List<SubstationEntity> substations = new List<SubstationEntity>();
        private List<NodeEntity> nodes = new List<NodeEntity>();
        private List<SwitchEntity> switches = new List<SwitchEntity>();
        private List<LineEntity> lines = new List<LineEntity>();
        public List<SubstationEntity> listSubstations { get => substations; set => substations = value; }
        public List<NodeEntity> listNodes { get => nodes; set => nodes = value; }
        public List<SwitchEntity> listSwitches { get => switches; set => switches = value; }
        public List<LineEntity> listLines { get => lines; set => lines = value; }



        public List<Ellipse> listaElipsi = new List<Ellipse>();


        public static readonly int velicina = 500;
        public TackaNaGridu[,] velicinaGrida = new TackaNaGridu[velicina, velicina];
        public int[,] poseceneTacke = new int[velicina, velicina];    //na gridu
        public Dictionary<long, Tuple<int, int>> cvor = new Dictionary<long, Tuple<int, int>>();


        public List<LinijaNaGridu> SveLinije = new List<LinijaNaGridu>();
        #endregion

        #region POMOCNE PROMENLJIVE
        public double X, Y, minX, maxX, minY, maxY;
        public bool pritisnutoDugmeLoad = false;
        #endregion

        #region DESNI KLIK PROMENLJIVE
        public string prethodnoPocetnoIme = String.Empty, prethodnoKrajnjeIme = String.Empty;
        public Brush prethodnaBojaPocetka, prethodnaBojaKraja;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < velicina; i++) // inicijalizacija grida --> VELICINA
            {
                for (int j = 0; j < velicina; j++)
                {
                    velicinaGrida[i, j] = new TackaNaGridu();
                }
            }
            LoadGeographicXml();
        }
        private void LoadGeographicXml()
        {
            if (pritisnutoDugmeLoad == false)
            {
                pritisnutoDugmeLoad = true;

                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList nodeList;
                xmlDoc.Load("Geographic.xml");
                #region substation 
                //iscitavamo iz xml-a  
                nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");//iscitace konkretne elemente xml-a
                foreach (XmlNode node in nodeList)
                {
                    ToLatLon(double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), 34, out double newX, out double newY);
                    listSubstations.Add(new SubstationEntity()
                    {//nisam pravio get set za ovo pa moram ovako
                        Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                        Name = node.SelectSingleNode("Name").InnerText,
                        X = newX,
                        Y = newY
                    });
                }
                #endregion
                #region nodes
                nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
                foreach (XmlNode node in nodeList)
                {
                    ToLatLon(double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), 34, out double newX, out double newY);
                    listNodes.Add(new NodeEntity()
                    {
                        Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                        Name = node.SelectSingleNode("Name").InnerText,
                        X = newX,
                        Y = newY
                    });
                }
                #endregion
                #region switches
                nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
                foreach (XmlNode node in nodeList)
                {
                    ToLatLon(double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), 34, out double newX, out double newY);
                    listSwitches.Add(new SwitchEntity()
                    {
                        Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                        Name = node.SelectSingleNode("Name").InnerText,
                        Status = node.SelectSingleNode("Status").InnerText,
                        X = newX,
                        Y = newY
                    });
                }
                #endregion
                #region lines
                nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
                foreach (XmlNode node in nodeList)
                {
                    LineEntity l = new LineEntity();

                    l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                    l.Name = node.SelectSingleNode("Name").InnerText;
                    if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true")) { l.IsUnderground = true; } else { l.IsUnderground = false; }
                    l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                    l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                    l.LineType = node.SelectSingleNode("LineType").InnerText;
                    l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                    l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                    l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                    bool istaLinija = lines.Any(line => (l.FirstEnd == line.SecondEnd && l.SecondEnd == line.FirstEnd) || (l.FirstEnd == line.FirstEnd && l.FirstEnd == line.SecondEnd));
                    if (!istaLinija)
                    {
                        lines.Add(l);
                    }
                }
                #endregion
                #region koordinate
                minX = Math.Min(Math.Min(listSubstations.Min((item) => item.X), listNodes.Min((item) => item.X)), listSwitches.Min((item) => item.X)) - 0.01;
                maxX = Math.Max(Math.Max(listSubstations.Max((item) => item.X), listNodes.Max((item) => item.X)), listSwitches.Max((item) => item.X)) + 0.01;
                X = (maxX - minX) / velicina;

                minY = Math.Min(Math.Min(listSubstations.Min((item) => item.Y), listNodes.Min((item) => item.Y)), listSwitches.Min((item) => item.Y)) - 0.01;
                maxY = Math.Max(Math.Max(listSubstations.Max((item) => item.Y), listNodes.Max((item) => item.Y)), listSwitches.Max((item) => item.Y)) + 0.01;
                Y = (maxY - minY) / velicina;
                #endregion
                #region draw
                Draw();
                #endregion
                #region crtaj linije
                DrawLine();
                #endregion
            }
        }
        #region DRAW
        private void Draw()
        {
            DrawSubstations();
            DrawNodes();
            DrawSwitches();
        }
        private void DrawSubstations()
        {
            foreach (var node in listSubstations)
            {//izracunaj
                double tmpX = maxX - node.X;
                double tmpY = maxY - node.Y;
                //ovde dobijamo koordinate gde bi trebalo da stavimo
                int gridX = (int)(tmpX / X);
                int gridY = (int)(tmpY / Y);
                if (!velicinaGrida[gridX, gridY].SadrziElement)
                { //ukoliko ne sadrzi element, stavimo elipsu u njega sa svim informacijama o substation
                    velicinaGrida[gridX, gridY].SadrziElement = true;
                    #region elipsa
                    Ellipse elipsa = new Ellipse()
                    {
                        Fill = Brushes.Green,
                        Stroke = Brushes.Green,
                        Width = 2,
                        Height = 2,
                        Name = String.Format("node_" + node.Id.ToString()),
                        ToolTip = String.Format("ID: {0} \nName: {1}", node.Id, node.Name)
                    };


                    //postavi liniju na sredinu
                    Canvas.SetTop(elipsa, gridX * 2 - 1); //oduzmemo 1 zato sto su dimenzije elipse 2*2 -->>>> gridX * 2 - 1 pozicija na canvasu
                    Canvas.SetLeft(elipsa, gridY * 2 - 1);
                    #endregion
                    listaElipsi.Add(elipsa);
                    cvor.Add(node.Id, new Tuple<int, int>(gridX, gridY));
                }
                else
                {   //ako je zauzeta pozicija na x i y
                    //
                    int brojac = 1;
                    while (true)
                    {
                        if (gridX - brojac >= 0 && !velicinaGrida[gridX - brojac, gridY].SadrziElement) { gridX -= brojac; break; }
                        else if (gridX + brojac <= 500 && !velicinaGrida[gridX + brojac, gridY].SadrziElement) { gridX += brojac; break; }
                        else if (gridY - brojac >= 0 && !velicinaGrida[gridX, gridY - brojac].SadrziElement) { gridY -= brojac; break; }
                        else if (gridY + brojac <= 500 && !velicinaGrida[gridX, gridY + brojac].SadrziElement) { gridY += brojac; break; }
                        else { brojac++; }
                    }
                    velicinaGrida[gridX, gridY].SadrziElement = true;
                    #region elipsa
                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = Brushes.Green,
                        Stroke = Brushes.Green,
                        Width = 2,
                        Height = 2,
                        Name = String.Format("node_" + node.Id.ToString()),
                        ToolTip = String.Format("ID: {0} \nName: {1}", node.Id, node.Name)
                    };
                    Canvas.SetTop(ellipse, gridX * 2 - 1);
                    Canvas.SetLeft(ellipse, gridY * 2 - 1);
                    #endregion
                    listaElipsi.Add(ellipse);
                    cvor.Add(node.Id, new Tuple<int, int>(gridX, gridY)); //id i koordinate na canvasu
                }

            }
        }
        private void DrawNodes()
        {
            foreach (var node in listNodes)
            {//izracunaj
                double tmpX = maxX - node.X;
                double tmpY = maxY - node.Y;
                int gridX = (int)(tmpX / X);
                int gridY = (int)(tmpY / Y);
                if (!velicinaGrida[gridX, gridY].SadrziElement)
                {
                    velicinaGrida[gridX, gridY].SadrziElement = true;
                    #region elipsa
                    Ellipse elipsa = new Ellipse()
                    {
                        Fill = Brushes.Red,
                        Stroke = Brushes.Red,
                        Width = 2,
                        Height = 2,
                        Name = String.Format("node_" + node.Id.ToString()),
                        ToolTip = String.Format("ID: {0} \nName: {1}", node.Id, node.Name)
                    };


                    //postavi liniju na sredinu
                    Canvas.SetTop(elipsa, gridX * 2 - 1);
                    Canvas.SetLeft(elipsa, gridY * 2 - 1);
                    #endregion
                    listaElipsi.Add(elipsa);
                    cvor.Add(node.Id, new Tuple<int, int>(gridX, gridY));
                }
                else
                {
                    int brojac = 1;
                    while (true)
                    {
                        if (gridX - brojac >= 0 && !velicinaGrida[gridX - brojac, gridY].SadrziElement) { gridX -= brojac; break; }
                        else if (gridX + brojac <= 500 && !velicinaGrida[gridX + brojac, gridY].SadrziElement) { gridX += brojac; break; }
                        else if (gridY - brojac >= 0 && !velicinaGrida[gridX, gridY - brojac].SadrziElement) { gridY -= brojac; break; }
                        else if (gridY + brojac <= 500 && !velicinaGrida[gridX, gridY + brojac].SadrziElement) { gridY += brojac; break; }
                        else { brojac++; }
                    }
                    velicinaGrida[gridX, gridY].SadrziElement = true;
                    #region elipsa
                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = Brushes.Red,
                        Stroke = Brushes.Red,
                        Width = 2,
                        Height = 2,
                        Name = String.Format("node_" + node.Id.ToString()),
                        ToolTip = String.Format("ID: {0} \nName: {1}", node.Id, node.Name)
                    };
                    Canvas.SetTop(ellipse, gridX * 2 - 1);
                    Canvas.SetLeft(ellipse, gridY * 2 - 1);
                    #endregion
                    listaElipsi.Add(ellipse);
                    cvor.Add(node.Id, new Tuple<int, int>(gridX, gridY));
                }
            }
        }
        private void DrawSwitches()
        {
            foreach (var node in listSwitches)
            {//izracunaj
                double tmpX = maxX - node.X;
                double tmpY = maxY - node.Y;
                int gridX = (int)(tmpX / X);
                int gridY = (int)(tmpY / Y);
                if (!velicinaGrida[gridX, gridY].SadrziElement)
                {
                    velicinaGrida[gridX, gridY].SadrziElement = true;
                    #region elipsa
                    Ellipse elipsa = new Ellipse()
                    {
                        Fill = Brushes.Blue,
                        Stroke = Brushes.Blue,
                        Width = 2,
                        Height = 2,
                        Name = String.Format("node_" + node.Id.ToString()),
                        ToolTip = String.Format("ID: {0} \nName: {1}", node.Id, node.Name)
                    };


                    //postavi liniju na sredinu
                    Canvas.SetTop(elipsa, gridX * 2 - 1);
                    Canvas.SetLeft(elipsa, gridY * 2 - 1);
                    #endregion
                    listaElipsi.Add(elipsa);
                    cvor.Add(node.Id, new Tuple<int, int>(gridX, gridY));
                }
                else
                {
                    int brojac = 1;
                    while (true)
                    {
                        if (gridX - brojac >= 0 && !velicinaGrida[gridX - brojac, gridY].SadrziElement) { gridX -= brojac; break; }
                        else if (gridX + brojac <= 500 && !velicinaGrida[gridX + brojac, gridY].SadrziElement) { gridX += brojac; break; }
                        else if (gridY - brojac >= 0 && !velicinaGrida[gridX, gridY - brojac].SadrziElement) { gridY -= brojac; break; }
                        else if (gridY + brojac <= 500 && !velicinaGrida[gridX, gridY + brojac].SadrziElement) { gridY += brojac; break; }
                        else { brojac++; }
                    }
                    velicinaGrida[gridX, gridY].SadrziElement = true;
                    #region elipsa
                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = Brushes.Blue,
                        Stroke = Brushes.Blue,
                        Width = 2,
                        Height = 2,
                        Name = String.Format("node_" + node.Id.ToString()),
                        ToolTip = String.Format("ID: {0} \nName: {1}", node.Id, node.Name)
                    };
                    Canvas.SetTop(ellipse, gridX * 2 - 1);
                    Canvas.SetLeft(ellipse, gridY * 2 - 1);
                    #endregion
                    listaElipsi.Add(ellipse);
                    cvor.Add(node.Id, new Tuple<int, int>(gridX, gridY));
                }
            }
        }
        #endregion
        #region DRAW LINE
        private void DrawLine()
        {
            foreach (LineEntity lineEntity in listLines)
            {
                //da li u recniku ne postoji lineentity prvi ili drugi onda nastavim dalje u foreach
                if (!cvor.ContainsKey(lineEntity.FirstEnd) || !cvor.ContainsKey(lineEntity.SecondEnd))
                {
                    continue;
                }
                PozicijaUMatrici pocetnaPozicija = new PozicijaUMatrici(){//red,kolona i roditelj
                    Red = cvor[lineEntity.FirstEnd].Item1,
                    Kolona = cvor[lineEntity.FirstEnd].Item2,
                    Roditelj = null
                };
                long krajnjiID = lineEntity.SecondEnd;
                long pocetniID = lineEntity.FirstEnd;
                //pocetna pozicija za iscrtavanje
                pocetnaPozicija = BreadthFirstSearch(pocetnaPozicija, krajnjiID);//ovde mi se trenutno nalazi krajna pozicija


                while (pocetnaPozicija != null)
                {
                    if (pocetnaPozicija.Roditelj == null) //znaci da sam na 1. cvoru
                        break;
                    //koordinate za 1. objekat
                    int x1 = pocetnaPozicija.Kolona * 2; 
                    int y1 = pocetnaPozicija.Red * 2;
                    //koordinate za 2. objekat
                    int x2 = pocetnaPozicija.Roditelj.Kolona * 2;
                    int y2 = pocetnaPozicija.Roditelj.Red * 2;
                    //da li postoji linija
                    bool postojiLinija = false;
                    foreach (LinijaNaGridu linijaNaGridu in SveLinije) //proveravam da li postoji vec nacrtana linija, iskoci ako postoji
                    {
                        if (linijaNaGridu.X1 == x1 && linijaNaGridu.Y1 == y1 && linijaNaGridu.X2 == x2 && linijaNaGridu.Y2 == y2) { postojiLinija = true; break; }
                        if (linijaNaGridu.X1 == x2 && linijaNaGridu.Y1 == y2 && linijaNaGridu.X2 == x1 && linijaNaGridu.Y2 == y1) { postojiLinija = true; break; }
                    }
                    if (!postojiLinija)//ako ne postoji
                    {
                        Line novaLinija = new Line()//nacrtaj liniju izmedju datih koordinata
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            ToolTip = String.Format("ID: {0} \nName: {1}", lineEntity.Id, lineEntity.Name),
                            X1 = x1,
                            X2 = x2,
                            Y1 = y1,
                            Y2 = y2,
                        };


                        //Desnim klikom na vod između dva entiteta treba ponuditi opciju da entiteti povezani tim vodom
                        //budu obojeni različitim bojama od ostalih kako bi korisnik znao koji su entiteti povezani tim vodom

                        novaLinija.MouseRightButtonDown += new MouseButtonEventHandler(Canvas_MouseRightButtonDown);
                        SveLinije.Add(new LinijaNaGridu(novaLinija.X1, novaLinija.Y1, novaLinija.X2, novaLinija.Y2, pocetniID, krajnjiID));
                        Canvas.Children.Add(novaLinija);
                        x1 = pocetnaPozicija.Kolona;
                        x2 = pocetnaPozicija.Roditelj.Kolona;
                        y1 = pocetnaPozicija.Red;
                        y2 = pocetnaPozicija.Roditelj.Red;

                        //uslovi da li se presecaju 2 linije
                        // napravi elipsu na preseku linija da izgleda kao cvor
                        if (!velicinaGrida[y1, x1].SadrziElement && !velicinaGrida[y1, x1].Posecen) { velicinaGrida[y1, x1].Posecen = true; velicinaGrida[y1, x1].PosecenId.Add(lineEntity.Id); }
                        else if (!velicinaGrida[y1, x1].SadrziElement && velicinaGrida[y1, x1].Posecen && !velicinaGrida[y1, x1].PosecenId.Contains(lineEntity.Id)) { NapraviOblik(lineEntity.Id, x1, y1); }

                        if (!velicinaGrida[y2, x2].SadrziElement && !velicinaGrida[y2, x2].Posecen) { velicinaGrida[y2, x2].Posecen = true; velicinaGrida[y2, x2].PosecenId.Add(lineEntity.Id); }
                        else if (!velicinaGrida[y2, x2].SadrziElement && velicinaGrida[y2, x2].Posecen && !velicinaGrida[y2, x2].PosecenId.Contains(lineEntity.Id)) { NapraviOblik(lineEntity.Id, x2, y2); }
                    }
                    pocetnaPozicija = pocetnaPozicija.Roditelj;//stavljam roditelja od end node na pocetnu poziciju
                }

            }
            foreach (Ellipse elipsa in listaElipsi) //za svaku elipsu koju sam dodavao u metodama za DRAW 
            {
                Canvas.Children.Add(elipsa); //dodaj je na Canvas, jer nisu prethodno dodate
                                             //  vec su u tim metodama samo punjene liste
            }
        }
        public void NapraviOblik(long lineId, int x, int y)
        {
            #region elipsa
            Ellipse ellipse = new Ellipse
            {
                ToolTip = String.Format("Cvor"),
                Fill = Brushes.Black,
                Width = 2,
                Height = 2
            };
            Canvas.SetTop(ellipse, y * 2 - 1);
            Canvas.SetLeft(ellipse, x * 2 - 1);
            #endregion
        
            Canvas.Children.Add(ellipse);
            velicinaGrida[y, x].PosecenId.Add(lineId);
        }

        private PozicijaUMatrici BreadthFirstSearch(PozicijaUMatrici pocetnaPozicija, long idKrajnjePozicije)
        {
            List<PozicijaUMatrici> pozicija = new List<PozicijaUMatrici>();
            pozicija.Add(pocetnaPozicija);
            while (true)
            {
                foreach (var poz in pozicija)
                { //cvor je svaki element
                    if (cvor[idKrajnjePozicije].Item1 == poz.Red && cvor[idKrajnjePozicije].Item2 == poz.Kolona) //da li postoji putanja od cvora a do cvora b
                    {
                        for (int i = 0; i < velicina; i++) 
                        {
                            for (int j = 0; j < velicina; j++)
                            {               //x,y
                                poseceneTacke[i, j] = 0; //matrica dimenzija 500 * 500 sa nulama na svim pozicijama
                                                         //kako bih znao na kojoj poziciji sam bio stavljajuci 1 na tu poziciju
                            }
                        }
                        return poz; //ovde vraca krajnju poziciju 
                    }
                }
                List<PozicijaUMatrici> listaPozicijaUMatrici = new List<PozicijaUMatrici>();
                foreach (var poz in pozicija)
                { //ispitamo sva ogranicenja
                    if (poz.Red + 1 >= 0 && poz.Kolona >= 0 && poz.Red + 1 < velicina && poz.Kolona < velicina && poseceneTacke[poz.Red + 1, poz.Kolona] == 0)
                    {
                        poseceneTacke[poz.Red + 1, poz.Kolona] = 1; //stavljam 1 na mesto 0
                        listaPozicijaUMatrici.Add(new PozicijaUMatrici() { Red = poz.Red + 1, Kolona = poz.Kolona, Roditelj = poz }); //dodam mu bas to sto sam ispitao i stavim da je roditelj cvor iz pozicije
                    }
                    if (poz.Red - 1 >= 0 && poz.Kolona >= 0 && poz.Red - 1 < velicina && poz.Kolona < velicina && poseceneTacke[poz.Red - 1, poz.Kolona] == 0)
                    {
                        poseceneTacke[poz.Red - 1, poz.Kolona] = 1;
                        listaPozicijaUMatrici.Add(new PozicijaUMatrici() { Red = poz.Red - 1, Kolona = poz.Kolona, Roditelj = poz });
                    }
                    if (poz.Red >= 0 && poz.Kolona + 1 >= 0 && poz.Red < velicina && poz.Kolona + 1 < velicina && poseceneTacke[poz.Red, poz.Kolona + 1] == 0)
                    {
                        poseceneTacke[poz.Red, poz.Kolona + 1] = 1;
                        listaPozicijaUMatrici.Add(new PozicijaUMatrici() { Red = poz.Red, Kolona = poz.Kolona + 1, Roditelj = poz });
                    }
                    if (poz.Red >= 0 && poz.Kolona - 1 >= 0 && poz.Red < velicina && poz.Kolona - 1 < velicina && poseceneTacke[poz.Red, poz.Kolona - 1] == 0)
                    {
                        poseceneTacke[poz.Red, poz.Kolona - 1] = 1;
                        listaPozicijaUMatrici.Add(new PozicijaUMatrici() { Red = poz.Red, Kolona = poz.Kolona - 1, Roditelj = poz });
                    }
                }
                pozicija = new List<PozicijaUMatrici>(listaPozicijaUMatrici);//ova lista je puna sad
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!prethodnoPocetnoIme.Equals("") && !prethodnoKrajnjeIme.Equals(""))
            {
                #region elipsa
                foreach (var elipsa in listaElipsi)
                {
                    if (prethodnoPocetnoIme.Equals(elipsa.Name))
                    {
                        elipsa.Fill = prethodnaBojaPocetka;
                        elipsa.Stroke = prethodnaBojaPocetka;
                        elipsa.Width = 2;
                        elipsa.Height = 2;
                    }
                    else if (prethodnoKrajnjeIme.Equals(elipsa.Name))
                    {
                        elipsa.Fill = prethodnaBojaKraja;
                        elipsa.Stroke = prethodnaBojaKraja;
                        elipsa.Width = 2;
                        elipsa.Height = 2;
                    }
                }
                #endregion
            }
            Line linija = (Line)sender;

            string pocetnoImeCvora = String.Empty, krajnjeImeCvora = String.Empty;

            foreach (var linije in SveLinije)
            {
                if (linije.X1 == linija.X1 && linije.X2 == linija.X2 && linije.Y1 == linija.Y1 && linije.Y2 == linija.Y2)
                {
                    pocetnoImeCvora = String.Format("node_" + linije.PocetniCvor.ToString());
                    krajnjeImeCvora = String.Format("node_" + linije.KrajnjiCvor.ToString());
                    break;
                }
            }

            foreach (var elipsa in listaElipsi)
            {
                if (elipsa.Name.Equals(pocetnoImeCvora))
                {
                    prethodnoPocetnoIme = pocetnoImeCvora;
                    prethodnaBojaPocetka = elipsa.Fill;
                    elipsa.Fill = Brushes.Yellow;
                    elipsa.Stroke = Brushes.Yellow;
                    elipsa.Width = 3;
                    elipsa.Height = 3;
                }
                else if (elipsa.Name.Equals(krajnjeImeCvora))
                {
                    prethodnoKrajnjeIme = krajnjeImeCvora;
                    prethodnaBojaKraja = elipsa.Fill;
                    elipsa.Fill = Brushes.Yellow;
                    elipsa.Stroke = Brushes.Yellow;
                    elipsa.Width = 3;
                    elipsa.Height = 3;
                }
            }
        }
        private static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
        #endregion
        #endregion
//========================================================================================================================
        #region DRUGI DEO OBLICI
        private bool elipsaBool = false;
        private bool textBool = false;
        private bool polygonBool = false;
        public bool undoBool = true;
        private bool redoBool = false;
        private int poslednjaAkcija;
        public UIElement canvasElementi;
        public List<UIElement> listaClear = new List<UIElement>();
        public List<UIElement> listaDodatihObjekata = new List<UIElement>();
        private List<System.Windows.Point> koordinate = new List<System.Windows.Point>();

        public int tretnutni = 0;
        public string trenutniOblik = "";

        #region AKCIJE
        //svaki click sadrzi bool na osnovu kog znamo da li je pritisnuto odredjeno dugme i onda radimo tu akciju
        private void Ellipse_Click(object sender, RoutedEventArgs e)
        {
            elipsaBool = true;
        }
        private void Polygon_Click(object sender, RoutedEventArgs e)
        {
            polygonBool = true;
        }
        private void Text_Click(object sender, RoutedEventArgs e)
        {
            textBool = true;
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {            
            if (poslednjaAkcija == 6 )
            {
                foreach (UIElement ui in listaClear)
                {
                    Canvas.Children.Add(ui);
                    listaDodatihObjekata.Add(ui);
                }
                listaClear.Clear();
                undoBool = false;
                poslednjaAkcija = -1;
            }
            else if(undoBool==true)
            {
                canvasElementi = Canvas.Children[Canvas.Children.Count - 1];
                Canvas.Children.RemoveAt(Canvas.Children.Count - 1);
                listaDodatihObjekata.Remove(canvasElementi);//ako sam uradio undo obrisem ga iz liste da mi posle clear ne bi vracalo sve elemente iz liste
                redoBool = true;
                undoBool = false;
            }
        }
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (redoBool == true)
            {
                Canvas.Children.Add(canvasElementi);
                listaDodatihObjekata.Add(canvasElementi);//vratim element iz undo u listu
                redoBool = false;
                undoBool = true; //sad moze da radi undo posle redo

            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            
            if (listaDodatihObjekata.Count > 0)  //ako u listi ima nesto mozes da odradis clear
            {       
                foreach (UIElement canvasOblici in listaDodatihObjekata)
                {
                    listaClear.Add(canvasOblici);//tu dodajem sve sto sam obrisao
                }
                for (int j = 0; j < listaDodatihObjekata.Count; j++)
                {
                    Canvas.Children.Remove(listaDodatihObjekata[j]);
                }
                listaDodatihObjekata.Clear();//obrisem celu listu jer sam obrisao sa canvasa objekte
                undoBool = true; //posle clear treba da bude funkcionalan undo
                poslednjaAkcija = 6; //6 akcija za clear
                redoBool = false; //ne moze da se radi redo posle clear
            }
        }
        #endregion
        #region AKCIJE MISOM
        private void ObliciUCanvasu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (polygonBool == true)
            {
                if (e.ButtonState == MouseButtonState.Pressed && koordinate.Count > 2)
                {
                    polygonBool = false;

                    PolygonDraw pd = new PolygonDraw();
                    foreach (System.Windows.Point p in koordinate)
                    {
                        pd.koordinatePoligona.Add(p);
                    }
                    koordinate.Clear();//OBRISEM KOORDINATE STAROG POLYGON-A, DA BI MOGAO NOVI DA KREIRAM
                    pd.Show();
                }
            }
            else if (elipsaBool == false  && textBool == false && polygonBool == false  && Canvas.Children.Count > 0)
            {

                if (e.OriginalSource is Ellipse)
                {
                    Ellipse el = (Ellipse)e.OriginalSource;
                    for (int i = 0; i < Canvas.Children.Count; i++)
                    {
                        if (Canvas.Children[i] == el) tretnutni = i;
                    }
                    trenutniOblik = "ELIPSA";
                    IzmeniObjekat io = new IzmeniObjekat();
                    io.Show();
                }
                else if (e.OriginalSource is Polygon)
                {
                    Polygon pol = (Polygon)e.OriginalSource;
                    for (int i = 0; i < Canvas.Children.Count; i++)
                    {
                        if (Canvas.Children[i] == pol) tretnutni = i;
                    }
                    trenutniOblik = "POLYGON";
                    IzmeniObjekat io = new IzmeniObjekat();
                    io.Show();
                }
                else if (e.OriginalSource is TextBlock)
                {
                    TextBlock tb = (TextBlock)e.OriginalSource;
                    for (int i = 0; i < Canvas.Children.Count; i++)
                    {
                        if (Canvas.Children[i] == tb) tretnutni = i;
                    }
                    trenutniOblik = "TEXT";
                    IzmeniText it = new IzmeniText();
                    it.Show();
                }
            }
        }
        private void ObliciUCanvasu_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point currentPoint = Mouse.GetPosition(Canvas);
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (elipsaBool == true)
                {
                    EllipseDraw ellipseDraw = new EllipseDraw();
                    ellipseDraw.xosa = currentPoint.X;
                    ellipseDraw.yosa = currentPoint.Y;
                    ellipseDraw.Show();
                    elipsaBool = false;
                    //poslednjaAkcija = 1;
                }

                if (polygonBool == true) //kada je kliknut desni klik ulazi se u biranje tacaka
                {
                    koordinate.Add(new System.Windows.Point(currentPoint.X, currentPoint.Y));
                }

                if(textBool == true)
                {
                    AddTextDraw adt = new AddTextDraw();
                    adt.xosa = currentPoint.X;
                    adt.yosa = currentPoint.Y;
                    adt.Show();
                    textBool = false;
                    //poslednjaAkcija = 2;
                }
            }
        }
        #endregion

        #endregion
    }
}

