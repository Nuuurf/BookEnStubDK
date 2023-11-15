using System.Data;
using System.Data.SqlClient;

namespace RestfulApi.DAL
{
    public sealed class DBConnection
    {
        private static readonly Lazy<DBConnection> lazy = new Lazy<DBConnection>(() => new DBConnection());
        public static DBConnection Instance => lazy.Value;

        private readonly string connectionString;

        private DBConnection()
        {
            string dataSource = "hildur.ucn.dk";
            string initialCatalog = "DMA-CSD-S225_10461233";
            string userID = "DMA-CSD-S225_10461233"; //42

            string password = "Password1!"; //Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD environment variable is not set.");

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
        // Method to begin a transaction
        public SqlTransaction BeginTransaction()
        {
            var connection = GetOpenConnection();
            return connection.BeginTransaction();
        }

        // Method to commit a transaction
        public void CommitTransaction(SqlTransaction transaction)
        {
            if (transaction != null && transaction.Connection != null)
            {
                transaction.Commit();
                transaction.Connection.Close();
            }
        }

        // Method to roll back a transaction
        public void RollbackTransaction(SqlTransaction transaction)
        {
            if (transaction != null && transaction.Connection != null)
            {
                transaction.Rollback();
                transaction.Connection.Close();
            }
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
