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
using System.Windows.Navigation;

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
        
        public MainWindow()
        {
            vm = new MainViewModel(this);
            vm.MainWindow = this;
            DataContext = vm;
            ExcelPackage.License.SetNonCommercialPersonal("Pavel Semenov");
            InitializeComponent();
            
            vm.MenuItems = new ObservableCollection<MenuItem>();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var json = await client.GetAllCatalogBork(ip);
            vm.CatalogBorkItems = JsonConvert.DeserializeObject<List<OrderItem>>(json);
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            ip = setting["Ip"];
            vm.ip = ip;
        }
        
        
        
       
    
        /// <summary>
        /// Work for closing menuItem by middle button of mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as Button)?.CommandParameter as MenuItem;
            if (item != null)
            {
                if (item.IsSelected == true)
                {
                    vm.MenuItems.Remove(item);
                    if (vm.MenuItems.Count != 0)
                    {
                        vm.MenuItems[vm.MenuItems.Count - 1].IsSelected = true;
                        vm.CurrentPage = vm.MenuItems[vm.MenuItems.Count - 1].Page;
                    }
                    else
                    {
                        vm.CurrentPage = null;
                    }
                }
                vm.MenuItems.Remove(item);
            }
        }
    }
}
