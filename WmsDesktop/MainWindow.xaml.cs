using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace WmsDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public class Cell
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    public class ID
    {
        public int id { get; set; }

    }
    public class Name
    {
        public string name { get; set; }

    }
    public class AssemblySession
    {
        public int supplierId { get; set; }
        public int outCell { get; set; }
        public string createdAt { get; set; }
        public string finishedAt { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public int amount { get; set; }
        public int lines { get; set; }


    }
    public class AssemblyBorkItem
    {
        public int goodsId {  get; set; }
        public int assemblyId {  get; set; }
        public int cellId { get; set; }
        public string startedAt{ get; set; }
        public string finishedAt{ get; set; }
        public string status { get; set; }
    }
    public class AssemblyAtomyItem
    {
        public int goodsId { get; set; }
        public int assemblyId { get; set; }
        public int cellId { get; set; }
        public string startedAt { get; set; }
        public string finishedAt { get; set; }
        public string status { get; set; }
    }
    public class UIItems
    {
        public string Art { get; set; }
        public string Barcode{ get; set; }
        public string Name { get; set; }
        public string Count{ get; set; }
        public OrderItem Catalog  { get; set; }
        public override string ToString() => Name;
    }
    public class BarcodeItem
    {
        public string Id { get; set; }
        public string catalogId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public override string ToString() => name;
    }
    public class AtomyGoodsItem
    {
        public string id { get; set; }
        public string catalogId { get; set; }
        public string cellId { get; set; }
        public int amount{ get; set; }
        public string TE { get; set; }
        public string date { get; set; }
        public string createdAt { get; set; }
        public override string ToString() => TE;
    }
    public class GoodsBorkItem
    {
        public string id { get; set; }
        public string catalogId { get; set; }
        public string cellId { get; set; }
        public int amount { get; set; }
        public string createdAt { get; set; }

        public override string ToString() => id;
    }
    public class BorkCatalogItem
    {
        public string Id { get; set; }
        public string name { get; set; }

    }
   
    public partial class MainWindow : Window
    {

        private MainViewModel vm;

        private readonly Client client = new Client();
        private string ip = "192.168.0.11";
        public List<OrderItem> ItemsList { get; set; }
        public MainWindow()
        {
            vm = new MainViewModel(this);
            vm.MainWindow = this;
            DataContext = vm;
            ExcelPackage.License.SetNonCommercialPersonal("Pavel Semenov");
            InitializeComponent();
            var selectedStyle = (Style)FindResource("TitleMenuItemSelected");
            var unselectedStyle = (Style)FindResource("TitleMenuItem");
            vm.MenuItems = new ObservableCollection<MenuItem>(){new MenuItem
            {
                IsSelected = true,
                Title = "NewItem",
                SelectedStyle = selectedStyle,
                UnselectedStyle = unselectedStyle
            }, new MenuItem
            {
                IsSelected = false,
                Title = "oldItem",
                SelectedStyle = selectedStyle,
                UnselectedStyle = unselectedStyle
            }, new MenuItem
            {
                IsSelected = false,
                Title = "some",
                SelectedStyle = selectedStyle,
                UnselectedStyle = unselectedStyle
            }
            };

        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var json = await client.GetAllCatalogBork(ip);
            vm.CatalogBorkItems = JsonConvert.DeserializeObject<List<OrderItem>>(json);
            listItems.ItemsSource = vm.CatalogBorkItems;
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            ip = setting["Ip"];
            vm.ip = ip;
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
            var func = AdapterHelper.getDataFromFile[vm.Supplier];
            var result = func(pathToFile, client, ip);
            vm.Items = new ObservableCollection<IUiItem>(await result);
        }
        private void textBoxSearhc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSearhc != null)
            {
                var content = textBoxSearhc.Text.ToString();
                var filtered = ItemsList.Where(item => item.Name != null && item.Name.ToLower().Contains(content)).ToList();
                listItems.ItemsSource = null;
                listItems.ItemsSource = filtered;
                
            }
        }
        private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listItems.SelectedItem != null)
            {
                // Получаем выбранный элемент как ListBoxItem
                var selectedItem = (OrderItem)listItems.SelectedItem;
                string text = selectedItem.Name;
                tbName.Text = text;
            }
        }
        private async void btnCatalogBorkAdd_Click(object sender, RoutedEventArgs e)
        {
            tbError.Text = "";

            bool isReturn =false;
            if(tbName.Text == "")
            {
                tbError.Text += "Пустое поле названия товара \n";
                isReturn = true;
            }
            if (tbBarcode.Text == "")
            {
                tbError.Text += "Пустое поле шк товара \n";
                isReturn = true;
            }
            if (isReturn)
            {
                return;
            }
            var barcode = await client.GetBarcodeByName(tbBarcode.Text, ip);
            var name = await client.GetCatalogBorkByName(tbName.Text, ip);
            if(barcode != null)
            {
                tbError.Text += "У данного шк уже присвоен товар. \n";
                return;
            }
            if(name!= null)
            {
                barcode = new BarcodeItem() { Id = null, catalogId = name.Id, name = tbBarcode.Text, type = "master" };
                var barcodeId = await client.SendBarcodeBork(barcode, ip);

            }
            else
            {
                name = new BorkCatalogItem() { Id = null, name = tbName.Text.ToString() };
                var catalogId = await client.SendCatalogBork(name, ip);
                ItemsList.Add(new OrderItem()
                { Id = catalogId.Id, Name = tbName.Text.ToString() });
                barcode = new BarcodeItem() { Id = null, catalogId = catalogId.Id, name = tbBarcode.Text, type = "master" };
                var barcodeId = await client.SendBarcodeBork(barcode, ip);
            }

            tbName.Text =  "";
            tbBarcode.Text = "";
            
        }

        public void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
