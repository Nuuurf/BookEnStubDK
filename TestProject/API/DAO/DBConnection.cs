using System.Data;
using System.Data.SqlClient;

namespace TestProject.API.DAO
{
    public sealed class DBConnection
    {
        private static readonly Lazy<DBConnection> lazy = new Lazy<DBConnection>(() => new DBConnection());
        public static DBConnection Instance => lazy.Value;

        private readonly string connectionString;

        private DBConnection()
        {
            string dataSource = "hildur.ucn.dk";
            string initialCatalog = "DMA-CSD-S225_10210213"; //33
            string userID = "DMA-CSD-S225_10210213"; //42
            string password = "Password1!";

            connectionString = $"Data Source={dataSource};Initial Catalog={initialCatalog};User ID={userID};Password={password};";

        }

        public SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();

            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Couldn't open connection to the database");

            return connection;
        }
        public List<ConnectionState> TryConnection()
        {
            List<ConnectionState> connStates = new List<ConnectionState>();

            using (var conn = GetOpenConnection())
            {
                connStates.Add(conn.State);
            }

            // After using statement, connection is closed, and you can add closed state to list
            connStates.Add(ConnectionState.Closed);

            return connStates;
        }
    }
}
