using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ADWebApplication.Models
{
    public class ReportIssueVM
    {
        [Required(ErrorMessage = "Please select a bin")]
        public int BinId { get; set; }
        
        // Auto-populated from selected bin
        public string LocationName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an issue type")]
        public string IssueType { get; set; } = string.Empty; // Overflow, Damaged, Access Issue, etc.

        [Required(ErrorMessage = "Please select severity level")]
        public string Severity { get; set; } = string.Empty; // Low, Medium, High

        [Required(ErrorMessage = "Please provide a description")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        public string Description { get; set; } = string.Empty;
        
        // For dropdown - list of bins from today's route
        public List<BinOption> AvailableBins { get; set; } = new();
    }
    
    public class BinOption
    {
        public int BinId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string DisplayText => $"Bin #{BinId} - {LocationName}";
    }
}