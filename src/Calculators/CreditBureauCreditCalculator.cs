using System.Linq;

using static System.Math;

namespace Zip.Credit
{
    /// <summary>
    /// Implementation of ICreditCalculator
    /// </summary>
    internal class CreditBureauCreditCalculator : AbstractCreditCalculator, ICreditPointsCalculator<Customer>, ICreditCalculator
    {
        public CreditBureauCreditCalculator(CreditCalculatorRepository data) : base(data)
        {
        }

        public CreditBureauCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data) : base(creditCalculator, data)
        {
        }

        public CreditBureauCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data, IValidator validator) : base(creditCalculator, data, validator)
        {
        }

        public CreditBureauCreditCalculator(CreditCalculatorRepository data, IValidator validator) : base(data, validator)
        {
        }

        public int CalculateCreditPoints(Customer customer)
        {
            if (ValidationProvider != null)
                ValidationProvider.Validate(customer, data);

            var creditPointsPrevious = 1000;
            var creditPointsNew = 1000;

            if (creditCalculator as ICreditPointsCalculator<Customer> != null)
            {
                var creditPointsProvider = (ICreditPointsCalculator<Customer>)creditCalculator;
                creditPointsPrevious = creditPointsProvider.CalculateCreditPoints(customer);
            }

            var pointsCollection = data.CreditBureauScoreRecords
                                        .Where(r => (customer.BureauScore >= r.BuroScoreBegin && customer.BureauScore <= r.BuroScoreEnd))
                                        .Select(r => r.Points);

            if (pointsCollection.Any())
            {
                creditPointsNew = pointsCollection.First();
            }
            else
            {
                //assume that bureau score > 1000 is a valid score
                var maxScore = data.CreditBureauScoreRecords.Max(r => r.BuroScoreEnd);
                if (customer.BureauScore > maxScore)
                {
                    creditPointsNew = data.CreditBureauScoreRecords.First(r => r.BuroScoreEnd == maxScore).Points;
                }
            }

            return Min(creditPointsPrevious, creditPointsNew);
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