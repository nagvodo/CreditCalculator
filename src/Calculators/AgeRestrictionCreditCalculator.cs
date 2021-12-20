using System.Linq;

using static System.Math;

namespace Zip.Credit
{
    /// <summary>
    /// Represents age restrictions to use Zip 
    /// </summary>
    class AgeRestrictionCreditCalculator : AbstractCreditCalculator, ICreditPointsCalculator<Customer>, ICreditCalculator
    {
        public AgeRestrictionCreditCalculator(CreditCalculatorRepository data) : base(data)
        {
        }

        public AgeRestrictionCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data) : base(creditCalculator, data)
        {
        }

        public AgeRestrictionCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data, IValidator validator) : base(creditCalculator, data, validator)
        {
        }

        public AgeRestrictionCreditCalculator(CreditCalculatorRepository data, IValidator validator) : base(data, validator)
        {
        }

        public int CalculateCreditPoints(Customer customer)
        {
            if (ValidationProvider != null)
                ValidationProvider.Validate(customer, data);

            var creditPointsPrevious = 1000;
            var maximumPoints = 1000;

            if (creditCalculator as ICreditPointsCalculator<Customer> != null)
            {
                var creditPointsProvider = (ICreditPointsCalculator<Customer>)creditCalculator;
                creditPointsPrevious = creditPointsProvider.CalculateCreditPoints(customer);
            }

            maximumPoints = data.AgeRecords.First(r => (customer.AgeInYears >= r.YearsBegin && customer.AgeInYears <= r.YearsEnd))
                                            .MaximumPoints;

            return Min(creditPointsPrevious, maximumPoints);
        }

        public decimal CalculateCredit(Customer customer)
        {
            var availableCredits = (decimal)0;
            var creditPoints = CalculateCreditPoints(customer);

            if (!data.AvailableCredits.TryGetValue(creditPoints, out availableCredits))
            {
                var maxPoints = data.AvailableCredits.Max(x => x.Key);
                var minPoints = data.AvailableCredits.Min(x => x.Key);
                if (creditPoints > maxPoints)
                {
                    availableCredits = data.AvailableCredits[maxPoints];
                }
                else if (creditPoints < minPoints)
                {
                    availableCredits = data.AvailableCredits[minPoints];
                }
            }

            return availableCredits;
        }
    }

}