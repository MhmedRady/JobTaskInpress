using JobTaskInpress.Api.DTOs;
using JobTaskInpress.Api.QuartzJobs;
using JobTaskInpress.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using Quartz;
using Task = JobTaskInpress.Domain.Entities.Task;

namespace JobTaskInpress.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoundController : ControllerBase
{
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    private readonly ILogger<RoundController> _logger;

    public RoundController(IGeneralRepository<Round, Guid> roundRepository, ILogger<RoundController> logger)
    {
        _roundRepository = roundRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var groupedRounds = _roundRepository.Where(null,"Tasks")
            .GroupBy(r => r.CreatedAt.Value.Date)
            .ToList();

        var roundSummaries = groupedRounds
            .Select(g => new RoundSummary
            {
                CreatedAt = g.Key,
                RoundCount = g.Count(),
                TotalTaskCount = g.Count() *10
                
            })
            .ToList();

        // foreach (var summary in roundSummaries)
        // {
        //     var roundTasks = groupedRounds
        //         .Where(g => g.Key == summary.CreatedAt)
        //         .SelectMany(g => g.Select(r => r.Tasks))
        //         .ToList();
        //
        //     summary.TotalTaskCount = roundTasks.Sum(t => t.Count);
        // }
        
        return Ok(roundSummaries);
    }
}

public class RoundSummary
{
    public DateTime? CreatedAt { get; set; }
    public int RoundCount { get; set; }
    public int TotalTaskCount { get; set; }
}