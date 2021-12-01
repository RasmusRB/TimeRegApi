namespace TimeReg_Api.TimeRegApp.Model.Account
{
    public class User
    {
        public long Id { get; internal set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Role { get; set; }
    }
}
