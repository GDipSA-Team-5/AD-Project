using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADWebApplication.Models
{
    public class CollectionBin
    {
        [Key]
        public int BinId { get; set; }

        // RegionId would go here if we implemented Region entity
        // public int RegionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string LocationName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string LocationAddress { get; set; } = string.Empty;

        [MaxLength(50)]
        public string BinType { get; set; } = string.Empty; // Small, Medium, Large

        [MaxLength(50)]
        public string BinStatus { get; set; } = "Active"; // Active, Maintenance, Full
    }
}
