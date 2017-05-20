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
using Android.Views;
using Android.Support.V4.Widget;
using Android.Animation;
using Android.Views.Animations;
using Android.Gms.Ads;

namespace IsItRaining
{
    [Activity(Label = "Is It Raining?", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity, ILocationListener
    {
        private Android.Support.V7.Widget.Toolbar toolBar;
        TextView _addressText;
        static Android.Locations.Location _currentLocation;
        static LocationManager _locationManager;

        private AdView adView; 

        string _locationProvider;

        TextView weatherStatus;
        TextView rainingOrNot;
        TextView currentTempurature;
        LinearLayout linearlayout;
        private DrawerLayout drawerLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);

            //toolbar_container = FindViewById<LinearLayout>(Resource.Id.toolbar_container);
            MobileAds.Initialize(this, "ca-app-pub-2637596544494423~2520806396");

            adView = FindViewById<AdView>(Resource.Id.adView);
            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.custom_toolBar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("Samsung-SM-N900A").Build();

            adView.LoadAd(adRequest);

            //_addressText = FindViewById<TextView>(Resource.Id.address_text);

            weatherStatus = FindViewById<TextView>(Resource.Id.weatherSummary_text);
            rainingOrNot = FindViewById<TextView>(Resource.Id.status);
            currentTempurature = FindViewById<TextView>(Resource.Id.tempurature_text);
            linearlayout = FindViewById<LinearLayout>(Resource.Id.linearlayout1);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //FindViewById<TextView>(Resource.Id.get_address_button).Click += AddressButton_OnClick;

            //StartService(new Android.Content.Intent(this, typeof(LocationService)));
            //GetCurrentWeather(GPSData.Latitude, GPSData.Longitude);

            InitializeLocationManager();

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
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
            if (latitde != 0 && longitude != 0)
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
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
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
                    linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    break;
                case "partly-cloudy-night":
                    linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
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
                AnimateText();
            }
            else
            {
                rainingOrNot.Text = "nahh";
                AnimateText();

            }
        }

        private void AnimateText()
        {
            Animation myAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.scaling);
            rainingOrNot.StartAnimation(myAnimation);

            weatherStatus.StartAnimation(myAnimation);
            rainingOrNot.StartAnimation(myAnimation);
            currentTempurature.StartAnimation(myAnimation);
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

