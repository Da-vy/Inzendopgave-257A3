using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inzendopgave_257A3.Classes
{
    public class Connection
    {
        //ConnectionString
        public static string ConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Inzendopgave_257A3.Properties.Settings.connectionString"].ConnectionString;
            return connectionString;
        }
    }
}
