using KC;
using Kokuban;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ZLogger;

namespace MasterData.Validator
{
	internal class Validator : ConsoleAppBase
	{
		readonly ILogger _logger;

		public Validator(ILogger<Validator> logger)
		{
			_logger = logger;
		}

		[Command("excutedirectory")]
		public async Task ExcuteDirectory([Option("i", "Input file directory.")] string inputDirectory)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			MasterDataManager.SetupMessagePackResolver();

			MasterDataManager manager = new MasterDataManager();

			_logger.ZLogInformation($"Start LoadSequentialAsync".ToStyledString(Chalk.Yellow));

			await manager.LoadSequentialAsync(inputDirectory, default);

			_logger.ZLogInformation($"Complete LoadSequentialAsync, elapsed: {sw.Elapsed}".ToStyledString(Chalk.Green));

			_logger.ZLogInformation($"Start Validate".ToStyledString(Chalk.Yellow));

			var validateResult = manager.DB.Validate();
			if (validateResult.IsValidationFailed)
			{
				sw.Stop();

				_logger.ZLogError(validateResult.FormatFailedResults().ToStyledString(Chalk.Red));
				_logger.ZLogInformation($"Failed Validation Generator ExcuteDirectory, elapsed: {sw.Elapsed}".ToStyledString(Chalk.Red));
			}
			else
			{
				sw.Stop();
				_logger.ZLogInformation($"Complete Generator ExcuteDirectory, elapsed: {sw.Elapsed}".ToStyledString(Chalk.Green));
			}
		}
	}
}
