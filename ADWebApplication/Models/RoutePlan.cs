using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADWebApplication.Models
{
    public class RoutePlan
    {
        [Key]
        public int RouteId { get; set; }

        public DateTime PlannedDate { get; set; } = DateTime.Today;

        [Required]
        [MaxLength(50)]
        public string RouteStatus { get; set; } = "Pending"; // e.g., Pending, InProgress, Completed

        public string GeneratedBy { get; set; } = string.Empty; // Admin/System

        // Navigation property
        public List<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
    }
}
