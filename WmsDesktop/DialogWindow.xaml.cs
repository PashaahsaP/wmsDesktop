using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
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

namespace WmsDesktop
{
    
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private object Sender{ get; set; }
        private List<OrderItem> Items { get; set; }
        MainViewModel MainViewModel { get; set; }
        ObservableCollection<IUiItem> UiItems { get; set; }
        internal DialogWindow(object sender, List<OrderItem> items, MainViewModel mainViewModel, System.Collections.ObjectModel.ObservableCollection<IUiItem> uiItems)
        {
            InitializeComponent();
            Sender = sender;
            Items = items;
            MainViewModel = mainViewModel;
            UiItems = uiItems;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listItems != null)
            {
                var content = textBox.Text.ToString();
                var filtered = Items.Where(item => item.Name != null && item.Name.ToLower().Contains(content)).ToList();
                listItems.ItemsSource = null;
                listItems.ItemsSource = filtered;
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var selected = (OrderItem)listItems.SelectedItem;
            var sen = (BorkItem)Sender;
            BorkItem newObj = UiItems.First(item =>
            {
                var borkItem = item as BorkItem;
                if (borkItem.Name == sen.Name)
                    return true;
                return false;
            }) as BorkItem;
            newObj.Name = selected.Name;
            newObj.Catalog = new OrderItem { Id = selected.Id, Name = "" };
            var newCollection = new ObservableCollection<IUiItem>();
            foreach (var item in UiItems)
            {
                if(item.Name == sen.Name)
                {
                    newCollection.Add(newObj);
                }
                else
                {
                    newCollection.Add(item);
                }
            }
            MainViewModel.Items = newCollection;
            this.Close();
            
        }
    }
}
