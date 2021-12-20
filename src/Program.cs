using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using static System.Console;



namespace Zip.Credit
{
    class Program
    {
        static void Main(string[] args)
        {
            var ageRecords = new List<AgeRecord>(4)
            {
                new AgeRecord(18, 25, 3),
                new AgeRecord(26, 35, 4),
                new AgeRecord(36, 50, 5),
                new AgeRecord(51, int.MaxValue, 6)
            };

            var availableCredits = new Dictionary<int, decimal>(7)
            {
                { 0, 0 },
                { 1, 100 },
                { 2, 200 },
                { 3, 300 },
                { 4, 400 },
                { 5, 500 },
                { 6, 600 }
            };

            var creditBureauRecords = new List<CreditBureauScoreRecord>(3)
            {
                new CreditBureauScoreRecord(451, 700, 1),
                new CreditBureauScoreRecord(701, 850, 2),
                new CreditBureauScoreRecord(851, 1000, 3)
            };

            var missedPayments = new Dictionary<int, int>(4)
            {
                { 0, 0 },
                { 1, -1 },
                { 2, -3 },
                { 3, -6 }
            };

            var completedPayments = new Dictionary<int, int>(4)
            {
                { 0, 0 },
                { 1, 2 },
                { 2, 3 },
                { 3, 4 }
            };

            var creditCalculatorRepository = new CreditCalculatorRepository(
                                    ageRecords.AsReadOnly(),
                                    new ReadOnlyDictionary<int, decimal>(availableCredits),
                                    creditBureauRecords.AsReadOnly(),
                                    new ReadOnlyDictionary<int, int>(missedPayments),
                                    new ReadOnlyDictionary<int, int>(completedPayments));
            while (true)
            {
                WriteLine("Please type in Customer's age");
                
                var age = int.Parse(ReadLine());

                WriteLine("Please type in Customers's missed payment count");
                var missedPaymentCount = int.Parse(ReadLine());

                WriteLine("Please type in Customer's completed payment count");
                var completedPaymentCount = int.Parse(ReadLine());

                WriteLine("Please type in Customer's credit bureau score (from 0 to 1000)");
                var creditBureauScore = int.Parse(ReadLine());

                try
                {
                    var customer = new Customer(creditBureauScore, missedPaymentCount, completedPaymentCount, age);
                    
                    var ageAndCreditBureauScoreValidator = new AgeValidator(new CreditBureauScoreValidator());

                    var creditBureauCreditCalculator = new CreditBureauCreditCalculator(creditCalculatorRepository, ageAndCreditBureauScoreValidator);
                    var missedPaymentsCreditCalculator = new MissedPaymentsCreditCalculator(creditBureauCreditCalculator, creditCalculatorRepository);
                    var completedPaymentsCreditCalculator = new CompletedPaymentsCreditCalculator(missedPaymentsCreditCalculator, creditCalculatorRepository);
                    var ageCreditCalculator = new AgeRestrictionCreditCalculator(completedPaymentsCreditCalculator, creditCalculatorRepository);

                    var res = ageCreditCalculator.CalculateCredit(customer);
                    WriteLine("Customer's available credit is {0}", res);
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }

                ReadLine();
            }

        }
    }
}
