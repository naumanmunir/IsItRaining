using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Util;

namespace IsItRaining.Includes
{
    [Service(Exported = false, Name = "com.IsItRaining.IsItRaining.LocationService")]
    public class LocationService : Service, ILocationListener
    {
        private static readonly string TAG = "BOOMBOOMTESTGPS";
        private LocationManager mLocationManager = null;
        private static readonly int LOCATION_INTERVAL = 1000;
        private static readonly float LOCATION_DISTANCE = 10f;


        Location mLastLocation;

        //public LocationListener(string provider)
        //{
        //    Log.Info(TAG, "LocationListener " + provider);
        //    mLastLocation = new Location(provider);
        //}


        public void OnLocationChanged(Location location)
        {
            Log.Info(TAG, "onLocationChanged " + location);
            GPSData.Latitude = location.Latitude;
            GPSData.Longitude = location.Longitude;
            //mLastLocation.Set(location);
                
        }

        public void OnProviderDisabled(string provider)
        {
            Log.Info(TAG, "OnProviderDisabled " + provider);
        }

        public void OnProviderEnabled(string provider)
        {
            Log.Info(TAG, "OnProviderEnabled " + provider);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            Log.Info(TAG, "OnStatusChanged " + provider);
        }
        

        //LocationListener[] mLocationListeners = new LocationListener[] {
        //        new LocationListener(LocationManager.GpsProvider),
        //        new LocationListener(LocationManager.NetworkProvider)
        //    };

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Info(TAG, "onStartCommand");
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnCreate()
        {
            Toast.MakeText(this, "Service Started", ToastLength.Long).Show();
            Log.Info(TAG, "OnCreate");
            
            InitializeLocationManager();

            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Android.Locations.Accuracy.Fine
            };

            IList<string> acceptableLocationProviders = mLocationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                try
                {
                    mLocationManager.RequestLocationUpdates(LocationManager.GpsProvider, 60000, 0, this);
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, "fail to request location update", ex.Message);
                }

                try
                {
                    mLocationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 60000, 0, this);
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, "fail to request location update", ex.Message);
                }
            }
            base.OnCreate();
        }

        private void InitializeLocationManager()
        {
            Log.Info(TAG, "initializeLocationManager");
            if (mLocationManager == null)
            {
                mLocationManager = (LocationManager)GetSystemService(LocationService);
            }
        }

        public override void OnDestroy()
        {
            Log.Info(TAG, "onDestroy");
            base.OnDestroy();
            if (mLocationManager != null)
            {

                try
                {
                    mLocationManager.RemoveUpdates(this);
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, "fail to remove location listners, ignore", ex.Message);
                }
                
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }

    public static class GPSData
    {
        public static double Latitude { get; set; }
        public static double Longitude { get; set; }
    }
}