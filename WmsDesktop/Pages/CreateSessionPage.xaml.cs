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
using System.Xml.Linq;
using WmsDesktop.Converter;
using WmsDesktop.ViewModels;
using WmsDesktop.vm;
using WmsDesktop.Windows;

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
        private Window _window = null;
      
        public CreateSessionPage(MainViewModel vm, Window window, IState state)
        {
            InitializeComponent();
            _window = window;
            DataContext = state as CreateSessionViewModel;
            localVm = state as CreateSessionViewModel;
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

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ui = (sender as FrameworkElement);
            var context = ui.DataContext;
            var element = context as IncomeItemVm;
            var result = new List<IncomeItemVm>();
            
            foreach (var item in localVm.Items)
            {
                if(item.Name == element.Name && item.Sku == element.Sku && item.Count == element.Count)
                {
                    var temp = new IncomeItemVm() { Count = element.Count, Sku = element.Sku, Name = element.Name, isValid = element.isValid, TE = element.TE, CatalogId = element.CatalogId };//selected
                    result.Add(temp);
                }
                else if (item is IncomeItemVm)//selected
                {
                    var temp = new IncomeItemVm() { Count = item.Count, Sku = item.Sku, Name = item.Name, isValid = item.isValid, TE = element.TE, CatalogId = element.CatalogId };
                    result.Add(temp);
                }
                else
                {
                    result.Add(item);
                }
            }

            localVm.Items = new ObservableCollection<IncomeItemVm>(result);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            var frameElem = (sender as TextBox);
            var context = (sender as FrameworkElement).DataContext;
            var element = context as IncomeItemVm; // selected
            var result = new List<IncomeItemVm>();
            foreach (var item in localVm.Items)
            {
                if (item.CatalogId == element.CatalogId && item.Name == element.Name && item.Sku == element.Sku)
                {
                    var temp = new IncomeItemVm() { Count = int.Parse(frameElem.Text), Sku = element.Sku, Name = element.Name, isValid = element.isValid, TE = element.TE, CatalogId = element.CatalogId };
                    result.Add(temp);
                }
                else
                {
                    var temp = new IncomeItemVm() { Count = item.Count, Sku = item.Sku, Name = item.Name, isValid = item.isValid, TE = item.TE, CatalogId = item.CatalogId };
                    result.Add(temp);
                }
            }

            localVm.Items = new ObservableCollection<IncomeItemVm>(result);
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var ui = (sender as TextBox);
            ui.Focus();
            ui.SelectAll();

        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var dialog = new AddItemWindow(new ObservableCollection<Classes.IncomeItemEntity>(localVm.CatalogItems.ToEntityList()));
            var uiItem = (sender as FrameworkElement).DataContext as IncomeItemVm;//wrong item was
            dialog.Owner = _window;

            bool? result = dialog.ShowDialog();
            OrderItem data = null;
            if (result == true)
            {
                data = dialog.orderItem;


                var resultCollection = new ObservableCollection<IncomeItemVm>();
                foreach (var item in localVm.Items)
                {
                    if (item == uiItem)
                        resultCollection.Add(new IncomeItemVm() { Count = uiItem.Count, isValid = true, Name = data.name, Sku = data.sku, TE = new List<Cell>(), CatalogId = data.id });
                    else
                        resultCollection.Add(item);

                }

                localVm.Items = resultCollection;
            }
        }

        private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = localVm.SelectedCatalogItem;
            localVm.Items.Add(new IncomeItemVm() { Count = 1, isValid = true, Name = t.name, Sku = t.sku });
        }

      

        private void TextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            var frameElem = (sender as TextBox);
            var context = (sender as FrameworkElement).DataContext;
            var element = context as IncomeItemVm;
            var result = new List<IncomeItemVm>();
            foreach (var item in localVm.Items)
            {
                if (item.CatalogId == element.CatalogId &&  item.Name == element.Name && item.Sku == element.Sku)
                {
                    var temp = new IncomeItemVm() { Count = element.Count, Sku = element.Sku, Name = element.Name, isValid = element.isValid, TE = new List<Cell>(), CatalogId = element.CatalogId };
                    result.Add(temp);
                }
                else
                {
                    var temp = new IncomeItemVm() { Count = item.Count, Sku = item.Sku, Name = item.Name, isValid = item.isValid, TE = item.TE, CatalogId = item.CatalogId };
                    result.Add(temp);
                }
            }

            localVm.Items = new ObservableCollection<IncomeItemVm>(result);
        }
    }
}
