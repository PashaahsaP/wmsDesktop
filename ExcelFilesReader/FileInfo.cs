using System;
using System.Collections.Generic;

namespace ExcelFileParser
{
    public class FileInfo
    {
        #region prop
        public string Path {  get; set; }
        public List<string> SessionField { get; set; } = new List<string>();
        public List<Tuple<string, int>> FileField { get; set; } = new List<Tuple<string, int>>();
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
