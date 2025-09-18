using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WmsDesktop.Pages;
using WmsDesktop.vm;

namespace WmsDesktop.ViewModels
{
    internal class HomePageViewModel : INotifyPropertyChanged
    {
        #region ui
        private int _widthSideBar = 150;
        private Page _page;
        public int WidthSideBar { get
            {
                return _widthSideBar;
            }
            set
            {
                _widthSideBar = value;
            }
        }
        #endregion region
        #region ui command
        public ICommand closePage { get; set; }
        public ICommand callAddingPage { get; set; }
        public ICommand callCreateCatalogItemPage { get; set; }
        #endregion
        #region ctor
        public HomePageViewModel(MainViewModel vm)
        {
            closePage = new RelayCommand((o) => {
                vm.HomePage = null;
            });
            callAddingPage = new RelayCommand((o) =>
            {
                vm.MenuItems.Add(new MenuItem
                {
                    IsSelected = true,
                    Title = "Создание заявки",
                    Page = new AddingCatalogsPage()
                });
            });
            callCreateCatalogItemPage = new RelayCommand((o) =>
            {
                vm.MenuItems.Add(new MenuItem
                {
                    IsSelected = true,
                    Title = "Создание заявки",
                    Page = new CreateSessionPage()
                });
            });
        }
        #endregion



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
