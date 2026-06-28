using ExcelFileParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WmsDesktop.vm;
using WmsDesktop.Windows;

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
        public ICommand parseData { get; set; }
        public FileReader Reader { get; set; }
        public CreateSessionByExcelFile Dialog { get; set; }


        public CreateSessionByExcelFileViewModel()
        {
            parseData = new RelayCommand(o =>
            {
                var selectedSupplier = Reader.fileInfo.Suppliers.FirstOrDefault(inner => inner.Item2 == true);
                var result = new List<IncomeItemVm>();
                Reader.fileInfo.Data.RemoveAt(0);
                foreach (var line in Reader.fileInfo.Data)
                {
                    if(selectedSupplier.Item1.SupplierType == 1)
                    {
                        result.Add(new IncomeItemWithDateVm() 
                        {
                            CatalogId = "",
                            Count = int.Parse(line[Data.FirstOrDefault(item => item.FieldName == "count").SelectedItem.Item2]),
                            Date = line[Data.FirstOrDefault(item => item.FieldName == "date").SelectedItem.Item2],
                            Name = line[Data.FirstOrDefault(item => item.FieldName == "name").SelectedItem.Item2],
                            Sku = line[Data.FirstOrDefault(item => item.FieldName == "sku").SelectedItem.Item2],
                            TE = line[Data.FirstOrDefault(item => item.FieldName == "te").SelectedItem.Item2],
                        }
                        );
                    }else if(selectedSupplier.Item1.SupplierType == 0)
                    {
                        result.Add(new IncomeItemVm()
                        {
                            CatalogId = "",
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
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
