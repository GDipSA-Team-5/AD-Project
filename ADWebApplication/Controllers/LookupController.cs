using ADWebApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADWebApplication.Controllers
{
    [ApiController]
    [Route("api/lookup")]
    public class LookupController : ControllerBase
    {
        private readonly LogDisposalDbContext _context;

        public LookupController(LogDisposalDbContext context)
        {
            _context = context;
        }

        [HttpGet("bins")]
        public async Task<IActionResult> GetBins()
        {
            return Ok(await _context.CollectionBins
                .AsNoTracking()
                .Select(b => new { binId = b.BinId, locationName = b.LocationName })
                .ToListAsync());
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _context.EWasteCategories
                .AsNoTracking()
                .Select(c => new { categoryId = c.CategoryId, categoryName = c.CategoryName })
                .ToListAsync());
        }

        [HttpGet("itemtypes")]
        public async Task<IActionResult> GetItemTypes([FromQuery] int categoryId)
        {
            return Ok(await _context.EWasteItemTypes
                .AsNoTracking()
                .Where(t => t.CategoryId == categoryId)
                .Select(t => new
                {
                    itemTypeId = t.ItemTypeId,
                    itemName = t.ItemName,
                    estimatedAvgWeight = t.EstimatedAvgWeight
                })
                .ToListAsync());
        }
    }
}