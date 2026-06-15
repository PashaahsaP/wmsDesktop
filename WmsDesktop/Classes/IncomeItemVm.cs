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
        public List<string> TE { get; set; } = new List<string>();
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
        public List<Batch> Batches { get; set; } = new List<Batch>();
    }

}
