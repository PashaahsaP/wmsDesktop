using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace WmsDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddingCatalogsPage.xaml
    /// </summary>
    public partial class AddingCatalogsPage : Page
    {
       
        public AddingCatalogViewModel viewModel;
        public AddingCatalogsPage(MainViewModel vm, IState localVm)
        {
            InitializeComponent();
            viewModel = localVm as AddingCatalogViewModel;
        }
         
       
        private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listItems.SelectedItem != null)
            {
                // Получаем выбранный элемент как ListBoxItem
                viewModel.IsEnabledNameField = false;
                viewModel.IsEnabledAppend = false;
                viewModel.IsEnabledSave = true;
                var selectedItem = (OrderItem)listItems.SelectedItem;
                //tbName.Text = selectedItem.name;
                //tbSku.Text = selectedItem.sku;
                //CBSuppliersCatalog.SelectedIndex = 0;
                viewModel.SelectedBarcodes.Clear();
                foreach (var item in viewModel.Barcodes.Where(item => item.CatalogId == selectedItem.id))
                {
                    viewModel.SelectedBarcodes.Add(item);
                };
                for (int i = 0; i < viewModel.Suppliers.Count; i++)
                {
                    if (selectedItem.supplierName == viewModel.Suppliers[i].Name) { }
                        //CBSuppliersCatalog.SelectedIndex = i;
                }
            }
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = viewModel;
            CBSuppliers.SelectedItem = viewModel.Suppliers.First();
        }
    }
}
