using ExcelFileParser;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WmsDesktop;
using WmsDesktop.Classes;
using WmsDesktop.Converter;
using WmsDesktop.Enums;
using WmsDesktop.vm;
using WmsDesktop.Windows;


namespace WmsDesktop.ViewModels
{
    internal class CreateSessionViewModel : INotifyPropertyChanged,IState
    {
        public Filter Filter { get; set; } = new Filter(new List<IncomeItemEntity>(), new List<Barcode>());
        private Window _window;
        private int _supplier;
        private static readonly Client client = new Client();
        private string _tbText = "";
        private bool _isSupplierSelected = false;
        private ObservableCollection<IncomeItemVm> _borkItems = new ObservableCollection<IncomeItemVm>();
        private ObservableCollection<Supplier> _suppliers = new ObservableCollection<Supplier>(); 
        private ObservableCollection<IncomeItemVm> _items;
        private Supplier _selectedSupplier;
        private DateTime? _date = new DateTime?();

        private MainViewModel vm;

        public string TbText
        {
            get
            {
                return _tbText;
            }
            set
            {
                _tbText = value;
                Filter.Sort = _tbText;
                _borkItems = Filter.Apply();
                OnPropertyChanged(nameof(TbText));
                OnPropertyChanged(nameof(CatalogItems));

            }
        }
        public int Supplier { get => _supplier; set { _supplier = value; } }
        public ObservableCollection<IncomeItemVm> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public List<CatalogItemBase> CatalogBorkItems { get; set; }
        public List<Batch> Batches { get; set; } = new List<Batch>();
        public ObservableCollection<IncomeItemVm> CatalogItems
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
        public List<IncomeItemEntity> CatalogData {  get; set; } = new List<IncomeItemEntity>();
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
        public Supplier SelectedSupplier { 
            get 
            {
                return _selectedSupplier;
            } 
            set 
            { 
                _selectedSupplier = value;
                var isEnabled = Suppliers.Any(item =>
                    item.Name == value.Name
                );
                if (isEnabled) { 
                    var selectedItems = CatalogData.Where(item => item.SupplierId == value.Id).ToList();
                    CatalogItems = new ObservableCollection<IncomeItemVm>(selectedItems.ToVmList());
                }
                IsSupplierSelected = isEnabled;
                OnPropertyChanged(nameof(SelectedSupplier));
            } 
        }
        public IncomeItemEntity SelectedCatalogItem { get; set; }
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
        public List<Cell> IncomeCells {  get; set; } = new List<Cell>();
        public List<Cell> Cells { get; set; } = new List<Cell>();
        public List<Goods> Goods {  get; set; } = new List<Goods> ();
        public List<Barcode> Barcodes {  get; set; } = new List<Barcode>();
        public PageStates PageState => PageStates.CreateSessionPage;
        public bool IsSupplierSelected
        {
            get
            {
                return _isSupplierSelected;
            }
            set
            {
                _isSupplierSelected = value;
                OnPropertyChanged(nameof(IsSupplierSelected));
            }
        }




        public ICommand callBorkDialog { get; set; }
        public ICommand clearItems { get; set; }
        public ICommand createSession { get; set; }
        public ICommand selectBork { get; set; }
        public ICommand selectAtomy { get; set; }
        public ICommand loadFile { get; set; }
        public ICommand removeLine {  get; set; }
        public ICommand pressEnterInTb { get; set; }



        public CreateSessionViewModel(string catalogAndSuppliers, string suppliers, string barcodes, string incomeCells, string batches, string goods, string cellTypes, Window window)
        {
            _window = window;
            Items = new ObservableCollection<IncomeItemVm>();
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
                Items = new ObservableCollection<IncomeItemVm>();
            });
            createSession = new RelayCommand(async o =>
            {
               
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
                if (true)
                {
                   /* var func = AdapterHelper.createAssebmlySession[_supplier];
                    await func(Client, Items, ip, Items.Sum(el => el.Count), Items.Count, _supplier);
                    Client.CreateAssebmlySession(Items, ip, Items.Sum(el => el.Count), Items.Count, 1);//REMAKE
                    Items = new ObservableCollection<IUiItem>();*/
                }
            });
            loadFile = new RelayCommand(async o =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                // dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    string path = dialog.FileName;
                    FileReader reader = new FileReader(path, Suppliers.Select(item => (
                    new ExcelFileParser.Supplier()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        SupplierType = item.SupplierType,
                    }, 
                    item == SelectedSupplier)).ToList());

                    var innerDialog = new CreateSessionByExcelFile(new CreateSessionByExcelFileViewModel(), reader.fileInfo);
                    innerDialog.Owner = _window;

                    bool? innerResult = innerDialog.ShowDialog();
                    if (innerResult == true)
                    {
                         
                    }

                }
            });
            removeLine = new RelayCommand(async o =>
            {
                Items.Remove(o as IncomeItemVm);
            });
            pressEnterInTb = new RelayCommand(async o =>
            {
               
                var result = new List<IncomeItemVm>();
                var element = Items.FirstOrDefault(inner => inner.isSelected);
                var temprary = new IncomeItemVm();
                foreach (var item in Items)
                {
                    if (item == element)
                    {
                        if (element is IncomeItemWithDateVm)
                        {
                            temprary = new IncomeItemWithDateVm()
                            {
                                Count = element.Count,
                                Sku = element.Sku,
                                Name = element.Name,
                                isValid = element.isValid,
                                TE = element.TE,
                                CatalogId = element.CatalogId,
                                Date = ((IncomeItemWithDateVm)element).Date,
                                isSelected = element.isSelected,
                                Other = element.Other
                            };

                        }
                        else if (element is IncomeItemWithBatchVm)
                        {
                            temprary = new IncomeItemWithBatchVm()
                            {
                                Count = element.Count,
                                Sku = element.Sku,
                                Name = element.Name,
                                isValid = element.isValid,
                                TE = element.TE,
                                CatalogId = element.CatalogId,
                                Batches = ((IncomeItemWithBatchVm)element).Batches,
                                isSelected = element.isSelected,
                                Other = element.Other
                            };
                        }
                        else
                        {
                            temprary = new IncomeItemVm()
                            {
                                Count = element.Count,
                                Sku = element.Sku,
                                Name = element.Name,
                                isValid = element.isValid,
                                TE = element.TE,
                                CatalogId = element.CatalogId
                            };
                        }

                        result.Add(temprary);
                    }
                    else
                    {
                        // var temp = new IncomeItemVm() { Count = item.Count, Sku = item.Sku, Name = item.Name, isValid = item.isValid, TE = item.TE, CatalogId = item.CatalogId };
                        result.Add(item);
                    }
                }

                Items = new ObservableCollection<IncomeItemVm>(result);

            });
            //parse suppliers
            var supplierData = JsonConvert.DeserializeObject<ObservableCollection<Supplier>>(suppliers);
            foreach (var item in supplierData)
            { Suppliers.Add(item); }

            //parse catalogs
            // make switch case for client types
            // creating income session items for each type
            // var temp = new IncomeBaseItem();
            //parse cells
            var parsedIncomeCells = JsonConvert.DeserializeObject<List<Cell>>(incomeCells);
            foreach (var item in parsedIncomeCells)
            {
                Cells.Add(item);

            }
           
            Filter.Cells = Cells;
            //parse goods
            var parsedGoods = JsonConvert.DeserializeObject<List<Goods>>(goods);
            foreach (var item in parsedGoods)
            {
                Goods.Add(item);

            }
            //parse cellTypes
            var parsedCellTypes = JsonConvert.DeserializeObject<List<CellTypes>>(cellTypes);
            var parsedData = JsonConvert.DeserializeObject<ObservableCollection<IncomeItemEntity>>(catalogAndSuppliers);
            IncomeItemEntity temp = new IncomeItemEntity();

            foreach (var item in parsedData)
            {
                
                var sup = Suppliers.FirstOrDefault(inner => inner.Id == item.SupplierId);
                if (Enum.IsDefined(typeof(ClientType), sup.SupplierType))
                {
                    ClientType currentStatus = (ClientType)sup.SupplierType;

                    switch (currentStatus)
                    {
                        case ClientType.Base:
                            temp = new IncomeItemEntity() { 
                                Name = item.Name, 
                                Sku = item.Sku,  
                                CatalogId = item.CatalogId, 
                                SupplierId = sup.Id,
                                SupplierName = sup.Name,
                                Other = item.Other };
                            break;
                        case ClientType.WithDate:
                            temp = new IncomeItemWithDateEntity() { 
                                Name = item.Name, 
                                Sku = item.Sku, 
                                CatalogId = item.CatalogId,
                                SupplierId = sup.Id,
                                SupplierName = sup.Name,
                                Other = item.Other,
                                Date = "10.10.2024" };
                            break;
                        case ClientType.WithBatch:
                            temp = new IncomeItemWithBatchEntity()
                            {
                                Name = item.Name,
                                Sku = item.Sku,
                                CatalogId = item.CatalogId,
                                SupplierId = sup.Id,
                                SupplierName = sup.Name,
                                Other = item.Other,
                                Batches = "234"
                            };
                            break;
                    }
                }

                CatalogData.Add(temp);
                Items.Add(temp.ToVm());
                CatalogItems.Add(temp.ToVm());

            }
            Filter.Items = parsedData.ToList();
            

            //parse barcodes
            var parsedBarcodes = JsonConvert.DeserializeObject<ObservableCollection<Barcode>>(barcodes);
            foreach (var item in parsedBarcodes)
            {
                Barcodes.Add(item);

            }
            
            //parse batches
            var parsedBatches = JsonConvert.DeserializeObject<ObservableCollection<Batch>>(batches);
            foreach (var item in parsedBatches)
            {
                Batches.Add(item);
            }

        }

        private bool IsTE(Cell cell, List<CellTypes> list)
        {
            var t = list.Any(cellType =>
            {
                var mask = cellType.Mask;
                if (mask == null)
                    return false;

                return mask.Length == cell.name.Length &&
                       mask.Select((c, i) => new { MaskChar = c, Index = i })
                           .All(x =>
                           {
                               switch (x.MaskChar)
                               {
                                   case '#':
                                       return char.IsDigit(cell.name[x.Index]);

                                   default:
                                       return x.MaskChar == cell.name[x.Index];
                               }
                           });
            });

            return t;
        }

        public static async Task<CreateSessionViewModel> CreateAsync(Window window)
        {
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            var ip = setting["Ip"];
            var catalogAndSuppliers = await client.GetAllCatalogsWithSuppliers(ip);
            var suppliers = await client.GetSuppliers(ip);
            var batches = await client.GetBatches(ip);
            var barcodes = await client.GetBarcodes(ip);
            var incomeCells = await client.GetIncomeCells(ip);
            var goods = await client.GetGoodsIncomeSession(ip);
            var cellTypes = await client.GetCellTypes(ip);
            return new CreateSessionViewModel(catalogAndSuppliers, suppliers, barcodes, incomeCells, batches, goods, cellTypes, window);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

