using ExcelFileParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WmsDesktop.vm;

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


        public CreateSessionByExcelFileViewModel()
        {
            parseData = new RelayCommand(o =>
            {
                Console.WriteLine();
                var q = Data;
                Console.WriteLine();
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
