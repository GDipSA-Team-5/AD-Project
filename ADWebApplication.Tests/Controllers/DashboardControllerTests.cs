using System.Collections.Generic;
using System.Threading.Tasks;
using ADWebApplication.Controllers;
using ADWebApplication.Data.Repository;
using ADWebApplication.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ADWebApplication.Tests.Controllers
{
    public class DashboardControllerTests
    {
        private class FakeRepo : IDashboardRepository
        {
            public Task<DashboardKPIs> GetAdminDashboardAsync(System.DateTime? forMonth = null) => Task.FromResult(new DashboardKPIs());
            public Task<List<CollectionTrend>> GetCollectionTrendsAsync(int monthsBack = 6) => Task.FromResult(new List<CollectionTrend>());
            public Task<List<CategoryBreakdown>> GetCategoryBreakdownAsync() => Task.FromResult(new List<CategoryBreakdown>());
            public Task<List<AvgPerformance>> GetAvgPerformanceMetricsAsync() => Task.FromResult(new List<AvgPerformance>());
            public Task<int> GetHighRiskUnscheduledCountAsync() => Task.FromResult(0);
            public Task<(int ActiveBins, int TotalBins)> GetBinCountsAsync() => Task.FromResult((0,0));
        }

        [Fact]
        public async Task Index_ReturnsViewWithViewModel()
        {
            var repo = new FakeRepo();
            var ctrl = new DashboardController(repo, NullLogger<DashboardController>.Instance);
            var res = await ctrl.Index();
            res.Should().BeOfType<ViewResult>();
            var vr = (ViewResult)res;
            vr.Model.Should().BeAssignableTo<AdminDashboardViewModel>();
        }

        [Fact]
        public async Task RefreshKPIs_ReturnsPartialView()
        {
            var repo = new FakeRepo();
            var ctrl = new DashboardController(repo, NullLogger<DashboardController>.Instance);
            var res = await ctrl.RefreshKPIs();
            res.Should().BeOfType<PartialViewResult>();
        }
    }
}
