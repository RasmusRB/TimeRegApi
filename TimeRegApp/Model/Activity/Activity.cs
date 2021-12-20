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

        public ActivityModel CreateActivity(DynamicParameters aCreate)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<ActivityModel>(@"INSERT INTO activities (activity) VALUES (@activity) returning *", aCreate).FirstOrDefault();
        }

        public IList<ActivityModel> GetAllActivities()
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<ActivityModel>(@"Select * from activities").ToList();
        }

        public ActivityModel GetRegistrationById(int id)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<ActivityModel>(@"SELECT * FROM activities WHERE id = @id", parms).FirstOrDefault();
        }

        public ActivityModel UpdateRegistrationById(int id, string activity)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);
            parms.Add("@activity", activity);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<ActivityModel>(@"UPDATE activities SET activity = @activity WHERE id = @id", parms).FirstOrDefault();
        }

        public ActivityModel DeleteRegistrationById(int id)
        {
            var parms = new DynamicParameters();
            parms.Add("@id", id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<ActivityModel>(@"DELETE FROM activities WHERE id = @id", parms).FirstOrDefault();
        }


    }
}
