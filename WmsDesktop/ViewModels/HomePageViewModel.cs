using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WmsDesktop.Pages;
using WmsDesktop.vm;

namespace WmsDesktop.ViewModels
{
    internal class HomePageViewModel : INotifyPropertyChanged, IState
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
        public PageStates PageState { get => PageStates.HomePage;}
        #endregion
        #region ctor
        public  HomePageViewModel(MainViewModel vm, Window window)
        {
            closePage = new RelayCommand((o) => {
                vm.HomePage = null;
            });
            callAddingPage = new RelayCommand(async (o) =>
            {
                var catalogVm = await AddingCatalogViewModel.CreateAsync();
                var newMenuItem = new MenuItem
                {
                    IsSelected = true,
                    Title = "Добавить наименование",
                    Page = new AddingCatalogsPage(vm, catalogVm),
                    State = catalogVm
                };

                AppendPage(newMenuItem, vm);
            });
            callCreateCatalogItemPage = new RelayCommand(async (o) =>
            {
                var newMenuItem = new MenuItem
                {
                    IsSelected = true,
                    Title = "Создать заявку",
                    Page = new CreateSessionPage(vm, window),
                    
                };
                AppendPage(newMenuItem, vm);
            });
           
       
            
        }
        #region helper methods
        private void AppendPage(MenuItem menuItem, MainViewModel vm)
        {
            foreach (var item in vm.MenuItems)// сброс выделения
            {
                item.IsSelected = false;
            }
            vm.MenuItems.Add(menuItem); // добавить в панель
            vm.CurrentPage = menuItem.Page; // Выделенный элемент на панели
            vm.HomePage = null; //закрыть боковую панель
        }
        #endregion
        #endregion



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
