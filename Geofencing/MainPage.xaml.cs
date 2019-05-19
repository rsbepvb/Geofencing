using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geofence.Plugin;
using Geofence.Plugin.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Geofencing
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public double latitude;
        public double longitude;

        public MainPage()
        {
            InitializeComponent();
            Console.WriteLine("Tracking -- in constructur Main");
            //Task.Run(() => this.StartMonitoringAsync()).Wait();
            StartMonitoringAsync();
        }


        public async Task StartMonitoringAsync() 
        {
            Console.WriteLine("Tracking -- in StartMonitoring -- using Plugin first line");

            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    latitude = location.Latitude;
                    longitude = location.Longitude;
                }
                else 
                {
                    Console.WriteLine("Tracking -- location == null");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine("Unable to get location -- not supported on device");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine("Unable to get location -- not enabled" + fneEx.ToString());
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("Unable to get location -- permission issue" + pEx.ToString());
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Unable to get location" + ex.ToString());
            }



            CrossGeofence.Current.StartMonitoring(new GeofenceCircularRegion("My Region", latitude, longitude, 10.0f)
            {

                //To get notified if user stays in region for at least 5 minutes
                NotifyOnStay = true,
                StayedInThresholdDuration = TimeSpan.FromSeconds(30), NotificationStayMessage="Stayed in Region for 30 seconds",
                NotifyOnExit = true, ExitThresholdDuration = TimeSpan.FromSeconds(30), NotificationExitMessage="Exited Region"

            });
        }
    }
}
