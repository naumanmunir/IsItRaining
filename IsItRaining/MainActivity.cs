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
using static Android.Views.Animations.Animation;
using Java.IO;
using System.IO;
using Android.Content;
using Android.Support.Design.Widget;
using IsItRaining.Fragments;
using Android.Support.V4.View;

namespace IsItRaining
{
    [Activity(Label = "Is It Raining?", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity, ILocationListener
    {
        private Android.Support.V7.Widget.Toolbar toolBar;
        TextView weatherStatus;
        TextView rainingOrNot;
        TextView currentTempurature;
        ImageView bgImage;
        LinearLayout linearlayout;


        private AdView adView; 

        NavigationView navView;
        public DrawerLayout drawerLayout;
        FrameLayout frameLayout;
        private Android.Support.V4.App.Fragment currSelectedFrag;

        private Android.Locations.Location _currentLocation;
        private LocationManager _locationManager;
        string _locationProvider;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
           
            MobileAds.Initialize(this, "ca-app-pub-2637596544494423~2520806396");

            weatherStatus = FindViewById<TextView>(Resource.Id.weatherSummary_text);
            rainingOrNot = FindViewById<TextView>(Resource.Id.status);
            currentTempurature = FindViewById<TextView>(Resource.Id.tempurature_text);
            linearlayout = FindViewById<LinearLayout>(Resource.Id.linearlayout1);

            adView = FindViewById<AdView>(Resource.Id.adView);


            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.custom_toolBar);

            frameLayout = FindViewById<FrameLayout>(Resource.Id.fragment_container);
            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            SupportFragmentManager.BackStackChanged += SupportFragmentManager_BackStackChanged;

            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("Samsung-SM-N900A").Build();

            adView.LoadAd(adRequest);

            navView.NavigationItemSelected += NavView_NavigationItemSelected;

            InitializeLocationManager();
        }

        private void SupportFragmentManager_BackStackChanged(object sender, EventArgs e)
        {
            
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

        private void GetCurrentWeather(double latitde, double longitude)
        {
            if (latitde != 0 && longitude != 0)
            {
                DarkSkyService dk = new DarkSkyService("689a0d61985dfd4dae9cb86f67dcad79");


                var request = dk.GetWeatherDataAsync(latitde, longitude);

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
                    linearlayout.SetBackgroundResource(Resource.Drawable.clear_day);
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
                    linearlayout.SetBackgroundResource(Resource.Drawable.snow);
                    AnimateImages();
                    break;
                case "sleet":
                    linearlayout.SetBackgroundResource(Resource.Drawable.sleet);
                    AnimateImages();
                    break;
                case "wind":
                    linearlayout.SetBackgroundResource(Resource.Drawable.windy);
                    AnimateImages();
                    break;
                case "fog":
                    linearlayout.SetBackgroundResource(Resource.Drawable.fog);
                    AnimateImages();
                    break;
                case "cloudy":
                    linearlayout.SetBackgroundResource(Resource.Drawable.cloudy_day);
                    AnimateImages();
                    break;
                case "partly-cloudy-day":
                    linearlayout.SetBackgroundResource(Resource.Drawable.partly_cloudy_day);
                    
                    AnimateImages();
                    break;
                case "partly-cloudy-night":
                    linearlayout.SetBackgroundResource(Resource.Drawable.partly_cloudy_night);
                    AnimateImages();
                    break;
                default:
                    break;
            }
        }

        private void AnimateImages()
        {
            //Animation fadeIn = AnimationUtils.LoadAnimation(this, Resource.Animation.fade_in);
            //linearlayout.StartAnimation(fadeIn);

            //fadeIn.AnimationEnd += FadeIn_AnimationEnd;
            
        }

        private void FadeIn_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            Animation fadeOut = AnimationUtils.LoadAnimation(this, Resource.Animation.fade_out);
            linearlayout.StartAnimation(fadeOut);
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

        private void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.attribution:
                    AttributeFragment af = new AttributeFragment();
                    currSelectedFrag = af;
                    ShowFragment(af);
                    break;
                case Resource.Id.feedback:
                    EmailIntent();
                    break;
                case Resource.Id.rateapp:
                    StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.Jublix.Orca&hl=en")));
                    break;
                default:
                    break;
            }
            
            drawerLayout.CloseDrawer(GravityCompat.Start);
        }

        private void ShowFragment(Android.Support.V4.App.Fragment frag)
        {
            if (!frag.IsVisible)
            {
                try
                {
                    var trans = SupportFragmentManager.BeginTransaction();

                    trans.SetCustomAnimations(Resource.Animation.right_slide_in, Resource.Animation.close_scale, Resource.Animation.open_scale, Resource.Animation.right_slide_out);

                    trans.Add(Resource.Id.fragment_container, frag);

                    

                    trans.Hide(currSelectedFrag);
                    trans.Show(frag);
                    trans.AddToBackStack(null);
                    trans.Commit();

                    currSelectedFrag = frag;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ToggleMenu();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void ToggleMenu()
        {
            
            
            if (drawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                
                drawerLayout.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                int cnt = SupportFragmentManager.BackStackEntryCount;

                if (cnt == 0)
                {
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                }
                else
                {
                    SupportFragmentManager.PopBackStack();
                }
            }
        }



        public override void OnBackPressed()
        {


            int cnt = SupportFragmentManager.BackStackEntryCount;

            if (cnt == 0)
            {
                Finish();
            }
            else
            {
                SupportFragmentManager.PopBackStack();
            }
            
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

        private void EmailIntent()
        {
            Intent emailIntent = new Intent(Intent.ActionSend);
            emailIntent.PutExtra(Intent.ExtraEmail, new string[1] { "naumanbmunir@gmail.com" });
            emailIntent.PutExtra(Intent.ExtraSubject, "Android App Feedback");
            emailIntent.PutExtra(Intent.ExtraText, "Hi, my suggestion is...");
            emailIntent.SetType("message/rfc822");
            StartActivity(emailIntent);
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
    }
}

