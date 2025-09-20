using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsDesktop.ViewModels
{
    internal class AddingCatalogViewModel : INotifyPropertyChanged
    {
        private readonly Client client = new Client();
        private string ip = "192.168.0.11";
        public List<OrderItem> ItemsList { get; set; }

        public AddingCatalogViewModel()
        {
            var jsonIp = File.ReadAllText("config.json");
            var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIp);
            ip = setting["Ip"];
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
