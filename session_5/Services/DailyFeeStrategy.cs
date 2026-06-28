namespace Services;
using Interfaces;
public class DailyFeeStrategy : IFeeStrategy
{
    private const int DailyRate = 500; 

    public decimal Calculate(DateTime entryTime, DateTime exitTime)
    {
        TimeSpan duration = exitTime - entryTime;
        int totalDays = (int)Math.Ceiling(duration.TotalDays);
        return totalDays * DailyRate;
    }
}