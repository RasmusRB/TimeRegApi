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

        public TimeRegistration CreateTimeRegistration(DynamicParameters tReg)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegistration>(@"INSERT INTO timeregistration (timereg_created, timereg_start, timereg_end) VALUES (@time_created, time_start) returning*", tReg).FirstOrDefault();
        }
    }
}
