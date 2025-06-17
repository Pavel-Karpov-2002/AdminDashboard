using System.Text.Json.Serialization;

namespace AdminDashboard.DbStuff.Models
{
    public class UserToken : BaseModel
    {
        public float CountToken { get; set; }

        public virtual Token? Token { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
