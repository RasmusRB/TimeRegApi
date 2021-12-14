using Dapper;
using Npgsql;
using System.Data;

namespace TimeReg_Api.TimeRegApp.Model.Activity
{
    public class Activity : IActivity
    {
        private string _connectionString;

        public Activity(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Database") + $"Database={config.GetConnectionString("DatabaseName")}";
        }

        public Activity CreateActivity(DynamicParameters aCreate)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<Activity>("@INSERT INTO activities (activity) VALUES (@activity) returning*", aCreate).FirstOrDefault();
        }

        public Activity GetRegistrationById(int id)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<Activity>("@SELECT * FROM activities WHERE id = @id").FirstOrDefault();
        }

        public Activity UpdateRegistrationById(int id, string activity)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);
            parms.Add("@activity", activity);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<Activity>("@UPDATE activities SET activity = @activity WHERE id = @id").FirstOrDefault();
        }

        public Activity DeleteRegistrationById(int id)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<Activity>("@DELETE FROM activities WHERE id = @id;").FirstOrDefault();
        }


    }
}
