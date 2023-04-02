using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MasterData.Core.GeneratorTemplate;
using System.IO.Packaging;

namespace MasterData.Core
{
    public class GeneratorCore
    {
        public static string IgnoreMemberAttribute = "[IgnoreMember]";

        public static void CreateManager(List<string> sheetNames, string savePath, string nameSpace)
        {
            MasterManagerTemplate template = new MasterManagerTemplate();
            template.Namespace = nameSpace;
            template.Name = "MasterDataManager";

            var properties = sheetNames.Where(sheetName =>
            {
                var fileType = Util.GetFileType(sheetName);
                return fileType == FileType.MasterData || fileType == FileType.Const;
            }).Select(sheetName => new { name = Path.GetFileNameWithoutExtension(sheetName), fileType = Util.GetFileType(sheetName).ToString() }).ToArray();

            template.Properties = new Property[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                var property = new Property();
                property.Name = properties[i].name;
                property.DataType = properties[i].fileType;

                template.Properties[i] = property;
            }

            template.Properties = template.Properties.OrderBy(_ => _.Name).ToArray();

            Console.WriteLine(Util.WriteToFile(savePath, template.Name, template.TransformText(), false));
        }

        public static void CreateFiles(string fileName, string savePath, string nameSpace, List<string> sheetNames)
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
                        sheetNames.Add(sheet.Name);

                        var fileType = Util.GetFileType(sheet.Name);

                        switch (fileType)
                        {
                            case FileType.None: break;

                            case FileType.MasterDataNew: CreateMasterDataFile(savePath, nameSpace, workbookPart, sheet); break;
                            case FileType.MasterData: CreateMasterDataFile(savePath, nameSpace, workbookPart, sheet); break;

                            case FileType.Enum: CreateEnumFile(savePath, nameSpace, workbookPart, sheet); break;

                            case FileType.Const: CreateConstFile(savePath, nameSpace, workbookPart, sheet); break;

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

        private static void CreateMasterDataFile(string savePath, string nameSpace, WorkbookPart workbookPart, Sheet sheet)
        {
            WorksheetPart worksheetPart = Util.GetWorksheetPart(workbookPart, sheet.Name);

            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            int totalColumnCount = sheetData.Descendants<Row>().ElementAt(1).Descendants<Cell>().Count();

            MasterDataTemplate masterDataTemplate = new MasterDataTemplate();
            masterDataTemplate.PrefixClassName = Path.GetFileNameWithoutExtension(sheet.Name) ?? string.Empty;
            masterDataTemplate.Namespace = nameSpace;

            var row = sheetData.Descendants<Row>().ToList();
            var rowZeroCells = row.ElementAt(0).Elements<Cell>();
            var rowOneCells = row.ElementAt(1).Elements<Cell>();
            var rowTwoCells = row.ElementAt(2).Elements<Cell>();

            List<Property> properties = new List<Property>(totalColumnCount);
            for (int i = 0; i < totalColumnCount; i++)
            {
                string attribute = Util.GetCellValue(workbookPart, rowZeroCells, i).Replace("\n", "\n\t");

                if (attribute.Equals(IgnoreMemberAttribute)) continue;

                var propertie = new Property();

                propertie.Attribute = attribute;

                string dataType = Util.GetCellValue(workbookPart, rowOneCells, i);
                propertie.DataType = dataType;

                string name = Util.GetCellValue(workbookPart, rowTwoCells, i);
                propertie.Name = name;

                properties.Add(propertie);
            }

            masterDataTemplate.Properties = properties.ToArray();

            Console.WriteLine(Util.WriteToFile(savePath, masterDataTemplate.ClassName, masterDataTemplate.TransformText(), false));
        }

        private static void CreateConstFile(string savePath, string nameSpace, WorkbookPart workbookPart, Sheet sheet)
        {
            WorksheetPart worksheetPart = Util.GetWorksheetPart(workbookPart, sheet.Name);

            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            int totalRowCount = sheetData.Descendants<Row>().Count();

            ConstTemplate constTemplate = new ConstTemplate();
            constTemplate.PrefixClassName = Path.GetFileNameWithoutExtension(sheet.Name) ?? string.Empty;
            constTemplate.Namespace = nameSpace;
            constTemplate.Properties = new Property[totalRowCount];

            var row = sheetData.Descendants<Row>().ToList();

            for (int i = 0; i < totalRowCount; i++)
            {
                var property = new Property();

                string dataType = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 0);
                property.DataType = dataType;

                string name = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 1);
                property.Name = name;

                constTemplate.Properties[i] = property;
            }

            Console.WriteLine(Util.WriteToFile(savePath, constTemplate.ClassName, constTemplate.TransformText(), false));
        }

        private static void CreateEnumFile(string savePath, string nameSpace, WorkbookPart workbookPart, Sheet sheet)
        {
            WorksheetPart worksheetPart = Util.GetWorksheetPart(workbookPart, sheet.Name);

            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            int totalRowCount = sheetData.Descendants<Row>().Count();

            EnumTemplate enumTemplate = new EnumTemplate();
            enumTemplate.PrefixClassName = Path.GetFileNameWithoutExtension(sheet.Name) ?? string.Empty;
            enumTemplate.Namespace = nameSpace;
            enumTemplate.Properties = new Property[totalRowCount];

            var row = sheetData.Descendants<Row>().ToList();

            for (int i = 0; i < totalRowCount; i++)
            {
                var property = new Property();

                string value = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 0);
                property.Value = value;

                string name = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 1);
                property.Name = name;

                string comment = Util.GetCellValue(workbookPart, row.ElementAt(i).Elements<Cell>(), 2);
                property.Comment = comment;

                enumTemplate.Properties[i] = property;
            }

            Console.WriteLine(Util.WriteToFile(savePath, enumTemplate.ClassName, enumTemplate.TransformText(), false));
        }
    }
}