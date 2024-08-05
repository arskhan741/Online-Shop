﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Online_Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }

    }
}