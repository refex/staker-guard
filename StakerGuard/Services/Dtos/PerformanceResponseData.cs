namespace StakerGuard.Services.Dtos
{
    public class PerformanceResponseData
    {
        public long Balance { get; set; }
        public int Performance1d { get; set; }
        public int Performance31d { get; set; }
        public int Performance365d { get; set; }
        public int Performance7d { get; set; }
        public int Validatorindex { get; set; }
    }
}
