using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public class Supplier
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }
    public class OrderItem
    {
        public string id { get; set; }
        public string supplierId { get; set; }
        public string name { get; set; }
        public string supplierName { get; set; }
        public string sku{ get; set; }

        public override string ToString() => name;
    }
    /// <summary>
    /// Интерфейс для ui элементов
    /// </summary>
    /// <param name="Count">Сколько нужно для заявки</param>
    /// <param name="Amount">Сколько есть Фактически</param>
    public interface IUiItem
    {
        string Left { get;  }
        string Center { get;  }
        string Right { get;  }

        string Name { get; set; }
        int Count { get; set; }
        int Amount { get; set; }
        OrderItem Catalog {  get; set; }

    }
}
