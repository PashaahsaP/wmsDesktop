using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop
{
    public abstract class IncomeSessionItemBase
    {
        public string Sku { get; set; }
        public string Name { get; set; }

        public int Count { get; set; }
        public bool isValid { get; set; }

    }

    public class IncomeSessionItem: IncomeSessionItemBase { }
    public class IncomeSessionSelectedItem: IncomeSessionItemBase { }
    public class IncomeSessionWrongItem: IncomeSessionItemBase { }
}
