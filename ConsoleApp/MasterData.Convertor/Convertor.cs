using MasterData.Core;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ZLogger;

namespace MasterData.Convertor
{
	public class Convertor : ConsoleAppBase
	{
		readonly ILogger _logger;

		public Convertor(ILogger<Convertor> logger)
		{
			_logger = logger;
		}

		[Command("excutesheet")]
		public async Task ExcuteSheet(
			[Option("i", "Input file.")] string inputFile,
			[Option("s", "Input sheet.")] string inputSheet,
			[Option("o", "Output file directory.")] string outputDirectory,
			[Option("n", "Namespace of converted files.")] string usingNamespace)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			await Task.Run(() =>
			{
				try
				{
					string excelFileName = Util.GetExcelFile(inputFile);

					ConvertorCore.CreateJson(excelFileName, outputDirectory, usingNamespace, inputSheet);
				}
				catch (Exception e)
				{
					_logger.ZLogError(e, "Exception ConvetorCore.CreateJson");
				}

			});

			sw.Stop();

			_logger.ZLogInformation($"Complete Convertor Excute, elapsed: {sw.Elapsed}");
		}

		[Command("excutefile")]
		public async Task ExcuteFile(
			[Option("i", "Input file.")] string inputFile,
			[Option("o", "Output file directory.")] string outputDirectory,
			[Option("n", "Namespace of converted files.")] string usingNamespace)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			await Task.Run(() =>
			{
				try
				{
					string excelFileName = Util.GetExcelFile(inputFile);

					ConvertorCore.CreateJson(excelFileName, outputDirectory, usingNamespace);
				}
				catch (Exception e)
				{
					_logger.ZLogError(e, "Exception ConvetorCore.CreateJson");
				}

			});

			sw.Stop();

			_logger.ZLogInformation($"Complete Convertor Excute, elapsed: {sw.Elapsed}");
		}

		[Command("excutedirectory")]
		public async Task ExcuteDirectory(
			[Option("i", "Input file directory.")] string inputDirectory,
			[Option("o", "Output file directory.")] string outputDirectory,
			[Option("n", "Namespace of converted files.")] string usingNamespace)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			DirectoryInfo directory = new DirectoryInfo(inputDirectory);

			List<string> excelFileNames = Util.GetExcelFiles(directory);

			List<Task> tasks = new List<Task>();

			List<string> sheetNames = new List<string>();

			foreach (var excelFileName in excelFileNames)
			{
				tasks.Add(Task.Run(() =>
				{
					try
					{
						ConvertorCore.CreateJson(excelFileName, outputDirectory, usingNamespace);
					}
					catch (Exception e)
					{
						_logger.ZLogError(e, "Exception ConvetorCore.CreateJson");
					}

				}));
			}

			await Task.WhenAll(tasks);

			try
			{
				ConvertorCore.CreateSavedMasterDatas(outputDirectory, usingNamespace);
			}
			catch (Exception e)
			{
				_logger.ZLogError(e, "Exception ConvetorCore.CreateSavedMasterDatas");
			}

			sw.Stop();

			_logger.ZLogInformation($"Complete Convertor Excute, elapsed: {sw.Elapsed}");
		}
	}
}
