namespace ADWebApplication.Models.DTOs
{
    public class RewardsSummaryDto
    {
        public int TotalPoints { get; set; }
        public int ExpiringSoonPoints { get; set; }
        public string? NearestExpiryDate { get; set; }

        public int TotalDisposals { get; set; }
        public int TotalRedeemed { get; set; }
        public int TotalReferrals { get; set; }
    }
}