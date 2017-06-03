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
    public class MainActivity : AppCompatActivity
    {
        private Android.Support.V7.Widget.Toolbar toolBar;

        private AdView adView; 

        NavigationView navView;
        public DrawerLayout drawerLayout;
        FrameLayout frameLayout;
        private Android.Support.V4.App.Fragment currSelectedFrag;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);
            
            //toolbar_container = FindViewById<LinearLayout>(Resource.Id.toolbar_container);
            MobileAds.Initialize(this, "ca-app-pub-2637596544494423~2520806396");

            adView = FindViewById<AdView>(Resource.Id.adView);
            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.custom_toolBar);
            frameLayout = FindViewById<FrameLayout>(Resource.Id.fragment_container);
            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            
            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("Samsung-SM-N900A").Build();

            adView.LoadAd(adRequest);

            WeatherFragment wfrag = new WeatherFragment();
            currSelectedFrag = wfrag;
            ShowFragment(wfrag);

            navView.NavigationItemSelected += NavView_NavigationItemSelected;
        }

        private void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.attribution:
                    AttributeFragment af = new AttributeFragment();
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

                    //trans.SetCustomAnimations(Resource.Animation.right_slide_in, Resource.Animation.right_slide_out, Resource.Animation.right_slide_in, Resource.Animation.right_slide_out);

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
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            int cnt = SupportFragmentManager.BackStackEntryCount;

            if (cnt == 1)
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

