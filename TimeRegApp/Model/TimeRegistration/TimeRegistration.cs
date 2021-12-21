using Dapper;
using Npgsql;
using System.Data;

namespace TimeReg_Api.TimeRegApp.Model.TimeRegistration
{
    public class TimeRegistration : ITimeRegistration
    {
        private string _connectionString;

        public TimeRegistration(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Database") + $"Database={config.GetConnectionString("DatabaseName")}";
        }

        /* Original
        public TimeRegistration CreateTimeRegistration(DynamicParameters tReg)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegistration>(@"INSERT INTO timeregistration (timereg_created, timereg_start, timereg_end) VALUES (@timereg_created, @timereg_start, @timereg_end) returning*", tReg).FirstOrDefault();
        }
        */

        // With user_id (who created a timr registration), by using session id somehow?
        // With activiy_id (what activity got registred).
        // timereg_created is set automatically, it's only neccesary to define "timereg_start" and end "timereg_end"" from a user.

        public List<TimeRegModel> GetTimeRegistrations()
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegModel>(@"Select * from timeregistration").AsList();
        }
        public TimeRegModel CreateTimeStamp(DynamicParameters tReg)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegModel>(@"INSERT INTO timeregistration (timereg_start, timereg_end, activity_id, user_id) VALUES (@timereg_start, @timereg_end, @activity_id, @user_id) returning*", tReg).FirstOrDefault();
        }
    }
}
