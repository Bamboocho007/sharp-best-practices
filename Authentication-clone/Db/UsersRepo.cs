using Authentication_clone.Models;
using Dapper;

namespace Authentication_clone.Db
{
    public class UsersRepo : IRepo<User>
    {
        private readonly DapperContext _dapperContext;
        public UsersRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<User> Add(User obj)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleAsync<User>(
                "INSERT INTO public.users(name, email, role, password) VALUES (@Name, @Email, @Role, @Password) RETURNING*;", obj);
        }

        public async Task<User?> GetById(int Id)
        {
            using var connection = _dapperContext.CreateConnection();
            var users = await connection.QueryAsync<User>("SELECT * FROM public.users WHERE id = @Id;", new { Id });
            return users.FirstOrDefault();
        }

        public async Task<User?> GetByEmail(string email)
        {
            using var connection = _dapperContext.CreateConnection();
            var res = await connection.QueryAsync<User>("SELECT * FROM public.users WHERE email = @email", new { email });
            return res.FirstOrDefault();
        }

        public async Task<User?> Update(User obj)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleAsync<User>(
                   "UPDATE public.users SET email=@Email, role=@Role, name=@Name, password=@Password WHERE id=@Id RETURNING*;",
                   obj);
        }

        public async Task<User?> Delete(int Id)
        {
            using var connection = _dapperContext.CreateConnection();
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
