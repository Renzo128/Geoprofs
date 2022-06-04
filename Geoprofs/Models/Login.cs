namespace Geoprofs.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Password { get; set; }
        public Coworker Coworker { get; set; }
    }
}
