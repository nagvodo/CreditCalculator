namespace Zip.Credit
{
    internal interface ICreditPointsCalculator<TCustomer> where TCustomer : Customer
    {
        int CalculateCreditPoints(TCustomer customer);
    }
}