using ExcelFileParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using WmsDesktop.vm;
using WmsDesktop.Windows;

using Tabula;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace WmsDesktop
{
    public class AddingExcelFile : INotifyPropertyChanged//view class
    {
        private string _fieldName = "";
        private List<Tuple<string, int>> _fileField = new List<Tuple<string, int>>();
        private Tuple<string, int> _selectedItem;
        public string FieldName
        {
            get => _fieldName;
            set
            {
                if (_fieldName != value)
                {
                    _fieldName = value;
                    OnPropertyChanged(nameof(FieldName)); 
                }
            }
        }
        public List<Tuple<string, int>> FileField
        {
            get => _fileField;
            set
            {
                if (_fileField != value)
                {
                    _fileField = value;
                    OnPropertyChanged(nameof(FileField)); 
                }
            }
        }
        public Tuple<string, int> SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!EqualityComparer<Tuple<string, int>>.Default.Equals(_selectedItem, value))
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem)); 
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public class CreateSessionByExcelFileViewModel : INotifyPropertyChanged
    {
        private List<AddingExcelFile> _data = new List<AddingExcelFile>();
        private string _fileType;
        public List<AddingExcelFile >Data 
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }
        public string FileType
        {
            get => _fileType;
            set
            {
                if (_fileType != value)
                {
                    _fileType = value;
                    OnPropertyChanged(nameof(FileType)); // Оповещаем интерфейс об изменении
                }
            }
        }
        public FileReader Reader { get; set; }
        public CreateSessionByExcelFile Dialog { get; set; }



        public ICommand parseData { get; set; }
        

        public CreateSessionByExcelFileViewModel(string fileType)
        {
            FileType = fileType;
            parseData = new RelayCommand(o =>
            {
                var selectedSupplier = Reader.fileInfo.Suppliers.FirstOrDefault(inner => inner.Item2 == true);
                var result = new List<IncomeItemVm>();
                Reader.fileInfo.Data.RemoveAt(0);
                //
               
                //
                foreach (var line in Reader.fileInfo.Data)
                {
                    if(selectedSupplier.Item1.SupplierType == 1)
                    {
                        var newDate = line[Data.FirstOrDefault(item => item.FieldName == "date").SelectedItem.Item2];
                        result.Add(new IncomeItemWithDateVm()
                        {
                            CatalogId = "",
                            Barcode = line[Data.FirstOrDefault(item => item.FieldName == "barcode").SelectedItem.Item2],
                            Count = int.Parse(line[Data.FirstOrDefault(item => item.FieldName == "count").SelectedItem.Item2]),
                            Date = DateTime.ParseExact(newDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                            Name = line[Data.FirstOrDefault(item => item.FieldName == "name").SelectedItem.Item2],
                            Sku = line[Data.FirstOrDefault(item => item.FieldName == "sku").SelectedItem.Item2],
                            TE = line[Data.FirstOrDefault(item => item.FieldName == "te").SelectedItem.Item2],
                        }
                        ); 
                    }
                    else if(selectedSupplier.Item1.SupplierType == 0)
                    {
                        result.Add(new IncomeItemVm()
                        {
                            CatalogId = "",
                            Barcode = line[Data.FirstOrDefault(item => item.FieldName == "barcode").SelectedItem.Item2],
                            Count = int.Parse(line[Data.FirstOrDefault(item => item.FieldName == "count").SelectedItem.Item2]),
                            Name = line[Data.FirstOrDefault(item => item.FieldName == "name").SelectedItem.Item2],
                            Sku = line[Data.FirstOrDefault(item => item.FieldName == "sku").SelectedItem.Item2],
                            TE = line[Data.FirstOrDefault(item => item.FieldName == "te").SelectedItem.Item2],
                        }
                        );
                    }
                    else if (selectedSupplier.Item1.SupplierType == 2)
                    {
                        result.Add(new IncomeItemWithBatchVm()
                        {
                            CatalogId = "",
                            Barcode = line[Data.FirstOrDefault(item => item.FieldName == "barcode").SelectedItem.Item2],
                            Count = int.Parse(line[Data.FirstOrDefault(item => item.FieldName == "count").SelectedItem.Item2]),
                            Batches = line[Data.FirstOrDefault(item => item.FieldName == "batch").SelectedItem.Item2],
                            Name = line[Data.FirstOrDefault(item => item.FieldName == "name").SelectedItem.Item2],
                            Sku = line[Data.FirstOrDefault(item => item.FieldName == "sku").SelectedItem.Item2],
                            TE = line[Data.FirstOrDefault(item => item.FieldName == "te").SelectedItem.Item2],
                        }
                        );
                    }

                }
                Dialog.Result = result;
                Dialog.DialogResult = true;
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
