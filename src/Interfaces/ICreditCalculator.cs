namespace Zip.Credit
{
    interface ICreditCalculator
    {
        /// <summary>
        /// Calculates the available credit (in $) for a given customer
        /// </summary>
        /// <param name="customer">The customer for whom we are calculating credit</param>
        /// <returns>Available credit amount in $</returns>
        decimal CalculateCredit(Customer customer);
    }
}