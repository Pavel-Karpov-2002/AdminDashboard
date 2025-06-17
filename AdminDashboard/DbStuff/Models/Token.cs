using System.Text.Json.Serialization;

namespace AdminDashboard.DbStuff.Models
{
    public class Token : BaseModel
    {
        public string? NameToken { get; set; }
        public float Rate { get; set; }

        [JsonIgnore]
        public virtual List<UserToken>? UserTokens { get; set; }
    }
}
