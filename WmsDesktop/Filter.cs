using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmsDesktop.Classes;
using WmsDesktop.Converter;

namespace WmsDesktop
{
    public class Filter
    {
        public string Sort { get; set; } = "";
        public Supplier Supplier { get; set; }
        public string Text {  get; set; }
        public List<IncomeItemEntity> Items{  get; set; }
        public List<Barcode> Barcodes {  get; set; }
        public Filter(List<IncomeItemEntity> items, List<Barcode> barcodes) 
        {
            Items = items;
            Barcodes = barcodes;
        }
        /// <summary>
        /// Apply filters and return data
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<IncomeItemVm> Apply()
        {
            var result = new ObservableCollection<IncomeItemVm>(Items.ToVmList());

            //apply supplier filter
            if (Supplier != null && Supplier.Id != -1 && Supplier.Name != "")
                result = new ObservableCollection<IncomeItemVm>(Items.Where(item => item.SupplierId == Supplier.Id).ToVmList());


            //apply text sorting
            if (Sort != "")
                result = new ObservableCollection<IncomeItemVm>(result.Where(item =>
                {
                    var barcodes = new List<Barcode>(Barcodes.Where(it => 
                        it.CatalogId == item.CatalogId
                        && it.Name.Contains(Sort.ToLower())));

                    return item.Name.ToLower().Contains(Sort.ToLower()) 
                    || item.Sku.ToLower().Contains(Sort.ToLower())
                    || barcodes.Count != 0;
                }));

            return result;
        }

    }
}
