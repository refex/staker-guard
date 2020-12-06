using System.Collections.Generic;

namespace StakerGuard.Options
{
    public class GuardOptions
    {
        public const string SettingsName = "Guard";
        //public string BeaconChainApiKey { get; set; }
        public string TelegramBotToken { get; set; }
        public IList<ValidatorOption> ValidatorOptions { get; set; }
    }
}