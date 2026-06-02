using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public class CatalogItemBase
    {
        public string Id {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int SupplierId { get; set; } = 0;
        public List<Barcode> BarcodeList { get; set; } = new List<Barcode>();
        public string Barcode { get; set; } = string.Empty;
        public string Other {  get; set; } = string.Empty;

    }
    public class WithDate : CatalogItemBase { 
        public DateTime Date { get; set; }
    }
    public class WithBatch : CatalogItemBase
    {
        public List<Batch> Batches { get; set; }
    }

}
