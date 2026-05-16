using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public class Filter
    {
        public string Sort { get; set; } = "";
        public Supplier Supplier { get; set; }
        public string Text {  get; set; }
        public List<OrderItem> Items{  get; set; }
        public Filter(List<OrderItem> items) 
        {
            Items = items;
        }
        /// <summary>
        /// Apply filters and return data
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<OrderItem> Apply()
        {
            var result = new ObservableCollection<OrderItem>(Items);

            //apply supplier filter
            if(Supplier != null &&  Supplier.Id != "-1" && Supplier.Name != "") 
               result = new ObservableCollection<OrderItem>(Items.Where(item => item.SupplierId == Supplier.Id));

            //apply text sorting
            if (Sort != "")
                result = new ObservableCollection<OrderItem>(result.Where(item =>
                item.Name.ToLower().Contains(Sort.ToLower())));

            return result;
        }

    }
}
