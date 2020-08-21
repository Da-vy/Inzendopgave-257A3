using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Inzendopgave_257A3.Classes
{
    class Player
    {
        
        //Connection String
        private static readonly string connectionString = Connection.ConnectionString();

        //Check of Player al voorkomt op de database
        //Geeft standaard true terug. False wanneer de speler al voorkomt.
        public static bool Exists(string name)
        { 
            bool exists = true;
            string query = @"SELECT COUNT(*) FROM Players WHERE Name = @name";

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

        //Select Player ID. 
        //Geeft standaard PlayerID -1 terug. Aan deze waarde is geen speler gekoppeld.
        public static int GetId(string player)
        {
            int id = -1;
            string query = "SELECT Id FROM Players WHERE Name = @player";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@player", SqlDbType.VarChar).Value = player;
                    
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        id = (int)dataReader["Id"];
                    }

                    return id;
                }
            }
        }

        //Insert Player
        public static void Insert(string name, string team)
        {                      
            string query = @"INSERT INTO Players (Name, Team) VALUES (@name, @team)";
            
            //Query wordt alleen uitgevoerd wanneer de speler nog niet bestaat, er iets ingevoerd is,
            //de ingevoerde naam niet langer is dan 100 poisities en het team ingevuld is.
            if (Exists(name) == false && string.IsNullOrWhiteSpace(name) == false && 
                name.Length < 100 && string.IsNullOrWhiteSpace(team) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                            command.Parameters.Add("@team", SqlDbType.VarChar).Value = Classes.Team.GetId(team);

                            int rowsAffected = command.ExecuteNonQuery();

                            //Als er een rij is toegevoegd aan de database verschijnt er melding met de naam van de persoon die is toegevoegd aan de database. 
                            if (rowsAffected == 1)
                            {
                                MessageBox.Show("Added " + name + " to table Players!");
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
            else if (Exists(name) == true)
            {
                MessageBox.Show(name + " bestaat al!");
            }
            else if (name.Length >= 100)
            {
                MessageBox.Show(name + " is te lang!");
            }
            else if (string.IsNullOrWhiteSpace(team) == true && string.IsNullOrWhiteSpace(name) == false)
            {
                MessageBox.Show("Team mag niet leeg zijn!");
            }
        }

        //Update Player
        public static void Update(string oldName, string newName, string oldTeam, string newTeam)
        {
            string query = @"UPDATE Players SET Name = @newName WHERE Name = @oldName";

            //Als er een ander team ingevuld wordt en het team bestaat al op de database
            //Dan wordt de speler ook gekoppeld aan het nieuwe team.
            if (oldTeam != newTeam && Team.Exists(newTeam) == true)
            {
                query = @"UPDATE Players SET Name = @newName, Team = @newTeam WHERE Name = @oldName AND Team = @oldTeam";
            }

            if (string.IsNullOrWhiteSpace(newName) == false)
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

                            //Als er een ander team ingevuld wordt en het team bestaat al op de database
                            //Dan wordt de speler gekoppeld aan het nieuwe team.
                            if (oldTeam != newTeam && Classes.Team.Exists(newTeam) == true) 
                            { 
                                command.Parameters.Add("@oldTeam", SqlDbType.VarChar).Value = Classes.Team.GetId(oldTeam);
                                command.Parameters.Add("@newTeam", SqlDbType.VarChar).Value = Classes.Team.GetId(newTeam);
                            }

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                            {
                                //Geeft een melding als de naam van de Speler is verandered en geeft ook aan waarin de naam is veranderd.
                                if (oldName != newName)
                                {
                                    MessageBox.Show("Changed " + oldName + " to " + newName + "!");
                                }
                                //Geeft een melding als het team van de Speler is verandered en geeft ook aan waarin de naam is veranderd. 
                                if (oldTeam != newTeam)
                                {
                                    MessageBox.Show("Moved " + newName + " from " + oldTeam + " to " + newTeam + "!");
                                }
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

        }

        //delete player
        public static void Delete(string name)
        {
            string query = @"DELETE FROM Players WHERE name = @name";

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

        //Selecteer alle spelers uit een bepaald team en voeg deze toe aan een List. 
        //Method moet static zijn om op te kunnen roepen vanuit MainWindow
        public static List<object> GetPlayersFromTeam(int teamId)
        {
            List<object> playerList = new List<object>();
            string query = "SELECT * FROM Players WHERE Team = @team ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        playerList.Add(dataReader["Name"]);
                    }

                    return playerList;
                }
            }
        }

        //Individuele resultaten van spelers toevoegen aan tabel Rankings
        public static void InsertRankings(string speler, string team, int punten)
        {
            string query = @"INSERT INTO Rankings ([Speler], [Team], [Punten]) VALUES (@speler, @team, @punten)";
            int playerId = GetId(speler);
            int teamId = Team.GetId(team);
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@speler", SqlDbType.Int).Value = playerId;
                        command.Parameters.Add("@team", SqlDbType.Int).Value = teamId;
                        command.Parameters.Add("@punten", SqlDbType.Int).Value = punten;

                        int rowsAffected = command.ExecuteNonQuery();

                        //als er niets is toegevoegd een melding geven.
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Database Transactie mislukt..");
                        }
                        //als er wel wat aan de database is toegevoegd worden deze waarden ook toegevoegd aan een XML bestand. 
                        //Deze is te vinden in de map waarin zich het uitvoerbestand zich vind. 
                        else
                        {
                            XDocument rankings = XDocument.Load("Rankings.xml");
                            XElement spelersXML = new XElement("Speler",
                                    new XElement("Naam", speler),
                                    new XElement("Team", team),
                                    new XElement("Punten", punten));
                            rankings.Root.Add(spelersXML);
                            rankings.Save("Rankings.xml");
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

        //Haalt gegevens uit tabel rankings op en voegt deze toe aan een datatable zodat deze makkelijk in een DataGrid kunnen worden weergegeven.
        public static DataTable GetRankings()
        {
            DataTable dataTable = new DataTable();

            //Geen spatie vergeten op het eind van elke regel..
            //Selecteer Naam van de speler, het Team waarvoor de speler gescoord heeft en de som van alle punten die een speler heeft gemaakt. 
            //Groepeer op spelersnaam en Teamnaam, en sorteer van meest gescoorde punten naar minst gescoorde punten.
            string query = "SELECT Players.Name AS Speler, Teams.Name AS Team, SUM(Rankings.Punten) AS Punten " +
                           "FROM Players " +
                           "LEFT JOIN Teams ON Teams.Id = Players.Team " +
                           "LEFT JOIN Rankings ON Players.Id = Rankings.Speler " +
                           "GROUP BY Players.Name, Teams.Name " + 
                           "ORDER BY Punten DESC";

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
