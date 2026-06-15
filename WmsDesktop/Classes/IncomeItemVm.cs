using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public abstract class IncomeItemVm
    {
        public string Id { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string TE { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Other {  get; set; } = string.Empty;
        public int Count { get; set; } = 0;
        public bool isValid { get; set; } = true;

    }

    public class BaseIncomeItemVm: IncomeItemVm { }
    public class IncomeBaseSelectedItem: IncomeItemVm { }
    public class IncomeSessionWrongItem: IncomeItemVm { }
    public class IncomeWithDateItem: IncomeItemVm 
    {
        public string Date { get; set; } = string.Empty;    
    }
    public class IncomeWithBatchItem: IncomeItemVm 
    {
        public List<Batch> Batches { get; set; } = new List<Batch>();
    }

}
