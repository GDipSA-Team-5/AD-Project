using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADWebApplication.Models
{
    public class RouteStop
    {
        [Key]
        public int StopId { get; set; }

        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public RoutePlan RoutePlan { get; set; } = null!;

        public int BinId { get; set; }
        [ForeignKey("BinId")]
        public CollectionBin CollectionBin { get; set; } = null!;

        public int StopSequence { get; set; }

        public TimeSpan? PlannedCollectionTime { get; set; }

        public string? IssueLog { get; set; } // Optional notes
    }
}
