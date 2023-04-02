using MasterData.Core;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ZLogger;

namespace MasterData.Generator
{
	public class Generator : ConsoleAppBase
	{
		readonly ILogger _logger;

		public Generator(ILogger<Generator> logger)
		{
			_logger = logger;
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
						GeneratorCore.CreateFiles(excelFileName, outputDirectory, usingNamespace, sheetNames);
					}
					catch (Exception e)
					{
						_logger.ZLogError(e, "Exception GeneratorCore.CreateFiles");
					}

				}));
			}

			//파일 분리 기능도 필요함
			//분리된 파일이지만 Generator도 하나만 Sample.m.new, Sample.m.back   .back은 무시
			//분리된 파일이지만 Convertor는 합침 Sample.m.new, Sample.m.back 

			await Task.WhenAll(tasks);

			try
			{
				GeneratorCore.CreateManager(sheetNames, outputDirectory, usingNamespace);
			}
			catch (Exception e)
			{
				_logger.ZLogError(e, "Exception GeneratorCore.CreateManager");
			}

			sw.Stop();

			_logger.ZLogInformation($"Complete Generator ExcuteDirectory, elapsed: {sw.Elapsed}");
		}
	}
}
