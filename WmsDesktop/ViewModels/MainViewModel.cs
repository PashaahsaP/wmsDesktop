using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    public class MenuItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        private Page _pages;
        public string Title { get; set; }
        public bool IsSelected {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public Page Page
        {
            get
            {
                return _pages;
            }
            set
            {
                _pages = value;
                OnPropertyChanged(nameof(Page));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        #region bisiness member
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
        private ObservableCollection<MenuItem> _menuItems;

        public ObservableCollection<MenuItem> MenuItems{ get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
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
        #endregion
        #region Display Window
        private Window _window;
   
        /// <summary>
        /// padding need for correct displaing in maximazed state of buttons
        /// </summary>
        private Thickness _titlePadding = new Thickness(10);
        public Thickness TitlePadding
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? _titlePadding : new Thickness(0);
            }
            set
            {
                _titlePadding = value;
            }
        }
        /// <summary>
        /// need for correct displaing menu items(second line)
        /// </summary>
        private Thickness _titleMenuPadding = new Thickness(8, 0, 8, 0);
        public Thickness TitleMenuPadding
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? _titleMenuPadding : new Thickness(0);
            }
            set
            {
                _titleMenuPadding = value;
            }
        }
        /// <summary>
        /// need for correct displaing button in top right
        /// </summary>
        private int _titleHeight = 30;
        public int TitleHeight 
        { get
            {
                return _window.WindowState == WindowState.Maximized ? 50 : _titleHeight;
            }

            set
            {
                _titleHeight = value;
            }
        } 
        public int MenuTitleHeight { get; set; } = 30;
        private Page _HomePage;
        public Page HomePage
        {
            get => _HomePage;
            set
            {
                if (_HomePage != value)
                {
                    _HomePage = value;
                    OnPropertyChanged(nameof(HomePage));
                }
            }
        }
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }

        #region commands
        public ICommand collapseWindow { get; set; }
        public ICommand expandWindow { get; set; }
        public ICommand closeWindow { get; set; }
        public ICommand selectMenuItem {  get; set; }
        public ICommand callHomeWindow {  get; set; }
        public ICommand closeMenuItem { get; set; }
        #endregion

        #endregion

        public MainViewModel(Window window)
        {
            #region businessLogic
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
            #endregion
            #region Display Window
            _window = window;
            collapseWindow = new RelayCommand(o => _window.WindowState = WindowState.Minimized);
            expandWindow = new RelayCommand(o => _window.WindowState = WindowState.Maximized);
            closeWindow = new RelayCommand(o => _window.Close());
            selectMenuItem = new RelayCommand((o) =>
            {
                foreach (var item in MenuItems)
                {
                    item.IsSelected = false;
                }
                MenuItem menuItem = o as MenuItem;
                menuItem.IsSelected = true;
                CurrentPage = menuItem.Page;
            });
            _window.StateChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TitlePadding)); 
                OnPropertyChanged(nameof(TitleHeight)); 
                OnPropertyChanged(nameof(TitleMenuPadding)); 
                OnPropertyChanged(nameof(MenuTitleHeight));
            };
            callHomeWindow = new RelayCommand(o =>
            {
                HomePage = new HomePage(this);
            });
            closeMenuItem = new RelayCommand(o => {
               
            });
            #endregion
        }
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}
