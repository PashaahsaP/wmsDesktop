using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        public string _tbText = string.Empty;
        public ObservableCollection<CatalogItemBase> _items = new ObservableCollection<CatalogItemBase>();

        public OrderItem SelectedItem { get; set; }
        public ObservableCollection<CatalogItemBase>  Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));// Добавить применение фильтром и прочее

            }
        }
        public Filter Filter { get; set; } = new Filter(new List<CatalogItemBase>(), new List<Barcode>());
        public string TbText {  
            get
            {
                return _tbText;
            }
            set 
            {
                _tbText = value;
                Filter.Sort = value;
                Items = Filter.Apply();
                OnPropertyChanged(nameof(TbText));// Добавить применение фильтром и прочее
                OnPropertyChanged(nameof(Items));// Добавить применение фильтром и прочее

            }
        }

        public AddItemViewModel(ObservableCollection<CatalogItemBase> items)
        {
            Items = items;
            Filter.Items = items.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
