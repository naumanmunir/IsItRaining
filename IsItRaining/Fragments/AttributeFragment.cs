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

namespace IsItRaining.Fragments
{
    public class AttributeFragment : Android.Support.V4.App.Fragment
    {
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

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.custom_toolBar);
            darkSkyCardView = view.FindViewById<CardView>(Resource.Id.card_view);

            darkSkyCardView.Click += DarkSkyCardView_Click;
            
            return view;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }



        private void DarkSkyCardView_Click(object sender, EventArgs e)
        {
            var url = Android.Net.Uri.Parse("https://darksky.net/poweredby/");
            Intent intent = new Intent(Intent.ActionView, url);
            view.Context.StartActivity(intent);
        }
    }
}