namespace AdminDashboard.DbStuff.Models
{
    public class UserToken : BaseModel
    {
        public float CountToken { get; set; }

        public virtual Token? Token { get; set; }
        public virtual User? User { get; set; }
    }
}
