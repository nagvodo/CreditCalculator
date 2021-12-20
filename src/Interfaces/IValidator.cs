namespace Zip.Credit
{
    internal interface IValidator
    {
        void Validate(Customer customer, CreditCalculatorRepository data);
    }
}