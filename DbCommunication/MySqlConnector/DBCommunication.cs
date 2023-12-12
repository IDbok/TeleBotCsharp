using MySqlConnector;
using TeleBot.Models;
using Telegram.Bot.Types;

namespace TeleBot.DbCommunication.MySqlConnector
{
    internal static class DBCommunication
    {
        public static void SaveMessageToDB(Message message)
        {
            string query;
            MySqlCommand command;
            MySqlDataReader reader;
            DBConfig settings = new DBConfig();
            DB db = new DB(settings);
            db.OpenConnection();
            Console.WriteLine("Connection opened");
            TextMessage msg = new TextMessage(message);
            // check if user exists
            query = $"SELECT * FROM `users` WHERE `id` = '{msg.User.UserId}'";
            command = new MySqlCommand(query, db.GetConnection());
            reader = command.ExecuteReader();
            bool userExists = reader.HasRows;
            reader.Close();
            if (!userExists)
            {
                query = $"INSERT INTO `users` (`id`, `Username`, `FirstName`, `LastName`) " +
                    $"VALUES (@id, @username, @firstname, @lastname)";
                command = new MySqlCommand(query, db.GetConnection());
                command.Parameters.AddWithValue("@id", msg.User.UserId);
                command.Parameters.AddWithValue("@username", msg.User.Username);
                command.Parameters.AddWithValue("@firstname", msg.User.FirstName);
                command.Parameters.AddWithValue("@lastname", msg.User.LastName);
                command.ExecuteNonQuery();
            }

            query = $"INSERT INTO `messages` " +
                $"(`ChatId`, `UserID`, `Username`,`Text`, `Date`) " +
                $"VALUES (@chatid, `UserID`, @username,  @text, @date)";
            command = new MySqlCommand(query, db.GetConnection());
            command.Parameters.AddWithValue("@chatid", msg.ChatId);
            command.Parameters.AddWithValue("@userid", msg.User.UserId);
            command.Parameters.AddWithValue("@username", msg.User.Username);
            command.Parameters.AddWithValue("@text", msg.Text);
            command.Parameters.AddWithValue("@date", msg.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            command.ExecuteNonQuery();
            db.CloseConnection();
            Console.WriteLine("Connection closed");
        }
    }
}
