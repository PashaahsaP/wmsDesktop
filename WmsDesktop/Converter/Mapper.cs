using System.Collections.Generic;
using WmsDesktop.Classes;

namespace WmsDesktop.Converter
{
    public static class Mapper
    {
        public static IncomeItemVm ToVm(this BaseIncomeItemEntity item)
        {
            return new BaseIncomeItemVm();
        }
        public static List<IncomeItemVm> ToVmList(this List<BaseIncomeItemEntity> item)
        {
            return new List<IncomeItemVm>();
        }
        public static List<IncomeItemVm> ToVmList(this IEnumerable<BaseIncomeItemEntity> item)
        {
            return new List<IncomeItemVm>();
        }
        public static BaseIncomeItemEntity ToEntity(this IncomeItemVm item)
        {
            return new BaseIncomeItemEntity();
        }
        public static List<BaseIncomeItemEntity> ToEntityList(this List<IncomeItemVm> item)
        {
            return new List<BaseIncomeItemEntity>();
        }
        public static List<BaseIncomeItemEntity> ToEntityList(this IEnumerable<IncomeItemVm> item)
        {
            return new List<BaseIncomeItemEntity>();
        }
    }
}
