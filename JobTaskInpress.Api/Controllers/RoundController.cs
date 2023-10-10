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
        var _result = _roundRepository.Where()
            .GroupBy(g => g.CreatedAt.Value)
            .Select(x=> x.FirstOrDefault().CreatedAt.Value).Distinct().ToListAsync();
        var list = new List<Round>();
        var rounds = await _roundRepository.Where().GroupBy(g=>g.CreatedAt.Value)
            .Select(grouping=> grouping.ToList()).ToListAsync();
        foreach (var round in rounds)
        {
            list.Add(round.FirstOrDefault());
        }
        var result = new RoundListItem(list.Select(round=>round.CreatedAt.Value.ToString("yyyy-M-d dddd")).FirstOrDefault(), list.Count(), list.Count()*10);
        return Ok(result);
    }
}