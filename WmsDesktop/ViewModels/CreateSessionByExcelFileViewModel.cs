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
    public class AddingExcelFile//view class
    {
        public string FieldName { get; set; } = "";
        public List<Tuple<string, int>> FileField { get; set; } = new List<Tuple<string, int>>();
    }
    
    public class CreateSessionByExcelFileViewModel : INotifyPropertyChanged
    {
        public List<AddingExcelFile >Data { get; set; } = new List<AddingExcelFile> { };
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
