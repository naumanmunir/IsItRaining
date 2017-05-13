using System;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using System.Threading.Tasks;
using System.Text;
using OpenWeatherMap;
using DarkSkyApi;
using ME.Grantland.Widget;
using IsItRaining.Includes;
using Android.Support.V7.App;

namespace IsItRaining
{
    [Activity(Label = "Is It Raining?", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity, ILocationListener
    {
        private Android.Support.V7.Widget.Toolbar toolBar;
        TextView _addressText;
        static Android.Locations.Location _currentLocation;
        static LocationManager _locationManager;
        

        string _locationProvider;

        TextView weatherStatus;
        TextView rainingOrNot;
        TextView currentTempurature;
        FrameLayout linearlayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);


            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.include_toolbar);
            toolBar.BringToFront();
            SetSupportActionBar(toolBar);
            

            StartService(new Android.Content.Intent(this, typeof(LocationService)));

            //_addressText = FindViewById<TextView>(Resource.Id.address_text);

            weatherStatus = FindViewById<TextView>(Resource.Id.weatherSummary_text);
            rainingOrNot = FindViewById<TextView>(Resource.Id.status);
            currentTempurature = FindViewById<TextView>(Resource.Id.tempurature_text);
            linearlayout = FindViewById<FrameLayout>(Resource.Id.linearlayout1);

            //FindViewById<TextView>(Resource.Id.get_address_button).Click += AddressButton_OnClick;


            //InitializeLocationManager();

        }

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);

            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Android.Locations.Accuracy.Fine
            };

            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();


                //_locationManager.GetLastKnownLocation("gps");
                //_locationManager.GetLastKnownLocation("network");

                _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
                _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, this);

                if (_currentLocation == null)
                {
                    //error
                    System.Diagnostics.Debug.WriteLine("No Location found");
                }
                else
                {
                    
                }
            }
            else
            {
                _locationProvider = string.Empty;

                //LocationSettingsRequest
            }

        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            _currentLocation = location;

            if (_currentLocation == null)
            {
                
            }
            else
            {
                GetCurrentWeather(_currentLocation.Latitude, _currentLocation.Longitude);
            }
        }

        private void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                //_addressText.Text = deviceAddress.ToString();
            }
            else
            {
                _addressText.Text = "Unable to determine the address. Try again in a few minutes.";
            }
        }

        //private async Task<Address> ReverseGeocodeCurrentLocation()
        //{
        //    Geocoder geocoder = new Geocoder(this);
        //    IList<Address> addressList =
        //        await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

        //    Address address = addressList.FirstOrDefault();
        //    return address;
        //}

        private void GetCurrentWeather(double latitde, double longitude)
        {
            DarkSkyService dk = new DarkSkyService("689a0d61985dfd4dae9cb86f67dcad79");

            var request = dk.GetWeatherDataAsync(latitde, longitude);

            SetStatus(request.Result.Currently.Icon);

            weatherStatus.Text = request.Result.Minutely.Summary;

            DisplayAccurateImage(request.Result.Minutely.Icon);

            if (request.Result.TimeZone.Contains("America"))
            {
                currentTempurature.Text = Math.Round(request.Result.Currently.ApparentTemperature).ToString() + " \u2109";
            }
            else
            {
                var toCelsius = (Math.Round(request.Result.Currently.ApparentTemperature) - 32) * (5 / 9);
                currentTempurature.Text = toCelsius.ToString() + " \u2103";
            }
            

            //request.Result.
        }

        private void DisplayAccurateImage(string summary)
        {
            var currentTime = DateTime.Now;

            switch (summary)
            {
                case "clear-day":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    break;
                case "clear-night":
                    linearlayout.SetBackgroundResource(Resource.Drawable.rain);
                    break;
                case "rain":
                    linearlayout.SetBackgroundResource(Resource.Drawable.rain);
                    break;
                case "snow":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    break;
                case "sleet":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    break;
                case "wind":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    break;
                case "fog":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    break;
                case "cloudy":
                    linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    break;
                case "partly-cloudy-day":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    break;
                case "partly-cloudy-night":
                    linearlayout.SetBackgroundResource(Resource.Drawable.rain);
                    break;
                default:
                    break;
            }
        }

        private void SetStatus(string summary)
        {
            if (summary.Contains("rain"))
            {
                rainingOrNot.Text = "yep";
            }
            else
            {
                rainingOrNot.Text = "nahh";
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            //_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            //_locationManager.RemoveUpdates(this);
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        async void AddressButton_OnClick(object sender, EventArgs eventArgs)
        {
            if (_currentLocation == null)
            {
                _addressText.Text = "Can't determine the current address. Try again in a few minutes.";
                return;
            }
            else
            {

            }
            //Address address = await ReverseGeocodeCurrentLocation();
            //DisplayAddress(address);
        }
    }
}

