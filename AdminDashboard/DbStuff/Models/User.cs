namespace AdminDashboard.DbStuff.Models
{
    public class User : BaseModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public virtual List<Token> Balance{ get; set; }
    }
}
