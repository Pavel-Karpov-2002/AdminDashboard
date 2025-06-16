namespace AdminDashboard.DbStuff.Models
{
    public class Token : BaseModel
    {
        public string? NameToken { get; set; }
        public float Rate { get; set; }

        public virtual List<UserToken>? UserTokens { get; set; }
    }
}
