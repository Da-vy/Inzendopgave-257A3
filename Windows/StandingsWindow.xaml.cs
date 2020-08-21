using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using Inzendopgave_257A3.Classes;

namespace Inzendopgave_257A3.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StandingsWindow : Window
    {
        public StandingsWindow()
        {
            InitializeComponent();
            StandingsXML();//Maak XML bestand aan als deze nog niet bestaat. 
            DataGrid1_Standings.ItemsSource = Team.GetStandings().DefaultView;//DataGrid1 vullen met Standings. 
        }

        //maak een XML file voor standings als deze nog niet bestaat.
        public static void StandingsXML()
        {
            if (!File.Exists("Standings.xml"))
            {
                XDocument standings = new XDocument(new XElement("Standings",
                                        new XElement("Team",
                                            new XElement("Naam", "Naam Team"),
                                            new XElement("Gespeeld", "Aantal wedstrijden gespeeld"),
                                            new XElement("Gewonnen", "Aantal wedstrijden gewonnen"),
                                            new XElement("Verloren", "Aantal wedstrijden verloren"),
                                            new XElement("Gelijkspel", "Aantal wedstrijden gelijk gespeeld"),
                                            new XElement("Dooelsaldo", "Doelsaldo"),
                                            new XElement("Punten", "Punten"))));

                standings.Save("Standings.xml");
            }
        }
       
    }
}
