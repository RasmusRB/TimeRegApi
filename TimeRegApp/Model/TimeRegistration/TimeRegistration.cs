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
            return db.Query<TimeRegistration>("@INSERT INTO timeregistration (timereg_created, timereg_start, timereg_end, fkactivity_id, fkuser_id) VALUES (@time_created, time_start, time_end, fkaktivity_id, fkuser_id) returning*", tReg).FirstOrDefault();
        }

        public TimeRegistration GetRegistrationByUser(int fkuser_id)
        {
            var parms = new DynamicParameters();
            parms.Add("@fkuser_id", fkuser_id);

            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<TimeRegistration>("@select timereg_id, timereg_created, timereg_start, timereg_end, user_id, user_firstname, user_lastname, user_email FROM timeregistration INNER JOIN users ON timeregistration.fkuser_id = users.user_id").FirstOrDefault();

            //KARRY KAN IKKE LAVES FÆRDIG FØR DE ANDRE TABELLER ER DER, MEN DENNE VIRKER MED NUVÆRENDE TABLLER.
            /*select timereg_id, timereg_created, timereg_start, timereg_end, user_id, user_firstname, user_lastname, user_email FROM timeregistration
            INNER JOIN users ON timeregistration.fkuser_id = users.user_id*/
        }
    }
}
