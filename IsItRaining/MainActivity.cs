﻿using System;
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
using static Android.Views.Animations.Animation;
using Java.IO;
using System.IO;
using Android.Content;
using Android.Support.Design.Widget;

namespace IsItRaining
{
    [Activity(Label = "Is It Raining?", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity, ILocationListener
    {
        private Android.Support.V7.Widget.Toolbar toolBar;
        static Android.Locations.Location _currentLocation;
        static LocationManager _locationManager;

        private AdView adView; 

        string _locationProvider;

        TextView weatherStatus;
        TextView rainingOrNot;
        TextView currentTempurature;
        LinearLayout linearlayout;
        NavigationView navView;
        public DrawerLayout drawerLayout;

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

            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            weatherStatus = FindViewById<TextView>(Resource.Id.weatherSummary_text);
            rainingOrNot = FindViewById<TextView>(Resource.Id.status);
            currentTempurature = FindViewById<TextView>(Resource.Id.tempurature_text);
            linearlayout = FindViewById<LinearLayout>(Resource.Id.linearlayout1);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            SetUpNavigationView();

            InitializeLocationManager();

        }

        //public void SaveWeatherData()
        //{
        //    if (fileExistance("test.txt"))
        //    {
        //        try
        //        {
        //            Stream filein = OpenFileInput("test.txt");
        //            StreamReader io = new StreamReader(filein);
        //            tstData.Text = io.ReadToEnd();
        //            io.Close();
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        Stream fileout = OpenFileOutput("test.txt", Android.Content.FileCreationMode.Private);
        //        StreamWriter opWriter = new StreamWriter(fileout);
        //        opWriter.Write("This is a test");
        //        opWriter.Close();

        //    }
        //}

        //public bool fileExistance(string fname)
        //{
        //    Java.IO.File file = BaseContext.GetFileStreamPath(fname);

        //    var h = file.AbsolutePath;
        //    return file.Exists();
        //}

        private void SetUpNavigationView()
        {
            navView.SetNavigationItemSelectedListener(new NavigationListener(this));
            
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

        private void GetCurrentWeather(double latitde, double longitude)
        {
            if (latitde != 0 && longitude != 0)
            {
                DarkSkyService dk = new DarkSkyService("689a0d61985dfd4dae9cb86f67dcad79");


                var request = dk.GetWeatherDataAsync(latitde, longitude);

                //foreach(var hrs in request.Result.Hourly.Hours)
                //{
                    
                //}

                DisplayAccurateImage(request.Result.Minutely.Icon);

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
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day_beach);
                    AnimateImages();
                    break;
                case "clear-night":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_night);
                    AnimateImages();
                    break; 
                case "rain":
                    linearlayout.SetBackgroundResource(Resource.Drawable.rain);
                    AnimateImages();
                    break;
                case "snow":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "sleet":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "wind":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "fog":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
                    AnimateImages();
                    break;
                case "cloudy":
                    linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    AnimateImages();
                    break;
                case "partly-cloudy-day":
                    linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    AnimateImages();
                    break;
                case "partly-cloudy-night":
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day_beach);
                    AnimateImages();
                    break;
                default:
                    break;
            }
        }

        private void AnimateImages()
        {
            Animation fadeIn = AnimationUtils.LoadAnimation(this, Resource.Animation.fade_in);
            linearlayout.StartAnimation(fadeIn);

            fadeIn.SetAnimationListener(new AnimationListener(this, linearlayout));
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
            Animation myAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.scaling);
            rainingOrNot.StartAnimation(myAnimation);

            weatherStatus.StartAnimation(myAnimation);
            rainingOrNot.StartAnimation(myAnimation);
            currentTempurature.StartAnimation(myAnimation);
        }



        protected override void OnResume()
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

        protected override void OnPause()
        {
            base.OnPause();
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

