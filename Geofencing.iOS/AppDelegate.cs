using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Geofence.Plugin;
using UIKit;
using UserNotifications;

namespace Geofencing.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public static LocationManagerCLS Manager { get; set; }



        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Calabash.Start();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            //CrossGeofence.Initialize<CrossGeofenceListener>();


            //cls.StartGeofenceMonitoringAsync();

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);


            Console.WriteLine("In iOS Finished Launching...");

            //FOR CLLOCATIONMANAGER 
            // As soon as the app is done launching, begin generating location updates in the location manager
            Manager = new LocationManagerCLS();
            Manager.StartLocationUpdates();
           // Manager.StartGeofenceMonitoringAsync();
            //Manager.LocationUpdated += HandleLocationChanged;


            ///so you can send location notifications:
            ///
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => { });

                // Watch for notifications while app is active
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }


            return base.FinishedLaunching(app, options);

          
        }



    }
}
