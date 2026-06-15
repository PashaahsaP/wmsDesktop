using System.Collections.Generic;
using WmsDesktop.Classes;

namespace WmsDesktop.Converter
{
    public static class Mapper
    {
        public static IncomeItemVm ToVm(this IncomeItemEntity item, List<Cell> cells)
        {
            return new IncomeItemVm()
            {
                CatalogId = item.CatalogId,
                Name = item.Name,
                Other = item.Other,
                Sku = item.Sku,
                TE = cells,
            };
        }
        public static List<IncomeItemVm> ToVmList(this List<IncomeItemEntity> item)
        {
            return new List<IncomeItemVm>();
        }
        public static List<IncomeItemVm> ToVmList(this IEnumerable<IncomeItemEntity> item)
        {
            return new List<IncomeItemVm>();
        }
        public static IncomeItemEntity ToEntity(this IncomeItemVm item)
        {
            return new IncomeItemEntity();
        }
        public static List<IncomeItemEntity> ToEntityList(this List<IncomeItemVm> item)
        {
            return new List<IncomeItemEntity>();
        }
        public static List<IncomeItemEntity> ToEntityList(this IEnumerable<IncomeItemVm> item)
        {
            return new List<IncomeItemEntity>();
        }
    }
}
