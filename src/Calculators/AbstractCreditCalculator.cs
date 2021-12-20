using System;

namespace Zip.Credit
{
    internal abstract class AbstractCreditCalculator
    {
        #region read-only fiels
        protected readonly ICreditCalculator creditCalculator;
        protected readonly CreditCalculatorRepository data;
        public IValidator ValidationProvider { get; }
        #endregion

        #region constructors
        protected AbstractCreditCalculator(CreditCalculatorRepository data)
        {
            this.data = data;
        }

        protected AbstractCreditCalculator(CreditCalculatorRepository data, IValidator validator)
        {
            this.data = data;
            this.ValidationProvider = validator;
        }
        protected AbstractCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data)
        {
            this.creditCalculator = creditCalculator;
            this.data = data;
        }

        protected AbstractCreditCalculator(ICreditCalculator creditCalculator, CreditCalculatorRepository data, IValidator validator)
        {
            this.creditCalculator = creditCalculator;
            this.data = data;
            this.ValidationProvider = validator;
        }

        protected void WriteToConsole(string text) => Console.WriteLine(text);
        #endregion
    }
}