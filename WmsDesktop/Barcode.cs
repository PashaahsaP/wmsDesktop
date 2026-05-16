using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public class Barcode
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public string CatalogId {  get; set; }
        public Barcode(string id, string name, string catalogId)
        {
            Id = id;
            Name = name;
            CatalogId = catalogId;
        }
    }
}
