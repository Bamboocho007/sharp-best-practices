using Authentication_clone.Models;
using Dapper;

namespace Authentication_clone.Db
{
    public class UsersRepo : IRepo<User>
    {
        private readonly IConfiguration _config;
        public UsersRepo(IConfiguration config)
        {
            _config = config;
        }

        public async Task<User> Add(User obj)
        {
            using var connection = Connection.GetPgConnection(_config);
            return await connection.QuerySingleAsync<User>(
                "INSERT INTO public.users(name, email, role, password) VALUES (@Name, @Email, @Role, @Password) RETURNING*;", obj);
        }

        public async Task<User?> GetById(int Id)
        {
            using var connection = Connection.GetPgConnection(_config);
            var users = await connection.QueryAsync<User>("SELECT * FROM public.users WHERE id = @Id;", new { Id });
            return users.FirstOrDefault();
        }

        public async Task<User?> GetByEmail(string email)
        {
            using var connection = Connection.GetPgConnection(_config);
            var res = await connection.QueryAsync<User>("SELECT * FROM public.users WHERE email = @email", new { email });
            return res.FirstOrDefault();
        }

        public async Task<User?> Update(User obj)
        {
            using var connection = Connection.GetPgConnection(_config);
            return await connection.QuerySingleAsync<User>(
                   "UPDATE public.users SET email=@Email, role=@Role, name=@Name, password=@Password WHERE id=@Id RETURNING*;",
                   obj);
        }

        public async Task<User?> Delete(int Id)
        {
            using var connection = Connection.GetPgConnection (_config);
            var user = await GetById(Id);

            if (user == null)
            {
                return null;
            }

            var res = await connection.ExecuteAsync("DELETE FROM public.users WHERE id=@Id;", new { Id });
            return res > 0 ? user : null;
        }
    }
}
