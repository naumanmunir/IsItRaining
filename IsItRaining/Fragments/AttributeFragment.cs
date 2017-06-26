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
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;

namespace IsItRaining.Fragments
{
    public class AttributeFragment : Android.Support.V4.App.Fragment, View.IOnClickListener
    {
        private Android.Support.V7.Widget.Toolbar toolBar;
        CardView darkSkyCardView;
        View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Attribution,
                   container, false);

            //View toolbar_view = view.FindViewById(Resource.Id.custom_toolbar_container_attribution);
            toolBar = view.FindViewById<Toolbar>(Resource.Id.custom_toolBar1);

            toolBar.SetNavigationOnClickListener(this);

            darkSkyCardView = view.FindViewById<CardView>(Resource.Id.card_view);


            ((AppCompatActivity)Activity).SetSupportActionBar(toolBar);

            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayShowTitleEnabled(false);
            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayShowHomeEnabled(true);
            ((AppCompatActivity)Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_back);


            darkSkyCardView.Click += DarkSkyCardView_Click;
            
            return view;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        //why cant I catch Resource.id.Home itemID ??? hard coded for now
        //        case Resource.Id.home:
        //            this.Activity.SupportFragmentManager.BeginTransaction().Remove(this).Commit();
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

        private void DarkSkyCardView_Click(object sender, EventArgs e)
        {
            var url = Android.Net.Uri.Parse("https://darksky.net/poweredby/");
            Intent intent = new Intent(Intent.ActionView, url);
            view.Context.StartActivity(intent);
        }

        public void OnClick(View v)
        {
            this.Activity.SupportFragmentManager.BeginTransaction().Remove(this).Commit();
        }
    }
}