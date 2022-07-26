using Npgsql;

namespace Authentication_clone.Db
{
    public static class Connection
    {
        public static NpgsqlConnection GetPgConnection(IConfiguration confing)
        {
            var connString = confing.GetConnectionString("PgConnection");
            var connection = new NpgsqlConnection(connString);
            return connection;
        }
    }
}
