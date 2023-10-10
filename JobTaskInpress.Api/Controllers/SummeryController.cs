using JobTaskInpress.Api.DTOs;
using JobTaskInpress.db;
using JobTaskInpress.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;

namespace JobTaskInpress.Api.Controllers;

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
        var list = new List<Round>();
        var rounds = await _dailyTaskContext.DaySummary.GroupBy(g=>g.DateSummary.Value).ToListAsync();
        var roundSummaries = rounds
            .Select(g => new DaySummeryListItem(g.Key.Date.ToString(), DependencyInjection.RandomNum(100, 199), DependencyInjection.RandomNum(100, 199)*10))
            .ToList();
        //var result = new RoundListItem(list.Select(round=>round.CreatedAt.Value.ToString("yyyy-M-d dddd")).FirstOrDefault(), list.Count(), list.Count()*10);
        return Ok(roundSummaries);
    }
}