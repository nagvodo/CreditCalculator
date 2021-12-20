using System.Linq;

namespace Zip.Credit
{
    /// <summary>
    /// Implementation of ICreditCalculator
    /// </summary>
    class CompletedPaymentsCreditCalculator : AbstractCreditCalculator, ICreditPointsCalculator<Customer>, ICreditCalculator
    {
        public CompletedPaymentsCreditCalculator(CreditCalculatorRepository data) : base(data)
        {
        }

        public CompletedPaymentsCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data) : base(creditCalculator, data)
        {
        }

        public CompletedPaymentsCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data, IValidator validator) : base(creditCalculator, data, validator)
        {
        }

        public CompletedPaymentsCreditCalculator(CreditCalculatorRepository data, IValidator validator) : base(data, validator)
        {
        }

        public int CalculateCreditPoints(Customer customer)
        {
            if (ValidationProvider != null)
                ValidationProvider.Validate(customer, data);

            var creditPointsPrevious = 1000;
            var completedPaymentsPoints = 1000;

            if (creditCalculator as ICreditPointsCalculator<Customer> != null)
            {
                var creditPointsProvider = (ICreditPointsCalculator<Customer>)creditCalculator;
                creditPointsPrevious = creditPointsProvider.CalculateCreditPoints(customer);
            }

            if (!data.CompletedPayments.TryGetValue(customer.CompletedPaymentCount, out completedPaymentsPoints))
            {
                var maxCompletedPaymentsCount = data.CompletedPayments.Max(x => x.Key);
                if (customer.CompletedPaymentCount > maxCompletedPaymentsCount)
                {
                    completedPaymentsPoints = data.CompletedPayments[maxCompletedPaymentsCount];
                }
            }

            return creditPointsPrevious + completedPaymentsPoints;
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