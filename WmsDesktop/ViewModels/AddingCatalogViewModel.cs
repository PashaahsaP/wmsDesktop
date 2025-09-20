using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using WmsDesktop.vm;

namespace WmsDesktop.ViewModels
{
    internal class AddingCatalogViewModel : INotifyPropertyChanged
    {
        private static readonly Client client = new Client();
        private static string ip = "192.168.0.11";
        private string _tbText = "";
        private string _tbName = "";
        private string _tbBarcode = "";
        private string _tblockError = "";

        public string TbText { get
            {
                return _tbText;
            }
            set
            {
                _tbText = value;
                OnPropertyChanged(nameof(TbText));
                OnPropertyChanged(nameof(ItemsList));
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
        private ObservableCollection<OrderItem> _items = new ObservableCollection<OrderItem>();
        private ObservableCollection<OrderItem> _borkItems = new ObservableCollection<OrderItem>();

        public ICommand updateCatalog { get; set; }
        public ObservableCollection<OrderItem> ItemsList { 
            get
            {
                var data = new ObservableCollection<OrderItem>(_borkItems.Where(x => x.Name.ToLower().Contains(TbText.ToLower())));
                return data;
            }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(ItemsList));
            }

        }
        public ObservableCollection<OrderItem> CatalogBorkItems { 
            get
            {
                return _borkItems;
            }
            set
            {
                _borkItems = value;
                OnPropertyChanged(nameof(CatalogBorkItems));
            }
        }
        public  AddingCatalogViewModel(string data)
        {
            var parsedData = JsonConvert.DeserializeObject<ObservableCollection<OrderItem>>(data);
            foreach (var item in parsedData)
            {
                CatalogBorkItems.Add(item);
            }
           
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public static async Task<AddingCatalogViewModel> CreateAsync()
        {
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            ip = setting["Ip"];
            var data = await client.GetAllCatalogBork(ip);
            await SetCommand();
            return new AddingCatalogViewModel(data);
        }
        public async void SetCommand()
        {
            updateCatalog = new RelayCommand(async o  =>
            {
                TblockError = "";

                bool isReturn = false;
                if (TbText == "")
                {
                    TblockError += "Пустое поле названия товара \n";
                    isReturn = true;
                }
                if (TbBarcode == "")
                {
                    TblockError += "Пустое поле шк товара \n";
                    isReturn = true;
                }
                if (isReturn)
                {
                    return;
                }
                var barcode = await client.GetBarcodeByName(TbBarcode, ip);
                var name = await client.GetCatalogBorkByName(TbName, ip);
                if (barcode != null)
                {
                    TblockError += "У данного шк уже присвоен товар. \n";
                    return;
                }
                if (name != null)
                {
                    barcode = new BarcodeItem() { Id = null, catalogId = name.Id, name = TbBarcode, type = "master" };
                    var barcodeId = await client.SendBarcodeBork(barcode, ip);

                }
                else
                {
                    name = new BorkCatalogItem() { Id = null, name = TbName };
                    var catalogId = await client.SendCatalogBork(name, ip);
                    ItemsList.Add(new OrderItem()
                    { Id = catalogId.Id, Name = TbName });
                    barcode = new BarcodeItem() { Id = null, catalogId = catalogId.Id, name = TbBarcode, type = "master" };
                    var barcodeId = await client.SendBarcodeBork(barcode, ip);
                }

                TbName = "";
                TbBarcode = "";
            });
        }
    }
}
