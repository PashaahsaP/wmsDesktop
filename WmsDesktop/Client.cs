using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WmsDesktop
{
    public class Client
    {
        private readonly HttpClient client = new HttpClient();
        internal async void CreateAssebmlySession(ObservableCollection<IUiItem> items, string ip, int countOfItems, int linesOfItem, int supplier)
        {
            
            

        }
        internal async Task<ID> SendCell(string name, string ip)
        {
            ID result = null;
            try
            {
                var json = JsonConvert.SerializeObject(new Name() { name = name });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/cell", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ID>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<ID> GetCellIdByName(string cellName, string ip)
        {
            ID result = null;
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/cell/name/{cellName}");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ID>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<Cell> GetCellIdById(string cellId, string ip)
        {
            Cell result = null;
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/cell/id/{cellId}");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<Cell>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        
        internal async Task<ID> SendAssemlbySession(AssemblySession session, string ip)
        {

            ID sessionId = null;
            try
            {
                var json = JsonConvert.SerializeObject(session);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/assembly_session", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                sessionId = JsonConvert.DeserializeObject<ID>(data);
            }
            catch (Exception ex)
            {
                return sessionId;
            }

            return sessionId;
        }
        internal async Task<Int32> GetGoodsBorkBalance(string catalogId, string ip)
        {
            ID cellId = await GetCellIdByName("Z900", ip);
            if (cellId == null)
            {
                cellId = await SendCell("Z900", ip);
            }
            List<GoodsBorkItem> collection = await GetAllBorkGoodsByCatalogId(catalogId, ip);
            collection = collection.Where(i => i.cellId != cellId.id.ToString()).ToList();
            var count = collection.Sum(sum => sum.amount);
            return count;
        }
        internal async Task<List<BarcodeItem>> GetBarcodeAndCatalogBork(List<string> barcodes, List<string> count, string ip)
        {
            var resultOfResponse = new List<BarcodeItem>();
            foreach (var barcode in barcodes)
            {
                try
                {
                    var response = await client.GetAsync($"http://{ip}:3000/barcodeBork/name/{barcode}");
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BarcodeItem>(data);
                    resultOfResponse.Add(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return resultOfResponse;
        }
        internal async Task<List<AtomyGoodsItem>> GetBarcodeAndCatalogAtomy(List<string> barcodes, List<string> count, string ip)
        {
            var resultOfResponse = new List<AtomyGoodsItem>();
            foreach (var barcode in barcodes)
            {
                try
                {
                    var response = await client.GetAsync($"http://{ip}:3000/goods_atomy/TE/{barcode}");
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AtomyGoodsItem>(data);
                    resultOfResponse.Add(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return resultOfResponse;
        }
        internal async Task<BarcodeItem> GetBarcodeByName(string barcode, string ip)
        {
            BarcodeItem barcodeResult = null;
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/barcodeBork/name/{barcode}");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BarcodeItem>(data);
                barcodeResult = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return barcodeResult;
        }
        internal async Task<List<GoodsBorkItem>> GetAllBorkGoodsByCatalogId(string catalogId, string ip)
        {
            List<GoodsBorkItem> barcodeResult = null;
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/goods_bork/catalogId/{catalogId}");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<GoodsBorkItem>>(data);
                barcodeResult = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return barcodeResult;
        }
        internal async Task<AtomyGoodsItem> GetAtomyGoodsByTE(string TE, string ip)
        {
            AtomyGoodsItem barcodeResult = null;
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/goods_atomy/TE/{TE}");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AtomyGoodsItem>(data);
                barcodeResult = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return barcodeResult;
        }
        internal async Task<BorkCatalogItem> GetCatalogBorkByName(string catalogName, string ip)
        {
            BorkCatalogItem barcodeResult = null;
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/catalogsBorks/name/{catalogName.Replace("/", "%2F")}");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BorkCatalogItem>(data);
                barcodeResult = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return barcodeResult;
        }
        internal async Task<BorkCatalogItem> SendCatalogBork(BorkCatalogItem catalog, string ip)
        {
            BorkCatalogItem result = null;
            try
            {
                var json = JsonConvert.SerializeObject(catalog);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/catalogBork", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BorkCatalogItem>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<ID> SendGoodsBork(GoodsBorkItem goods, string ip)
        {
            ID result = null;
            try
            {
                var json = JsonConvert.SerializeObject(goods);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/goodsBork", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ID>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<string> SendAssemblyItemBork(AssemblyBorkItem asItem, string ip)
        {
            string result = null;
            try
            {
                var json = JsonConvert.SerializeObject(asItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/assemblyBorkItem", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<string>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<string> SendAssemblyItemAtomy(AssemblyBorkItem asItem, string ip)
        {
            string result = null;
            try
            {
                var json = JsonConvert.SerializeObject(asItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/assemblyAtomyItem", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<string>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<string> UpdateBorkGoodsAsync(GoodsBorkItem item, string ip)
        {
            string result = null;
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"http://{ip}:3000/goodsBork/update/", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<string>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<string> UpdateAtomyGoodsAsync(AtomyGoodsItem item, string ip)
        {
            string result = null;
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"http://{ip}:3000/goodsAtomy/update/", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<string>(data);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
        internal async Task<string> SendBarcodeBork(BarcodeItem barcode, string ip)
        {
            string barcodeId = null;
            try
            {
                var json = JsonConvert.SerializeObject(barcode);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{ip}:3000/barcodeBork", content);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                barcodeId = JsonConvert.DeserializeObject<string>(data);
            }
            catch (Exception ex)
            {

            }

            return barcodeId;
        }
        internal async Task<string> GetAllCatalogBork(string ip)
        {
            var result = "";
            try
            {
                var response = await client.GetAsync($"http://{ip}:3000/allCatalogBork");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                result = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return result;
        }
    }
}
