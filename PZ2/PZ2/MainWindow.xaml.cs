using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Controller.XMLLoader.LoadXml();

            //GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            //GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            //gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            //GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            ////gmap.SetPositionByKeywords("Novi Sad, Serbia");
            //gmap.Position = new GMap.NET.PointLatLng(45.254765, 19.844184);
            //gmap.ShowCenter = false;
        }
    }
}
