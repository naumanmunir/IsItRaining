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
using static Android.Support.Design.Widget.NavigationView;
using IsItRaining.Fragments;
using Android.Support.V4.View;

namespace IsItRaining.Includes
{
    public class NavigationListener : Java.Lang.Object, IOnNavigationItemSelectedListener
    {
        MainActivity mainActivity;

        public NavigationListener(MainActivity ac)
        {
            mainActivity = ac;
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.attribution:
                    AttributeFragment af = new AttributeFragment();
                    //mainActivity.OverridePendingTransition(Resource.Animation.right_slide_in, Resource.Animation.close_scale);
                    //mainActivity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, af);
                    mainActivity.SupportFragmentManager.BeginTransaction().AddToBackStack("attribution");
                    mainActivity.SupportFragmentManager.BeginTransaction().Commit();

                    var g = mainActivity.SupportFragmentManager.BackStackEntryCount;
                    //mainActivity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragment_container, af).Commit();
                    mainActivity.drawerLayout.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.feedback:
                    mainActivity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.Jublix.Orca&hl=en")));
                    return true;
                default:
                    break;
            }

            return true;
        }

        
    }
}