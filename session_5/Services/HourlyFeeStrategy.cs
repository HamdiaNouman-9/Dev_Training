namespace Services;
using Interfaces;
public class HourlyFeeStrategy : IFeeStrategy
{
    private const int HourlyRate = 50; 

    public decimal Calculate(DateTime entryTime, DateTime exitTime)
    {
        TimeSpan duration = exitTime - entryTime;
        int totalHours = (int)Math.Ceiling(duration.TotalHours);
        return totalHours * HourlyRate;
    }
}