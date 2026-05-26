using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelFileParser
{
    public class FileInfo
    {
        #region varible
        public List<List<string>> data = new List<List<string>>();
        #endregion
        #region prop
        public string Path {  get; set; }
        public List<string> SessionField { get; set; } = new List<string>();
        public List<Tuple<string, int>> FileField { get; set; } = new List<Tuple<string, int>>();
        public List<List<string>> Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                if (data.Count != 0)
                {
                    var firstLine = data.First();
                    for (int i = 0; i < firstLine.Count; i++) // 0 чтобы получить чисто данные первой колонки
                    {
                        FileField.Add(Tuple.Create (firstLine[i], i));
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion
        #region ctor
        public FileInfo(string path)
        {
            Path = path;
        }
        public FileInfo(string path, List<string> sessionFields, List<Tuple<string, int>> fileFields)
        {
            Path = path;
            SessionField = sessionFields;
            FileField = fileFields;
        }
        #endregion
    }
}
