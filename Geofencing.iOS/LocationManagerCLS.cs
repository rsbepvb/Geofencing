using System;
using System.Threading.Tasks;
using CoreLocation;
using Plugin.LocalNotifications;
using UIKit;
using Xamarin.Essentials;

namespace Geofencing.iOS
{
    public class LocationManagerCLS
    {
        double longitude;
        double latitude;
        protected CLLocationManager locMgr;
        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };
        public event EventHandler<RegionChangedEventArgs> RegionLeft = delegate { };
        public event EventHandler<RegionChangedEventArgs> RegionEntered = delegate { };

        public LocationManagerCLS()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;


            LocationUpdated += PrintLocation;

            Console.WriteLine("About to Check entitlements");
            // iOS 8 has additional permissions requirements
            try
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    locMgr.RequestAlwaysAuthorization(); // works in background
                                                         //locMgr.RequestWhenInUseAuthorization (); // only in foreground
                    Console.WriteLine("System Version 8.0 or higher -- requesting always authorization");
                }

                if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    locMgr.AllowsBackgroundLocationUpdates = true;
                    Console.WriteLine("System Version 9.0 or higher -- AllowBackgroundLocationUpdates set to true");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting entitlements: " + ex.ToString());
            }

           StartGeofenceMonitoringAsync().ContinueWith(
                   t =>
                   {
                       if (t.IsFaulted)
                           Console.WriteLine(t.Exception);
                   });

        }


        public async Task StartGeofenceMonitoringAsync() 
        {

            locMgr = new CLLocationManager();
            locMgr.PausesLocationUpdatesAutomatically = false;
            locMgr.ShowsBackgroundLocationIndicator = true;


            try
            {
                Console.WriteLine("Getting location in ios ");
                var location = await Geolocation.GetLocationAsync();
                longitude = location.Longitude;
                latitude = location.Latitude;

                if (location != null)
                {
                    Console.WriteLine("Xamarin Essentials Location");
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }//END GET LOCATION


            CLCircularRegion region = new CLCircularRegion(new CLLocationCoordinate2D(latitude, longitude), 10, "The Standard");

            if (CLLocationManager.IsMonitoringAvailable(typeof(CLCircularRegion)))
            {

                LocMgr.DidStartMonitoringForRegion +=  (o, e) => 
                {
                    Console.WriteLine("Now monitoring region {0}", e.Region.ToString());
                    CrossLocalNotifications.Current.Show("Now monitoring region.", e.Region.ToString());
                };

                locMgr.RegionEntered += (o, e) =>
                {
                    Console.WriteLine("Just entered " + e.Region.ToString());
                    CrossLocalNotifications.Current.Show("Just enterecd region.", e.Region.ToString());
                };

                locMgr.RegionLeft += (o, e) =>
                {
                    Console.WriteLine("Just left " + e.Region.ToString());
                    CrossLocalNotifications.Current.Show("Just left region.", e.Region.ToString());
                };

                region.NotifyOnEntry = true;
                region.NotifyOnExit = true;
                locMgr.StartMonitoring(region);
                Console.WriteLine("Now monitoring in Native iOS Region");
            }
            else
            {
                Console.WriteLine("This app requires region monitoring, which is unavailable on this device");
            }


        }

      
        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }

        public  void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired accuracy, in meters
                LocMgr.DesiredAccuracy = 1;

                LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    // fire our custom Location Updated event
                    LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                };
                LocMgr.StartUpdatingLocation();
                locMgr.StartMonitoringSignificantLocationChanges();
               //await StartGeofenceMonitoringAsync();
            }

        }

        //This will keep going in the background and the foreground
        public  void PrintLocation(object sender, LocationUpdatedEventArgs e)
        {
            CLLocation location = e.Location;
            //Console.WriteLine("Altitude: " + location.Altitude + " meters");
           // Console.WriteLine("Longitude: " + location.Coordinate.Longitude);
           // Console.WriteLine("Latitude: " + location.Coordinate.Latitude);
           // Console.WriteLine("Course: " + location.Course);
           // Console.WriteLine("Speed: " + location.Speed);



        } 


    }  //END MAIN CLASS



    public class LocationUpdatedEventArgs : EventArgs
    {
        CLLocation location;

        public LocationUpdatedEventArgs(CLLocation location)
        {
            this.location = location;
        }

        public CLLocation Location
        {
            get { return location; }
        }
    }


    public class RegionChangedEventArgs : EventArgs
    {
        CLCircularRegion region;

        public RegionChangedEventArgs(CLCircularRegion region)
        {
            this.region = region;
        }

        public CLCircularRegion Region
        {
            get { return region; }
        }
    }
}
