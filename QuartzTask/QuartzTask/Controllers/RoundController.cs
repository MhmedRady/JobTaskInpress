using QuartzTask.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using QuartzTask.db;

namespace QuartzTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoundController : ControllerBase
{
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    private readonly DailyTaskContext _dailyTaskContext;
    private readonly ILogger<RoundController> _logger;

    public RoundController(IGeneralRepository<Round, Guid> roundRepository, ILogger<RoundController> logger, DailyTaskContext dailyTaskContext)
    {
        _roundRepository = roundRepository;
        _logger = logger;
        _dailyTaskContext = dailyTaskContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {

        var rounds = await _roundRepository.Where(null, "Tasks")
            .GroupBy(x=>x.CreatedAt.Value.Date)
            .Select(y=>new RoundSummary
            {
                CreatedAt = y.Key,
                RoundCount = y.Count(),
                TotalTaskCount = y.Count()*10
            }).OrderBy(r=>r.CreatedAt).ToListAsync();
        
        var lastDateSummery = await _dailyTaskContext.DaySummary.OrderByDescending(summary => summary.DateSummary).FirstOrDefaultAsync();
        var _rounds = await _roundRepository.Where(round =>round.CreatedAt.Value.Date > lastDateSummery.DateSummary, "Tasks")
            .GroupBy(x=>x.CreatedAt.Value.Date)
            .Select(y=>new RoundSummary
            {
                CreatedAt = y.Key,
                RoundCount = y.Count(),
                TotalTaskCount = y.Count()*10
            }).OrderBy(r=>r.CreatedAt).Select(round => new DaySummary()
            {
                TotalTasks = round.TotalTaskCount,
                TotalRounds = round.RoundCount,
                DateSummary = round.CreatedAt
            }).ToListAsync();

        if (_rounds.Any())
        {
            await _dailyTaskContext.DaySummary.AddRangeAsync(_rounds);
            await _dailyTaskContext.SaveChangesAsync();
        }
        
        return Ok(rounds);
    }
}

public class RoundSummary
{
    public DateTime? CreatedAt { get; set; }
    public int RoundCount { get; set; }
    public int TotalTaskCount { get; set; }
}