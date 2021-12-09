namespace TimeReg_Api.TimeRegApp.Model
{
    // Model class for creating a user
    public class CreateUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Telephone { get; set; }
    }
}
