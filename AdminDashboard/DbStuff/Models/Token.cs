namespace AdminDashboard.DbStuff.Models
{
    public class Token : BaseModel
    {
        public string? NameToken { get; set; }
        public int CountToken { get; set; }
        public virtual User User { get; set; }
    }
}
