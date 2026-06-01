using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using WmsDesktop.vm;

namespace WmsDesktop.ViewModels
{
    public class AddingCatalogViewModel : INotifyPropertyChanged, IState
    {
        private static readonly Client client = new Client();
        private static string ip = "192.168.0.11";
        private string _tbText = "";
        private string _tbName = "";
        private string _tbSku = "";
        private string _tbBarcode = "";
        private string _tblockError = "";
        private bool _isEnabled = true;
        private bool _isEnabledAppend = true;
        private bool _isEnabledSave = false;

        public string TbText { get
            {
                return _tbText;
            }
            set
            {
                _tbText = value;
                Filter.Sort = _tbText;
                _items = Filter.Apply();
                OnPropertyChanged(nameof(TbText));
                OnPropertyChanged(nameof(ItemsList));

            }
        }
        public string TbSku
        {
            get
            {
                return _tbSku;
            }
            set
            {
                _tbSku = value;
                OnPropertyChanged(nameof(TbSku));
            }
        }
        public string TbName
        {
            get
            {
                return _tbName;
            }
            set
            {
                _tbName = value;
                OnPropertyChanged(nameof(TbName));
            }
        }
        public string TbBarcode
        {
            get
            {
                return _tbBarcode;
            }
            set
            {
                _tbBarcode = value;
                OnPropertyChanged(nameof(TbBarcode));
            }
        }
        public string TblockError
        {
            get
            {
                return _tblockError;
            }
            set
            {
                _tblockError = value;
                OnPropertyChanged(nameof(TblockError));
            }
        }
        private ObservableCollection<CatalogItemBase> _items = new ObservableCollection<CatalogItemBase>();
        private ObservableCollection<CatalogItemBase> _catalogItems = new ObservableCollection<CatalogItemBase>();
        private ObservableCollection<Supplier> _suppliers = new ObservableCollection<Supplier>();
        private Supplier _selectedSupplier = new Supplier() { Id = -1, Name = "" };
        private Supplier _selectedSupplierCatalog = new Supplier() { Id = -1, Name = "" };
        public ICommand addBarcode { get; set; }
        public ICommand removeBarcode { get; set; }
        public ICommand clearFields { get; set; }
        public ICommand addEntity { get; set; }
        public ICommand updateEntity { get; set; }
        public bool IsEnabledNameField
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabledNameField));
            }
        }
        public bool IsEnabledAppend
        {
            get
            {
                return _isEnabledAppend;
            }
            set
            {
                _isEnabledAppend = value;
                OnPropertyChanged(nameof(IsEnabledAppend));
            }
        }
        public bool IsEnabledSave
        {
            get
            {
                return _isEnabledSave;
            }
            set
            {
                _isEnabledSave = value;
                OnPropertyChanged(nameof(IsEnabledSave));
            }
        }
        public Visibility PartyVisibility { get; set; } = Visibility.Collapsed;
        public Filter Filter { get; set; } = new Filter(new List<CatalogItemBase>(), new List<Barcode>());
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                Filter.Supplier = _selectedSupplier;
                _items = Filter.Apply();
                OnPropertyChanged(nameof(SelectedSupplier));// Добавить применение фильтром и прочее
                OnPropertyChanged(nameof(ItemsList));// Добавить применение фильтром и прочее
            }
        }
        public Supplier SelectedSupplierCatalog
        {
            get => _selectedSupplierCatalog;
            set
            {
                _selectedSupplierCatalog = value;
                if(value.Name == "Atomy") 
                    PartyVisibility = Visibility.Visible;
                else
                    PartyVisibility=Visibility.Collapsed;

                OnPropertyChanged(nameof(PartyVisibility));// Добавить применение фильтром и прочее
                OnPropertyChanged(nameof(SelectedSupplierCatalog));// Добавить применение фильтром и прочее
            }
        }
        public List<Barcode> Barcodes {  get; set; } = new List<Barcode>();
        public ObservableCollection<Barcode> SelectedBarcodes {  get; set; } = new ObservableCollection<Barcode>();
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
        public ObservableCollection<CatalogItemBase> ItemsList { 
            get
            {
                return _items;
                
            }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(ItemsList));
            }

        }
        public ObservableCollection<CatalogItemBase> CatalogItems { 
            get
            {
                return _catalogItems;
            }
            set
            {
                _catalogItems = value;
                OnPropertyChanged(nameof(CatalogItems));
            }
        }
        public OrderItem SelectedItem { get; set; }

        public PageStates PageState => PageStates.AddingCatalogPage;

        public  AddingCatalogViewModel(string data, string suppliers, string barcodes, string batches)
        {
            //parse barcodes
            var parsedBarcodes = JsonConvert.DeserializeObject<ObservableCollection<Barcode>>(barcodes);
            foreach (var item in parsedBarcodes)
            {
                Barcodes.Add(item);

            }
            //parse catalog items
            var parsedData = JsonConvert.DeserializeObject<ObservableCollection<CatalogItemBase>>(data);
            foreach (var item in parsedData)
            {
                CatalogItems.Add(item);

            }
            var temp = new ObservableCollection<CatalogItemBase>(_catalogItems.Where(x => x.Name.ToLower().Contains(TbText.ToLower())));
            ItemsList = temp;
            //parse suppliers
            var supplierData = JsonConvert.DeserializeObject<ObservableCollection<Supplier>>(suppliers);
            
            Suppliers.Add(SelectedSupplier);
            foreach (var item in supplierData)
            {
                Suppliers.Add(item);

            }
            
            
            Filter.Items = temp.ToList();
            Filter.Barcodes = parsedBarcodes.ToList();
            removeBarcode = new RelayCommand(o =>
            {
                var barcode = (Barcode)o;
                var item = SelectedBarcodes.First(it => it.Id == barcode.Id && it.Name == barcode.Name);
                for (int i = 0; i < SelectedBarcodes.Count; i++)
                {
                    if (SelectedBarcodes[i].Id == item.Id && SelectedBarcodes[i].Name == item.Name)
                    {
                        SelectedBarcodes.RemoveAt(i);
                    }
                    
                }
                
            });
            clearFields = new RelayCommand(o => {
                IsEnabledSave = false;
                IsEnabledAppend = true;
                IsEnabledNameField = true;
                TbBarcode = "";
                TbName = "";
                TbSku = "";
                SelectedBarcodes.Clear();
                SelectedSupplierCatalog = Suppliers.First();

            });
            addBarcode = new RelayCommand(async o =>
            {
                var curCatalog = CatalogItems.FirstOrDefault(it => it.name == TbName) ;
                if (TbBarcode != "")
                {
                    SelectedBarcodes.Add(new Barcode("?", TbBarcode, curCatalog != null? curCatalog.id : "none"));
                    TbBarcode = "";
                }
            });
            addEntity = new RelayCommand(async o => {
                var text = "";
                bool isOk = true;
                if (SelectedSupplierCatalog.Name == "") {
                    isOk = false;
                    text = "Выберите поставщика\n";
                }
                if (TbName == "")
                {
                    isOk = false;
                    text += "Добавьте наименование товара\n";
                }

                TblockError = text;
                if (isOk) {
                    var suppId = Int32.Parse(SelectedSupplierCatalog.Id);
                    var catalogId = await client.SendCatalog(new Catalog() { name = TbName, supplierId = suppId, sku = TbSku}, ip);
                    var barcodeId = "";
                    foreach (var barcode in SelectedBarcodes)
                    {
                        barcodeId = await client.SendBarcode(
                        new BarcodeItem() { name = barcode.Name, supplierId = suppId, catalogId = catalogId }, ip);
                        Barcodes.Add(new Barcode(barcodeId, barcode.Name, catalogId));
                    }
                    ItemsList.Add(new OrderItem()
                    {
                        id = catalogId, 
                        name = TbName,
                        sku = TbSku,
                        supplierId = SelectedSupplierCatalog.Id,
                        supplierName = SelectedSupplierCatalog.Name
                    });
                    Filter.Items = new List<OrderItem>(ItemsList);
                    clearFields.Execute(null);
                }
            });
            updateEntity = new RelayCommand(async o =>{
                if(SelectedItem != null)
                {
                    var updateItem = new OrderItem()
                    {
                        id = SelectedItem.id,
                        supplierId = SelectedSupplierCatalog.Id,
                        name= TbName,
                        sku= TbSku,
                        supplierName = SelectedSupplierCatalog.Name
                    };
                    var updatedId = await client.UpdateCatalog(updateItem, ip);
                    if(updatedId == SelectedItem.id)
                    {
                        var t = ItemsList.First(it => it.id == SelectedItem.id);
                        t.name = TbName;
                        t.sku = TbSku;
                        t.supplierId = SelectedSupplierCatalog.Id;
                        ItemsList = new ObservableCollection<OrderItem>(ItemsList);
                    }


                    Barcodes.Where(bar => bar.CatalogId == updateItem.id).ToList()
                        .ForEach(async item => {
                            if (!SelectedBarcodes.Any(it => it.Name == item.Name))
                            {
                                //удалить запись из бд
                                var t = await client.RemoveBarcode(item, ip);
                                //удалить запись из локальной
                            }
                            
                        });
                    foreach (var item in SelectedBarcodes)
                    {
                        if (item.Id == "?")
                        {
                            //добавить в бд
                            var addedId = await client.SendBarcode(new BarcodeItem()
                            {
                                name = item.Name,
                                catalogId = item.CatalogId,
                                supplierId = int.Parse(SelectedSupplierCatalog.Id)
                            }, ip);
                            Console.WriteLine();
                            //добавить в локальное хранилище
                        }
                    }
                }
            }); 
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public static async Task<AddingCatalogViewModel> CreateAsync()
        {
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            ip = setting["Ip"];
            var batches = await client.GetBatches(ip);
            var data = await client.GetAllCatalogsWithSuppliers(ip);
            var suppliers = await client.GetSuppliers(ip);
            var barcodes = await client.GetBarcodes(ip);
            return new AddingCatalogViewModel(data, suppliers, barcodes, batches);
        }
         
    }
}
