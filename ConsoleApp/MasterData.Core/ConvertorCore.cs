using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using System.IO.Packaging;

namespace MasterData.Core
{
    public class ConvertorCore
    {
        public static string IgnoreMemberAttribute = "[IgnoreMember]";

        public static List<MasterData> SavedMasterDatas = new List<MasterData>();

        public static void CreateJson(string fileName, string savePath, string nameSpace, string? sheetName = default)
        {
            Package spreadsheetPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(spreadsheetPackage))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                Workbook workbook = workbookPart.Workbook;

                Sheets sheets = workbook.Sheets;

                foreach (Sheet sheet in sheets.OfType<Sheet>())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(sheetName))
                        {
                            if (sheet.Name.Equals(sheetName)) continue;
                        }

                        var fileType = Util.GetFileType(sheet.Name);

                        switch (fileType)
                        {
                            case FileType.None: break;
                            case FileType.MasterDataNew: SaveMasterData(savePath, nameSpace, workbookPart, sheet); break;
                            case FileType.MasterDataBackup: SaveMasterData(savePath, nameSpace, workbookPart, sheet); break;

                            case FileType.MasterData: CreateMasterDataToJson(savePath, nameSpace, workbookPart, sheet); break;

                            case FileType.Const: CreateConstToJson(savePath, nameSpace, workbookPart, sheet); break;

                            default: break;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public static void CreateSavedMasterDatas(string savePath, string nameSpace)
        {
            foreach (var savedMasterData in SavedMasterDatas)
            {
                StringWriter writer = new StringWriter();
                IConvertorReader reader = new MasterDataReader(savedMasterData);
                ConvertToJson(reader, writer);
                string json = writer.ToString();

                Console.WriteLine(Util.WriteToFile(savePath, savedMasterData.SheetName, json, false, ".json"));
            }
        }

        private static void SaveMasterData(string savePath, string nameSpace, WorkbookPart workbookPart, Sheet sheet)
        {
            MasterData masterData = ExportMasterData(workbookPart, sheet);

            var findMasterData = SavedMasterDatas.Find(_ => _.SheetName.Equals(masterData.SheetName));
            if (findMasterData == null)
            {
                SavedMasterDatas.Add(masterData);
            }
            else
            {
                findMasterData.MergeFrom(masterData);
            }
        }

        private static void CreateConstToJson(string savePath, string nameSpace, WorkbookPart workbookPart, Sheet sheet)
        {
            WorksheetPart worksheetPart = Util.GetWorksheetPart(workbookPart, sheet.Name);
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            var row = sheetData.Descendants<Row>().ToList();

            int totalRowCount = row.Count;

            List<string> dataTypes = new List<string>(totalRowCount);
            for (int i = 0; i < totalRowCount; i++)
            {
                string dataType = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 0);
                dataTypes.Add(dataType.ToLower());
            }

            List<string> names = new List<string>(totalRowCount);
            for (int i = 0; i < totalRowCount; i++)
            {
                string name = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 1);
                names.Add(name);
            }

            List<string> values = new List<string>(totalRowCount);
            for (int i = 0; i < totalRowCount; i++)
            {
                string value = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 2);
                values.Add(value);
            }

            StringWriter writer = new StringWriter();
            IConvertorReader reader = new ConstReader(dataTypes, names, values);
            ConvertToJson(reader, writer);
            string json = writer.ToString();

            Console.WriteLine(Util.WriteToFile(savePath, Path.GetFileNameWithoutExtension(sheet.Name), json, false, ".json"));
        }

        public static readonly int SkipRow = 3;

        public class MasterData
        {
            public MasterData(string sheetName, List<string> dataTypes, List<string> names, List<string> values)
            {
                SheetName = sheetName;
                DataTypes = dataTypes;
                Names = names;
                Values = values;
            }

            public string SheetName { get; set; }
            public List<string> DataTypes { get; set; }
            public List<string> Names { get; set; }
            public List<string> Values { get; set; }

            internal void MergeFrom(MasterData masterData)
            {
                Values.AddRange(masterData.Values);
            }
        }

        private static void CreateMasterDataToJson(string savePath, string nameSpace, WorkbookPart workbookPart, Sheet sheet)
        {
            MasterData masterData = ExportMasterData(workbookPart, sheet);

            StringWriter writer = new StringWriter();
            IConvertorReader reader = new MasterDataReader(masterData);
            ConvertToJson(reader, writer);
            string json = writer.ToString();

            Console.WriteLine(Util.WriteToFile(savePath, Path.GetFileNameWithoutExtension(sheet.Name), json, false, ".json"));
        }

        private static MasterData ExportMasterData(WorkbookPart workbookPart, Sheet sheet)
        {
            WorksheetPart worksheetPart = Util.GetWorksheetPart(workbookPart, sheet.Name);
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            var row = sheetData.Descendants<Row>();

            int totalRowCount = row.Skip(SkipRow).Count();
            int totalColumnCount = row.ElementAt(1).Descendants<Cell>().Count();

            List<int> ignoreIndexs = new List<int>();
            var rowZeroCells = row.ElementAt(0).Elements<Cell>();

            for (int i = 0; i < totalColumnCount; i++)
            {
                string attribute = Util.GetCellValue(workbookPart, rowZeroCells, i);
                if (!attribute.Equals(IgnoreMemberAttribute)) continue;

                ignoreIndexs.Add(i);
            }

            var rowOneCells = row.ElementAt(1).Elements<Cell>();
            var dataTypes = new List<string>(totalColumnCount);
            for (int i = 0; i < totalColumnCount; i++)
            {
                if (ignoreIndexs.Contains(i)) continue;

                string dataType = Util.GetCellValue(workbookPart, rowOneCells, i);
                dataTypes.Add(dataType.ToLower());
            }

            var rowTwoCells = row.ElementAt(2).Elements<Cell>();
            var names = new List<string>(totalColumnCount);
            for (int i = 0; i < totalColumnCount; i++)
            {
                if (ignoreIndexs.Contains(i)) continue;

                string name = Util.GetCellValue(workbookPart, rowTwoCells, i);
                names.Add(name);
            }

            var values = new List<string>(totalRowCount * totalColumnCount);
            for (int i = 0; i < totalRowCount; ++i)
            {
                for (int j = 0; j < totalColumnCount; ++j)
                {
                    if (ignoreIndexs.Contains(j)) continue;

                    string value = Util.GetCellValue(workbookPart, row.ElementAt(i + SkipRow).Elements<Cell>(), j);
                    values.Add(value);
                }
            }

            return new MasterData(Path.GetFileNameWithoutExtension(sheet.Name), dataTypes, names, values);
        }

        public interface IConvertorReader
        {
            int RowLength();
            int ColumnLength();
            string ReadDataType();
            string ReadName();
            string ReadValue();
        }

        public struct ConstReader : IConvertorReader
        {
            private List<string> dataTypes;
            private List<string> names;
            private List<string> values;

            private int rowCount;
            private int columnCount = 1;
            private int valueIndex = 0;

            public ConstReader(List<string> dataTypes, List<string> names, List<string> values) : this()
            {
                this.dataTypes = dataTypes;
                this.names = names;
                this.values = values;

                rowCount = values.Count;
            }

            public int RowLength() => columnCount;
            public int ColumnLength() => rowCount;

            public string ReadDataType()
            {
                return dataTypes[valueIndex];
            }

            public string ReadName()
            {
                return names[valueIndex];
            }

            public string ReadValue()
            {
                return values[valueIndex++];
            }
        }

        public struct MasterDataReader : IConvertorReader
        {
            private List<string> dataTypes;
            private List<string> names;
            private List<string> values;

            private int rowCount;
            private int columnCount;

            private int currentColumnIndex = 0;
            private int currentValueIndex = 0;

            public MasterDataReader(MasterData masterData) : this()
            {
                this.dataTypes = masterData.DataTypes;
                this.names = masterData.Names;
                this.values = masterData.Values;

                columnCount = dataTypes.Count;
                rowCount = values.Count / columnCount;
            }

            public MasterDataReader(List<string> dataTypes, List<string> names, List<string> values) : this()
            {
                this.dataTypes = dataTypes;
                this.names = names;
                this.values = values;

                columnCount = dataTypes.Count;
                rowCount = values.Count / columnCount;
            }

            public int RowLength() => rowCount;
            public int ColumnLength() => columnCount;

            public string ReadDataType()
            {
                return dataTypes[currentColumnIndex];
            }

            public string ReadName()
            {
                return names[currentColumnIndex];
            }

            public string ReadValue()
            {
                UpdateCurrentColumnIndex();

                return values[currentValueIndex++];
            }

            private void UpdateCurrentColumnIndex()
            {
                currentColumnIndex += 1;
                if (currentColumnIndex >= columnCount)
                {
                    currentColumnIndex = 0;
                }
            }
        }

        private static void ConvertToJson(IConvertorReader reader, StringWriter writer)
        {
            int rowLength = reader.RowLength();
            int columnLength = reader.ColumnLength();

            if (rowLength > 1)
            {
                writer.Write("[");
            }

            for (int i = 0; i < rowLength; ++i)
            {
                writer.Write("{");

                for (int j = 0; j < columnLength; ++j)
                {
                    bool isLastColumn = j == columnLength - 1;

                    WriteMap(writer, reader.ReadDataType(), reader.ReadName(), reader.ReadValue(), isLastColumn);
                }

                writer.Write("}");

                bool isLastRow = i == rowLength - 1;
                if (!isLastRow)
                {
                    writer.Write(",");
                }
            }

            if (rowLength > 1)
            {
                writer.Write("]");
            }
        }

        private static void WriteMap(StringWriter writer, string dataType, string name, string value, bool isLast = false)
        {
            WriteKey(writer, name);

            writer.Write(":");

            WriteBody(writer, dataType, value);

            if (!isLast)
            {
                writer.Write(",");
            }
        }

        private static void WriteKey(StringWriter writer, string name)
        {
            WriteJsonString(writer, name);
        }

        private static void WriteBody(StringWriter writer, string dataType, string value)
        {
            if (IsArray(dataType))
            {
                WriteArray(writer, dataType, value);
            }
            else
            {
                WriteObject(writer, dataType, value);
            }
        }

        private static readonly char[] delimiterChars = { ',' };

        private static void WriteArray(StringWriter writer, string dataType, string value)
        {
            string[] values = value.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

            writer.Write("[");

            dataType = GetMiddleString(dataType, "list<", ">");

            for (int i = 0; i < values.Length; i++)
            {
                WriteObject(writer, dataType, values[i]);

                if (i != values.Length - 1)
                {
                    writer.Write(",");
                }
            }

            writer.Write("]");
        }

        public static string GetMiddleString(string str, string begin, string end)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            string result = string.Empty;
            if (str.IndexOf(begin) > -1)
            {
                str = str.Substring(str.IndexOf(begin) + begin.Length);
                if (str.IndexOf(end) > -1) result = str.Substring(0, str.IndexOf(end));
                else result = str;
            }
            return result;
        }

        private static void WriteObject(StringWriter writer, string dataType, string value)
        {
            switch (dataType)
            {
                case "int":
                    writer.Write(int.Parse(value, CultureInfo.InvariantCulture));
                    break;
                case "long":
                    writer.Write(long.Parse(value, CultureInfo.InvariantCulture));
                    break;
                case "float":
                    writer.Write(float.Parse(value, CultureInfo.InvariantCulture));
                    break;
                case "double":
                    writer.Write(double.Parse(value, CultureInfo.InvariantCulture));
                    break;
                case "string":
                    WriteJsonString(writer, value);
                    break;
                case "DropItem":
                    WriteDropItem(writer, value);
                    break;
                default:
                    WriteJsonString(writer, value);
                    break;
            }
        }

        private static void WriteDropItem(StringWriter writer, string value)
        {
            writer.Write("{");

            var reader = new CustomClassReader(value);

            WriteMap(writer, "string", "ItemType", reader.ReadValue());
            WriteMap(writer, "long", "Id", reader.ReadValue());
            WriteMap(writer, "double", "Prob", reader.ReadValue());
            WriteMap(writer, "long", "Count", reader.ReadValue());

            writer.Write("{");
        }

        private static bool IsArray(string dataType)
        {
            return dataType.StartsWith("list<") && dataType.EndsWith(">");
        }

        private static void WriteJsonString(TextWriter writer, string value)
        {
            writer.Write('\"');

            var len = value.Length;
            for (int i = 0; i < len; i++)
            {
                var c = value[i];
                switch (c)
                {
                    case '"':
                        writer.Write("\\\"");
                        break;
                    case '\\':
                        writer.Write("\\\\");
                        break;
                    case '\b':
                        writer.Write("\\b");
                        break;
                    case '\f':
                        writer.Write("\\f");
                        break;
                    case '\n':
                        writer.Write("\\n");
                        break;
                    case '\r':
                        writer.Write("\\r");
                        break;
                    case '\t':
                        writer.Write("\\t");
                        break;
                    default:
                        writer.Write(c);
                        break;
                }
            }

            writer.Write('\"');
        }
    }

    public struct CustomClassReader
    {
        private string[] values;

        private int index;

        public CustomClassReader(string value)
        {
            string[] datas = value.Split(new char[] { '(', ')', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            values = datas[0].Split(':');
            index = 0;
        }

        public string ReadValue()
        {
            return values[index++];
        }
    }
}
