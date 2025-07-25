﻿using System.Text.Json.Serialization;

namespace AdminDashboard.DbStuff.Models
{
    public class Payment : BaseModel
    {
        public DateTime DateOfPurchase { get; set; }
        public string PaymentName { get; set; }
        public float PaymentCost { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
