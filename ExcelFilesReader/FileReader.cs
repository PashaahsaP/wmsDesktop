using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using Tabula;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace ExcelFileParser
{
    public class Supplier//dto
    {
        public int Id { get; set; }
        public int SupplierType { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
    public class FileReader
    {
        public List<FileInfo> filesInfo = new List<FileInfo>();
        public List<string> TablesList { get; set; }
        public FileReader(string path, List<(Supplier, bool)> suppliers)
        {
            var split = path.Split('.');
            var ext = split[split.Length - 1];
            // надо сделать коллекцию для таблиц
            // дефолтно присваивать первый элемент для data
            if (ext == "pdf")
            {
                using (PdfDocument document = PdfDocument.Open(path))
                {
                    // 2. Создаем объект для парсинга структуры страниц Tabula
                    PageArea page = ObjectExtractor.ExtractPage(document, 1);

                    // 3. Используем алгоритм SpreadsheetExtractionAlgorithm 
                    // (подходит для таблиц с видимыми линиями/границами)
                    var algorithm = new SpreadsheetExtractionAlgorithm();


                    // Извлекаем все таблицы со страницы
                    var tables = algorithm.Extract(page).Where(inner => inner.Cells.Count > 5);
                    TablesList = tables.Select(inner => $"Таблица {inner.Rows.Count}").ToList();
                    foreach (var table in tables)
                    {
                        var newFileInfo = new FileInfo(path, suppliers) { FileType = ext };
                        var data = new List<List<string>>();
                        foreach (var row in table.Rows)
                        {
                            data.Add(row.Select(item => item.TextElements.Count == 0 ? "" : item.TextElements[0].GetText())
                                .ToList());//надо нормальный данные получить, сделал чтобы ошибки исправить
                            Console.WriteLine();
                        }
                        newFileInfo.Data = data;
                        filesInfo.Add(newFileInfo);
                    }
                }
                Console.WriteLine();
            }
            else if (ext == "xlsx")
            {
                using (var package = new ExcelPackage(path))
                {
                    var lines = new List<List<string>>();
                    var ws = package.Workbook.Worksheets[0];
                    var newFileInfo = new FileInfo(path, suppliers) { FileType = ext };
                    int lastRow = 0;
                    int lastColumn = 0;
                    if (ws.Dimension != null)
                    {
                        var notEmptyCells = ws.Cells.Where(cell => cell.Value != null);

                        if (notEmptyCells.Any())
                        {
                            lastRow = notEmptyCells.Max(cell => cell.Start.Row);
                            lastColumn = notEmptyCells.Max(cell => cell.Start.Column);
                        }
                    }

                    for (int row = 1; row < lastRow + 1; row++)
                    {
                        var temp = new List<string>();
                        for (int column = 1; column < lastColumn + 1; column++)
                        {
                            var data = ws.Cells[row, column].Text;
                            temp.Add(data);
                        }
                        lines.Add(new List<string>(temp));
                    }
                    newFileInfo.Data = lines;
                    filesInfo.Add(newFileInfo);
                }
            }
            else
            {

            }
        }
    }
}
