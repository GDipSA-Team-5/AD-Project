using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ADWebApplication.Models;
using ADWebApplication.Services;
using ADWebApplication.Data.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ADWebApplication.Tests.Services
{
    public class CampaignServiceTests
    {
        private class FakeCampaignRepository : ICampaignRepository
        {
            public Campaign? LastSaved;
            public Func<int> AddResult = () => 42;
            public Task<int> AddCampaignAsync(Campaign campaign)
            {
                LastSaved = campaign;
                return Task.FromResult(AddResult());
            }
            public Task<bool> DeleteCampaignAsync(int campaignId) => Task.FromResult(true);
            public Task<IEnumerable<Campaign>> GetActiveCampaignsAsync() => Task.FromResult<IEnumerable<Campaign>>(new List<Campaign>());
            public Task<IEnumerable<Campaign>> GetAllCampaignsAsync() => Task.FromResult<IEnumerable<Campaign>>(new List<Campaign>());
            public Task<Campaign?> GetCampaignByIdAsync(int campaignId) => Task.FromResult<Campaign?>(null);
            public Task<IEnumerable<Campaign>> GetScheduledCampaignsAsync() => Task.FromResult<IEnumerable<Campaign>>(new List<Campaign>());
            public Task<IEnumerable<Campaign>> GetByStatusAsync(string status) => Task.FromResult<IEnumerable<Campaign>>(new List<Campaign>());
            public Task<Campaign?> GetCurrentCampaignAsync() => Task.FromResult<Campaign?>(null);
            public Task<bool> UpdateCampaignAsync(Campaign campaign) => Task.FromResult(true);
        }

        [Fact]
        public async Task AddCampaignAsync_WithEndBeforeStart_Throws()
        {
            var repo = new FakeCampaignRepository();
            var svc = new CampaignService(repo, NullLogger<CampaignService>.Instance);
            var campaign = new Campaign { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(-1), IncentiveValue = 1 };

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddCampaignAsync(campaign));
        }

        [Fact]
        public async Task AddCampaignAsync_WithNegativeIncentive_Throws()
        {
            var repo = new FakeCampaignRepository();
            var svc = new CampaignService(repo, NullLogger<CampaignService>.Instance);
            var campaign = new Campaign { StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), IncentiveValue = -5 };

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddCampaignAsync(campaign));
        }

        [Fact]
        public async Task AddCampaignAsync_WithFutureStart_SetsPlannedStatusAndReturnsId()
        {
            var repo = new FakeCampaignRepository();
            repo.AddResult = () => 123;
            var svc = new CampaignService(repo, NullLogger<CampaignService>.Instance);
            var campaign = new Campaign { StartDate = DateTime.UtcNow.AddDays(10), EndDate = DateTime.UtcNow.AddDays(20), IncentiveValue = 1 };

            var id = await svc.AddCampaignAsync(campaign);

            id.Should().Be(123);
            repo.LastSaved.Should().NotBeNull();
            repo.LastSaved!.Status.Should().Be("Planned");
        }

        [Fact]
        public async Task ActivateCampaignAsync_WhenNotFound_ReturnsFalse()
        {
            var repo = new FakeCampaignRepository();
            var svc = new CampaignService(repo, NullLogger<CampaignService>.Instance);

            var result = await svc.ActivateCampaignAsync(999);

            result.Should().BeFalse();
        }
    }
}
