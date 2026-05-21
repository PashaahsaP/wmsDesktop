using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WmsDesktop.ViewModels;
using WmsDesktop.vm;

namespace WmsDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateSessionPage.xaml
    /// </summary>
    public partial class CreateSessionPage : Page
    {
        private readonly Client client = new Client();
        private MainViewModel _vm = null;
        private CreateSessionViewModel localVm = null;
        private string ip = "192.168.0.11";
      
        public CreateSessionPage(MainViewModel vm, Window window, IState state)
        {
            InitializeComponent();
            DataContext = state as CreateSessionViewModel;
            _vm = vm;
        }
        private void WrapPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length == 1)
                {
                    GetDataFromFile(files[0], client);
                }
            }
        }//DONE
        private async Task GetDataFromFile(String pathToFile, Client client)
        {
           /* var func = AdapterHelper.getDataFromFile[localVm.Supplier];
            var result = func(pathToFile, client, ip);
            localVm.Items = new ObservableCollection<IUiItem>(await result);*/
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
     
    }
}
