using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inzendopgave_257A3.Classes
{
    class Coach
    {
        //Connection String
        private static readonly string connectionString = Connection.ConnectionString();

        //Check of Coach al voorkomt op de database
        //Geeft standaard true terug. False wanneer de coach al voorkomt.
        public static bool Exists(string name)
        {
            bool exists = true;
            string query = @"SELECT COUNT(*) FROM Coaches WHERE Name = @name";

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

        //Insert Coach
        public static void Insert(string name, string team)
        {
            string query = @"INSERT INTO Coaches (Name, Team) VALUES (@name, @team)";
            
            //uitvoeren wanneer Coach niet bestaat, er een waarde is ingevoerd, 
            //de naam van de coach niet langer is dan 50 posities, de coach geen team heeft en het team gevuld is. 
            if (Exists(name) == false && string.IsNullOrWhiteSpace(name) == false && 
                name.Length < 50 && Team.HasCoach(team) == false &&
                string.IsNullOrWhiteSpace(team) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                            command.Parameters.Add("@team", SqlDbType.VarChar).Value = Team.GetId(team);
                            
                            int rowsAffected = command.ExecuteNonQuery();

                            //Als er een rij is toegvoegd aan de database een Messagbox laten zien met de naam van de coach die toegevoegd is. 
                            if (rowsAffected == 1)
                            {
                                MessageBox.Show("Added " + name + " to table Coaches!");
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
            else if (Team.HasCoach(team) == true)
            {
                MessageBox.Show(team + " heeft al een Coach!");
            }
            else if (name.Length >= 50)
            {
                MessageBox.Show(name + " is te lang!");
            }
            else if (string.IsNullOrWhiteSpace(team) == true && string.IsNullOrWhiteSpace(name) == false)
            {
                MessageBox.Show("Team mag niet leeg zijn!");
            }
        }

        
        //update Coach
        //Gegevens bij de coach updaten        
        public static void Update(string oldName, string newName, string oldTeam, string newTeam)
        {
            string query = @"UPDATE Coaches SET Name = @newName WHERE Name = @oldName";

            //Als er een ander team ingevuld wordt en het team bestaat al op de database
            //Dan wordt de coach ook gekoppeld aan het nieuwe team (als het Team nog geen Coach heeft!)
            if (oldTeam != newTeam && Team.Exists(newTeam) == true && Team.HasCoach(newTeam) == false)
            {
                query = @"UPDATE Coaches SET Name = @newName, Team = @newTeam WHERE Name = @oldName AND Team = @oldTeam";
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
                            //Dan wordt de coach gekoppeld aan het nieuwe team.
                            if (oldTeam != newTeam && Team.Exists(newTeam) == true)
                            {
                                command.Parameters.Add("@oldTeam", SqlDbType.VarChar).Value = Team.GetId(oldTeam);
                                command.Parameters.Add("@newTeam", SqlDbType.VarChar).Value = Team.GetId(newTeam);
                            }

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                            {
                                //Geeft een melding als de naam van de Coach is verandered en geeft ook aan waarin de naam is veranderd. 
                                if (oldName != newName)
                                {
                                    MessageBox.Show("Changed " + oldName + " to " + newName + "!");
                                }
                                //Geeft een melding als het team van de Coach is verandered en geeft ook aan waarin de naam is veranderd. 
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

        //delete coach 
        public static void Delete(string name)
        {
            string query = @"DELETE FROM Coaches WHERE name = @name";

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
    }
}
