using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class MonthSummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthLabel { get; set; }   // "Oca 25" formatı
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Net => TotalIncome - TotalExpense;
        public bool IsPositive => Net >= 0;
    }

    public class CategorySummary
    {
        public string Category { get; set; }
        public decimal Total { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
    }

    public class ReportTransaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncome { get; set; }
    }

    public class ReportViewModel
    {
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }

        public decimal PeriodIncome { get; set; }
        public decimal PeriodExpense { get; set; }
        public decimal PeriodNet => PeriodIncome - PeriodExpense;

        public decimal AllTimeIncome { get; set; }
        public decimal AllTimeExpense { get; set; }
        public decimal AllTimeNet => AllTimeIncome - AllTimeExpense;

        public List<MonthSummary> MonthlyHistory { get; set; } = new List<MonthSummary>();
        public List<CategorySummary> CategoryBreakdown { get; set; } = new List<CategorySummary>();
        public List<ReportTransaction> Transactions { get; set; } = new List<ReportTransaction>();
        public List<int> AvailableYears { get; set; } = new List<int>();

        // PDF için hazır string listeler (JSON serialize edilmiş)
        public string PeriodLabel { get; set; }
    }
}