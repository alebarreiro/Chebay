using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace WindowsPhoneApp.Data
{
    public class ProductoItem
    {
        public long ProductoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        //public DateTime fecha_cierre { get; set; }
        public int PrecioActual { get; set; }
        public string IDOfertante { get; set; }
        public string ImagePath { get; set; }
    }

    public sealed class ProductoSource
    {
        private static ProductoSource _sampleDataSource = new ProductoSource();

        private ObservableCollection<ProductoItem> _items = new ObservableCollection<ProductoItem>();
        public ObservableCollection<ProductoItem> Items
        {
            get { return this._items; }
        }
     
        public static async Task<ProductoItem> GetItemAsync(long uniqueId)
        {
            //await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Items.Select(prod => prod).Where((item) => item.ProductoID.Equals(uniqueId));
            //.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static void AddItem(ProductoItem pi)
        {
            _sampleDataSource.Items.Add(pi);
        }
    }


}
