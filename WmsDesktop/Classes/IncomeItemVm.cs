using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public class IncomeItemVm
    {
        public string CatalogId { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string TE { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Other {  get; set; } = string.Empty;
        public int Count { get; set; } = 1;
        public bool isValid { get; set; } = true;
        public bool isSelected { get; set; } = false;
        public override string ToString()
        {
            return Name;
        }
    }

    public class IncomeItemWithDateVm: IncomeItemVm 
    {
        public string Date { get; set; } = string.Empty;    
    }
    public class IncomeItemWithBatchVm: IncomeItemVm 
    {
        public string Batches { get; set; } = string.Empty;
    }

    //Selected. Для отображения от типа класса DataTemplate. via binding
    public class SelectedIncomeItemVm : IncomeItemVm {}
    public class SelectedIncomeItemWithDateVm: IncomeItemWithDateVm { }
    public class SelectedIncomeItemWithBatchVM: IncomeItemWithBatchVm { }
}
