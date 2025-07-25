﻿using System.Text.Json.Serialization;

namespace AdminDashboard.DbStuff.Models
{
    public class User : BaseModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual List<UserToken> TokenBalance { get; set; }
        public virtual List<Payment> Payments { get; set; }
    }
}
