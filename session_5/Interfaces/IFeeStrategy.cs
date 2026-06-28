namespace Interfaces;
public interface IFeeStrategy
{
    decimal Calculate(DateTime entryTime, DateTime exitTime);
}