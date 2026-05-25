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
using System.Windows.Shapes;
using WmsDesktop.ViewModels;

namespace WmsDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public AddItemViewModel localVm = null;
        public OrderItem orderItem = null;
        public AddItemWindow(System.Collections.ObjectModel.ObservableCollection<OrderItem> catalogItems)
        {
            InitializeComponent();
            localVm = new AddItemViewModel(catalogItems);
            DataContext = localVm;
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderItem = localVm.SelectedItem;
            DialogResult = true;
        }
    }
}
