using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StakerGuard.Options;
using StakerGuard.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StakerGuard
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEth2Service _eth2Service;
        private readonly GuardOptions _guardOptions;

        public Worker(ILogger<Worker> logger, IEth2Service eth2Service, IOptions<GuardOptions> guardOptions)
        {
            _logger = logger;
            _eth2Service = eth2Service;
            _guardOptions = guardOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    await WatchValidators();
                }
                catch (Exception ex)
                {
                    _logger.LogError(new EventId(1, "Fatal"), ex, "Worker encountered a fatal error: {0}.", ex.Message);
                }

#if DEBUG
                await Task.Delay(1000, stoppingToken);
#else
                await Task.Delay(3 * 60000, stoppingToken);
#endif
            }
        }

        private async Task WatchValidators()
        {
            foreach (var validatorOption in _guardOptions.ValidatorOptions)
            {
                var validatorStatus = await _eth2Service.CheckValidator(validatorOption.PublicKey);

                if (!validatorStatus.IsOk)
                {
                    _logger.LogWarning($"Something's wrong with validator {validatorOption.PublicKey}.");
                    await Notify(validatorOption, validatorStatus);
                }
            }
        }

        private async Task Notify(ValidatorOption validatorOption, Services.Dtos.ValidatorStatus validatorStatus)
        {
            var botClient = new TelegramBotClient(_guardOptions.TelegramBotToken);
            await botClient.SendTextMessageAsync(
                 chatId: new ChatId(validatorOption.TelegramAlertChatId),
                 parseMode: ParseMode.Markdown,
                 text: $"Your validator is NOT ok." +
                       $"{Environment.NewLine}{Environment.NewLine}" +
                       $"Balance: {validatorStatus.Balance} Ξ" +
                       $"{Environment.NewLine}" +
                       $"Day performance: {validatorStatus.DayPerformance} Ξ" +
                       $"{Environment.NewLine}" +
                       $"Week performance: {validatorStatus.WeekPerformance} Ξ" +
                       $"{Environment.NewLine}" +
                       $"Month performance: {validatorStatus.MonthPerformance} Ξ" +
                       $"{Environment.NewLine}{Environment.NewLine}" +
                       $"[Please check](https://beaconcha.in/validator/{validatorOption.PublicKey})"
                 );
        }
    }
}
