using Dapper;
using Npgsql;
using System.Data;

namespace TimeReg_Api.TimeRegApp.Model.Account
{
    // Manager class for user account
    public class Account : IAccount
    {
        private string _connectionString;

        public Account(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Database") + $"Database={config.GetConnectionString("DatabaseName")}";
        }

        public User CreateUser(DynamicParameters user)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return
            // Using Dapper for object mapping
            db.Query<User>(
              @"INSERT INTO users(email, password, firstname, lastname, telephone, isadmin)
          VALUES (@email, @password, @firstname, @lastname, @phone, @isAdmin)
          returning *",
              user
            ).FirstOrDefault();
        }

        public User UpdateUser(int id, DynamicParameters user)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<User>(
                @"UPDATE users SET email = @email, password = @password, firstname = @firstname, lastname = @lastname, telephone = @telephone WHERE id = @id"
            , user
            ).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            var parms = new DynamicParameters();
            parms.Add("@email", email);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<User>(
              $"Select * from users where email = @email",
              parms
              ).FirstOrDefault();
        }

        public List<User> GetAllUsers()
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<User>(@"Select * from users").AsList();
        }

        public bool DeleteUser(string userEmail)
        {

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            db.Execute(@"DELETE FROM users WHERE email = @Email", new DynamicParameters(new { Email = userEmail }));
            return true;
        }
    }
}
