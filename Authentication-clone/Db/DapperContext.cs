using Npgsql;

namespace Authentication_clone.Db
{
    public class DapperContext
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DapperContext(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("PgConnection");
        }

        public NpgsqlConnection CreateConnection() => new(_connectionString);
    }
}
