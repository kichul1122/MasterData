using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using System.IO.Packaging;
using System.Text;

namespace MasterData.Core
{
	public enum FileType { None, MasterData, MasterDataNew, MasterDataBackup, Enum, Const }

	public class Util
	{

		public readonly static List<string> IncludeExtensions = new List<string>() { ".xlsm", ".xlsx" };
		public const string MasterDataBackupFileExtension = ".mb";
		public const string MasterDataNewFileExtension = ".mn";
		public const string MasterDataFileExtension = ".m";
		public const string EnumFileExtension = ".e";
		public const string ConstFileExtension = ".c";

		public static FileType GetFileType(string sheetName)
		{
			if (string.IsNullOrWhiteSpace(sheetName)) return FileType.None;

			string extension = Path.GetExtension(sheetName).ToLower();

			return extension switch
			{
				".mb" => FileType.MasterDataBackup,
				".mn" => FileType.MasterDataNew,
				".m" => FileType.MasterData,
				".e" => FileType.Enum,
				".c" => FileType.Const,
				_ => FileType.None,
			};
		}

		//public const string SvaePath = @"../../../cs";
		//public const string DirectoryPath = @"C:\UnityProject\god\MasterData";

		public static string GetExcelFile(string inputFile)
		{
			FileInfo fileInfo = new FileInfo(inputFile);

			if (IncludeExtensions.FindIndex(extension => fileInfo.Extension.Equals(extension)) != -1)
			{
				return fileInfo.FullName;
			}

			throw new Exception("Not IncludeExtensions - GetExcelFile");
		}

		public static List<string> GetExcelFiles(DirectoryInfo directory)
		{

			List<string> excelFileList = new List<string>();

			foreach (var item in directory.GetFiles())
			{
				if (item.Name.Contains(@"~$")) continue;
				if (IncludeExtensions.FindIndex(extension => item.Extension.Equals(extension)) != -1)
				{
					excelFileList.Add(item.FullName);
				}
			}

			return excelFileList;
		}

		//public static object ParseValue(string type, string rawValue)
		//      {
		//	if (type.Equals(nameof())) return rawValue;


		//          return null;
		//}

		public static object ParseValue(Type type, string rawValue)
		{
			if (type == typeof(string)) return rawValue;

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				if (string.IsNullOrWhiteSpace(rawValue)) return null;
				return ParseValue(type.GenericTypeArguments[0], rawValue);
			}

			if (type.IsEnum)
			{
				var value = Enum.Parse(type, rawValue);
				return value;
			}

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					// True/False or 0,1
					if (int.TryParse(rawValue, out var intBool))
					{
						return Convert.ToBoolean(intBool);
					}
					return Boolean.Parse(rawValue);
				case TypeCode.Char:
					return Char.Parse(rawValue);
				case TypeCode.SByte:
					return SByte.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Byte:
					return Byte.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Int16:
					return Int16.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.UInt16:
					return UInt16.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Int32:
					return Int32.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.UInt32:
					return UInt32.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Int64:
					return Int64.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.UInt64:
					return UInt64.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Single:
					return Single.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Double:
					return Double.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.Decimal:
					return Decimal.Parse(rawValue, CultureInfo.InvariantCulture);
				case TypeCode.DateTime:
					return DateTime.Parse(rawValue, CultureInfo.InvariantCulture);
				default:
					if (type == typeof(DateTimeOffset))
					{
						return DateTimeOffset.Parse(rawValue, CultureInfo.InvariantCulture);
					}
					else if (type == typeof(TimeSpan))
					{
						return TimeSpan.Parse(rawValue, CultureInfo.InvariantCulture);
					}
					else if (type == typeof(Guid))
					{
						return Guid.Parse(rawValue);
					}

					// or other your custom parsing.
					throw new NotSupportedException();
			}
		}

		//public static void ReadExcelFileSAX(string fileName)
		//{
		//	//Console.WriteLine($"Start threadId: { System.Environment.CurrentManagedThreadId} fileName: {fileName}");

		//	//해당 파일이 열려있어도 IO가 가능한 세팅
		//	Package spreadsheetPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

		//	using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(spreadsheetPackage))
		//	{
		//		WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

		//		IEnumerable<WorksheetPart> worksheetParts = workbookPart.WorksheetParts;

		//		Workbook workbook = workbookPart.Workbook;

		//		Sheets sheets = workbook.Sheets;

		//		SharedStringTable sharedStringTable = spreadsheetDocument.WorkbookPart.SharedStringTablePart.SharedStringTable;

		//		foreach (Sheet sheet in sheets.OfType<Sheet>())
		//		{
		//			try
		//			{
		//				//DataTable dt = new DataTable();

		//				if (!sheet.Name?.Value?.Contains(".m") ?? false) continue;

		//				WorksheetPart worksheetPart = GetWorksheetPart(workbookPart, sheet.Name);

		//				SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
		//				IEnumerable<Row> rows = sheetData.Descendants<Row>();

		//				int totalHeaderCount = sheetData.Descendants<Row>().ElementAt(1).Descendants<Cell>().Count();

		//				MasterDataTemplate masterDataTemplate = new MasterDataTemplate();
		//				masterDataTemplate.PrefixClassName = Path.GetFileNameWithoutExtension(sheet.Name);
		//				masterDataTemplate.Namespace = "GOD";
		//				masterDataTemplate.Properties = new Property[totalHeaderCount];

		//				for (int i = 0; i < totalHeaderCount; i++)
		//				{
		//					masterDataTemplate.Properties[i] = new Property();

		//					string attribute = GetCellValue(workbookPart, sheetData.Descendants<Row>().ElementAt(0).Elements<Cell>().ToList(), i);

		//					masterDataTemplate.Properties[i].Attribute = attribute.Replace("\n", "\n\t");

		//					string dataType = GetCellValue(workbookPart, sheetData.Descendants<Row>().ElementAt(1).Elements<Cell>().ToList(), i);
		//					masterDataTemplate.Properties[i].DataType = dataType;

		//					string name = GetCellValue(workbookPart, sheetData.Descendants<Row>().ElementAt(2).Elements<Cell>().ToList(), i);
		//					masterDataTemplate.Properties[i].Name = name;
		//				}

		//				Console.WriteLine(WriteToFile(SvaePath, masterDataTemplate.ClassName, masterDataTemplate.TransformText(), true));

		//				//Console.WriteLine("-------------------");

		//				//for (int i = 1; i <= totalHeaderCount; i++)
		//				//{
		//				//	string cellvalue = GetCellValue(workbookPart, sheetData.Descendants<Row>().ElementAt(0).Elements<Cell>().ToList(), i);
		//				//	Console.WriteLine(cellvalue);
		//				//	//dt.Columns.Add(cellvalue);
		//				//}

		//				//Console.WriteLine("-------------------");

		//				//foreach (Row r in sheetData.Descendants<Row>())
		//				//{
		//				//	//if (r.RowIndex > 0)
		//				//	{
		//				//		//DataRow tempRow = dt.NewRow();

		//				//		for (int i = 1; i <= totalHeaderCount; i++)
		//				//		{
		//				//			string cellValue = GetCellValue(workbookPart, r.Elements<Cell>().ToList(), i);
		//				//			Console.WriteLine(cellValue);
		//				//			//tempRow[i - 1] = cellValue;
		//				//		}
		//				//		//dt.Rows.Add(tempRow);
		//				//	}

		//				//	Console.WriteLine("-------------------");
		//				//}
		//			}
		//			catch
		//			{
		//				File.WriteAllTextAsync(@$"{SvaePath}\Error_{sheet.Name}", string.Empty);
		//			}
		//		}
		//	}
		//}

		public static string GetCellValue(WorkbookPart wbPart, IEnumerable<Cell> theCells, string cellColumnReference)
		{
			Cell theCell = null;
			string value = "";
			foreach (Cell cell in theCells)
			{
				if (cell.CellReference.Value.StartsWith(cellColumnReference))
				{
					theCell = cell;
					break;
				}
			}

			if (theCell != null)
			{
				value = theCell.InnerText;
				// If the cell represents an integer number, you are done. 
				// For dates, this code returns the serialized value that represents the date. The code handles strings and 
				// Booleans individually. For shared strings, the code looks up the corresponding value in the shared string table. For Booleans, the code converts the value into the words TRUE or FALSE.
				if (theCell.DataType != null)
				{
					switch (theCell.DataType.Value)
					{
						case CellValues.SharedString:
							// For shared strings, look up the value in the shared strings table.
							var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
							// If the shared string table is missing, something is wrong. Return the index that is in the cell. Otherwise, look up the correct text in the table.
							if (stringTable != null)
							{
								value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
							}
							break;
						case CellValues.Boolean:
							switch (value)
							{
								case "0":
									value = "FALSE";
									break;
								default:
									value = "TRUE";
									break;
							}
							break;
					}
				}
			}
			return value;
		}

		public static string GetCellValue(WorkbookPart wbPart, IEnumerable<Cell> theCells, int index)
		{
			return GetCellValue(wbPart, theCells, GetExcelColumnName(index + 1));
		}

		public static string GetExcelColumnName(int columnNumber)
		{
			int dividend = columnNumber;
			string columnName = string.Empty;
			int modulo;
			while (dividend > 0)
			{
				modulo = (dividend - 1) % 26;
				columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
				dividend = (dividend - modulo) / 26;
			}
			return columnName;
		}

		public static WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName)
		{
			string relId = workbookPart.Workbook.Descendants<Sheet>().First(s => sheetName.Equals(s.Name)).Id;
			return (WorksheetPart)workbookPart.GetPartById(relId);
		}

		public static Sheets GetAllWorksheets(string fileName)
		{
			Sheets theSheets = null;

			Package spreadsheetPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

			using (SpreadsheetDocument document = SpreadsheetDocument.Open(spreadsheetPackage))
			{
				WorkbookPart wbPart = document.WorkbookPart;
				theSheets = wbPart.Workbook.Sheets;
			}
			return theSheets;
		}

		public static string NormalizeNewLines(string content)
		{
			// The T4 generated code may be text with mixed line ending types. (CR + CRLF)
			// We need to normalize the line ending type in each Operating Systems. (e.g. Windows=CRLF, Linux/macOS=LF)
			return content.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
		}

		public static string WriteToFile(string directory, string fileName, string content, bool forceOverwrite, string extension = ".cs")
		{
			var path = Path.Combine(directory, $"{fileName}{extension}");
			var contentBytes = Encoding.UTF8.GetBytes(NormalizeNewLines(content));

			// If the generated content is unchanged, skip the write.
			if (!forceOverwrite && File.Exists(path))
			{
				if (new FileInfo(path).Length == contentBytes.Length && contentBytes.AsSpan().SequenceEqual(File.ReadAllBytes(path)))
				{
					return $"Generate {fileName} to: {path} (Skipped)";
				}
			}

			File.WriteAllBytes(path, contentBytes);
			return $"Generate {fileName} to: {path}";
		}
	}
}
