using JobTaskInpress.db;
using JobTaskInpress.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using Quartz;
using Task = System.Threading.Tasks.Task;

namespace JobTaskInpress.Api.QuartzJobs;

public class CreateJob: IJob
{
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    private readonly ILogger<CreateJob> _logger;
    private readonly MainDbContext _mainDbContext;
    public CreateJob(ILogger<CreateJob> logger, IGeneralRepository<Round, Guid> roundRepository, MainDbContext mainDbContext)
    {
        _logger = logger;
        _roundRepository = roundRepository;
        _mainDbContext = mainDbContext;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", SaveRoundList());
        return Task.CompletedTask;
    }

    private bool SaveRoundList()
    {
        var rCount = _roundRepository.Count();
        var lastRound = _roundRepository.Get(orderby: round1 => round1.Id, orderbyDirection: "DESC").FirstOrDefault();

        for (int r = rCount; r <= rCount+10; r++)
        {
            var round = new Round()
            {
                Name = $"Round_{rCount+1}",
                Start = DateTime.Now.AddDays(rCount - 1).Date.ToString("yyyy-MM-dd"),
                End = DateTime.Now.AddDays(rCount + 1).Date.ToString("yyyy-MM-dd"),
                CreatedAt = DateTime.Now.AddDays(DependencyInjection.RandomNum(1,50)),
                LastModified = DateTime.Now.AddDays(rCount).Date.ToString("yyyy-MM-dd"),
            };
        
            for (int i = 1; i <= 10; i++)
            {
                round.Tasks.Add(new Domain.Entities.Task()
                {
                    Name = $"Task_Round_{rCount+i}",
                    Result = DependencyInjection.RandomNum(),
                    LastModified = DateTime.Now.AddDays(rCount).Date.ToString("yyyy-MM-dd"),
                    CreatedAt = DateTime.Now
                });
            }
            _roundRepository.Add(round);
        }
        return _roundRepository.SaveChanges() == 1;
    }
}