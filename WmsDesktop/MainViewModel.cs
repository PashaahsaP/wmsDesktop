using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WmsDesktop.vm;

namespace WmsDesktop
{
    internal class AtomyItem : IUiItem
    {
        private string _name;
        private int _count;
        private int _amount;
        private string _TE;
        private string _barcode;
        private OrderItem _catalog;
        public string Name { get => _name; set => _name = value; }
        public int Count { get => _count; set => _count = value; }
        public string TE { get => _TE; set => _TE = value; }
        public string Barcode { get => _barcode; set => _barcode = value; }
        public OrderItem Catalog { get => _catalog; set => _catalog = value; }
        public int Amount { get => _amount; set => _amount = value; }
        public string Left { get => _name; }
        public string Center { get => TE; }
        public string Right { get => $"{_count}/{_amount}"; }
    }
    internal class BorkItem : IUiItem
    {
        private string _name;
        private int _count;
        private int _amount;
        private string _art;
        private string _barcode;
        private OrderItem _catalog;
        public string Name { get => _name; set => _name = value; }
        public int Count { get => _count; set => _count = value; }
        public string Art { get => _art; set => _art = value; }
        public string Barcode { get => _barcode; set => _barcode = value; }
        public OrderItem Catalog { get => _catalog; set => _catalog = value; }
        public int Amount { get => _amount; set => _amount = value; }
        public string Left { get => _name; }
        public string Center { get => _barcode; }
        public string Right { get => $"{_count}/{_amount}"; }

    }
    class MainViewModel : INotifyPropertyChanged
    {
        private int _supplier;
        public Client Client { get; set; }
        public string ip {  get; set; }
        private ObservableCollection<IUiItem> _items;
        public ObservableCollection<IUiItem> Items { get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public List<OrderItem> CatalogBorkItems { get; set; }
        public MainWindow MainWindow { get; set; }
        public int Supplier { get => _supplier; set { _supplier = value; } }
        public ICommand selectBork { get; set; }
        public ICommand selectAtomy { get; set; }
        public ICommand callBorkDialog { get; set; }
        public ICommand clearItems { get; set; }
        public ICommand createSession { get; set; }


        public MainViewModel()
        {
            Client = new Client();
            selectAtomy = new RelayCommand(o =>
            {
                _supplier = 0;
            });
            selectBork = new RelayCommand(o =>
            {
                _supplier = 1;
            });
            callBorkDialog = new RelayCommand(o =>
            {
                var dialog = new DialogWindow(o, CatalogBorkItems, this, Items);
                dialog.Owner = MainWindow;
                dialog.listItems.ItemsSource = CatalogBorkItems;
                dialog.Show();
            }, c =>
            {
                var isBork = c.GetType().GetProperty("TE") == null;
                
                if(isBork)
                {
                    var catalogExist = (BorkItem)c;
                    if(catalogExist.Catalog == null)
                        return true;
                }
                return false;
            });
            clearItems = new RelayCommand(o =>
            {
                Items = new ObservableCollection<IUiItem>();
            });
            createSession = new RelayCommand(async o =>
            {
                bool isGood = true;
                foreach (var item in Items)
                {
                    var func = AdapterHelper.getGoodsBalance[_supplier];
                    var str = _supplier == 0 ? (item as AtomyItem).TE : item.Catalog.Id;
                    Int32 count = item.Catalog != null ? await func(str, Client, ip) : 0;
                    if (item.Catalog == null)
                    {
                        isGood = false;
                    }
                    if (item.Count > count)
                    {
                        isGood = false;
                        MessageBox.Show($"{item.Name} не хватает {item.Count - count}");
                    }
                }
                if (isGood)
                {
                    var func = AdapterHelper.createAssebmlySession[_supplier];
                    await func(Client, Items, ip, Items.Sum(el => el.Count), Items.Count, _supplier);
                    Client.CreateAssebmlySession(Items, ip, Items.Sum(el => el.Count), Items.Count, 1);//REMAKE
                    Items = new ObservableCollection<IUiItem>();
                }
            });
            Items = new ObservableCollection<IUiItem>();
        }
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}
