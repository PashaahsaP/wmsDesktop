using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public class AddingExcelFile
    {
        public string FieldName { get; set; } = "";
        public List<Tuple<string, int>> FileField { get; set; } = new List<Tuple<string, int>>();
    }
    public class CreateSessionByExcelFileViewModel : INotifyPropertyChanged
    {
        public List<AddingExcelFile >Data { get; set; } = new List<AddingExcelFile> { };


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
