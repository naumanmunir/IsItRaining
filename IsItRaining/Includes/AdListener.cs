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
using Android.Gms.Ads;


namespace IsItRaining.Includes
{
    public class AdListener : Android.Gms.Ads.AdListener
    {
        MainActivity mainActivity;
        private InterstitialAd ads = null;
        
        public AdListener(MainActivity act)
        {
            mainActivity = act;
        }

        public override void OnAdLoaded()
        {
            base.OnAdLoaded();
            //mainActivity.ban
        }
    }
}