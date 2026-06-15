using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmsDesktop.Classes;

namespace WmsDesktop.Converter
{
    public static class Mapper
    {
        public static IncomeItemVm ToVm(this BaseIncomeItemEntity item)
        {
            return new BaseIncomeItemVm();
        }
        public static List<IncomeItemVm> ToVmLsit(this List<BaseIncomeItemEntity> item)
        {
            return new List<IncomeItemVm>();
        }
        public static List<IncomeItemVm> ToVmLsit(this IEnumerable<BaseIncomeItemEntity> item)
        {
            return new List<IncomeItemVm>();
        }
    }
}
