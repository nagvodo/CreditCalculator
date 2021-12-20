using System.Linq;

namespace Zip.Credit
{
    /// <summary>
    /// Implementation of ICredtiCalculator
    /// </summary>
    class MissedPaymentsCreditCalculator : AbstractCreditCalculator, ICreditPointsCalculator<Customer>, ICreditCalculator
    {
        public MissedPaymentsCreditCalculator(CreditCalculatorRepository data) : base(data)
        {
        }

        public MissedPaymentsCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data) : base(creditCalculator, data)
        {
        }

        public MissedPaymentsCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data, IValidator validator) : base(creditCalculator, data, validator)
        {
        }

        public MissedPaymentsCreditCalculator(CreditCalculatorRepository data, IValidator validator) : base(data, validator)
        {
        }

        public int CalculateCreditPoints(Customer customer)
        {
            if (ValidationProvider != null)
                ValidationProvider.Validate(customer, data);

            var creditPointsPrevious = 1000;
            var missedPaymentsPoints = 0;

            if (creditCalculator as ICreditPointsCalculator<Customer> != null)
            {
                var creditPointsProvider = (ICreditPointsCalculator<Customer>)creditCalculator;
                creditPointsPrevious = creditPointsProvider.CalculateCreditPoints(customer);
            }

            if (!data.MissedPayments.TryGetValue(customer.MissedPaymentCount, out missedPaymentsPoints))
            {
                var maxMissedPaymentsCount = data.MissedPayments.Max(x => x.Key);
                if (customer.MissedPaymentCount > maxMissedPaymentsCount)
                {
                    missedPaymentsPoints = data.MissedPayments[maxMissedPaymentsCount];
                }
            }

            return creditPointsPrevious + missedPaymentsPoints;
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