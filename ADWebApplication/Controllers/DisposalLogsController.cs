using ADWebApplication.Data;
using ADWebApplication.Models.DTOs;
using ADWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADWebApplication.Controllers
{
    [ApiController]
    [Route("api/disposallogs")]
    public class DisposalLogsController : ControllerBase
    {
        private readonly LogDisposalDbContext _context;

        public DisposalLogsController(LogDisposalDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDisposalLogRequest request)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var log = new DisposalLogs
                {
                    BinId = request.BinId,
                    EstimatedTotalWeight = request.EstimatedWeightKg,
                    DisposalTimeStamp = DateTime.UtcNow,
                    Feedback = request.Feedback,
                    UserId = request.UserId
                };

                _context.DisposalLogs.Add(log);
                await _context.SaveChangesAsync();

                var item = new DisposalLogItem
                {
                    LogId = log.LogId,
                    ItemTypeId = request.ItemTypeId,
                    SerialNo = request.SerialNo,
                };

                _context.DisposalLogItems.Add(item);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
                return Ok(new { log.LogId });
            }
            catch
            {
                await tx.RollbackAsync();
                return StatusCode(500, "Failed to create disposal log");
            }
        }
    }
}