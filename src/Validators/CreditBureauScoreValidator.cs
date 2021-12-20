using System;
using System.Linq;

namespace Zip.Credit
{
    class CreditBureauScoreValidator : IValidator 
    {
        private readonly IValidator validator;

        public CreditBureauScoreValidator()
        {
        }

        public CreditBureauScoreValidator(IValidator validator)
        {
            this.validator = validator;
        }

        public void Validate(Customer customer, CreditCalculatorRepository data)
        {
            if (validator != null)
                validator.Validate(customer, data);

            var minAcceptedCreditBureauScore = data.CreditBureauScoreRecords.Min(r => r.BuroScoreBegin);
            if (customer.BureauScore < minAcceptedCreditBureauScore)
            {
                throw new ApplicationException($"Customer's credit bureau score ({customer.BureauScore}) " +
                    $"is less than a minimum accepted score ({minAcceptedCreditBureauScore})." +
                    "Customer is not allowed to use Zip.");
            }
        }
    }
}