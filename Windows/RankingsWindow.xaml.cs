using Inzendopgave_257A3.Classes;
using System;
using System.Collections.Generic;
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

namespace Inzendopgave_257A3.Windows
{
    public partial class RankingsWindow : Window
    {
        public RankingsWindow()
        {
            InitializeComponent();
            RankingsXML();//Maak XML bestand aan als deze nog niet bestaat. 
            DataGrid1_Rankings.ItemsSource = Player.GetRankings().DefaultView;//DataGrid1 vullen met Rankings. 
        }

        //maak een XML file voor individual results als deze nog niet bestaat.
        public static void RankingsXML()
        {
            if (!File.Exists("Rankings.xml"))
            {
                XDocument standings = new XDocument(new XElement("Rankings",
                                        new XElement("Speler",
                                            new XElement("Naam", "Naam Speler"),
                                            new XElement("Team", "Naam Team"),
                                            new XElement("Punten", "Totaal aantal gescoorde punten"))));

                standings.Save("Rankings.xml");
            }
        }
    }
}
