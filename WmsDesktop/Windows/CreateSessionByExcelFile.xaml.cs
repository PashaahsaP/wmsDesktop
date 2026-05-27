using ExcelFileParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WmsDesktop.ViewModels;

namespace WmsDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateSessionByExcelFile.xaml
    /// </summary>
    public partial class CreateSessionByExcelFile : Window
    {
        public CreateSessionByExcelFile(CreateSessionByExcelFileViewModel vm, FileInfo fileInfo)
        {
            foreach (var item in fileInfo.SessionField)
            {
                vm.Data.Add(new AddingExcelFile() { FieldName = item, FileField = fileInfo.FileField});
            }
            DataContext = vm;
            InitializeComponent();
        }
    }
}
