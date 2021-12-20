using System;
using System.Linq;

namespace Zip.Credit
{
    class AgeValidator : IValidator
    {
        private readonly IValidator validator;

        public AgeValidator()
        {
        }

        public AgeValidator(IValidator validator)
        {
            this.validator = validator;
        }

        public void Validate(Customer customer, CreditCalculatorRepository data)
        {
            if (validator != null)
                validator.Validate(customer, data);

            var minAcceptedAge = data.AgeRecords.Min(r => r.YearsBegin);

            if (customer.AgeInYears < minAcceptedAge)
            {
                throw new ApplicationException($"Customer's age ({customer.AgeInYears}) is less than a minimum accepted age {minAcceptedAge}." +
                    "Customer is not allowed to use Zip.");
            }         
        }
    }
}