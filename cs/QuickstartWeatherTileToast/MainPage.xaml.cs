using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QuickstartWeatherTileToast
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            // Generate the notification content
            XmlDocument content = NotificationHelper.GenerateToastContent();

            // Create the notification
            ToastNotification notif = new ToastNotification(content);

            // And show it
            ToastNotificationManager.CreateToastNotifier().Show(notif);
        }

        private async void ButtonPinUpdateTile_Click(object sender, RoutedEventArgs e)
        {
            SecondaryTile tile = new SecondaryTile(DateTime.Now.Ticks.ToString())
            {
                DisplayName = "Seattle",
                Arguments = "action=viewForecast&zip=98008"
            };

            tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
            tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
            tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");

            if (!await tile.RequestCreateAsync())
            {
                return;
            }

            // Generate the notification content
            XmlDocument content = NotificationHelper.GenerateTileContent();

            // Create the notification
            TileNotification notif = new TileNotification(content);

            // And update the tile with the notification
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).Update(notif);
        }
    }
}
