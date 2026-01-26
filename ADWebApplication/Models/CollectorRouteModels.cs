using System;
using System.Collections.Generic;

namespace ADWebApplication.Models
{
    public class CollectorRoute
    {
        public string RouteId { get; set; } = string.Empty;
        public string RouteName { get; set; } = string.Empty; // e.g. "Route A-12"
        public string Zone { get; set; } = string.Empty; // e.g. "Downtown District"
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; } = string.Empty; // "Scheduled", "In Progress", "Completed"
        public List<CollectionPoint> CollectionPoints { get; set; } = new();
        
        // Computed Properties for Dashboard
        public int TotalPoints => CollectionPoints.Count;
        public int CompletedPoints => CollectionPoints.Count(p => p.Status == "Collected");
        public double TotalWeightCollected => CollectionPoints.Sum(p => p.CollectedWeightKg);
    }

    public class CollectionPoint
    {
        public string PointId { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty; // e.g. "Downtown Plaza - Bin #45"
        public string Address { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
        public int EstimatedTimeMins { get; set; }
        public int CurrentFillLevel { get; set; } // Percentage
        public string Status { get; set; } = string.Empty; // "Pending", "Collected", "Issue"
        public double CollectedWeightKg { get; set; }
        public DateTime? CollectedAt { get; set; }
    }
}
