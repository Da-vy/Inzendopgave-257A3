using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Inzendopgave_257A3.Classes;
using System.Threading;

namespace Inzendopgave_257A3.Windows
{
    /// <summary>
    /// OPDRACHT:
    ///uitslagen onderhouden(CRUD (Create, Read, Update, Delete) van wedstrijden tussen de teams
    ///bijhouden wie het doelpunt of rondetijd e.d.heeft gemaakt
    ///het team veranderen (update) zonder dat de eerdere resultaten van een speler verdwijnen.

    ///Er moet een stand opgemaakt worden die in een ander venster na elke uitslag meteen wordt bijgewerkt.
    ///Op een derde venster worden de individuele resultaten getoond en ook dit venster moet automatisch bijgewerkt worden.
    ///Zorg voor een JSON- of XML-bestand dat na elke wijziging bijgewerkt wordt.Deze bestanden bevatten de laatste gegevens van punt 1 en punt 2.
    ///Maak gebruik van multithreading of asynchronous processing.
    /// </summary>
     
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //DataGrid vullen met Teams, Players en Coaches
            DataGrid1.ItemsSource = Team.GetEverything().DefaultView;
        }

        //Methode om TextBoxes leeg te maken.
        private void ClearTextBoxes()
        {
            TextBox1_Team.Text = "";
            TextBox2_Player.Text = "";
            TextBox3_Coach.Text = "";
        }

        //Methode om DataGrid1 te Verversen
        private void RefreshDataGrid1()
        {
            DataGrid1.ItemsSource = null;
            DataGrid1.ItemsSource = Team.GetEverything().DefaultView;
        }

        //Insert Button
        private void Button1_Click_Insert(object sender, RoutedEventArgs e)
        {
            try
            {   //Als er op insert gedrukt wordt terwijl de tekstvelden leeg zijn een melding geven.           
                if (string.IsNullOrWhiteSpace(TextBox1_Team.Text) &&
                    string.IsNullOrWhiteSpace(TextBox2_Player.Text) &&
                    string.IsNullOrWhiteSpace(TextBox3_Coach.Text))
                {
                    MessageBox.Show("Er zijn geen waarden ingevoerd!");
                }
                else
                {
                    //Bij Insert Team alleen een melding geven als het Team bestaat wanneer de andere 2 velden leeg zijn.
                    //Als de andere 2 velden gevuld zijn niet deze melding geven omdat spelers en coaches gekoppeld worden aan aan reeds bestaand team.
                    if (Team.Exists(TextBox1_Team.Text) == true && 
                        string.IsNullOrWhiteSpace(TextBox2_Player.Text) == true && 
                        string.IsNullOrWhiteSpace(TextBox3_Coach.Text) == true)
                    {
                        MessageBox.Show(TextBox1_Team.Text + " bestaat al!");
                    }
                    else
                    {
                        Team.Insert(TextBox1_Team.Text);
                        Player.Insert(TextBox2_Player.Text, TextBox1_Team.Text);
                        Coach.Insert(TextBox3_Coach.Text, TextBox1_Team.Text);
                        RefreshDataGrid1();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //update values
        private void Button2_Click_Update(object sender, RoutedEventArgs e)
        {
            //Bij bij selectie uit DataGrid wordt er al een variabele gevuld = Method DataGrid1_SelectedItem()
            //Deze variabele selecteren. Check doen of 1 van de 3 velden (of allemaal) afwijkt van de waarde in de database. 
            //vervolgens update uitvoeren.
            try
            {
                if (DataGrid1_SelectedItem() != null)
                {
                    string team = DataGrid1_SelectedItem()[0];
                    string player = DataGrid1_SelectedItem()[1];
                    string coach = DataGrid1_SelectedItem()[2];

                    //Alleen als team in TextBox1_Team niet bestaat het team updaten.
                    if (Team.Exists(TextBox1_Team.Text) == false)
                    {
                        Team.Update(team, TextBox1_Team.Text);
                    }

                    //Player updaten. Als het Team een bestaand Team is dan deze aanpassen bij de speler.
                    Player.Update(player, TextBox2_Player.Text, team, TextBox1_Team.Text);

                    //Coach updaten. Als het Team een bestaand Team is dan deze aanpassen bij de speler. 
                    Coach.Update(coach, TextBox3_Coach.Text, team, TextBox1_Team.Text);

                    RefreshDataGrid1();
                }
                else if (string.IsNullOrWhiteSpace(TextBox1_Team.Text) && string.IsNullOrWhiteSpace(TextBox2_Player.Text) &&
                    string.IsNullOrWhiteSpace(TextBox3_Coach.Text))
                {
                    MessageBox.Show("Geen waarden, kan niet updaten!");
                }
                else if (DataGrid1_SelectedItem() == null)
                {
                    MessageBox.Show("Selecteer eerst een waarde om te updaten!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Delete button
        private void Button3_Click_Delete(object sender, RoutedEventArgs e)
        {
            //delete selected item. 
            //Een Team kan niet worden verwijderd wanneer hier nog spelers of coaches aan hangen.
            //Werkt alleen op een enkel veld. De overige twee dienen leeg te zijn. 
            if (string.IsNullOrWhiteSpace(TextBox1_Team.Text) == false && string.IsNullOrWhiteSpace(TextBox2_Player.Text) == true && 
                string.IsNullOrWhiteSpace(TextBox3_Coach.Text) == true)
            {
                //delete Team
                Team.Delete(TextBox1_Team.Text);
                RefreshDataGrid1();
            }
            else if (string.IsNullOrWhiteSpace(TextBox1_Team.Text) == true && string.IsNullOrWhiteSpace(TextBox2_Player.Text) == false && 
                     string.IsNullOrWhiteSpace(TextBox3_Coach.Text) == true)
            {
                //delete Player
                Player.Delete(TextBox2_Player.Text);
                RefreshDataGrid1();
            }
            else if (string.IsNullOrWhiteSpace(TextBox1_Team.Text) == true && string.IsNullOrWhiteSpace(TextBox2_Player.Text) == true && 
                     string.IsNullOrWhiteSpace(TextBox3_Coach.Text) == false)
            {
                //delete Coach
                Coach.Delete(TextBox3_Coach.Text);
                RefreshDataGrid1();
            }
            else if (string.IsNullOrWhiteSpace(TextBox1_Team.Text) == true && string.IsNullOrWhiteSpace(TextBox2_Player.Text) == true && 
                     string.IsNullOrWhiteSpace(TextBox3_Coach.Text) == true)
            {
                MessageBox.Show("Er is niets om te verwijderen!");
            }
            else
            {
                MessageBox.Show("Kan maar 1 waarde per keer verwijderen!");
            }
            

        }

        //Clear Fields
        private void Button4_Click_Reset(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();

            //DataGrid1 selectie wordt null 
            //Zodat er opnieuw waardes worden weergegeven in de textboxes wanneer we hetzelfde item selecteren.
            DataGrid1.SelectedItem = null;

            RefreshDataGrid1();
        }

        //En messagebox met wat help text. 
        private void Button5_Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Om een nieuwe team, speler en/of coach aan te maken dien je de lege velden in te vullen en op insert te klikken.\n" +
                            "\n" +
                            "Om een bestaande waarde te updaten klik je de waarde aan in de tabel. Vervolgens kun je deze waarde aanpassen in de textvelden.\n" +
                            "De aanpassing wordt doorgevoerd door op update te klikken.\n" +
                            "\n" +
                            "Een specifieke waarde deleten doe je door deze waarde in te typen of te selecteren en op delete te drukken.\n" +
                            "Let op! Een Team kan niet worden verwijderd wanneer hier nog spelers of coaches aan hangen.");
        }

        //Window Standings openen
        private void Button6_Standings_Click(object sender, RoutedEventArgs e)
        {
            StandingsWindow standingsWindow = new StandingsWindow();
            standingsWindow.Show();
        }

        //Window Results openen
        private void Button7_Results_Click(object sender, RoutedEventArgs e)
        {
            ResultsWindow resultsWindow = new ResultsWindow();
            resultsWindow.Show();
        }

        //Window Rankings openen
        private void Button8_Rankings_Click(object sender, RoutedEventArgs e)
        {
            RankingsWindow rankingsWindow = new RankingsWindow();
            rankingsWindow.Show();
        }

        //List om huidige DataGrid1 selectie in op te slaan. 
        //Deze List gebruiken we om geselecteerde Items te kunnen updaten. 
        //Geeft een List met stringwaarden of null terug. 
        private List<string> DataGrid1_SelectedItem()
        {
            DataRowView DataGrid1Row = DataGrid1.SelectedItem as DataRowView;

            List<string> selectedItems = new List<string>();

            if (DataGrid1Row != null)
            {
                selectedItems.Add(DataGrid1Row.Row.ItemArray[0].ToString());
                selectedItems.Add(DataGrid1Row.Row.ItemArray[1].ToString());
                selectedItems.Add(DataGrid1Row.Row.ItemArray[2].ToString());
            }
            else if (DataGrid1Row == null)
            {
                selectedItems = null;
            }

            return selectedItems;

        }

        //TextBoxes vullen bij veranderen Selectie DataGrid. 
        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Als er geen null terugegeven wordt door DataGrid1_SelectedItem worden de TextBoxes voorzien van de waarden uit DataGrid1.
            if (DataGrid1_SelectedItem() != null)
            {
                TextBox1_Team.Text = DataGrid1_SelectedItem()[0];
                TextBox2_Player.Text = DataGrid1_SelectedItem()[1];
                TextBox3_Coach.Text = DataGrid1_SelectedItem()[2];
            }
            //Als null teruggegeven wordt is er geen data of een lege cel in DataGrid 1 geselecteerd. 
            //De textboxes worden leeggemaakt met methode ClearTextBoxes()
            else if (DataGrid1_SelectedItem() == null)
            {
                ClearTextBoxes();
            }
        }
    }
}
