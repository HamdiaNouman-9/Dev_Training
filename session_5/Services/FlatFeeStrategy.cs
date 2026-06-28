namespace Services;
using Interfaces;
public class FlatFeeStrategy : IFeeStrategy
{
    private const int FlatRate = 200; 

    public decimal Calculate(DateTime entryTime, DateTime exitTime)
    {
        return FlatRate;
    }
}