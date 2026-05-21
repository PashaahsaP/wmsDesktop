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
using System.Windows.Input;
using WmsDesktop.Pages;
using WmsDesktop.vm;

namespace WmsDesktop.ViewModels
{
    internal class CreateSessionViewModel : INotifyPropertyChanged,IState
    {
        private int _supplier;
        private static readonly Client client = new Client();
        private ObservableCollection<OrderItem> _borkItems = new ObservableCollection<OrderItem>();
        private ObservableCollection<Supplier> _suppliers = new ObservableCollection<Supplier>();
        private ObservableCollection<Cell> _cells = new ObservableCollection<Cell>();
        private DateTime? _date = new DateTime?();

        private MainViewModel vm;

        public int Supplier { get => _supplier; set { _supplier = value; } }
        private ObservableCollection<IUiItem> _items;
        public ObservableCollection<IUiItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public List<OrderItem> CatalogBorkItems { get; set; }
        public ObservableCollection<OrderItem> CatalogItems
        {
            get
            {
                return _borkItems;
            }
            set
            {
                _borkItems = value;
                OnPropertyChanged(nameof(CatalogItems));
            }
        }
        public ObservableCollection<Supplier> Suppliers
        {
            get
            {
                return _suppliers;
            }
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public Supplier SelectedSupplier {  get; set; }
        public Cell SelectedCell {  get; set; }
        public DateTime? Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        public ObservableCollection<Cell> Cells
        {
            get
            {
                return _cells;
            }
            set
            {
                _cells = value;
                OnPropertyChanged(nameof(Cells));
            }
        }
        public List<Barcode> Barcodes {  get; set; } = new List<Barcode>();
        public PageStates PageState => PageStates.CreateSessionPage;



        public ICommand callBorkDialog { get; set; }
        public ICommand clearItems { get; set; }
        public ICommand createSession { get; set; }
        public ICommand selectBork { get; set; }
        public ICommand selectAtomy { get; set; }
        

        public CreateSessionViewModel(string catalogAndSuppliers, string suppliers, string barcodes, string cells)
        {
            
            Items = new ObservableCollection<IUiItem>();
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
                //dialog.Owner = window;
                dialog.listItems.ItemsSource = CatalogBorkItems;
                dialog.Show();
            }, c =>
            {
                var isBork = c.GetType().GetProperty("TE") == null;

                if (isBork)
                {
                    var catalogExist = (BorkItem)c;
                    if (catalogExist.Catalog == null)
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
                var t = new DateTimeOffset(Date.Value).ToUnixTimeSeconds();
                var q = SelectedCell;
                var v = SelectedSupplier;
                bool isGood = true;
                foreach (var item in Items)
                {
                   /* var func = AdapterHelper.getGoodsBalance[_supplier];
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
                    }*/
                }
                if (isGood)
                {
                   /* var func = AdapterHelper.createAssebmlySession[_supplier];
                    await func(Client, Items, ip, Items.Sum(el => el.Count), Items.Count, _supplier);
                    Client.CreateAssebmlySession(Items, ip, Items.Sum(el => el.Count), Items.Count, 1);//REMAKE
                    Items = new ObservableCollection<IUiItem>();*/
                }
            });

            //parse catalogs
            var parsedData = JsonConvert.DeserializeObject<ObservableCollection<OrderItem>>(catalogAndSuppliers);
            foreach (var item in parsedData)
            {
                CatalogItems.Add(item);

            }
            //parse suppliers
            var supplierData = JsonConvert.DeserializeObject<ObservableCollection<Supplier>>(suppliers);
            foreach (var item in supplierData)
            {
                Suppliers.Add(item);

            }
            //parse barcodes
            var parsedBarcodes = JsonConvert.DeserializeObject<ObservableCollection<Barcode>>(barcodes);
            foreach (var item in parsedBarcodes)
            {
                Barcodes.Add(item);

            }
            //parse cells
            var parsedCells = JsonConvert.DeserializeObject<ObservableCollection<Cell>>(cells);
            foreach (var item in parsedCells)
            {
                Cells.Add(item);

            }
        }

        public static async Task<CreateSessionViewModel> CreateAsync()
        {
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            var ip = setting["Ip"];
            var catalogAndSuppliers = await client.GetAllCatalogsWithSuppliers(ip);
            var suppliers = await client.GetSuppliers(ip);
            var barcodes = await client.GetBarcodes(ip);
            var incomeCells = await client.GetIncomeCells(ip);
            return new CreateSessionViewModel(catalogAndSuppliers, suppliers, barcodes, incomeCells);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
