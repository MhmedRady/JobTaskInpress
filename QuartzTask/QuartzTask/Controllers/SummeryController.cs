using QuartzTask.Api;
using QuartzTask.Api.DTOs;
using QuartzTask.db;
using QuartzTask.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;

namespace QuartzTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummeryController: ControllerBase
{
    private readonly DailyTaskContext _dailyTaskContext;
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    public SummeryController(DailyTaskContext dailyTaskContext, IGeneralRepository<Round, Guid> roundRepository)
    {
        _dailyTaskContext = dailyTaskContext;
        _roundRepository = roundRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roundSummaries = await _dailyTaskContext.DaySummary.GroupBy(g=>g.DateSummary.Value.Date)
            .Select(g => 
                new DaySummeryListItem(g.Key.Date.ToString(), g.ToList().Sum(s=>s.TotalRounds), g.ToList().Sum(s=>s.TotalTasks))).ToListAsync();
        return Ok(roundSummaries);
    }
}