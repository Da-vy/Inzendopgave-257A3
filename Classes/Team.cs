using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using Inzendopgave_257A3.Windows;

namespace Inzendopgave_257A3.Classes
{
    public class Team
    {
        //Connection String
        private static readonly string connectionString = Connection.ConnectionString();

        //Check of Team al voorkomt op de database
        //Geeft standaard true terug. False wanneer de speler al voorkomt.
        public static bool Exists(string name)
        {      
            bool exists = true;
            string query = @"SELECT COUNT(*) FROM Teams WHERE Name = @name";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;

                        int existsCount = (int)command.ExecuteScalar();

                        if (existsCount < 1)
                        {
                            exists = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }

                return exists;

            }
        }

        //Check of het Team een Coach heeft.
        public static bool HasCoach(string team)
        {
            bool hasCoach = true;
            
            string query = @"SELECT COUNT(*) FROM Coaches WHERE Team = @team";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@team", SqlDbType.Int).Value = GetId(team);

                        int existsCount = (int)command.ExecuteScalar();

                        if (existsCount < 1)
                        {
                            hasCoach = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }

                return hasCoach;
            }

        }
        //Check of het Team Spelers heeft.
        public static bool HasPlayers(string team)
        {
            bool hasPlayers = true;

            string query = @"SELECT COUNT(*) FROM Players WHERE Team = @team";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@team", SqlDbType.Int).Value = GetId(team);

                        int existsCount = (int)command.ExecuteScalar();

                        if (existsCount < 1)
                        {
                            hasPlayers = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }

                return hasPlayers;
            }

        }

        //Select Team ID. 
        //Geeft standaard TeamID -1 terug. Aan deze waarde is geen Team gekoppeld.
        public static int GetId(string team)
        {
            int id = -1;
            string query = "SELECT Id FROM Teams WHERE Name = @team";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@team", team);
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        id = (int)dataReader["Id"];
                    }

                    return id;
                }

            }
        }

        //Insert Team
        public static void Insert(string name)
        {
            string query = @"INSERT INTO Teams (Name) VALUES (@name)";
            
            //Query alleen uitvoeren als het Team nog niet bestaat, er iets ingevuld is en de naam van het team niet langer is dan 50 posities
            if (Exists(name) == false && string.IsNullOrWhiteSpace(name) == false && name.Length < 50 )
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                            {
                                MessageBox.Show("Added " + name + " to table Teams!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                    }
                }
            }            
            else if (name.Length >= 50)
            {
                MessageBox.Show(name + " is te lang!");
            }
        }

        //Update Team
        public static void Update(string oldName, string newName)
        {            
            string query = @"UPDATE Teams SET Name = @newName WHERE Name = @oldName";

            //Query alleen uitvoeren wanneer de nieuwe naam afwijkt van de oude naam en de nieuwe naam nog niet bestaat op de database.
            if (oldName != newName && Exists(newName) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@oldName", SqlDbType.VarChar).Value = oldName;
                            command.Parameters.Add("@newName", SqlDbType.VarChar).Value = newName;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                            {
                                MessageBox.Show("Changed " + oldName + " to " + newName + "!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                    }
                }

            }
            //Melding geven wanneer nieuwe naam al voorkomt op db
            else if (Exists(newName) == true && oldName != newName)
            {
                MessageBox.Show("Team bestaat al, kan deze update niet uitvoeren.");
            }

        }

        //delete team (alleen wanneer er geen players of coaches meer in het team aanwezig zijn)
        public static void Delete(string name)
        {
            string query = @"DELETE FROM Teams WHERE name = @name";

            //query wordt alleen uitgevoerd als team geen coach of spelers meer heeft. 
            if (HasCoach(name) == false && HasPlayers(name) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show(name + " is verwijderd!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                    }
                }
            }
            //Melding geven als er nog spelers en/of coaches in het team aanwezig zijn. 
            else
            {
                MessageBox.Show("Er bevinden zich nog spelers of coaches in dit Team.\n" +
                                "\n" +
                                "Team kan niet worden verwijderd.");
            }
           
        }

        //List vullen met teamnamen.
        //Method moet static zijn om op te kunnen roepen vanuit MainWindow
        public static List<object> GetNames()
        {
            List<object> teamsList = new List<object>();
            string query = "SELECT * FROM Teams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        teamsList.Add(dataReader["Name"]);
                    }

                    return teamsList;
                }                               
            }
        }

        //Alle spelers en coach in een team aan datatable.
        //Datatable gebruiken als itemssource voor datagrid1 in Mainwindow. 
        public static DataTable GetEverything()
        {
            DataTable dataTable = new DataTable();

            //Geen spatie vergeten op het eind van elke regel..
            //AS gebruiken voor Players.Name en Coaches.Name zodat er een binding kan worden gemaakt in de XAML bij Datagrid.
            string query = "SELECT Teams.Name, Players.Name AS PlayerName, Coaches.Name AS CoachName " +
                            "FROM Teams " +
                            "LEFT JOIN Players ON Teams.Id = Players.Team " +
                            "LEFT JOIN Coaches ON Teams.Id  = Coaches.Team";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

        //Eindresultaten van een wedstrijd invoeren in tabel Results. 
        //Tabel results wordt gebruikt om standings te creëren
        public static void InsertResults(string thuis, string uit, int scoreThuis, int scoreUit)
        {
            string query = @"INSERT INTO Results ([Team Thuis], [Team Uit], [Score Thuis], [Score Uit]) VALUES (@thuis, @uit, @scoreThuis, @scoreUit)";
            int thuisId = GetId(thuis);
            int uitId = GetId(uit);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@thuis", SqlDbType.Int).Value = thuisId;
                            command.Parameters.Add("@uit", SqlDbType.Int).Value = uitId;
                            command.Parameters.Add("@scoreThuis", SqlDbType.Int).Value = scoreThuis;
                            command.Parameters.Add("@scoreUit", SqlDbType.Int).Value = scoreUit;

                        int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                            {
                                MessageBox.Show(thuis + " tegen " + uit + " toegevoegd aan database.\n" +
                                                "\n" +
                                                "Uitslag: " + scoreThuis + " - " + scoreUit + " !");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                    }
                }

        }

        //Toevoegen van een team aan tabel standings.  
        //Alleen toevoegen aan de tabel wanneer het team hier nog niet in voorkomt. 
        public static void InsertStandings(string team)
        {
            //query om te tellen hoevaak het team voorkomt in Standings.
            string queryExists = @"SELECT count(*) FROM Standings WHERE [Team] = @team";
            string query = @"INSERT INTO Standings ([Team], [Gespeeld], [Gewonnen], [Verloren], [Gelijkspel], [Doelsaldo], [Punten]) VALUES (@team, @gespeeld, @gewonnen, @verloren, @gelijkspel, @doelsaldo, @punten)";
            int teamId = GetId(team);
            int gespeeld = 0;
            int gewonnen = 0;
            int verloren = 0;
            int gelijkspel = 0;
            int doelsaldo = 0;
            int punten = 0;                       

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int exists = 0;
                    
                    //query geeft 0 terug als er niets is gevonden. 1 als er wel een team wordt gevonden.
                    using (SqlCommand commandExists = new SqlCommand(queryExists, connection))
                    {
                        commandExists.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                        exists = (int)commandExists.ExecuteScalar();
                    }

                    //als een team minder dan 1 keer voorkomt in tabel rankings mag de query om deze toe te voegen pas uitgevoerd worden.
                    if (exists < 1)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            command.Parameters.Add("@gespeeld", SqlDbType.Int).Value = gespeeld;
                            command.Parameters.Add("@gewonnen", SqlDbType.Int).Value = gewonnen;
                            command.Parameters.Add("@verloren", SqlDbType.Int).Value = verloren;
                            command.Parameters.Add("@gelijkspel", SqlDbType.Int).Value = gelijkspel;
                            command.Parameters.Add("@doelsaldo", SqlDbType.Int).Value = doelsaldo;
                            command.Parameters.Add("@punten", SqlDbType.Int).Value = punten;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                            {
                                MessageBox.Show(team + " toegevoegd aan Standings.");
                            }
                        }
                    }
                    //Extra controle
                    /*//Melding als team al voorkomt in standings
                    else if(exists > 0)
                    {
                        MessageBox.Show("Team bestaat al in Standings.");
                    }*/                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }
            }
        }

        //Methode waarmee tabel standings geüpdatet wordt!
        public static void UpdateStandings(string team)
        {
            //query om alle gegevens (op punten na, deze wordt berekend nadat alle gegevens zijn ingevoerd in Standings) te updaten bij bestaande teams in de database.  
            string query = @"Update Standings SET [Gespeeld] = @gespeeld, [Gewonnen] = @gewonnen, [Verloren] = @verloren, [Gelijkspel] = @gelijkspel, [Doelsaldo] = @doelsaldo WHERE [Team] = @team";
            int teamId = GetId(team);

            //Totaal aantal gespeelde wedstrijden.
            //Wordt berekend aan de hand van alle gespeelde wedstrijden thuis + alle gespeelde wedstrijden uit. 
            string queryGespeeld = @"SELECT (SELECT COUNT([Team Thuis]) FROM Results WHERE [Team Thuis] = @team) + (SELECT COUNT([Team Uit]) FROM Results WHERE [Team Uit] = @team) FROM Results";
            int gespeeld = 0;

            //Totaal aantal wedstrijden gewonnen.
            //gewonnen wanner Score Thuis > Score Uit wanneer het Team het Thuisteam is en Score Thuis < Score Uit wanneer het team het uitteam is.
            string queryGewonnen = @"SELECT (SELECT COUNT([Team Thuis]) FROM Results WHERE [Team Thuis] = @team AND [Score Thuis] > [Score Uit]) + " +
                                           "(SELECT COUNT([Team Uit]) FROM Results WHERE [Team Uit] = @team AND [Score Uit] > [Score Thuis]) FROM Results";
            int gewonnen = 0;

            //Totaal aantal wedstrijden verloren.
            //Zie gewonnen, maar dan het omgekeerde
            string queryVerloren = @"SELECT (SELECT COUNT([Team Thuis]) FROM Results WHERE [Team Thuis] = @team AND [Score Thuis] < [Score Uit]) + " +
                                           "(SELECT COUNT([Team Uit]) FROM Results WHERE [Team Uit] = @team AND [Score Uit] < [Score Thuis]) FROM Results";
            int verloren = 0;

            //Totaal aantal wedstrijden dat in gelijkspel is geeindigd. 
            //checkt alle wedstrijden waarbij de score gelijk is voor zowel de gespeelde wedstrijden thuis als uit.
            string queryGelijkspel = @"SELECT (SELECT COUNT([Team Thuis]) FROM Results WHERE [Team Thuis] = @team AND [Score Thuis] = [Score Uit]) + " +
                                             "(SELECT COUNT([Team Uit]) FROM Results WHERE [Team Uit] = @team AND [Score Uit] = [Score Thuis]) FROM Results";
            int gelijkspel = 0;

            //Berekenen van het doelsaldo aan de hand het aantal gescoorde punten thuis - het aantal gescorode punten uit en andersom. 
            //Eerst deel van de query gaf een foutmelding als een team alleen nog maar thuis of uit had gespeeld. 
            //Query uitgebreid met verkapte IF ELSE statement. Er wordt gekeken of een team zowel thuis als uit heeft gespeeld of alleen maar thuis of alleen maar uit
            string queryDoelsaldo = @"IF EXISTS (SELECT * FROM Results WHERE [Team Thuis] = @team) AND EXISTS (SELECT * FROM Results WHERE [Team Uit] = @team)
                                          SELECT(SELECT SUM([Score Thuis]) FROM Results WHERE [Team Thuis] = @team) - 
                                                (SELECT SUM([Score Uit]) FROM Results WHERE [Team Thuis] = @team) + 
                                                (SELECT SUM([Score Uit]) FROM Results WHERE [Team Uit] = @team) - 
                                                (SELECT SUM([Score Thuis]) FROM Results WHERE [Team Uit] = @team) 
                                      ELSE 
                                        IF EXISTS (SELECT * FROM Results WHERE [Team Thuis] = @team) AND NOT EXISTS (SELECT * FROM Results WHERE [Team Uit] = @team)
                                            SELECT(SELECT SUM([Score Thuis]) FROM Results WHERE[Team Thuis] = @team) - 
                                                  (SELECT SUM([Score Uit]) FROM Results WHERE [Team Thuis] = @team)                                           
                                        ELSE  
                                            IF NOT EXISTS (SELECT * FROM Results WHERE [Team Thuis] = @team) AND EXISTS (SELECT * FROM Results WHERE [Team Uit] = @team) 
                                                SELECT(SELECT SUM([Score Uit]) FROM Results WHERE [Team Uit] = @team) - 
                                                      (SELECT SUM([Score Thuis]) FROM Results WHERE [Team Uit] = @team)";
            int doelsaldo = 0;

            //Totaal aantal punten wat een team verdiend heeft. 
            //Gewonnen = 3 punten; Gelijkspel is 1 punten; Verloren is 0 punten. 
            //Eerst een qeury om het aantal punten op te halen (kan later hergebruikt worden om het aantal punten op te halen). 
            string queryPunten = @"SELECT ((SELECT Gewonnen FROM Standings WHERE [Team] = @team) * 3) + (SELECT Gelijkspel FROM Standings Where [Team] = @team)";
            //Query pas uitvoeren nadat alle andere gegevens zijn ingevoerd (threading).
            //Query om de punten in te voeren.
            string queryInsertPunten = @"UPDATE Standings SET [Punten] = @punten WHERE [Team] = @team";
            
            int punten = 0;
            //string queryInsertPunten = @"UPDATE Standings SET [Punten] = (SELECT ((SELECT Gewonnen FROM Standings WHERE [Team] = @team) * 3) + (SELECT Gelijkspel FROM Standings Where [Team] = @team)) WHERE [Team] = @team";
            

            //Queries uitvoeren met behulp van threads. Zodat dit in de goede volgorde gebeurd. 
            //Updaten na elke keer dat de results geüpdatet zijn. 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Thread om alle gegevens (op punten na, deze wordt berekend nadat alle gegevens zijn ingevoerd in Standings) op te halen uit results. 
                    Thread threadStandings = new Thread(new ThreadStart(() =>
                    {
                        using (SqlCommand commandGespeeld = new SqlCommand(queryGespeeld, connection))
                        {
                            commandGespeeld.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            gespeeld = (int)commandGespeeld.ExecuteScalar();
                        }

                        using (SqlCommand commandGewonnen = new SqlCommand(queryGewonnen, connection))
                        {
                            commandGewonnen.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            gewonnen = (int)commandGewonnen.ExecuteScalar();
                        }

                        using (SqlCommand commandVerloren = new SqlCommand(queryVerloren, connection))
                        {
                            commandVerloren.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            verloren = (int)commandVerloren.ExecuteScalar();
                        }

                        using (SqlCommand commandGelijkspel = new SqlCommand(queryGelijkspel, connection))
                        {
                            commandGelijkspel.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            gelijkspel = (int)commandGelijkspel.ExecuteScalar();
                        }

                        using (SqlCommand commandDoelsaldo = new SqlCommand(queryDoelsaldo, connection))
                        {
                            commandDoelsaldo.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            doelsaldo = (int)commandDoelsaldo.ExecuteScalar();
                        }

                    }));
                    threadStandings.Start();
                    threadStandings.Join();//Wachten tot deze thread klaar is. 

                    //Thread om alle opgehaalde gegevens in te voeren in tabel Standings. 
                    Thread threadInsertStandings = new Thread(new ThreadStart(() =>
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                        command.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                        command.Parameters.Add("@gespeeld", SqlDbType.Int).Value = gespeeld;
                        command.Parameters.Add("@gewonnen", SqlDbType.Int).Value = gewonnen;
                        command.Parameters.Add("@verloren", SqlDbType.Int).Value = verloren;
                        command.Parameters.Add("@gelijkspel", SqlDbType.Int).Value = gelijkspel;
                        command.Parameters.Add("@doelsaldo", SqlDbType.Int).Value = doelsaldo;
                        
                        int rowsAffected = command.ExecuteNonQuery();

                        //Extra controle
                        /*if (rowsAffected == 1)
                        {
                            MessageBox.Show("Standings geüpdatet");
                        }*/
                        }
                    }));
                    threadInsertStandings.Start();
                    threadInsertStandings.Join();//wachten tot deze thread klaar is. 

                    //Thread waarmee het aantal punten wordt opgehaald. 
                    Thread threadPunten = new Thread(new ThreadStart(() =>
                    {
                        using (SqlCommand commandPunten = new SqlCommand(queryPunten, connection))
                        {
                            commandPunten.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            punten = (int)commandPunten.ExecuteScalar();
                        }

                    }));
                    threadPunten.Start();
                    threadPunten.Join();//wachten tot deze thread klaar is. 

                    //Thread om ook het aantal punten toe te voegen aan tabel Standings. 
                    Thread threadInsertPunten = new Thread(new ThreadStart(() =>
                    {
                        using (SqlCommand commandInsertPunten = new SqlCommand(queryInsertPunten, connection))
                        {
                            commandInsertPunten.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                            commandInsertPunten.Parameters.Add("@punten", SqlDbType.Int).Value = punten;
                            commandInsertPunten.ExecuteNonQuery();
                        }
                        
                    }));
                    threadInsertPunten.Start();
                    threadInsertPunten.Join();//wachten tot deze thread klaar is. 

                    //Thread om het bestaande XML document Standings.xml (wordt automatisch aangemaakt) te updaten met de laatst ingevoerde gegevens. 
                    Thread threadUpdateXML = new Thread(new ThreadStart(() =>
                    {
                        XDocument standings = XDocument.Load("Standings.xml");
                        XElement teamXML = new XElement("Team",
                            new XElement("Naam", team),
                            new XElement("Gespeeld", gespeeld),
                            new XElement("Gewonnen", gewonnen),
                            new XElement("Verloren", verloren),
                            new XElement("Gelijkspel", gelijkspel),
                            new XElement("Dooelsaldo", doelsaldo),
                            new XElement("Punten", punten));
                        standings.Root.Add(teamXML);
                        standings.Save("Standings.xml");

                    }));
                    threadUpdateXML.Start();
                    threadUpdateXML.Join();//wachten tot deze thread klaar is. 

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }
            }     
        }

        //Haalt gegevens uit tabel standings op en voegt deze toe aan een datatable zodat deze makkelijk in een datagrid kan worden weergegeven
        public static DataTable GetStandings()
        {
            DataTable dataTable = new DataTable();

            //Geen spatie vergeten op het eind van elke regel..
            string query = "SELECT Teams.Name AS Team, Standings.Gespeeld AS Gespeeld, Standings.Gewonnen AS Gewonnen, Standings.Verloren AS Verloren, " +
                            "Standings.Gelijkspel AS Gelijkspel, Standings.Doelsaldo AS Doelsaldo, Standings.Punten AS Punten " +
                            "FROM Teams " +
                            "LEFT JOIN Standings ON Teams.Id = Standings.Team";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

    }
}
