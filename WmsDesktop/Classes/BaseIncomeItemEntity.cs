using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop.Classes
{
    public class BaseIncomeItemEntity
    {
        public string Id { get; set; } = string.Empty;
        public int SupplierId { get; set; } = 0;

        public string Name { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Other { get; set; } = string.Empty;
    }
    public class WithDateIncomeItemEntity : BaseIncomeItemEntity
    {
        public string Date { get; set;  } = string.Empty;
    }
    public class WithBatchIncomeItemEntity : BaseIncomeItemEntity 
    {
        public List<Batch> Batches { get; set; } = new List<Batch>();
    }
}
