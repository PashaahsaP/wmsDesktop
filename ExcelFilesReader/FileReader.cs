using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
        public FileInfo fileInfo = null;
        public FileReader(string path, List<(Supplier, bool)> suppliers)
        {
            fileInfo = new FileInfo(path, suppliers);
            //
            using (PdfDocument document = PdfDocument.Open(fileInfo.Path))
            {
                // 2. Создаем объект для парсинга структуры страниц Tabula
                PageArea page = ObjectExtractor.ExtractPage(document, 1);

                // 3. Используем алгоритм SpreadsheetExtractionAlgorithm 
                // (подходит для таблиц с видимыми линиями/границами)
                var algorithm = new SpreadsheetExtractionAlgorithm();


                // Извлекаем все таблицы со страницы
                var tables = algorithm.Extract(page).Where(inner => inner.Cells.Count > 5);
                foreach (var table in tables)
                {
                    Console.WriteLine("--- Найдена таблица ---");

                    // Перебираем строки таблицы
                    foreach (var row in table.Rows)
                    {
                        List<string> rowCells = new List<string>();

                        // Перебираем ячейки в строке
                        foreach (var cell in row)
                        {
                            // Очищаем текст от лишних пробелов и переносов строк
                            string cellText = cell.GetText().Trim().Replace("\r\n", " ");
                            rowCells.Add(cellText);
                        }

                        // Выводим строку, разделяя ячейки знаком табуляции
                        Console.WriteLine(string.Join("\t | \t", rowCells));
                    }
                }
            }
            //
            using (var package = new ExcelPackage(fileInfo.Path))
            {
                var lines = new List<List<string>>();
                var ws = package.Workbook.Worksheets[0];

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
                fileInfo.Data = lines;
            
        }
    }
    }
}
