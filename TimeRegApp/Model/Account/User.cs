namespace TimeReg_Api.TimeRegApp.Model.Account
{
    // Model class for user
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Telephone { get; set; }
        public bool isAdmin { get; set; }
    }
}
