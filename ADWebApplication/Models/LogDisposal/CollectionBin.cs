using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADWebApplication.Models.LogDisposal;
    [Table("CollectionBin")]
    public class CollectionBin
    {
        [Key]
        [Column("binId")]
        public int BinId { get; set; }

        [Column("regionId")]
        public int? RegionId { get; set; }

        [Column("locationName")]
        public string? LocationName { get; set; }

        [Column("locationAddress")]
        public string? LocationAddress { get; set; }

        [Column("binCapacity")]
        public int BinCapacity { get; set; }

        [Column("binStatus")]
        public string BinStatus { get; set; } = "Active";

        public Region? Region { get; set; }

    }
