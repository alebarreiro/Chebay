using WindowsPhoneApp.Common;
using WindowsPhoneApp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using Windows.ApplicationModel;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WindowsPhoneApp
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            //this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        private void GroupSection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(SectionPage), groupId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Debug.WriteLine("ASDF");
            var itemId = ((ProductoItem)e.ClickedItem).ProductoID;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async Task<string> BuscarProducto()
        {
            HttpClient client = new HttpClient();
            string url = "http://chebayrest1956.azurewebsites.net/api/subasta";
            var baseUrl = string.Format(url);
            Debug.WriteLine("C");
            string result = await client.GetStringAsync(baseUrl);
            Debug.WriteLine("D: " + result);
            return result;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        
        private async void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            string json = await BuscarProducto();
            await deserializeJsonAsync(json);
        }

        private const string JSONFILENAME = "data.json";

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        private async Task deserializeJsonAsync(string json)
        {
            using (Stream s = GenerateStreamFromString(json))
            {
                string content = String.Empty;

                List<ProductoItem> myCars;
                var jsonSerializer = new DataContractJsonSerializer(typeof(List<ProductoItem>));

                //var myStream = await Package.Current.InstalledLocation.OpenStreamForReadAsync(JSONFILENAME);

                myCars = (List<ProductoItem>)jsonSerializer.ReadObject(s);

                foreach (var car in myCars)
                {
                    content += String.Format("ID: {0}, Make: {1}, Model: {2} ... ", car.ProductoID, car.Nombre, car.Descripcion);
                    ProductoSource.AddItem(car);
                }
                Debug.WriteLine(content);
                this.DefaultViewModel["Productos"] = myCars;
            }


            /*

            string content = String.Empty;

            List<ProductoItem> myCars;
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<ProductoItem>));
            
            var myStream = await Package.Current.InstalledLocation.OpenStreamForReadAsync(JSONFILENAME);

            myCars = (List<ProductoItem>)jsonSerializer.ReadObject(myStream);
            
            foreach (var car in myCars)
            {
                content += String.Format("ID: {0}, Make: {1}, Model: {2} ... ", car.ProductoID, car.Nombre, car.Descripcion);
                ProductoSource.AddItem(car);
            }
            Debug.WriteLine(content);
            this.DefaultViewModel["Productos"] = myCars;*/
         //   resultTextBlock.Text = content;
        }

    }
}
