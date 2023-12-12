using MySqlConnector;

namespace TeleBot.DbCommunication.MySqlConnector
{
    internal class DB
    {
        public string connectionString;

        MySqlConnection connection = new();

        public DB(DBConfig settings)
        {
            connectionString = $"Server={settings.Server};Database={settings.Database};Uid={settings.User};Pwd={settings.Password};";
            connection = new MySqlConnection(connectionString);
        }

        public DB(string password, string server, string database, string user)
        {
            connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
            connection = new MySqlConnection(connectionString);
        }

        public DB(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new MySqlConnection(connectionString);
        }

        public void DatabaseHelper(string password, string server, string database, string user)
        {
            connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
        }

        public void DatabaseHelper(DBConfig settings)
        {
            connectionString = $"Server={settings.Server};Database={settings.Database};Uid={settings.User};Pwd={settings.Password};";
        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection GetConnection() { return connection; }
    }
}
