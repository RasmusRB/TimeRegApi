using Dapper;
using Npgsql;
using System.Data;
using TimeReg_Api.TimeRegApp.Model.TimeRegistration;

namespace TimeRegApi.TimeRegApp.Model.Activity
{
    public class Activity
    {
        private string _connectionString;

        public Activity(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Database") + $"Database={config.GetConnectionString("DatabaseName")}";
        }

        // public Activity CreateActivity(DynamicParameters aCreate)
        // {
        //     using IDbConnection db = new NpgsqlConnection(_connectionString);
        //     return db.Query<TimeRegistration>("@INSERT INTO activities (activity) VALUES (@activity)", aCreate).FirstOrDefault();
        // }

        public TimeRegistration GetRegistrationById(int id)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegistration>("@SELECT * FROM activities WHERE id = @id").FirstOrDefault();
        }

        public TimeRegistration UpdateRegistrationById(int id, string activity)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);
            parms.Add("@activity", activity);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegistration>("@UPDATE activities SET activity = @activity WHERE id = @id").FirstOrDefault();
        }

        public TimeRegistration DeleteRegistrationById(int id)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegistration>("@DELETE FROM activities WHERE id = @id;").FirstOrDefault();
        }


    }
}
