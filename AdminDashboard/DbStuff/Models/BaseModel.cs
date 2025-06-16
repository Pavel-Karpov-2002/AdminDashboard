using AdminDashboard.DbStuff.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.DbStuff.Models
{
    public class BaseModel : IBaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
