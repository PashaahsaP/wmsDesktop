using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ExcelFileParser
{
    public class FileReader
    {
        public FileInfo fileInfo = null;
        public FileReader(string path)
        {
            fileInfo = new FileInfo(path);
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
