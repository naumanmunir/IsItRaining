using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Views.Animations;
using DarkSkyApi;
using IsItRaining.Includes;
using Android.Support.V7.App;

namespace IsItRaining.Fragments
{
    public class WeatherFragment : Android.Support.V4.App.Fragment, ILocationListener
    {
        TextView weatherStatus;
        TextView rainingOrNot;
        TextView currentTempurature;
        ImageView bgImage;
        LinearLayout linearlayout;
        Android.Support.V7.Widget.Toolbar toolBar;

        static Location _currentLocation;
        static LocationManager _locationManager;
        string _locationProvider;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Weather,
       container, false);

            weatherStatus = view.FindViewById<TextView>(Resource.Id.weatherSummary_text);
            rainingOrNot = view.FindViewById<TextView>(Resource.Id.status);
            currentTempurature = view.FindViewById<TextView>(Resource.Id.tempurature_text);
            //linearlayout = view.FindViewById<LinearLayout>(Resource.Id.linearlayout1);
            //bgImage = view.FindViewById<ImageView>(Resource.Id.bgImage);

            View toolbar_view = view.FindViewById(Resource.Id.custom_toolbar_container_weather);
            toolBar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.custom_toolBar);

            ((AppCompatActivity)Activity).SetSupportActionBar(toolBar);

            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayShowTitleEnabled(false);
            ((AppCompatActivity)Activity).SupportActionBar.SetHomeButtonEnabled(true);
            ((AppCompatActivity)Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            InitializeLocationManager();

            // Use this to return your custom view for this Fragment
            return view;

        }

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);

            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Android.Locations.Accuracy.Fine
            };

            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();


                var loc = _locationManager.GetLastKnownLocation("gps");
                loc = _locationManager.GetLastKnownLocation("network");

                if (loc != null && loc.Time > DateTime.Now.Millisecond - 2 * 60 * 1000)
                {
                    GetCurrentWeather(loc.Latitude, loc.Longitude);
                }
                else
                {
                    _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
                    _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, this);
                }

            }
            else
            {
                _locationProvider = string.Empty;
            }

        }

        private void GetCurrentWeather(double latitde, double longitude)
        {
            if (latitde != 0 && longitude != 0)
            {
                DarkSkyService dk = new DarkSkyService("689a0d61985dfd4dae9cb86f67dcad79");


                var request = dk.GetWeatherDataAsync(latitde, longitude);

                //DisplayAccurateImage(request.Result.Minutely.Icon);

                SetStatus(request.Result.Currently.Icon);

                weatherStatus.Text = request.Result.Minutely.Summary;

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
        }

        private void DisplayAccurateImage(string summary)
        {
            var currentTime = DateTime.Now;

            switch (summary)
            {
                case "clear-day":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_day_beach);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_day_beach);
                    AnimateImages();
                    break;
                case "clear-night":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_night);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_night);
                    AnimateImages();
                    break;
                case "rain":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.rain);
                    bgImage.SetBackgroundResource(Resource.Drawable.rain);

                    AnimateImages();
                    break;
                case "snow":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_day);

                    AnimateImages();
                    break;
                case "sleet":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "wind":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "fog":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "cloudy":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    bgImage.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    AnimateImages();
                    break;
                case "partly-cloudy-day":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    bgImage.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    AnimateImages();
                    break;
                case "partly-cloudy-night":
                    //linearlayout.SetBackgroundResource(Resource.Drawable.clear_day_beach);
                    bgImage.SetBackgroundResource(Resource.Drawable.clear_day_beach);
                    AnimateImages();
                    break;
                default:
                    break;
            }
        }

        private void AnimateImages()
        {
            //Animation fadeIn = AnimationUtils.LoadAnimation(Context, Resource.Animation.fade_in);
            //linearlayout.StartAnimation(fadeIn);

            //fadeIn.SetAnimationListener(new AnimationListener(Context, linearlayout));
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
                rainingOrNot.Text = "nah";
                AnimateText();

            }
        }

        private void AnimateText()
        {
            Animation myAnimation = AnimationUtils.LoadAnimation(Context, Resource.Animation.scaling);
            rainingOrNot.StartAnimation(myAnimation);

            weatherStatus.StartAnimation(myAnimation);
            rainingOrNot.StartAnimation(myAnimation);
            currentTempurature.StartAnimation(myAnimation);
        }

        public void OnLocationChanged(Location location)
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

        public override void OnResume()
        {
            base.OnResume();

            if (_locationManager == null)
            {
                InitializeLocationManager();
            }
            else
            {
                var loc = _locationManager.GetLastKnownLocation("gps");
                loc = _locationManager.GetLastKnownLocation("network");

                if (loc != null && loc.Time > DateTime.Now.Millisecond - 2 * 60 * 1000)
                {
                    GetCurrentWeather(loc.Latitude, loc.Longitude);
                }
                else
                {
                    _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
                    _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, this);
                }
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}