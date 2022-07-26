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
            var res = await connection.QueryFirstAsync<User?>(
                "INSERT INTO public.users(name, email, role, password) VALUES (@Name, @Email, @Role, @Password); SELECT * FROM public.users WHERE email = @Email;",
                new { obj.Name, obj.Email, obj.Role, obj.Password }
            );
            Console.WriteLine($"new user name = {res?.Name}");
            return res;
        }

        public async Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetById(int Id)
        {
            User? user;
            using (var connection = Connection.GetPgConnection(_config))
            {
                var res = await connection.QueryAsync<User>("SELECT * FROM public.users WHERE id = @Id", new { Id });
                user = res.FirstOrDefault();
                Console.WriteLine($"user name = {user?.Name}");
            }
            return user;
        }

        public async Task<User?> GetByEmail(string email)
        {
            User? user;
            using (var connection = Connection.GetPgConnection(_config))
            {
                var res = await connection.QueryAsync<User>("SELECT * FROM public.users WHERE email = @email", new { email });
                user = res.FirstOrDefault();
            }
            return user;
        }

        public async Task<User> Update(int Id, User newData)
        {
            throw new NotImplementedException();
        }
    }
}
