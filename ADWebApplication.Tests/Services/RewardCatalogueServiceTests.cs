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
    public class RewardCatalogueServiceTests
    {
        private class FakeRepo : IRewardCatalogueRepository
        {
            public RewardCatalogue? LastSaved;
            public Task<int> AddRewardAsync(RewardCatalogue reward)
            {
                LastSaved = reward;
                return Task.FromResult(7);
            }
            public Task<bool> DeleteRewardAsync(int rewardId) => Task.FromResult(true);
            public Task<IEnumerable<RewardCatalogue>> GetAllRewardsAsync() => Task.FromResult<IEnumerable<RewardCatalogue>>(new List<RewardCatalogue>());
            public Task<RewardCatalogue?> GetRewardByIdAsync(int rewardId) => Task.FromResult<RewardCatalogue?>(null);
            public Task<IEnumerable<RewardCatalogue>> GetAvailableRewardsAsync() => Task.FromResult<IEnumerable<RewardCatalogue>>(new List<RewardCatalogue>());
            public Task<IEnumerable<string>> GetAllRewardCategoriesAsync() => Task.FromResult<IEnumerable<string>>(new List<string>());
            public Task<IEnumerable<RewardCatalogue>> GetRewardsByCategoryAsync(string category) => Task.FromResult<IEnumerable<RewardCatalogue>>(new List<RewardCatalogue>());
            public Task<bool> UpdateRewardAsync(RewardCatalogue reward) => Task.FromResult(true);
        }

        [Fact]
        public async Task AddRewardAsync_WithZeroPoints_Throws()
        {
            var repo = new FakeRepo();
            var svc = new RewardCatalogueService(repo, NullLogger<RewardCatalogueService>.Instance);
            var reward = new RewardCatalogue { Points = 0, StockQuantity = 1 };

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddRewardAsync(reward));
        }

        [Fact]
        public async Task CheckRewardAvailabilityAsync_ReturnsTrueWhenAvailable()
        {
            IRewardCatalogueRepository repo = new FakeRepoWithGet(new RewardCatalogue { RewardId = 1, Availability = true, StockQuantity = 5 });
            var svc = new RewardCatalogueService(repo, NullLogger<RewardCatalogueService>.Instance);

            var ok = await svc.CheckRewardAvailabilityAsync(1, 3);
            ok.Should().BeTrue();
        }

        private class FakeRepoWithGet : IRewardCatalogueRepository
        {
            private readonly RewardCatalogue _r;
            public FakeRepoWithGet(RewardCatalogue r) { _r = r; }
            public Task<int> AddRewardAsync(RewardCatalogue reward) => Task.FromResult(1);
            public Task<bool> DeleteRewardAsync(int rewardId) => Task.FromResult(true);
            public Task<IEnumerable<RewardCatalogue>> GetAllRewardsAsync() => Task.FromResult<IEnumerable<RewardCatalogue>>(new List<RewardCatalogue>());
            public Task<RewardCatalogue?> GetRewardByIdAsync(int rewardId) => Task.FromResult<RewardCatalogue?>(_r);
            public Task<IEnumerable<RewardCatalogue>> GetAvailableRewardsAsync() => Task.FromResult<IEnumerable<RewardCatalogue>>(new List<RewardCatalogue>());
            public Task<IEnumerable<string>> GetAllRewardCategoriesAsync() => Task.FromResult<IEnumerable<string>>(new List<string>());
            public Task<IEnumerable<RewardCatalogue>> GetRewardsByCategoryAsync(string category) => Task.FromResult<IEnumerable<RewardCatalogue>>(new List<RewardCatalogue>());
            public Task<bool> UpdateRewardAsync(RewardCatalogue reward) => Task.FromResult(true);
        }
    }
}
