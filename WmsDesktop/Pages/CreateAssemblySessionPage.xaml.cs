using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WmsDesktop;
using WmsDesktop.Pages;
using WmsDesktop.ViewModels;

namespace WmsDesktop.Pages
{
    
    /// <summary>
    /// Логика взаимодействия для CreateAssemblySessionPage.xaml
    /// </summary>
    public partial class CreateAssemblySessionPage : Page
    {
        private readonly Client client = new Client();
        private MainViewModel _vm = null;
        private CreateIncomeSessionViewModel localVm = null;
        private Window _window = null;
        public CreateAssemblySessionPage(MainViewModel vm, Window window, IState state)
        {
            InitializeComponent();
            _window = window;
            localVm = state as CreateIncomeSessionViewModel;
            DataContext = state as CreateIncomeSessionViewModel;
            _vm = vm;

        }
    }
}


