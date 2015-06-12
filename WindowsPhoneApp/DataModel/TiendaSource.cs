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
    public class TiendaItem
    {
        public string idTienda { get; set; }
    }

    public sealed class TiendaSource
    {
        private static TiendaSource _sampleDataSource = new TiendaSource();

        private ObservableCollection<TiendaItem> _items = new ObservableCollection<TiendaItem>();
        public ObservableCollection<TiendaItem> Items
        {
            get { return this._items; }
        }

        public static async Task<TiendaItem> GetItemAsync(string uniqueId)
        {
            //await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Items.Select(prod => prod).Where((item) => item.idTienda.Equals(uniqueId));
            //.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static void AddItem(TiendaItem pi)
        {
            _sampleDataSource.Items.Add(pi);
        }
    }


}
