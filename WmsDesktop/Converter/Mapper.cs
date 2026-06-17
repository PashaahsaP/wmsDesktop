using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WmsDesktop.Classes;

namespace WmsDesktop.Converter
{
    public static class Mapper
    {
        public static IncomeItemVm ToVm(this IncomeItemEntity item)
        {
            return new IncomeItemVm()
            {
                CatalogId = item.CatalogId,
                Name = item.Name,
                Other = item.Other,
                Sku = item.Sku,
                TE = new List<Cell>(),
            };
        }
        public static List<IncomeItemVm> ToVmList(this List<IncomeItemEntity> collection)
        {
            var result = new List<IncomeItemVm>(); 
            foreach (var item in collection)
            {
                result.Add(item.ToVm());
            }
            return result;
        }
        public static List<IncomeItemVm> ToVmList(this IEnumerable<IncomeItemEntity> collection)
        {
            var result = new List<IncomeItemVm>();
            foreach (var item in collection)
            {
                result.Add(item.ToVm());
            }
            return result;
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
