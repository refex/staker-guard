using Newtonsoft.Json;
using StakerGuard.Services.Dtos;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StakerGuard.Services
{
    public class BeaconChainService : IEth2Service
    {
        private readonly HttpClient _httpClient;
        private const int GweiForEth = 1000000000;
        public BeaconChainService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://beaconcha.in/api/v1/");
        }

        public async Task<ValidatorStatus> CheckValidator(string publicKey)
        {
            var performance = await GetValidatorPerformance(publicKey);

            return new ValidatorStatus
            {
                IsOk = performance.Status == "OK",
                DayPerformance = GweiToEth(performance.Data.Performance1d),
                WeekPerformance = GweiToEth(performance.Data.Performance7d),
                MonthPerformance = GweiToEth(performance.Data.Performance31d),
                Balance = GweiToEth(performance.Data.Balance)
            };
        }

        private static double GweiToEth(long performance)
        {
            return Math.Round((double)performance / GweiForEth, 4);
        }

        private async Task<PerformanceResponse> GetValidatorPerformance(string publicKey)
        {
            var responseString = await _httpClient.GetStringAsync($"validator/{publicKey}/performance");
            return JsonConvert.DeserializeObject<PerformanceResponse>(responseString);
        }
    }
}