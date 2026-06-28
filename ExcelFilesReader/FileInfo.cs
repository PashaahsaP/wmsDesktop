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
        public List<string> SessionField { get; set; } = new List<string>() { "sku", "name", "barcode", "count"};// Должны быть в зависимости от типа клиента
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
            }
        }
        public List<(Supplier, bool)> Suppliers {  get; set; }
        #endregion
        #region ctor
        public FileInfo(string path, List<(Supplier, bool)> supplier)// надо сюда передавать тип клиента, для настройки полей
        {
            Suppliers = supplier;
            Path = path;
            var selectedSup = supplier.FirstOrDefault(item => item.Item2);
            switch (selectedSup.Item1.SupplierType)
            {
                case 0: SessionField = new List<string>() { "sku","te", "name", "barcode", "count" };break;
                case 1: SessionField = new List<string>() { "sku","te", "name", "barcode", "date", "count" }; break;
                case 2: SessionField = new List<string>() { "sku", "te", "name", "barcode", "batch", "count" }; break;
                default: SessionField = new List<string>() { "sku", "te", "name", "barcode", "count" }; break;
            }
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
