using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Online_Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; } = null!;
    }
}
