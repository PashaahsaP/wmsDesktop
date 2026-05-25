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
                var art = new List<string>();
                var barcode = new List<string>();
                var name = new List<string>();
                var count = new List<string>();
                var ws = package.Workbook.Worksheets[0];
                var temp = ws.Cells.Select(item => item.Text);
                Console.WriteLine();
                /*int rows = ws.Dimension.End.Row;
                int cols = ws.Dimension.End.Column;
                for (int r = 1; r <= rows; r++)
                {
                    for (int c = 1; c <= cols; c++)
                    {
                        var value = ws.Cells[r, c].Text.ToString();
                        var valueLength = value.Length;
                        if (c == 2 && valueLength > 0)
                        {
                            art.Add(value);
                        }
                        else if (c == 4 && valueLength > 0)
                        {
                            barcode.Add(value);
                        }
                        else if (c == 1 && valueLength > 0)
                        {
                            name.Add(value);
                        }
                        else if (c == 3 && valueLength > 0)
                        {
                            count.Add(value);
                        }

                    }
                }*/

            
            
        }
    }
    }
}
