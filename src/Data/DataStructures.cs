using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Zip.Credit
{
    /// <summary>
    /// Represents an Age record
    /// </summary>
    internal struct AgeRecord
    {
        public int YearsBegin { get; }
        public int YearsEnd { get; }
        public int MaximumPoints { get; }
        public AgeRecord(int yearsBegin, int yearsEnd, int maximumPoints)
        {
            YearsBegin = yearsBegin;
            YearsEnd = yearsEnd;
            MaximumPoints = maximumPoints;
        }
    }

    /// <summary>
    /// Represents a record with credit bureau score information
    /// </summary>
    internal struct CreditBureauScoreRecord
    {
        public int BuroScoreBegin { get; }
        public int BuroScoreEnd { get; }
        public int Points { get; }
        public CreditBureauScoreRecord(int buroScoreBegin, int buroScoreEnd, int points)
        {
            BuroScoreBegin = buroScoreBegin;
            BuroScoreEnd = buroScoreEnd;
            Points = points;
        }
    }

    /// <summary>
    /// Represents a record with payment information
    /// </summary>
    internal struct PaymentRecord
    {
        public int Payment { get; }
        public int Points { get; }
        public PaymentRecord(int payment, int points)
        {
            Payment = payment;
            Points = points;
        }
    }
}