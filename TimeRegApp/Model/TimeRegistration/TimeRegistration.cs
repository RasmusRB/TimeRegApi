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

        public List<TimeRegModel> GetAllTimeregistrations()
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegModel>(@"Select * from timeregistration").AsList();
        }

        public TimeRegModel CreateTimeStamp(DynamicParameters tReg)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegModel>(@"INSERT INTO timeregistration (timereg_start, timereg_end, timereg_comment, activity_id, user_id) VALUES (@timereg_start, @timereg_end, @timereg_comment, @activity_id, @user_id) returning*", tReg).FirstOrDefault();
        }

        public List<TimeRegModel> GetTimeRegByDate(DateTime date)
        {
            var parms = new DynamicParameters();
            parms.Add("@date", date);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegModel>(@"SELECT * FROM timeregistration WHERE CAST(timereg_created as DATE) = @date", parms).AsList();
        }
    }
}
