using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WmsDesktop.ViewModels
{
    internal class CreateAssemblySessionViewModel : INotifyPropertyChanged, IState
    {
        public PageStates PageState => PageStates.CreateAssemblySessionPage;

        public static async Task<CreateAssemblySessionViewModel> CreateAsync(Window window)
        {
            /* var jsonIp = File.ReadAllText("config.json");
             var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
             var ip = setting["Ip"];
             var catalogAndSuppliers = await client.GetAllCatalogsWithSuppliers(ip);
             var suppliers = await client.GetSuppliers(ip);
             var batches = await client.GetBatches(ip);
             var barcodes = await client.GetBarcodes(ip);
             var incomeCells = await client.GetIncomeCells(ip);
             var cellTypes = await client.GetCellTypes(ip);
             return new CreateAssemblySessionViewModel(catalogAndSuppliers, suppliers, barcodes, incomeCells, batches, cellTypes, window);*/
            return new CreateAssemblySessionViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
