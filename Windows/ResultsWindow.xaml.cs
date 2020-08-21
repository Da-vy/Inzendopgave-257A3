using Inzendopgave_257A3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inzendopgave_257A3.Windows
{
    public partial class ResultsWindow : Window
    {
        public ResultsWindow()
        {
            InitializeComponent();
            
            //Combobox1 en 2 vullen met Teams.
            ComboBox1_Thuis.ItemsSource = Team.GetNames();
            ComboBox2_Uit.ItemsSource = Team.GetNames();

            //Standaard combobox Text voor combobox 3 en 4
            //Wanneer er nog geen team geselecteerd is kan er ook geen speler geselecteer worden.
            List<string> defaultComboBoxText = new List<string>
            {
                "Selecteer eerst een Team"
            };
            ComboBox3_Thuis_Speler.ItemsSource = defaultComboBoxText;
            ComboBox4_Uit_Speler.ItemsSource = defaultComboBoxText;
                         
            //Tabel standings vullen met bestaande teams. 
            //Team wordt niet toegvoegd wanneer deze al voorkomt op de db (zie methode InsertStandings)
            foreach (string team in Team.GetNames())
            {
                Team.InsertStandings(team);
            }

            //Maak XML aan voor Rankings als deze nog niet bestaat.
            RankingsWindow.RankingsXML();

            //Maak XML aan voor Standings als deze nog niet bestaat.
            StandingsWindow.StandingsXML();

        }


        private void ComboBox1_Thuis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Kan niet hetzelfde team kiezen als in ComboBox2
            if (ComboBox2_Uit.SelectedItem != null && ComboBox1_Thuis.SelectedItem != null && ComboBox1_Thuis.SelectedItem.ToString() == ComboBox2_Uit.SelectedItem.ToString())
            {
                System.Windows.MessageBox.Show("Team is al gekozen.");
                ComboBox1_Thuis.SelectedItem = null;
            }
            //Combobox3 vullen met spelers uit geselecteerde team in Combobox1
            else if (ComboBox1_Thuis.SelectedItem != null)
            {
                ComboBox3_Thuis_Speler.ItemsSource = Player.GetPlayersFromTeam(Team.GetId(ComboBox1_Thuis.SelectedItem.ToString()));
            }                       
        }

        private void ComboBox2_Uit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Kan niet hetzelfde team kiezen als in ComboBox1
            if (ComboBox1_Thuis.SelectedItem != null && ComboBox2_Uit.SelectedItem != null && ComboBox2_Uit.SelectedItem.ToString() == ComboBox1_Thuis.SelectedItem.ToString())
            {
                System.Windows.MessageBox.Show("Team is al gekozen.");
                ComboBox2_Uit.SelectedItem = null;
            }
            //Combobox4 vullen met spelers uit geselecteerde team in Combobox2
            else if (ComboBox2_Uit.SelectedItem != null)
            {
                ComboBox4_Uit_Speler.ItemsSource = Player.GetPlayersFromTeam(Team.GetId(ComboBox2_Uit.SelectedItem.ToString()));
            }            
        }

        //Methode om Listbox Thuiskant en thuisscore refreshen
        private void RefreshThuis()
        {
            //Refresh ListBox1
            ListBox1_Thuis.ItemsSource = null;
            ListBox1_Thuis.ItemsSource = thuisList;

            //Refresh ThuisScore
            TextBlock1_Thuis.Text = thuisList.Sum(t => t.Item2).ToString();
        }

        //Een Tuple List creëren voor de Thuiskant om scores per persoon bij te houden.
        List<Tuple<string, int>> thuisList = new List<Tuple<string, int>>();

        //Met klikken op add wordt de geselecteerde speler met het aantal gescorode punten toegevoegd aan list thuisList.
        private void Button1_Add_Thuis_Click(object sender, RoutedEventArgs e)
        {   
            if (ComboBox3_Thuis_Speler.SelectedItem != null && ComboBox5_Score_Thuis.SelectedItem != null)
            {
                thuisList.Add(new Tuple<string, int>(ComboBox3_Thuis_Speler.SelectedItem.ToString(), int.Parse(ComboBox5_Score_Thuis.SelectionBoxItem.ToString())));

                RefreshThuis();
            }
        }

        //Methode om Listbox uitkant en uitscore refreshen
        private void RefreshUit()
        {
            //Refresh ListBox2
            ListBox2_Uit.ItemsSource = null;
            ListBox2_Uit.ItemsSource = uitList;

            //Refresh UitScore
            TextBlock2_Uit.Text = uitList.Sum(t => t.Item2).ToString();
        }

        //Een Tuple List creëren voor de Thuiskant om scores per persoon bij te houden.
        List<Tuple<string, int>> uitList = new List<Tuple<string, int>>();

        //Met klikken op add wordt de geselecteerde speler met het aantal gescorode punten toegevoegd aan list uitList.
        private void Button2_Add_Uit_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox4_Uit_Speler.SelectedItem != null && ComboBox6_Score_Uit.SelectedItem != null)
            {
                uitList.Add(new Tuple<string, int>(ComboBox4_Uit_Speler.SelectedItem.ToString(), int.Parse(ComboBox6_Score_Uit.SelectionBoxItem.ToString())));

                RefreshUit();
            }
        }

        //Maakt de laatste toevoeging aan Thuislist ongedaan.
        private void Button3_Undo_Thuis_Click(object sender, RoutedEventArgs e)
        {
            if (thuisList.Any()) //prevent IndexOutOfRangeException for empty list
            {
                thuisList.RemoveAt(thuisList.Count - 1);
                RefreshThuis();
            }            
        }

        //Maakt de laatste toevoeging aan uitlist ongedaan.
        private void Button4_Undo_Uit_Click(object sender, RoutedEventArgs e)
        {
            if (uitList.Any()) //prevent IndexOutOfRangeException for empty list
            {
                uitList.RemoveAt(uitList.Count - 1);
                RefreshUit();
            }                        
        }

        //Wanneer de wedstrijd afgelopen is wordt er op Game Finished geklikt om de resulstaten weg te schrijven naar de database. 
        private void Button5_Game_Finished_Click(object sender, RoutedEventArgs e)
        {
            //Game finished. 

            //Alleen uitvoeren wanneer er teams gekozen zijn! 
            if (ComboBox1_Thuis.SelectedItem != null && ComboBox2_Uit.SelectedItem != null)
            {
                //Eindscore naar tabel Results.
                //ComboBox1_Thuis.SelectedItem.ToString(); = Thuis Team
                //thuisList.Sum(t => t.Item2) = Thuis Score
                //ComboBox2_Uit.SelectedItem.ToString(); = Uit Team
                //uitList.Sum(t => t.Item2) = Uit Score
                Team.InsertResults(ComboBox1_Thuis.SelectedItem.ToString(), ComboBox2_Uit.SelectedItem.ToString(), thuisList.Sum(t => t.Item2), uitList.Sum(t => t.Item2));

                //Punten per teamlid naar tabel Rankings
                //Alleen uitvoeren wanneer score hoger dan 0 is. 
                if (thuisList.Sum(t => t.Item2) > 0)
                {
                    foreach (Tuple<string, int> tuple in thuisList)
                    {
                        Player.InsertRankings(tuple.Item1.ToString(), ComboBox1_Thuis.SelectedItem.ToString(), int.Parse(tuple.Item2.ToString()));
                    }
                }
                if (uitList.Sum(t => t.Item2) > 0)
                {
                    foreach (Tuple<string, int> tuple in uitList)
                    {
                        Player.InsertRankings(tuple.Item1.ToString(), ComboBox2_Uit.SelectedItem.ToString(), int.Parse(tuple.Item2.ToString()));
                    }
                }
                
                //Update Standings  
                //Updaten na elke keer dat de results geüpdatet zijn.  
                foreach (string team in Team.GetNames())
                {
                    Team.UpdateStandings(team);
                }

                //Sluit ResultsWindow nadat de updates uitgevoerd zijn.
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }));
                thread.Start();
                thread.Join();
            }
                  
        }

        //En messagebox met wat help text. 
        private void Button6_Help_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Selecteer als eerst een Thuisteam en een Uitteam\n" +
                                           "\n" +
                                           "Als er gescoord is kies je de speler die heeft gescoord en het aantal punten.\n" +
                                           "Om deze punten toe te voegen klik je op add, onder het aantal punten.\n" +
                                           "\n" +
                                           "Als een score ongedaan gemaakt moet worden klik je op undo, onder het venster waar alle gescoorde punten staan.\n" +
                                           "Als de wedstrijd is afgelopen klik je op Finished! Het overzicht Standings en Rankings wordt hiermee geüpdatet!");
        }
    }
}
