namespace StakerGuard.Services.Dtos
{
    public class ValidatorStatus
    {
        public bool IsOk { get; internal set; }
        public double DayPerformance { get; internal set; }
        public double WeekPerformance { get; internal set; }
        public double MonthPerformance { get; internal set; }
        public double ExpectedMonthPerformance { get; internal set; }
        public double ExpectedWeekPerformance { get; internal set; }
        public double ExpectedDayPerformance { get; internal set; }
        public double Balance { get; internal set; }
    }
}