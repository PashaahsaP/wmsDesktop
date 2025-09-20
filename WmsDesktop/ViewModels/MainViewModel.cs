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
        
        public string ip {  get; set; }

        private ObservableCollection<MenuItem> _menuItems;

        public ObservableCollection<MenuItem> MenuItems{ get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }
        public MainWindow MainWindow { get; set; }
        
       
        
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
        /// <summary>
        /// Event for closing button on press X on menu item(еще есть закрытие на среднию кнопку и это события cs коде)
        /// </summary>
        public ICommand closeMenuItem { get; set; }
        #endregion

        #endregion

        public MainViewModel(Window window)
        {
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
                HomePage = new HomePage(this, window);
            });
            closeMenuItem = new RelayCommand(o => {
                var item = o as MenuItem;
                if (item != null)
                {
                    if (item.IsSelected == true)
                    {
                        MenuItems.Remove(item);
                        if (MenuItems.Count != 0)
                        {
                            MenuItems[MenuItems.Count - 1].IsSelected = true;
                            CurrentPage = MenuItems[MenuItems.Count - 1].Page;
                        }
                        else
                        {
                            CurrentPage = null;
                        }
                    }
                    MenuItems.Remove(item);
                }
            });
            #endregion
        }
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}
