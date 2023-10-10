namespace JobTaskInpress.Api.DTOs;

public record RoundListItem(string CreatedDate, int RoundsCount, int TasksCount);
public record DaySummeryListItem(string CreatedDate, int TotalRounds, int TotalTasks);