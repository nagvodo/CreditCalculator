using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Zip.Credit
{
    /// <summary>
    /// Represents a data neded to calculate customer's available credit
    /// </summary>
    internal class CreditCalculatorRepository
    {
        public IReadOnlyList<AgeRecord> AgeRecords { get; }
        public ReadOnlyDictionary<int, decimal> AvailableCredits { get; }
        public IReadOnlyList<CreditBureauScoreRecord> CreditBureauScoreRecords { get; }
        public ReadOnlyDictionary<int, int> MissedPayments { get; }
        public ReadOnlyDictionary<int, int> CompletedPayments { get; }
        public CreditCalculatorRepository(IReadOnlyList<AgeRecord> ageRecords,
                                    ReadOnlyDictionary<int, decimal> availableCredits,
                                    IReadOnlyList<CreditBureauScoreRecord> creditBureauScoreRecords,
                                    ReadOnlyDictionary<int, int> missedPayments,
                                    ReadOnlyDictionary<int, int> completedPayments)
        {
            AgeRecords = ageRecords;
            AvailableCredits = availableCredits;
            CreditBureauScoreRecords = creditBureauScoreRecords;
            MissedPayments = missedPayments;
            CompletedPayments = completedPayments;
        }
    }
}