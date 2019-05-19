using System;
using Geofence.Plugin.Abstractions;

namespace Geofencing
{
    public class CrossGeofenceListener : IGeofenceListener
        {
            public void OnMonitoringStarted(string region)
            {
                //Debug.WriteLine(string.Format("{0} - Monitoring started in region: {1}", CrossGeofence.Tag, region));
            }

            public void OnMonitoringStopped()
            {
                //Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Tag, "Monitoring stopped for all regions"));
            }

            public void OnMonitoringStopped(string identifier)
            {
                //Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Tag, "Monitoring stopped in region", identifier));
            }

            public void OnError(string error)
            {
                // Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Tag, "Error", error));
            }

            // Note that you must call CrossGeofence.GeofenceListener.OnAppStarted() from your app when you want this method to run.
            public void OnAppStarted()
            {
                //Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Tag, "App started"));
            }

            public void OnRegionStateChanged(GeofenceResult result)
            {
                //Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Tag, result.ToString()));
            }

            public void OnLocationChanged(GeofenceLocation location)
            {
            Console.WriteLine("Tracking -- LocationChanged Sent");
            }
      }

}
