using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop.Classes
{
    internal class Goods
    {
        public string Id { get; set; } = string.Empty;
        public int Amount { get; set; } = -1;
        public string CellId { get; set; } = string.Empty;
        public string PackageId { get; set; } = string.Empty;
        public string CatalogId { get; set; } = string.Empty;
        public long CreatedAt { get; set; } = -1;
        public bool IsAvailable{  get; set; } = false;
        public long UpdatedAt { get; set;  } = -1;
        public long DeletedAt { get; set;  } = -1;
        public bool IsDeleted{ get; set; } = false;
        public string Other{ get; set; } = string.Empty;
    }
}
