# staker-guard

StakerGuard is a very basic C# worker for checking the health of one or multiple Ethereum 2 validator.
Validators are notified by a Telegram bot.

## Installation

This project uses dotnet core 3.1 and Docker so building and running it is standard and trivial.
Just remember to edit the appSettings.json before.

## Configuration

You need to configure the appSettings.json with the:
- Telegram bot's token from a bot you created
- A collection of couples:
  - Validator's public key
  - Telegram target user's chatId

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)
