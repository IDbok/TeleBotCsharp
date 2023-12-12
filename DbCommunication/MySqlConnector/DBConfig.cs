namespace TeleBot.DbCommunication.MySqlConnector
{
    internal class DBConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public DBConfig()
        {
            Server = "localhost";
            Database = "telegramdb";
            User = "root";
            Password = "root";
        }

    }
}
